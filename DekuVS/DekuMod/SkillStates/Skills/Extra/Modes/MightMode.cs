﻿using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using ExtraSkillSlots;
using RoR2.Skills;
using AK.Wwise;
using DekuMod.Modules;
using R2API.Networking;
using UnityEngine;
using DekuMod.SkillStates.BaseStates;
using static RoR2.CameraTargetParams;
using EntityStates.Huntress;

namespace DekuMod.SkillStates
{

	public class MightMode : BaseDekuSkillState
	{

        public SkillDef skilldef1;
        public SkillDef skilldef2;
        public SkillDef skilldef3;
        public SkillDef skilldef4;

        private bool resetSwappedSkill2;
        private bool resetSwappedSkill3;

        private bool hasFired;
        private bool isSwitch;
        private float duration;
        private float attackTime;

        private float slamRadius;
        private GameObject slamIndicatorInstance;
        private float dropForce = StaticValues.mightSwitchDropForce;

        public enum positionState { GROUND, AIR };
        public positionState state;
        private float dropTimer;
        private bool hasDropped;


        private CharacterCameraParamsData texasCameraParams = new CharacterCameraParamsData()
        {
            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = new Vector3(0, 0f, -20f),
            wallCushion = 0.1f,
        };

        private CameraParamsOverrideHandle camOverrideHandle;
        private float damage;
        private float force;

        public override void OnEnter()
		{
			base.OnEnter();


            skilldef1 = characterBody.skillLocator.primary.skillDef;
            skilldef2 = characterBody.skillLocator.secondary.skillDef;
            skilldef3 = characterBody.skillLocator.utility.skillDef;
            skilldef4 = characterBody.skillLocator.special.skillDef;

            if (dekucon.resetSkill2)
            {
                resetSwappedSkill2 = true;
            }
            if (dekucon.resetSkill3)
            {
                resetSwappedSkill3 = true;
            }

            if (base.skillLocator.secondary.stock >= 1)
            {
                dekucon.resetSkill2 = true;
            }
            else if (base.skillLocator.secondary.cooldownRemaining > 0)
            {
                dekucon.resetSkill2 = false;
            }

            if (skilldef3 == Deku.mightUtilitySkillDef && base.skillLocator.utility.stock >= base.skillLocator.utility.GetBaseMaxStock())
            {
                dekucon.resetSkill3 = true;
            }
            else if (skilldef3 == Deku.mightUtilitySkillDef && base.skillLocator.utility.stock < base.skillLocator.utility.maxStock)
            {
                dekucon.resetSkill3 = false;
            }
            else if (base.skillLocator.utility.cooldownRemaining > 0)
            {
                dekucon.resetSkill3 = false;
            }


            if (skilldef1 != Deku.mightPrimarySkillDef)
            {
                base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, skilldef1, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.mightPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, skilldef2, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.mightSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, skilldef3, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.mightUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.special.UnsetSkillOverride(base.skillLocator.utility, skilldef4, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.special.SetSkillOverride(base.skillLocator.utility, Deku.mightSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);

                if (resetSwappedSkill2)
                {
                    base.skillLocator.secondary.AddOneStock();
                }

                //make sure all stocks get given back
                if (resetSwappedSkill3)
                {
                    for (int i = 0; i < base.skillLocator.utility.GetBaseMaxStock(); i++)
                    {
                        base.skillLocator.utility.AddOneStock();
                    }
                }

                if(!resetSwappedSkill2 || !resetSwappedSkill3)
                {
                    skillLocator.DeductCooldownFromAllSkillsAuthority(dekucon.skillCDTimer);
                    //make sure number of stocks = time taken
                    for (int i = 0; i < (int)dekucon.skillCDTimer; i++)
                    {
                        base.skillLocator.utility.AddOneStock();
                    }
                }
                dekucon.skillCDTimer = 0f;

                if (energySystem.currentPlusUltra > Modules.StaticValues.super1Cost)
                {
                    energySystem.SpendPlusUltra(Modules.StaticValues.super1Cost);
                    SwitchAttack();
                }
            }


        }


        protected virtual void SwitchAttack()
        {
            isSwitch = true;
            base.skillLocator.ResetSkills();
            for (int i = 0; i < base.skillLocator.utility.maxStock; i++)
            {
                if(base.skillLocator.utility.stock < base.skillLocator.utility.maxStock)
                {
                    base.skillLocator.utility.AddOneStock();
                }
            }

            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
            }

            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);

            slamRadius = StaticValues.mightSwitchRadius * attackSpeedStat;
            damage = StaticValues.mightSwitchDamage * attackSpeedStat;
            force = StaticValues.mightSwitchForce;

            if (characterMotor.isGrounded)
            {
                state = positionState.GROUND;
                duration = 0.5f /attackSpeedStat;
                attackTime = 0.25f /attackSpeedStat;

                CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
                {
                    cameraParamsData = texasCameraParams,
                    priority = 0,
                };

                camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, attackTime);

