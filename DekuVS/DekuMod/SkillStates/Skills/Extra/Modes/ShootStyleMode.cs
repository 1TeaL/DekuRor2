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
        private float slamForce = StaticValues.shootSwitchSlamForce;
        private GameObject slamIndicatorInstance;
        private float dropTimer;
        private bool hasDropped;
        private float movespeedMultiplier;
        private float slamRadius;

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

            characterBody.ApplyBuff(Buffs.mightBuff.buffIndex, 1, characterBody.GetBuffCount(Buffs.mightBuff) + StaticValues.mightBuffDuration);
            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

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
            slamForce *= movespeedMultiplier;
            slamRadius = StaticValues.shootSwitchRadius + (1 + dropTimer / 2) * movespeedMultiplier;
            base.characterMotor.disableAirControlUntilCollision = true;

            if (dekucon.GetTrackingTarget())
            {
                characterBody.characterMotor.Motor.SetPositionAndRotation(dekucon.GetTrackingTarget().transform.position + Vector3.up * StaticValues.shootSwitchHeightStart, Quaternion.LookRotation(base.GetAimRay().direction));
            }
            else
            {
                characterBody.characterMotor.Motor.SetPositionAndRotation(characterBody.footPosition + Vector3.up * StaticValues.shootSwitchHeightStart, Quaternion.LookRotation(base.GetAimRay().direction));
            }
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
                characterBody.skillLocator.secondary.AddOneStock();
                characterBody.skillLocator.utility.AddOneStock();
            }

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (isSwitch)
            {
                dropTimer += Time.fixedDeltaTime;
                slamRadius = StaticValues.shootSwitchRadius + (1 + dropTimer / 2) * movespeedMultiplier;
                if (!this.hasDropped)
                {
                    this.StartDrop();
                    base.PlayCrossfade("FullBody, Override", "ManchesterEnd", "Attack.playbackRate", 0.5f, 0.2f);
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
                    this.LandingImpact();
                    this.outer.SetNextStateToMain();
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
                AkSoundEngine.PostEvent(4108468048, base.gameObject);
                Ray aimRay = base.GetAimRay();

                base.characterMotor.velocity *= 0.1f;

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = slamRadius ;
                blastAttack.procCoefficient = 1f;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * StaticValues.shootSwitchDamage * (1 + dropTimer / 2) * movespeedMultiplier;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = slamForce * (1 + dropTimer);
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



            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;

            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}