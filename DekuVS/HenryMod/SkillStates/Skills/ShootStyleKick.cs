﻿using System;
using System.Collections.Generic;
using EntityStates;
using RoR2.Skills;
using EntityStates.Merc;
using EntityStates.Treebot.Weapon;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using DekuMod.Modules.Survivors;

namespace DekuMod.SkillStates
{

	public class ShootStyleKick : BaseSkillState

	{
		public static SkillDef primaryDef = Deku.primaryaltSkillDef;
		public float previousMass;
		private Ray aimRay;
		private Vector3 aimRayDir;
		private Vector3 previousPosition;
		private Transform modelTransform;
		private CharacterModel characterModel;

		public static float baseduration = 0.5f;
		public static float duration;
		public static float hitExtraDuration = 0.44f;
		public static float minExtraDuration = 0.2f;
		public static float initialSpeedCoefficient = 8f;
		public static float SpeedCoefficient;
		public static float finalSpeedCoefficient = 0f;
		public static float bounceForce = 2000f;
		private Vector3 bounceVector;
		private float stopwatch;
		private OverlapAttack detector;
		private OverlapAttack attack;
		protected string hitboxName = "ModelHitbox";
		protected string hitboxName2 = "BigBodyHitbox";
		protected DamageType damageType = DamageType.ResetCooldownsOnKill |DamageType.Generic;
		protected float procCoefficient = 1f;
		protected float pushForce = 400f;
		protected Vector3 bonusForce = new Vector3(10f, 100f, 0f);
		protected float baseDuration = 1f;
		protected float attackStartTime = 0.2f;
		protected float attackEndTime = 0.4f;
		protected float baseEarlyExitTime = 0.4f;
		protected float hitStopDuration = 0.19f;
		protected float attackRecoil = 0.75f;
		protected float hitHopVelocity = 250f;
		private float hitPauseTimer;
		protected bool inHitPause;
		private BaseState.HitStopCachedState hitStopCachedState;
		private Vector3 storedVelocity;
		public static GameObject muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashMageLightningLarge");
		public static GameObject muzzleEffectPrefab2 = Resources.Load<GameObject>("prefabs/effects/BoostJumpEffect");
		protected GameObject swingEffectPrefab;
		protected GameObject hitEffectPrefab;
		public static string dodgeSoundString = "HenryRoll";
		public static float dodgeFOV = 82f;
		private float extraDuration;
		private float rollSpeed;
		private Vector3 forwardDirection;
		private Animator animator;
		private Vector3 direction;
		private bool hasHopped;
        private float speedattack;

        public override void OnEnter()
		{
			base.OnEnter();
			this.aimRayDir = aimRay.direction;
			speedattack = this.attackSpeedStat / 3;
			if (speedattack < 1)
			{
				speedattack = 1;
			}
			duration = baseduration / speedattack;
			SpeedCoefficient = initialSpeedCoefficient;


			base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration/2);
			this.animator = base.GetModelAnimator();
			this.animator.SetBool("attacking", true);
			base.characterBody.SetAimTimer(2f);
			HitBoxGroup hitBoxGroup = null;
			HitBoxGroup hitBoxGroup2 = null;
			Transform modelTransform = base.GetModelTransform();
			bool flag = modelTransform.gameObject.GetComponent<AimAnimator>();
			if (flag)
			{
				modelTransform.gameObject.GetComponent<AimAnimator>().enabled = false;
			}
			this.modelTransform = base.GetModelTransform();
			if (this.modelTransform)
			{
				this.animator = this.modelTransform.GetComponent<Animator>();
				this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
			}

			//EffectManager.SimpleMuzzleFlash(ShootStyleKick.muzzleEffectPrefab, base.gameObject, "LFoot", false);
			EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, "LFoot", false);

