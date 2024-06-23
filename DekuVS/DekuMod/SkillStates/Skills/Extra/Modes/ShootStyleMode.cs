using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using ExtraSkillSlots;
using RoR2.Skills;

namespace DekuMod.SkillStates
{

	public class ShootStyleMode : BaseMode
    {

        public SkillDef skilldef1;
        public SkillDef skilldef2;
        public SkillDef skilldef3;
        public SkillDef skilldef4;
        public override void OnEnter()
        {
            base.OnEnter();


            skilldef1 = characterBody.skillLocator.primary.skillDef;
            skilldef2 = characterBody.skillLocator.secondary.skillDef;
            skilldef3 = characterBody.skillLocator.utility.skillDef;
            skilldef4 = characterBody.skillLocator.special.skillDef;


            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, skilldef1, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.shootPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, skilldef2, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.shootSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, skilldef3, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.shootUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.special.UnsetSkillOverride(base.skillLocator.utility, skilldef4, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.special.SetSkillOverride(base.skillLocator.utility, Deku.shootSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);


        }

        protected override void SwitchAttack()
        {
            base.SwitchAttack();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.outer.SetNextStateToMain();

        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}