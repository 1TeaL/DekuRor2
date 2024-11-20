using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using ExtraSkillSlots;
using RoR2.Skills;
using DekuMod.Modules;
using R2API.Networking;
using UnityEngine;
using EntityStates.Huntress;
using DekuMod.SkillStates.BaseStates;
using static RoR2.CameraTargetParams;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace DekuMod.SkillStates
{

	public class ShootStyleMode : BaseDekuSkillState
    {

        public SkillDef skilldef1;
        public SkillDef skilldef2;
        public SkillDef skilldef3;
        public SkillDef skilldef4;

        private bool resetSwappedSkill2;
        private bool resetSwappedSkill3;

        private bool isSwitch;
        private BlastAttack blastAttack;
        private float dropForce = StaticValues.shootSwitchDropForce;
        private float slamForce;
        private float damage;
        private GameObject slamIndicatorInstance;
        private float dropTimer;
        private bool hasDropped;
        private float movespeedMultiplier;
        private float slamRadius;

        private float attackTime;
        private bool hasJumped;

        private CharacterCameraParamsData manchesterCameraParams = new CharacterCameraParamsData()
        {
            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = new Vector3(0, 20.0f, -50f),
            wallCushion = 0.1f,
        };

        private CameraParamsOverrideHandle camOverrideHandle;
        private Vector3 startPos;
        private Vector3 endPos;

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

            if (base.skillLocator.secondary.cooldownRemaining > 0)
            {
                dekucon.resetSkill2 = false;
            }
            if (skilldef3 == Deku.mightUtilitySkillDef && base.skillLocator.utility.stock < base.skillLocator.utility.maxStock)
            {
                dekucon.resetSkill3 = false;
            }
            else if(base.skillLocator.utility.cooldownRemaining > 0)
            {
                dekucon.resetSkill3 = false;
            }

            if (skilldef1 != Deku.shootPrimarySkillDef)
            {
                base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, skilldef1, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.shootPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, skilldef2, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.shootSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, skilldef3, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.shootUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.special.UnsetSkillOverride(base.skillLocator.utility, skilldef4, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.special.SetSkillOverride(base.skillLocator.utility, Deku.shootSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);

                if (resetSwappedSkill2)
                {
                    base.skillLocator.secondary.AddOneStock();
                }
                if (resetSwappedSkill3)
                {
                    base.skillLocator.utility.AddOneStock();
                }
                if (!resetSwappedSkill2 || !resetSwappedSkill3)
                {
                    skillLocator.DeductCooldownFromAllSkillsAuthority(dekucon.skillCDTimer);
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

            attackTime = 0.5f / attackSpeedStat;

            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
            }
            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            GetModelAnimator().SetBool("manchesterDownEnd", false);
            PlayAnimation("Body", "Jump", "Attack.playbackRate", attackTime);

            //characterBody.ApplyBuff(Buffs.mightBuff.buffIndex, 1, characterBody.GetBuffCount(Buffs.mightBuff) + StaticValues.mightBuffDuration);
            //base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;
            startPos = characterBody.corePosition;

            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            float num2 = (num / base.characterBody.baseMoveSpeed - 1f) * 0.67f;
            movespeedMultiplier = num2 + 1f;
            dropForce *= movespeedMultiplier;
            base.characterMotor.disableAirControlUntilCollision = true;

            switch (level)
            {
                case 0:
                    break;
                case 1:
                    dropForce *= StaticValues.mightSwitchLevel2Multiplier;
                    break;
                case 2:
                    dropForce *= StaticValues.mightSwitchLevel3Multiplier;

                    characterBody.ApplyBuff(Buffs.fajinStoredBuff.buffIndex, characterBody.GetBuffCount(Buffs.fajinStoredBuff) + StaticValues.shootSwitchFajinStacks);
                    break;
            }

            if (dekucon.GetTrackingTarget())
            {
                //characterBody.characterMotor.Motor.SetPositionAndRotation(dekucon.GetTrackingTarget().transform.position + Vector3.up * StaticValues.shootSwitchHeightStart, Quaternion.LookRotation(base.GetAimRay().direction));
                endPos = dekucon.GetTrackingTarget().transform.position + Vector3.up * StaticValues.shootSwitchHeightStart;
            }
            else
            {
                //characterBody.characterMotor.Motor.SetPositionAndRotation(characterBody.footPosition + Vector3.up * StaticValues.shootSwitchHeightStart, Quaternion.LookRotation(base.GetAimRay().direction));
                endPos = characterBody.footPosition + Vector3.up * StaticValues.shootSwitchHeightStart;
            }
            TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
            temporaryOverlay.duration = 1f;
            temporaryOverlay.animateShaderAlpha = true;
            temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            temporaryOverlay.destroyComponentOnEnd = true;
            temporaryOverlay.originalMaterial = DekuAssets.fullCowlingMaterial;
            temporaryOverlay.inspectorCharacterModel = this.animator.gameObject.GetComponent<CharacterModel>();
        }

        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
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
                characterBody.skillLocator.secondary.AddOneStock();
                characterBody.skillLocator.utility.AddOneStock();
            }

        }
        private void SetPosition(Vector3 newPosition)
        {
            if (base.characterMotor)
            {
                base.characterMotor.Motor.SetPositionAndRotation(newPosition, Quaternion.identity, true);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (isSwitch)
            {
                if(base.fixedAge <= 0.2f)
                {
                    this.SetPosition(Vector3.Lerp(startPos, endPos, base.fixedAge / 0.2f));

                }
                else if (base.fixedAge > 0.2f)
                {
                    if (!hasJumped)
                    {
                        hasJumped = true;
                        PlayAnimation("FullBody, Override", "ManchesterSmashDown", "Attack.playbackRate", attackTime);

                        if(level == 2)
                        {
                            if (dekucon.BODYGEARSHIFT.isStopped)
                            {
                                dekucon.BODYGEARSHIFT.Play();
                            }
                        }
                    }
                }

                if(base.fixedAge > attackTime)
                {

                    dropTimer += Time.fixedDeltaTime;
                    slamRadius = StaticValues.shootSwitchRadius + (1 + dropTimer / 2) * movespeedMultiplier;
                    if (!this.hasDropped)
                    {
                        AkSoundEngine.PostEvent("shootstyledashsfx", this.gameObject);
                        this.StartDrop();
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
                        GetModelAnimator().SetBool("manchesterDownEnd", true);
                        switch (level)
                        {
                            case 0:
                                LandingImpact();
                                break;
                            case 1:
                                LandingImpact();
                                break;
                            case 2:
                                LandingImpact();
                                break;
                        }
                        this.outer.SetNextStateToMain();
                        return;
                    }
                }

                //incase player stuck
                if (base.fixedAge > 6f)
                {
                    this.outer.SetNextStateToMain();
                    return;
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
            base.characterMotor.velocity.y = -dropForce;



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
                //AkSoundEngine.PostEvent(4108468048, base.gameObject);
                Ray aimRay = base.GetAimRay();

                switch (level)
                {
                    case 0:
                        slamRadius = StaticValues.shootSwitchRadius + (1 + dropTimer / 2) * attackSpeedStat;
                        damage = StaticValues.shootSwitchDamage * (1 + dropTimer / 2) * movespeedMultiplier;
                        slamForce = StaticValues.shootSwitchSlamForce * (1 + dropTimer);
                        break;
                    case 1:
                        slamRadius = StaticValues.shootSwitchRadius + (1 + dropTimer / 2) * attackSpeedStat * StaticValues.shootSwitchLevel2Multiplier;
                        slamForce = StaticValues.shootSwitchSlamForce * (1 + dropTimer) * attackSpeedStat * StaticValues.shootSwitchLevel2Multiplier;
                        damage = StaticValues.shootSwitchDamage * (1 + dropTimer / 2) * movespeedMultiplier * StaticValues.shootSwitchLevel2Multiplier;
                        break;
                    case 2:
                        slamRadius = StaticValues.shootSwitchRadius + (1 + dropTimer / 2) * attackSpeedStat * StaticValues.shootSwitchLevel3Multiplier;
                        slamForce = StaticValues.shootSwitchSlamForce * (1 + dropTimer) * attackSpeedStat * StaticValues.shootSwitchLevel3Multiplier;
                        damage = StaticValues.shootSwitchDamage * (1 + dropTimer / 2) * movespeedMultiplier * StaticValues.shootSwitchLevel3Multiplier;
                        break;
                }


                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = slamRadius;
                blastAttack.procCoefficient = 1f;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * damage;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = slamForce;
                blastAttack.teamIndex = base.teamComponent.teamIndex;
                blastAttack.damageType = DamageType.Generic;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                BlastAttack.Result result = blastAttack.Fire();
                if (result.hitCount > 0)
                {
                    this.OnHitEnemyAuthority(result);
                }
                for (int i = 0; i <= 4; i += 1)
                {
                    Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * (slamRadius * (1 + dropTimer) * 0.5f));
                    effectPosition.y = base.characterBody.footPosition.y;
                    EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, new EffectData
                    {
                        origin = effectPosition,
                        scale = slamRadius * (1 + dropTimer / 2) * movespeedMultiplier,
                    }, true);
                }
                EffectManager.SpawnEffect(Modules.DekuAssets.mageLightningBombEffectPrefab, new EffectData
                {
                    origin = characterBody.corePosition,
                    scale = StaticValues.mightSwitchRadius * attackSpeedStat,
                    rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                }, true);

                //EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, new EffectData
                //{
                //    origin = base.characterBody.footPosition,
                //    scale = slamRadius * (1 + dropTimer / 2),
                //}, true);


            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.slamIndicatorInstance)
                this.slamIndicatorInstance.SetActive(false);
            EntityState.Destroy(this.slamIndicatorInstance);



            //base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;

            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}