                base.PlayCrossfade("FullBody, Override", "TexasSmash", "Attack.playbackRate", duration, 0.01f);

                AkSoundEngine.PostEvent("shootstyledashcombosfx", this.gameObject);

                switch (level)
                {
                    case 0:
                        break;
                    case 1:
                        slamRadius *= StaticValues.mightSwitchLevel2Multiplier;
                        force *= StaticValues.mightSwitchLevel2Multiplier;
                        break;
                    case 2:
                        slamRadius *= StaticValues.mightSwitchLevel3Multiplier;
                        force *= StaticValues.mightSwitchLevel3Multiplier;
                        if (dekucon.RARMGEARSHIFT.isStopped)
                        {
                            dekucon.RARMGEARSHIFT.Play();
                        }
                        break;
                }
            }
            else if(!characterMotor.isGrounded)
            {

                state = positionState.AIR;
                base.characterMotor.Motor.ForceUnground();
                base.characterMotor.velocity = Vector3.zero;

                //base.gameObject.layer = LayerIndex.fakeActor.intVal;
                base.characterMotor.Motor.RebuildCollidableLayers();
                base.characterMotor.disableAirControlUntilCollision = true;

                GetModelAnimator().SetBool("texasSmashAirEnd", false);

                attackTime = 0.8f / attackSpeedStat;
                base.PlayAnimation("FullBody, Override", "TexasSmashAir", "Attack.playbackRate", attackTime);

                CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
                {
                    cameraParamsData = texasCameraParams,
                    priority = 0,
                };

                camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, attackTime);

