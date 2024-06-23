//using DekuMod.Modules.Survivors;
//using EntityStates;
//using RoR2.Skills;
//using RoR2;
//using UnityEngine.Networking;
//using UnityEngine;

//namespace DekuMod.SkillStates
//{

//	public class OFAcycledown : BaseSkillState
//	{
//		public static float baseDuration = 0.05f;
//		public DekuController dekucon;
//		const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";

//		private float duration;
//		public override void OnEnter()
//		{
//			base.OnEnter();
//			this.duration = baseDuration;
//			dekucon = base.GetComponent<DekuController>();
//			//dekucon.OFAeye.Stop();
//			//dekucon.OFA.Stop();

//			bool active = NetworkServer.active;
//			if (active)
//			{
//				if (base.characterBody.HasBuff(Modules.Buffs.ofaBuff))
//				{
//					base.characterBody.RemoveBuff(Modules.Buffs.ofaBuff);
//				}
//				if (base.characterBody.HasBuff(Modules.Buffs.supaofaBuff))
//				{
//					base.characterBody.RemoveBuff(Modules.Buffs.supaofaBuff);
//				}
//				if (base.characterBody.HasBuff(Modules.Buffs.ofaBuff45))
//				{
//					base.characterBody.RemoveBuff(Modules.Buffs.ofaBuff45);
//				}
//				if (base.characterBody.HasBuff(Modules.Buffs.supaofaBuff45))
//				{
//					base.characterBody.RemoveBuff(Modules.Buffs.supaofaBuff45);
//				}
//			}
//			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.ofacycledownSkillDef, GenericSkill.SkillOverridePriority.Contextual);
//			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, Deku.ofacycle1SkillDef, GenericSkill.SkillOverridePriority.Contextual);

//			switch (base.skillLocator.primary.skillNameToken)
//			{
//				case prefix + "FIST100PRIMARY_NAME":
//					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fist100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.fistPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//				case prefix + "LEG100PRIMARY_NAME":
//					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.leg100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.legPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//				case prefix + "QUIRK100PRIMARY_NAME":
//					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirk100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;

//                case prefix + "FIST100SECONDARY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fist100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.fistSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEG100SECONDARY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.leg100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.legSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRK100SECONDARY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirk100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FIST100UTILITY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fist100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.fistUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEG100UTILITY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.leg100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.legUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRK100UTILITY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirk100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//            }
//			switch (base.skillLocator.secondary.skillNameToken)
//            {
//                case prefix + "FIST100PRIMARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fist100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.fistPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEG100PRIMARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.leg100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.legPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRK100PRIMARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirk100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FIST100SECONDARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fist100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.fistSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEG100SECONDARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.leg100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.legSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRK100SECONDARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirk100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FIST100UTILITY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fist100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.fistUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEG100UTILITY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.leg100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.legUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRK100UTILITY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirk100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//            }
//			switch (base.skillLocator.utility.skillNameToken)
//            {
//                case prefix + "FIST100PRIMARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fist100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.fistPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEG100PRIMARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.leg100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.legPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRK100PRIMARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirk100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FIST100SECONDARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fist100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.fistSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEG100SECONDARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.leg100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.legSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRK100SECONDARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirk100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FIST100UTILITY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fist100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.fistUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEG100UTILITY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.leg100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.legUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRK100UTILITY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirk100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//            }

//			base.skillLocator.DeductCooldownFromAllSkillsServer(dekucon.skillCDTimer);
//			dekucon.skillCDTimer = 0f;
//		}
//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			this.outer.SetNextStateToMain();

//		}
//		public override InterruptPriority GetMinimumInterruptPriority()
//		{
//			return InterruptPriority.Frozen;
//		}
//	}
//}