			bool flag2 = modelTransform;
			if (flag2)
			{
				hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxName);
				hitBoxGroup2 = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxName2);
			}
			ChargeSonicBoom chargeSonicBoom = new ChargeSonicBoom();
			Util.PlaySound(chargeSonicBoom.sound, base.gameObject);
			this.attack = new OverlapAttack();
			this.attack.damageType = this.damageType;
			this.attack.attacker = base.gameObject;
			this.attack.inflictor = base.gameObject;
			this.attack.teamIndex = base.GetTeam();
			this.attack.damage = Modules.StaticValues.shootkickDamageCoefficient * this.damageStat;
			this.attack.procCoefficient = this.procCoefficient;
			this.attack.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab;
			this.attack.forceVector = this.bonusForce;
			this.attack.pushAwayForce = this.pushForce;
			this.attack.hitBoxGroup = hitBoxGroup;
			this.attack.isCrit = base.RollCrit();


			this.detector = new OverlapAttack();
			this.detector.damageType = this.damageType;
			this.detector.attacker = base.gameObject;
			this.detector.inflictor = base.gameObject;
			this.detector.teamIndex = base.GetTeam();
			this.detector.damage = 0f;
			this.detector.procCoefficient = 0f;
			this.detector.hitEffectPrefab = null;
			this.detector.forceVector = Vector3.zero;
			this.detector.pushAwayForce = 0f;
			this.detector.hitBoxGroup = hitBoxGroup2;
			this.detector.isCrit = false;
			this.direction = base.GetAimRay().direction.normalized;
			base.characterDirection.forward = base.characterMotor.velocity.normalized;
			float num = this.moveSpeedStat;
			bool isSprinting = base.characterBody.isSprinting;
			if (isSprinting)
			{
				num /= base.characterBody.sprintingSpeedMultiplier;
			}
			float num2 = (num / base.characterBody.baseMoveSpeed - 1f) * 0.67f;
			this.extraDuration = Math.Max(ShootStyleKick.hitExtraDuration / (num2 + 1f), ShootStyleKick.minExtraDuration);
			base.PlayAnimation("FullBody, Override", "ShootStyleKick", "Attack.playbackRate", ShootStyleKick.duration * 1f);
			
			AkSoundEngine.PostEvent(3842300745, this.gameObject);
			AkSoundEngine.PostEvent(573664262, this.gameObject);

			GetComponent<CharacterBody>().bodyFlags = CharacterBody.BodyFlags.SprintAnyDirection;

		}

		//private void RecalculateRollSpeed()
		//{
		//	this.rollSpeed = this.moveSpeedStat * ShootStyleKick.SpeedCoefficient;
		//}
		private void RecalculateRollSpeed()
		{
			float num = this.moveSpeedStat;
			bool isSprinting = base.characterBody.isSprinting;
			if (isSprinting)
			{
				num /= base.characterBody.sprintingSpeedMultiplier;
			}
			this.rollSpeed = num * Mathf.Lerp(ShootStyleKick.SpeedCoefficient, ShootStyleKick.finalSpeedCoefficient, base.fixedAge / ShootStyleKick.duration);
		}

		//private void CreateBlinkEffect(Vector3 origin)
		//{
		//	EffectData effectData = new EffectData();
		//	effectData.rotation = Util.QuaternionSafeLookRotation(this.aimRayDir);
		//	effectData.origin = origin;
		//	EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
		//}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.RecalculateRollSpeed();

            //this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
            if (this.modelTransform)
            {
                TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = 0.6f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay2.duration = 0.7f;
                temporaryOverlay2.animateShaderAlpha = true;
                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay2.destroyComponentOnEnd = true;
                temporaryOverlay2.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashExpanded");
                temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
            }
            this.FireAttack();
			this.hitPauseTimer -= Time.fixedDeltaTime;
			bool flag = this.hitPauseTimer <= 0f && this.inHitPause;
			if (flag)
			{
				base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
				this.inHitPause = false;
				base.characterMotor.velocity = Vector3.zero;
				base.characterMotor.ApplyForce(this.bounceVector, true, false);
				FireSonicBoom fireSonicBoom = new FireSonicBoom();
				Util.PlaySound(fireSonicBoom.sound, base.gameObject);
				this.attack.Fire(null);
			}
			bool flag2 = !this.inHitPause;
			if (flag2)
			{
				this.stopwatch += Time.fixedDeltaTime;
			}
			else
			{
				base.characterDirection.forward = this.bounceVector * -1f;
				bool flag3 = base.characterMotor;
				if (flag3)
				{
					base.characterMotor.velocity = Vector3.zero;
				}
                bool flag4 = this.animator;
                if (flag4)
                {
                    this.animator.SetFloat("Attack.playbackRate", 0f);
                }
            }
            bool flag5 = !this.hasHopped;
			if (flag5)
			{
				base.characterDirection.forward = this.direction;
				base.characterMotor.velocity = Vector3.zero;
				base.characterMotor.rootMotion += this.direction * this.rollSpeed * Time.fixedDeltaTime;
			}
			bool flag6 = base.cameraTargetParams;
			if (flag6)
			{
				base.cameraTargetParams.fovOverride = Mathf.Lerp(ShootStyleKick.dodgeFOV, 60f, base.fixedAge / ShootStyleKick.duration);
			}
			bool flag7 = base.isAuthority && this.stopwatch >= ShootStyleKick.duration;
			if (flag7)
			{
				this.outer.SetNextStateToMain();
			}

		}

		public override void OnExit()
		{
			Transform modelTransform = base.GetModelTransform();
			bool flag = modelTransform.gameObject.GetComponent<AimAnimator>();
			if (flag)
			{
				modelTransform.gameObject.GetComponent<AimAnimator>().enabled = false;
			}
			base.characterMotor.velocity /= 1.75f;
			bool flag2 = base.cameraTargetParams;
			if (flag2)
			{
				base.cameraTargetParams.fovOverride = -1f;
			}
			this.animator.SetBool("attacking", false);
			base.OnExit();
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(this.forwardDirection);
		}


		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.forwardDirection = reader.ReadVector3();
		}

		private void FireAttack()
		{
			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				List<HurtBox> list = new List<HurtBox>();
				bool flag = this.detector.Fire(list);
				if (flag)
				{
                    foreach (HurtBox hurtBox in list)
                    {
                        bool flag2 = hurtBox.healthComponent && hurtBox.healthComponent.body;
                        if (flag2)
                        {
                            this.ForceFlinch(hurtBox.healthComponent.body);
                        }
                    }
                    this.OnHitEnemyAuthority();
				}
			}
		}

		protected virtual void OnHitEnemyAuthority()
		{
			//base.PlayAnimation("FullBody, Override", "Backflip", "Roll.playbackRate", RollBounce.duration * 0.9f);
			base.characterBody.SetAimTimer(2f);
			base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, ShootStyleKick.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
			bool flag = !this.hasHopped;
			if (flag)
			{

				base.PlayAnimation("FullBody, Override", "ShootStyleKickExit", "Attack.playbackRate", ShootStyleKick.duration * 0.9f);
				this.stopwatch = ShootStyleKick.duration - this.extraDuration;
				bool flag2 = base.cameraTargetParams;
				if (flag2)
				{
					base.cameraTargetParams.fovOverride = -1f;
				}
				EffectManager.SimpleMuzzleFlash(ShootStyleKick.muzzleEffectPrefab, base.gameObject, "LFoot", false);
				EffectManager.SimpleMuzzleFlash(ShootStyleKick.muzzleEffectPrefab2, base.gameObject, "LFoot", false);

				bool flag3 = base.characterMotor;
				if (flag3)
				{
					base.characterMotor.velocity = Vector3.zero;
					this.bounceVector = base.GetAimRay().direction * -1f;
					this.bounceVector.y = 0.2f;
					this.bounceVector *= ShootStyleKick.bounceForce;
				}
				bool flag4 = !this.inHitPause && this.hitStopDuration > 0f;
				if (flag4)
				{
					this.storedVelocity = base.characterMotor.velocity;
					this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Attack.playbackRate");
					float num = this.moveSpeedStat;
					bool isSprinting = base.characterBody.isSprinting;
					if (isSprinting)
					{
						num /= base.characterBody.sprintingSpeedMultiplier;
					}
					float num2 = 1f + (num / base.characterBody.baseMoveSpeed - 1f);
					this.hitPauseTimer = this.hitStopDuration / num2;
					this.inHitPause = true;
				}
				this.hasHopped = true;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
        protected virtual void ForceFlinch(CharacterBody body)
        {
            SetStateOnHurt component = body.healthComponent.GetComponent<SetStateOnHurt>();
            bool flag = component == null;
            if (!flag)
            {
                bool canBeHitStunned = component.canBeHitStunned;
                if (canBeHitStunned)
                {
                    component.SetPain();
                    bool flag2 = body.characterMotor;
                    if (flag2)
                    {
                        body.characterMotor.velocity = Vector3.zero;
                    }
                }
            }
        }


    }
}