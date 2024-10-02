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
using static RoR2.BulletAttack;
using EntityStates.VagrantMonster;
using Random = UnityEngine.Random;
using System.Net;

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

        private GameObject areaIndicator;

        private bool animChange;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        
        private float duration;
        private float attackStartTime;

        private int super2NumberOfHits;
        private float super2Interval;
        private float super2Stopwatch;

        private float attackStopwatch;
        private float maxCharge;
        private int baseMaxCharge = StaticValues.detroit3BaseCharge;
        private float maxDistance;
        private float chargePercent;
        private float baseDistance = StaticValues.detroit3Distance;
        private RaycastHit raycastHit;
        private float hitDis;
        private float damageMult;
        private float radius;
        private float baseRadius = StaticValues.detroit3Radius;
        private Vector3 maxMoveVec;
        private Vector3 randRelPos;
        private int randFreq;
        private bool reducerFlipFlop;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        private enum super3State { CHARGING, RELEASE, END};
        private super3State detroit3state;

        private Transform modelTransform;
        private Animator animator;
        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;

        public override void OnEnter()
		{
			base.OnEnter(); 
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
            }

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


        }

		protected override void NeutralSuper()
		{
			base.NeutralSuper();
            blastRadius = StaticValues.detroitRadius;
            blastDamage = StaticValues.detroitDamageCoefficient * damageStat * attackSpeedStat;
			//set in front of deku exactly
			blastPosition = characterBody.corePosition + (characterDirection.forward * (StaticValues.detroitRadius * 0.5f));
            blastForce = StaticValues.detroitForce;
            duration = 0.83f;
            attackStartTime = duration * 0.3f;
            this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            //play animation of quick punch
            base.PlayAnimation("FullBody, Override", "DetroitSmash1");

        }
        protected override void BackwardSuper()
        {
			base.BackwardSuper();
            super2NumberOfHits = Mathf.RoundToInt(StaticValues.detroit2BaseHits * attackSpeedStat);
            if(super2NumberOfHits < StaticValues.detroit2BaseHits)
            {
                super2NumberOfHits = StaticValues.detroit2BaseHits;
            }
			//characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);

            //bullet attack going up 
            duration = 0.7f;
            attackStartTime = duration * 0.5f;
            super2Interval = (duration-attackStartTime)/super2NumberOfHits;
            //play animation of uppercut
            base.PlayAnimation("FullBody, Override", "DetroitSmash2");
        }
        protected override void ForwardSuper()
        {
            base.ForwardSuper();

            float[] source = new float[]
            {
                this.attackSpeedStat,
                4f
            };
            this.maxCharge = (float)this.baseMaxCharge / source.Min();
            this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);

            Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);

            //play animation of charging up
            base.PlayAnimation("FullBody, Override", "DetroitSmash3");
            base.animator.SetBool("detroitSmash3Dashing", false);
            detroit3state = super3State.CHARGING;
        }
        public override void FixedUpdate()
		{
			base.FixedUpdate();

			switch(state)
			{
				case superState.SUPER1:
					if(base.fixedAge > attackStartTime && !hasFired)
					{
						hasFired = true;
						blastAttack.Fire();

                        EffectManager.SpawnEffect(Modules.DekuAssets.mageLightningBombEffectPrefab, new EffectData
                        {
                            origin = blastPosition,
                            scale = blastRadius,
                            rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                        }, true);
                        EffectManager.SpawnEffect(Modules.DekuAssets.detroitweakEffect, new EffectData
                        {
                            origin = characterBody.corePosition,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                        }, true);


                        AkSoundEngine.PostEvent("impactsfx", this.gameObject);
                    }
                    if (base.fixedAge > duration)
					{
						this.outer.SetNextStateToMain();
						return;
					}
					break;
				case superState.SUPER2:
                    if (base.fixedAge > attackStartTime)
                    {
                        super2Stopwatch += Time.fixedDeltaTime;
                        if(super2Stopwatch > super2Interval)
                        {
                            super2Interval = 0f;

                            var bulletAttack = new BulletAttack
                            {
                                bulletCount = 1U,
                                aimVector = Vector3.up,
                                origin = characterBody.corePosition,
                                damage = Modules.StaticValues.detroit2DamageCoefficient * damageStat,
                                damageColorIndex = DamageColorIndex.Default,
                                damageType = DamageType.Stun1s,
                                falloffModel = FalloffModel.DefaultBullet,
                                maxDistance = StaticValues.detroit2Range,
                                force = 0f,
                                hitMask = LayerIndex.CommonMasks.bullet,
                                minSpread = 0f,
                                maxSpread = 0f,
                                isCrit = RollCrit(),
                                owner = gameObject,
                                muzzleName = "RFinger",
                                smartCollision = false,
                                procChainMask = default,
                                procCoefficient = 1f,
                                radius = StaticValues.detroit2Radius,
                                sniper = false,
                                stopperMask = LayerIndex.noCollision.mask,
                                weapon = null,
                                //tracerEffectPrefab = Modules.Projectiles.bulletTracer,
                                spreadPitchScale = 0f,
                                spreadYawScale = 0f,
                                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                //hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                                hitEffectPrefab = DekuAssets.dekuHitImpactEffect

                            };
                            bulletAttack.Fire();
                        }

                        if(!hasFired)
                        {

                            EffectManager.SpawnEffect(Modules.DekuAssets.detroitUpperEffect, new EffectData
                            {
                                origin = characterBody.corePosition,
                                scale = 1f,
                                rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                            }, true);
                            AkSoundEngine.PostEvent("impactsfx", this.gameObject);

                        }
                    }
                    if (base.fixedAge > duration)
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break; 
				case superState.SUPER3:

                    switch(detroit3state)
                    {
                        case super3State.CHARGING:
                            if (base.fixedAge < this.maxCharge && base.IsKeyDownAuthority())
                            {
                                //base.PlayAnimation("FullBody, Override", "SmashFullCharge", "Attack.playbackRate", 1f);
                                this.chargePercent = base.fixedAge / this.maxCharge;
                                this.randRelPos = new Vector3((float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f);
                                this.randFreq = Random.Range(baseMaxCharge * 50, this.baseMaxCharge * 100) / 100;
                                bool flag2 = this.reducerFlipFlop;
                                if (flag2)
                                {
                                    bool flag3 = (float)this.randFreq <= this.chargePercent;
                                    if (flag3)
                                    {
                                        EffectData effectData = new EffectData
                                        {
                                            scale = 1f,
                                            origin = base.characterBody.corePosition + this.randRelPos
                                        };
                                        EffectManager.SpawnEffect(this.effectPrefab, effectData, true);
                                    }
                                    this.reducerFlipFlop = false;
                                }
                                else
                                {
                                    this.reducerFlipFlop = true;
                                }
                                //base.characterMotor.walkSpeedPenaltyCoefficient = 1f - this.chargePercent / 3f;
                                this.IndicatorUpdator();
                            }
                            else
                            {
                                detroit3state = super3State.RELEASE;
                            }
                            break;
                        case super3State.RELEASE:
                            if (base.isAuthority && base.characterMotor && base.characterBody)
                            {
                                Ray ownerAimRay = base.GetAimRay();
                                Vector3 normalized = (maxMoveVec - characterBody.corePosition).normalized;
                                Vector3 force = normalized * StaticValues.delaware3Acceleration * moveSpeedStat/characterBody.baseMoveSpeed;


                                if (this.characterModel)
                                {
                                    this.characterModel.invisibilityCount++;
                                }
                                if (this.hurtboxGroup)
                                {
                                    HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                                }
                                if (base.characterMotor)
                                {
                                    base.characterMotor.enabled = false;
                                }

                                //keep going towards point until in range
                                if (Vector2.Distance(maxMoveVec,characterBody.corePosition) > 5f)
                                {
                                    base.characterMotor.ApplyForce(force * (base.characterMotor.mass * Time.deltaTime), true, true);
                                }
                                else
                                {
                                    detroit3state = super3State.END;
                                }
                            }
                            break;
                        case super3State.END:

                            if (!hasFired)
                            {
                                if (this.modelTransform && EntityStates.ImpMonster.BlinkState.destealthMaterial)
                                {
                                    TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
                                    temporaryOverlay.duration = 1f;
                                    temporaryOverlay.destroyComponentOnEnd = true;
                                    temporaryOverlay.originalMaterial = EntityStates.ImpMonster.BlinkState.destealthMaterial;
                                    temporaryOverlay.inspectorCharacterModel = this.animator.gameObject.GetComponent<CharacterModel>();
                                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                                    temporaryOverlay.animateShaderAlpha = true;

                                }
                                if (this.characterModel)
                                {
                                    this.characterModel.invisibilityCount--;
                                }
                                if (this.hurtboxGroup)
                                {
                                    HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                                }
                                if (base.characterMotor)
                                {
                                    base.characterMotor.enabled = true;
                                }

                                hasFired = true;
                                int chooseAnim = Random.RandomRangeInt(0, 2);
                                switch(chooseAnim)
                                {
                                    case 0:
                                        attackStartTime = 0.4f;
                                        base.PlayAnimation("FullBody, Override", "DetroitSmash3End");
                                        break;
                                    case 1:
                                        attackStartTime = 0.4f;
                                        base.PlayAnimation("FullBody, Override", "DetroitSmash3End2");
                                        break;
                                    case 2:
                                        attackStartTime = 0.4f;
                                        base.PlayAnimation("FullBody, Override", "DetroitSmash3End3");
                                        break;
                                }
                            }
                            attackStopwatch += Time.fixedDeltaTime;
                            if(attackStopwatch > attackStartTime && hasFired)
                            {
                                hasFired = false;
                                blastDamage = damageMult;
                                blastRadius = radius;
                                blastPosition = maxMoveVec;
                                blastType = DamageType.Stun1s;
                                blastAttack.baseForce = damageMult * StaticValues.detroit3Force;

                                EffectData effectData = new EffectData
                                {
                                    scale = 1f,
                                    origin = maxMoveVec,
                                    rotation = Quaternion.LookRotation(characterDirection.forward),
                                };
                                EffectManager.SpawnEffect(DekuAssets.detroitEffect, effectData, true);
                                blastAttack.Fire();
                                this.outer.SetNextStateToMain();
                                return;
                            }
                            break;
                    }                    
                    break;
			}

			
		}

        public void IndicatorUpdator()
        {
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            aimRay.origin = base.characterBody.corePosition;
            this.maxDistance = (baseDistance + 4f * this.chargePercent) * this.baseDistance * (this.moveSpeedStat / 7f);
            Physics.Raycast(aimRay.origin, aimRay.direction, out this.raycastHit, this.maxDistance);
            this.hitDis = this.raycastHit.distance;
            bool flag = this.hitDis < this.maxDistance && this.hitDis > 0f;
            if (flag)
            {
                this.maxDistance = this.hitDis;
            }
            this.damageMult = Modules.StaticValues.detroit3DamageCoefficient + StaticValues.detroit3DamageMultiplier * (this.chargePercent * Modules.StaticValues.detroit3DamageCoefficient);
            this.radius = (this.baseRadius * this.damageMult) / 4f;
            this.maxMoveVec = this.maxDistance * direction;
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
                    break;
                case superState.SUPER3:
                    
                    if (this.areaIndicator)
                    {
                        Ray aimRay = base.GetAimRay();
                        this.areaIndicator.transform.localScale = Vector3.one * this.radius;
                        this.areaIndicator.transform.localPosition = aimRay.origin + this.maxMoveVec;
                    }

                    break;

            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (areaIndicator)
            {
                this.areaIndicator.SetActive(false);
                EntityState.Destroy(this.areaIndicator);
            }
            //if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Death;
		}
	}
}