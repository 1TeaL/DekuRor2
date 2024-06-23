using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using ExtraSkillSlots;
using RoR2.Skills;
using AK.Wwise;
using DekuMod.Modules;

namespace DekuMod.SkillStates
{

	public class MightMode : BaseMode
	{

        public SkillDef skilldef1;
        public SkillDef skilldef2;
        public SkillDef skilldef3;
        public SkillDef skilldef4;

        private bool isSwitch;
        private bool hasFired;
        private float duration;
        private BlastAttack blastAttack;

        public override void OnEnter()
		{
			base.OnEnter();

            skilldef1 = characterBody.skillLocator.primary.skillDef;
            skilldef2 = characterBody.skillLocator.secondary.skillDef;
            skilldef3 = characterBody.skillLocator.utility.skillDef;
            skilldef4 = characterBody.skillLocator.special.skillDef;


            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, skilldef1, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.mightPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, skilldef2, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.mightSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, skilldef3, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.mightUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.special.UnsetSkillOverride(base.skillLocator.utility, skilldef4, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.special.SetSkillOverride(base.skillLocator.utility, Deku.mightSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);

            if (isSwitch)
            {
                base.skillLocator.ResetSkills();
            }

        }

        protected override void SwitchAttack()
        {
            base.SwitchAttack();
            isSwitch = true;

            duration = 0.5f;
        }

        public override void FixedUpdate()
		{
			base.FixedUpdate();

            if(isSwitch)
            {
                if(base.fixedAge > duration)
                {

                    //blast attack
                    blastAttack = new BlastAttack();
                    blastAttack.procCoefficient = 1f;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.baseForce = 1000f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.damageType = DamageType.Generic;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    blastAttack.radius = StaticValues.mightSwitchRadius * attackSpeedStat;
                    blastAttack.baseDamage = damageStat * StaticValues.mightSwitchDamage * attackSpeedStat;
                    blastAttack.position = characterBody.corePosition + base.GetAimRay().direction * 3f;
                    blastAttack.baseForce = 10000f;

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
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}