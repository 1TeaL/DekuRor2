using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EntityStates.Merc;
using DekuMod.Modules;
using AK.Wwise;
using System;
using static UnityEngine.ParticleSystem.PlaybackState;
using EntityStates.Huntress;
using EntityStates.Loader;

namespace DekuMod.SkillStates.Might
{

	public class MightSuper : BaseSpecial
	{
		private bool hasFired;

		private BlastAttack blastAttack;
        private float blastRadius;
        private float blastDamage;
        private Vector3 blastPosition;
        private float blastForce;
        private DamageType blastType = DamageType.Generic;
        private float totalDuration;

        private GameObject areaIndicator;
        private Vector3 idealDirection;

        private bool animChange;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        private EffectData effectData;
        private EffectData effectData2;
        private Animator animator;
        private float duration;

        public override void OnEnter()
		{
			base.OnEnter();
            this.animator = base.GetModelAnimator();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);

			Ray aimRay = base.GetAimRay();
			characterBody.SetAimTimer(2f);

			//blast attack
			blastAttack = new BlastAttack();
			blastAttack.procCoefficient = 1f;
			blastAttack.attacker = base.gameObject;
			blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
			blastAttack.falloffModel = BlastAttack.FalloffModel.None;
			blastAttack.baseForce = 1000f;
			blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
			blastAttack.damageType = blastType;
			blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.radius = blastRadius;
            blastAttack.baseDamage = blastDamage;
            blastAttack.position = blastPosition;
            blastAttack.baseForce = blastForce;

            effectData = new EffectData
            {
                scale = blastRadius,
                origin = blastPosition,
                rotation = Quaternion.LookRotation(new Vector3(base.GetAimRay().direction.x, base.GetAimRay().direction.y, base.GetAimRay().direction.z)),
            };
            effectData2 = new EffectData
            {
                scale = attackSpeedStat,
                origin = blastPosition,
                rotation = Quaternion.LookRotation(new Vector3(base.GetAimRay().direction.x, base.GetAimRay().direction.y, base.GetAimRay().direction.z)),
            };

        }

		protected override void NeutralSuper()
		{
			base.NeutralSuper();
            blastRadius = StaticValues.detroitRadius * attackSpeedStat;
            blastDamage = StaticValues.detroitDamageCoefficient * damageStat * attackSpeedStat;
			//set in front of deku exactly
			blastPosition = characterBody.corePosition + (characterDirection.forward * (StaticValues.detroitRadius * attackSpeedStat * 0.5f));
            blastForce = StaticValues.detroitForce;
            duration = 1f;
            this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            //play animation of quick punch

        }
        protected override void BackwardSuper()
        {
			base.BackwardSuper();

			characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);

