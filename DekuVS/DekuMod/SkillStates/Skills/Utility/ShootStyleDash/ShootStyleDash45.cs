﻿using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Audio;
using System;
using EntityStates.Merc;
using System.Linq;
using DekuMod.Modules.Survivors;

namespace DekuMod.SkillStates
{
	public class ShootStyleDash45 : BaseSkillState
	{
		private Transform modelTransform;
		public static GameObject blinkPrefab;
		private float stopwatch;
		private Vector3 dashVector = Vector3.zero;
		public static float smallHopVelocity = 0.5f;
		public static float dashPrepDuration = 0.1f;
		public static float dashDuration = 0.1f;
		public static float speedCoefficient = 15f;
		public static string beginSoundString;
		public static string endSoundString;
		public static float overlapSphereRadius = 5f;
		public static float lollypopFactor = 1f;
		private Animator animator;
		private CharacterModel characterModel;
		private HurtBoxGroup hurtboxGroup;
		private bool isDashing;
		private CameraTargetParams.AimRequest aimRequest;

		public float fajin;
		protected DamageType damageType;
		public DekuController dekucon;
		public override void OnEnter()
		{
			base.OnEnter();
			AkSoundEngine.PostEvent(687990298, this.gameObject);
			AkSoundEngine.PostEvent(1918362945, this.gameObject);
			this.modelTransform = base.GetModelTransform();
			if (base.cameraTargetParams)
			{
				this.aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
			}
			if (this.modelTransform)
			{
				this.animator = this.modelTransform.GetComponent<Animator>();
				this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
			}
			if (base.isAuthority)
			{
				base.SmallHop(base.characterMotor, smallHopVelocity);
			}
			if (NetworkServer.active)
			{
				base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
			}
			base.PlayAnimation("FullBody, Override", "LegSmashFollow", "Attack.playbackRate", dashPrepDuration);
			this.dashVector = base.inputBank.aimDirection;
			base.characterDirection.forward = this.dashVector;
			base.StartAimMode(dashPrepDuration, true);
			dekucon = base.GetComponent<DekuController>();
			//if (dekucon.isMaxPower)
			//{
			//	fajin = 2f;
			//}
			//else
			//{
			//	fajin = 1f;
			//}


		}
		private void CreateBlinkEffect(Vector3 origin)
		{
			EffectData effectData = new EffectData();
			effectData.rotation = Util.QuaternionSafeLookRotation(this.dashVector);
			effectData.origin = origin;
			EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			if (this.stopwatch > dashPrepDuration && !this.isDashing)
			{
				this.isDashing = true;
				this.dashVector = base.inputBank.aimDirection;
				this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
				if (this.modelTransform)
				{
					//TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
					//temporaryOverlay.duration = 0.6f;
					//temporaryOverlay.animateShaderAlpha = true;
					//temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
					//temporaryOverlay.destroyComponentOnEnd = true;
					//temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
					//temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
					//TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
					//temporaryOverlay2.duration = 0.7f;
					//temporaryOverlay2.animateShaderAlpha = true;
					//temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
					//temporaryOverlay2.destroyComponentOnEnd = true;
					//temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
					//temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
				}
			}
			bool flag = this.stopwatch >= dashDuration + dashPrepDuration;
			if (this.isDashing)
			{
				if (base.characterMotor && base.characterDirection)
				{
					base.characterMotor.rootMotion += this.dashVector * (this.moveSpeedStat * speedCoefficient * Time.fixedDeltaTime * fajin);
				}
				if (base.isAuthority)
				{
					Collider[] array = Physics.OverlapSphere(base.transform.position, base.characterBody.radius + overlapSphereRadius * (flag ? lollypopFactor : 1f), LayerIndex.entityPrecise.mask);
					for (int i = 0; i < array.Length; i++)
					{
						HurtBox component = array[i].GetComponent<HurtBox>();
						if (component && component.healthComponent != base.healthComponent)
						{
							ShootStyleDashAttack45 nextState = new ShootStyleDashAttack45();
							this.outer.SetNextState(nextState);
							return;
						}
					}
				}
			}
			if (flag && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
			}
		}
		public override void OnExit()
		{
			Ray aimRay = base.GetAimRay();
			//if (dekucon.isMaxPower)
			//{
			//	EffectManager.SpawnEffect(Modules.Asset.impactEffect, new EffectData
			//	{
			//		origin = base.transform.position,
			//		scale = 1f,
			//		rotation = Quaternion.LookRotation(aimRay.direction)
			//	}, true);
			//}
			//dekucon.RemoveBuffCount(50);
			Util.PlaySound(EvisDash.endSoundString, base.gameObject);
			base.characterMotor.velocity *= 0.1f;
			base.SmallHop(base.characterMotor, smallHopVelocity);
			CameraTargetParams.AimRequest aimRequest = this.aimRequest;
			if (aimRequest != null)
			{
				aimRequest.Dispose();
			}
			base.PlayAnimation("FullBody, Override", "LegSmash", "Attack.playbackRate", dashPrepDuration);
			if (NetworkServer.active)
			{
				base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
			}
			base.OnExit();
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}

}
