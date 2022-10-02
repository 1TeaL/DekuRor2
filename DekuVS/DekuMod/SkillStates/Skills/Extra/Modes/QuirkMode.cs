using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using ExtraSkillSlots;

namespace DekuMod.SkillStates
{

	public class QuirkMode : BaseMode
	{
		public DekuController dekucon;
		const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";
		private ExtraSkillLocator extraskillLocator;

		public override void OnEnter()
		{
			base.OnEnter();
			

		}

        protected override void DoSkill()
        {
			dekucon = base.GetComponent<DekuController>();
			extraskillLocator = base.GetComponent<ExtraSkillLocator>();

			switch (base.skillLocator.primary.skillNameToken)
			{
				case prefix + "LEGPRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.legPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG45PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.leg45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirk45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG100PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.leg100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirk100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FISTPRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fistPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST45PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fist45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirk45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST100PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fist100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirk100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (base.skillLocator.secondary.skillNameToken)
			{
				case prefix + "LEGSECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.legSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG45SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.leg45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirk45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG100SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.leg100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirk100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FISTSECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fistSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST45SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fist45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirk45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST100SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fist100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirk100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (base.skillLocator.utility.skillNameToken)
			{
				case prefix + "LEGUTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.legUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG45UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.leg45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirk45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG100UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.leg100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirk100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FISTUTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fistUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST45UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fist45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirk45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST100UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fist100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirk100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}

			switch (extraskillLocator.extraFourth.skillNameToken)
			{
				case prefix + "LEGSPECIAL_NAME":
					extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, Deku.legSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, Deku.quirkSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FISTSPECIAL_NAME":
					extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, Deku.fistSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, Deku.quirkSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			base.skillLocator.primary.AddOneStock();
			base.skillLocator.secondary.AddOneStock();
			base.skillLocator.utility.AddOneStock();

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