﻿//using DekuMod.Modules.Survivors;
//using EntityStates;
//using RoR2.Skills;
//using RoR2;
//using UnityEngine.Networking;
//using UnityEngine;

//namespace DekuMod.SkillStates
//{

//	public class OFAcycle1 : BaseSkillState
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
//			//dekucon.OFA.Play();

//			bool active = NetworkServer.active;
//			if (active)
//			{ 				
//				base.characterBody.AddBuff(Modules.Buffs.ofaBuff45);
//			}


//			if (base.isAuthority)
//			{
//				AkSoundEngine.PostEvent("ofavoice", this.gameObject);
//				AkSoundEngine.PostEvent("ofasfx", this.gameObject);
//			}
//			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, Deku.ofacycle2SkillDef, GenericSkill.SkillOverridePriority.Contextual);


//			switch (base.skillLocator.primary.skillNameToken)
//			{
//				case prefix + "FISTPRIMARY_NAME":
//					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fistPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.fist45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//				case prefix + "LEGPRIMARY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.legPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.leg45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//				case prefix + "QUIRKPRIMARY_NAME":
//					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirk45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;


//                case prefix + "FISTSECONDARY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fistSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.fist45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEGSECONDARY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.legSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.leg45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRKSECONDARY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirk45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FISTUTILITY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fistUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.fist45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEGUTILITY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.legUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.leg45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRKUTILITY_NAME":
//                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.quirk45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//            }
//			switch (base.skillLocator.secondary.skillNameToken)
//			{
//                case prefix + "FISTPRIMARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fistPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.fist45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEGPRIMARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.legPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.leg45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRKPRIMARY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirk45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FISTSECONDARY_NAME":
//					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fistSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.fist45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//				case prefix + "LEGSECONDARY_NAME":
//					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.legSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.leg45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//				case prefix + "QUIRKSECONDARY_NAME":
//					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirk45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;

//                case prefix + "FISTUTILITY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fistUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.fist45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEGUTILITY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.legUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.leg45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRKUTILITY_NAME":
//                    base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.quirk45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//            }
//			switch (base.skillLocator.utility.skillNameToken)
//            {
//                case prefix + "FISTPRIMARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fistPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.fist45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEGPRIMARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.legPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.leg45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRKPRIMARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirk45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FISTSECONDARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fistSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.fist45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "LEGSECONDARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.legSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.leg45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;
//                case prefix + "QUIRKSECONDARY_NAME":
//                    base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirk45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//                    break;

//                case prefix + "FISTUTILITY_NAME":
//					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fistUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.fist45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//				case prefix + "LEGUTILITY_NAME":
//					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.legUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.leg45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//				case prefix + "QUIRKUTILITY_NAME":
//					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.quirk45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
//					break;
//			}

//			base.skillLocator.DeductCooldownFromAllSkillsServer(dekucon.skillCDTimer);
//			dekucon.skillCDTimer = 0f;

//		}

	
//        public override void FixedUpdate()
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