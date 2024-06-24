using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using ExtraSkillSlots;
using RoR2.Skills;
using DekuMod.Modules;
using R2API.Networking;
using UnityEngine;
using R2API;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.SkillStates
{

	public class BlackwhipMode : BaseDekuSkillState
    {

        public SkillDef skilldef1;
        public SkillDef skilldef2;
        public SkillDef skilldef3;
        public SkillDef skilldef4;
        private bool isSwitch;
        private float duration;
        private BlastAttack blastAttack;

        public override void OnEnter()
        {
            base.OnEnter();


            skilldef1 = characterBody.skillLocator.primary.skillDef;
            skilldef2 = characterBody.skillLocator.secondary.skillDef;
            skilldef3 = characterBody.skillLocator.utility.skillDef;
            skilldef4 = characterBody.skillLocator.special.skillDef;


            if (skilldef1 != Deku.blackwhipPrimarySkillDef)
            {
                base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, skilldef1, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.blackwhipPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, skilldef2, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.blackwhipSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, skilldef3, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.blackwhipUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.special.UnsetSkillOverride(base.skillLocator.utility, skilldef4, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.special.SetSkillOverride(base.skillLocator.utility, Deku.blackwhipSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);

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

            duration = 0.5f;
            characterBody.ApplyBuff(Buffs.blackwhipCDBuff.buffIndex, 1, characterBody.GetBuffCount(Buffs.blackwhipCDBuff) + StaticValues.blackwhipCDBuffDuration);

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (isSwitch)
            {
                if (base.fixedAge > duration)
                {
                    base.characterMotor.velocity *= 0.1f;
                    //blast attack
                    blastAttack = new BlastAttack();
                    blastAttack.procCoefficient = 1f;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.baseForce = 20000f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.damageType = DamageType.Generic;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    blastAttack.radius = StaticValues.blackwhipSwitchRadius * attackSpeedStat;
                    blastAttack.baseDamage = damageStat * StaticValues.blackwhipSwitchDamage * attackSpeedStat;
                    blastAttack.position = characterBody.corePosition;

                    DamageAPI.AddModdedDamageType(blastAttack, Damage.blackwhipImmobilise);
                    blastAttack.Fire();

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

        public override void OnExit()
        {
            base.OnExit();

            base.skillLocator.DeductCooldownFromAllSkillsServer(dekucon.skillCDTimer);
            dekucon.skillCDTimer = 0f;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}