            blastRadius = StaticValues.detroitRadius2 * attackSpeedStat;
            blastDamage = StaticValues.detroitDamageCoefficient2 * damageStat * attackSpeedStat;
            //set around deku
            blastPosition = characterBody.corePosition;
			blastType = DamageType.Stun1s;
			blastForce = StaticValues.detroitForce2;
            //indicator
            this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            duration = 1f;
            //play animation of slam punch
        }
        protected override void ForwardSuper()
        {
            base.ForwardSuper();

            characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);

            blastRadius = StaticValues.detroitRadius3 * attackSpeedStat;
            blastDamage = StaticValues.detroitDamageCoefficient3 * damageStat * attackSpeedStat;
            //set in front of deku exactly
            blastPosition = characterBody.corePosition + (characterDirection.forward * (StaticValues.detroitRadius * attackSpeedStat * 0.5f));
            blastType = DamageType.Stun1s;
            blastForce = StaticValues.detroitForce3;

            Util.PlaySound(BaseChargeFist.startChargeLoopSFXString, base.gameObject);

            //set speed
            if (base.isAuthority)
            {
                base.characterBody.characterMotor.useGravity = false;
                base.characterBody.baseAcceleration = 420f;
                base.characterDirection.turnSpeed = 30f;
                base.characterMotor.walkSpeedPenaltyCoefficient = 0f;
                base.cameraTargetParams.fovOverride = 100f;

                if (base.inputBank)
                {
                    this.idealDirection = base.inputBank.aimDirection;
                    base.characterBody.isSprinting = true;
                    this.UpdateDirection();

                    base.characterBody.inputBank.enabled = false;
                }
            }
            //indicator
            this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            //play animation of beginning to jump and fly
        }
        public override void FixedUpdate()
		{
			base.FixedUpdate();

			switch(state)
			{
				case superState.SUPER1:
					if(base.fixedAge > 0.5f && !hasFired)
					{
						hasFired = true;
						blastAttack.Fire();

                        EffectManager.SpawnEffect(Modules.Asset.mageLightningBombEffectPrefab, effectData, true);
                        EffectManager.SpawnEffect(Modules.Asset.detroitEffect, effectData2, true);


                        AkSoundEngine.PostEvent("impactsfx", this.gameObject);
                    }
                    if (base.fixedAge > duration)
					{
						this.outer.SetNextStateToMain();
						return;
					}
					break;
				case superState.SUPER2:
                    if (base.fixedAge > 0.5f && !hasFired)
                    {
                        hasFired = true;
                        blastAttack.Fire();
                        EffectManager.SpawnEffect(Modules.Asset.mageLightningBombEffectPrefab, effectData, true);
                        EffectManager.SpawnEffect(Modules.Asset.detroitEffect, effectData2, true);


                        AkSoundEngine.PostEvent("impactsfx", this.gameObject);
                    }
                    if (base.fixedAge > duration)
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break; 
				case superState.SUPER3:

                    totalDuration += Time.fixedDeltaTime;

                    if (base.fixedAge > 0.5f)
                    {
                        Loop();
                    }
                    //if (base.fixedAge > 1f && !hasFired)
                    //{
                    //    hasFired = true;
                    //    blastAttack.Fire();
                    //    EffectManager.SpawnEffect(Modules.Asset.mageLightningBombEffectPrefab, effectData, true);
                    //    EffectManager.SpawnEffect(Modules.Asset.detroitEffect, effectData2, true);


                    //    AkSoundEngine.PostEvent("impactsfx", this.gameObject);
                    //}
                    //if (base.fixedAge > 1.5f)
                    //{
                    //    this.outer.SetNextStateToMain();
                    //    return;
                    //}
                    break;
			}

			
		}

        public void Loop()
        {
            bool isAuthority = base.isAuthority;
            if (isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                EffectManager.SpawnEffect(Modules.Asset.bisonEffect, new EffectData
                {
                    origin = base.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(aimRay.direction)
                }, true);

                //this.direction = base.GetAimRay().direction.normalized;
                //base.characterDirection.forward = this.direction;


                base.characterBody.isSprinting = true;
                base.characterDirection.moveVector = this.idealDirection;
                base.characterMotor.rootMotion += this.GetIdealVelocity() * Time.fixedDeltaTime;
                //Vector3 position = base.transform.position + base.characterDirection.forward.normalized * 0.5f;
                Vector3 position = base.characterBody.corePosition + Vector3.up * 0.5f + aimRay.direction.normalized * 1f;
                float radius = 0.3f;
                LayerIndex layerIndex = LayerIndex.world;
                int num = layerIndex.mask;
                layerIndex = LayerIndex.entityPrecise;
                int num2 = Physics.OverlapSphere(position, radius, num | layerIndex.mask).Length;
                bool flag2 = num2 != 0;
                if (flag2 || !base.IsKeyDownAuthority())
                {
                    base.characterMotor.velocity = Vector3.zero;

                    this.animator.SetBool("detroitRelease", true);
                    PlayAnimation("FullBody, Override", "Detroit100Smash");

                    if(totalDuration > 1f)
                    {
                        blastAttack.radius *= totalDuration;
                        blastAttack.baseDamage *= totalDuration;
                        blastAttack.baseForce *= totalDuration;
                    }


                    EffectManager.SimpleMuzzleFlash(Modules.Asset.dekuKickEffect, base.gameObject, "DownSwing", true);
                    for (int i = 0; i <= 10; i++)
                    {
                        float rand = 60f;
                        Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
                        float rand2 = 0.01f;
                        rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                        rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
                        EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
                        {
                            origin = base.characterBody.corePosition,
                            scale = blastRadius,
                            rotation = rotation
                        }, false);
                    }

                    Util.PlaySound(EntityStates.Bison.Headbutt.attackSoundString, base.gameObject);

                    AkSoundEngine.PostEvent("detroitexitsfx", base.gameObject);

                    blastAttack.position = base.transform.position;
                    if (blastAttack.Fire().hitCount > 0)
                    {
                        this.OnHitEnemyAuthority();
                    }
                    this.outer.SetNextStateToMain();
                }
                else
                {
                    PlayAnimation("FullBody, Override", "Detroit100Charging");
                    this.UpdateDirection();
                }

            }
            else
            {
                this.outer.SetNextStateToMain();
            }
        }


        public override void Update()
        {
            base.Update();
            //indicator update
            switch (state)
            {
                case superState.SUPER1:
                    if (this.areaIndicator)
                    {
                        this.areaIndicator.transform.localScale = Vector3.one * blastRadius * (attackSpeedStat);
                        this.areaIndicator.transform.localPosition = blastPosition;
                    }
                    break;
                case superState.SUPER2:
                    if (this.areaIndicator)
                    {
                        this.areaIndicator.transform.localScale = Vector3.one * blastRadius * (attackSpeedStat);
                        this.areaIndicator.transform.localPosition = blastPosition;
                    }
                    break;
                case superState.SUPER3:
                    if (this.areaIndicator)
                    {
                        this.areaIndicator.transform.localScale = Vector3.one * blastRadius * (attackSpeedStat) * (1 + totalDuration);
                        this.areaIndicator.transform.localPosition = base.transform.position;
                    }
                    break;

            }
        }

        protected virtual void OnHitEnemyAuthority()
        {
            Ray aimRay = base.GetAimRay();

            EffectData effectData = new EffectData
            {
                scale = blastRadius,
                origin = base.characterBody.corePosition,
                rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
            };
            EffectManager.SpawnEffect(Modules.Asset.mageLightningBombEffectPrefab, effectData, true);

            EffectData effectData2 = new EffectData
            {
                scale = attackSpeedStat,
                origin = base.characterBody.corePosition,
                rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
            };
            EffectManager.SpawnEffect(Modules.Asset.detroitEffect, effectData2, true);
        }
        private void UpdateDirection()
        {
            this.idealDirection = base.inputBank.aimDirection;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, true);
        }

        private Vector3 GetIdealVelocity()
        {
            return idealDirection * base.characterBody.moveSpeed * 3f;
        }
        public override void OnExit()
        {
            base.OnExit();
            base.characterBody.characterMotor.useGravity = true;
            base.characterBody.inputBank.enabled = true;
            if (areaIndicator)
            {
                this.areaIndicator.SetActive(false);
                EntityState.Destroy(this.areaIndicator);
                base.characterBody.characterMotor.useGravity = true;
                base.characterBody.inputBank.enabled = true;

                bool isAuthority = base.isAuthority;
                if (isAuthority)
                {
                    base.characterBody.baseAcceleration = 40f;
                    base.characterDirection.turnSpeed = 720f;
                    base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
                    base.cameraTargetParams.fovOverride = -1f;
                    base.characterBody.isSprinting = false;
                    base.OnExit();
                }
                Util.PlaySound(BaseChargeFist.endChargeLoopSFXString, base.gameObject);
            }
            //if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Death;
		}
	}
}