                switch (level)
                {
                    case 0:
                        break;
                    case 1:
                        dropForce *= StaticValues.mightSwitchLevel2Multiplier;
                        break;
                    case 2:
                        dropForce *= StaticValues.mightSwitchLevel3Multiplier;
                        break;
                }

            }
        }

        public override void FixedUpdate()
		{
			base.FixedUpdate();

            if(isSwitch)
            {
                switch (state)
                {
                    case positionState.GROUND:

                        if (base.fixedAge > attackTime && !hasFired)
                        {
                            hasFired = true;

                            switch (level)
                            {
                                case 0:
                                    LandingImpact();
                                    break;
                                case 1:
                                    LandingImpact();
                                    LandingImpact();
                                    break;
                                case 2:
                                    LandingImpact();
                                    LandingImpact();
                                    LandingImpact();
                                    LandingImpact();
                                    LandingImpact();
                                    break;
                            }

                        }
                        if (base.fixedAge > duration)
                        {                                                    

                            this.outer.SetNextStateToMain();
                            return;
                        }
                        break;
                    case positionState.AIR:

                        if (base.fixedAge <= attackTime)
                        {
                            characterMotor.velocity.y = 0f;
                        }

                        if(base.fixedAge > attackTime)
                        {
                            if (dekucon.RARM.isStopped)
                            {
                                dekucon.RARM.Play();
                            }
                            TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
                            temporaryOverlay.duration = 1f;
                            temporaryOverlay.animateShaderAlpha = true;
                            temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                            temporaryOverlay.destroyComponentOnEnd = true;
                            temporaryOverlay.originalMaterial = DekuAssets.fullCowlingMaterial;
                            temporaryOverlay.inspectorCharacterModel = this.animator.gameObject.GetComponent<CharacterModel>();

                            dropTimer += Time.fixedDeltaTime;
                            switch (level)
                            {
                                case 0:
                                    slamRadius = StaticValues.mightSwitchRadius + (1 + dropTimer / 2) * attackSpeedStat;
                                    damage = StaticValues.mightSwitchDamage + (1 + dropTimer / 2) * attackSpeedStat;
                                    force = StaticValues.mightSwitchForce * (1 + dropTimer);
                                    break;
                                case 1:
                                    slamRadius = StaticValues.mightSwitchRadius + (1 + dropTimer / 2) * attackSpeedStat * StaticValues.mightSwitchLevel2Multiplier;
                                    force = StaticValues.mightSwitchForce * (1 + dropTimer/2) * attackSpeedStat * StaticValues.mightSwitchLevel2Multiplier;
                                    damage = StaticValues.mightSwitchDamage + (1 + dropTimer / 2) * attackSpeedStat * StaticValues.mightSwitchLevel2Multiplier;
                                    break;
                                case 2:
                                    slamRadius = StaticValues.mightSwitchRadius + (1 + dropTimer / 2) * attackSpeedStat * StaticValues.mightSwitchLevel3Multiplier;
                                    force = StaticValues.mightSwitchForce * (1 + dropTimer/2) * attackSpeedStat * StaticValues.mightSwitchLevel3Multiplier;
                                    damage = StaticValues.mightSwitchDamage + (1 + dropTimer / 2) * attackSpeedStat * StaticValues.mightSwitchLevel3Multiplier;
                                    break;
                            }
                            if (!this.hasDropped)
                            {
                                this.StartDrop();

                                AkSoundEngine.PostEvent("shootstyledashsfx", this.gameObject);
                            }

                            if (!this.slamIndicatorInstance)
                            {
                                this.CreateIndicator();
                            }
                            if (this.slamIndicatorInstance)
                            {
                                this.UpdateSlamIndicator();
                            }

                            if (this.hasDropped && base.isAuthority && !base.characterMotor.disableAirControlUntilCollision)
                            {
                                GetModelAnimator().SetBool("texasSmashAirEnd", true);
                                switch (level)
                                {
                                    case 0:
                                        LandingImpact();
                                        break;
                                    case 1:
                                        LandingImpact();
                                        LandingImpact();
                                        break;
                                    case 2:
                                        LandingImpact();
                                        LandingImpact();
                                        LandingImpact();
                                        LandingImpact();
                                        LandingImpact();
                                        if (dekucon.RARMGEARSHIFT.isStopped)
                                        {
                                            dekucon.RARMGEARSHIFT.Play();
                                        }
                                        break;
                                }
                                this.outer.SetNextStateToMain();
                                return;
                            }

                        }
                        
                        //incase player stuck
                        if(base.fixedAge > 6f)
                        {
                            this.outer.SetNextStateToMain();
                            return;
                        }

                        break;
                }
                    

            }
            else
            {
                this.outer.SetNextStateToMain();
                return;
            }
			
		}

        private void StartDrop()
        {
            this.hasDropped = true;

            base.characterMotor.disableAirControlUntilCollision = true;
            base.characterMotor.velocity.y = -dropForce * attackSpeedStat;



        }

        private void CreateIndicator()
        {
            if (EntityStates.Huntress.ArrowRain.areaIndicatorPrefab)
            {
                this.slamIndicatorInstance = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                this.slamIndicatorInstance.SetActive(true);

            }
        }
        private void UpdateSlamIndicator()
        {
            if (this.slamIndicatorInstance)
            {
                this.slamIndicatorInstance.transform.localScale = Vector3.one * slamRadius;
                this.slamIndicatorInstance.transform.localPosition = base.transform.position;
            }
        }
        private void LandingImpact()
        {

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                if(state == positionState.AIR)
                {
                    base.characterMotor.velocity *= 0.1f;
                    
                }


                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = slamRadius;
                blastAttack.procCoefficient = 1f;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * damage;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = force;
                blastAttack.bonusForce = new Vector3(0f, force/10f, 0f);
                blastAttack.teamIndex = base.teamComponent.teamIndex;
                blastAttack.damageType = DamageType.Generic;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                BlastAttack.Result result = blastAttack.Fire();
                if (result.hitCount > 0)
                {
                    this.OnHitEnemyAuthority(result);
                }
                //for (int i = 0; i <= 2; i += 1)
                //{
                //    Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * (slamRadius));
                //    effectPosition.y = base.characterBody.footPosition.y;
                //    EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, new EffectData
                //    {
                //        origin = effectPosition,
                //        scale = slamRadius,
                //    }, true);
                //}

                EffectManager.SpawnEffect(Modules.DekuAssets.mageLightningBombEffectPrefab, new EffectData
                {
                    origin = characterBody.corePosition,
                    scale = StaticValues.mightSwitchRadius * attackSpeedStat,
                    rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                }, true);
                EffectManager.SpawnEffect(Modules.DekuAssets.texasEffect, new EffectData
                {
                    origin = characterBody.corePosition,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                }, true);

            }
        }

        protected virtual void OnHitEnemyAuthority(BlastAttack.Result result)
        {
            AkSoundEngine.PostEvent("impactsfx", this.gameObject);
            foreach (BlastAttack.HitPoint hitpoint in result.hitPoints)
            {
                EffectManager.SpawnEffect(Modules.DekuAssets.dekuHitImpactEffect, new EffectData
                {
                    origin = hitpoint.hurtBox.healthComponent.body.gameObject.transform.position,
                    scale = 1f,

                }, true);
            }
            //base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 20));

        }

        public override void OnExit()
        {
            base.OnExit();

            base.cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.3f);

            if (slamIndicatorInstance)
            {
                this.slamIndicatorInstance.SetActive(false);
                EntityState.Destroy(this.slamIndicatorInstance);
            }

            if (isSwitch)
            {
                switch (level)
                {
                    case 0:
                        characterBody.ApplyBuff(Buffs.mightBuff.buffIndex, 1, StaticValues.mightBuffDuration);
                        break;
                    case 1:
                        characterBody.ApplyBuff(Buffs.mightBuff.buffIndex, 1, (int)(StaticValues.mightBuffDuration * StaticValues.mightSwitchLevel2Multiplier));
                        break;
                    case 2:
                        characterBody.ApplyBuff(Buffs.mightBuff.buffIndex, 1, (int)(StaticValues.mightBuffDuration * StaticValues.mightSwitchLevel3Multiplier));
                        break;
                }

            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}