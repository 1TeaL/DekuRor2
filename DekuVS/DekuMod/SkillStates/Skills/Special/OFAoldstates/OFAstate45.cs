﻿//using DekuMod.Modules.Survivors;
//using EntityStates;
//using RoR2.Skills;
//using RoR2;
//using UnityEngine.Networking;
//using UnityEngine;

//namespace DekuMod.SkillStates
//{

//	public class OFAstate45 : BaseSkillState
//	{
//		public static float baseDuration = 0.05f;
//		public static SkillDef primaryDef = Deku.primaryboost45SkillDef;
//		public static SkillDef secondaryDef = Deku.secondaryboost45SkillDef;
//		public static SkillDef utilityDef = Deku.utilityboost45SkillDef;
//		public static SkillDef specialDef = Deku.ofadownSkillDef;
//		public DekuController dekucon;


//		private float duration;
//		public override void OnEnter()
//		{
//			base.OnEnter();
//			this.duration = baseDuration;
//			dekucon = base.GetComponent<DekuController>();
//			dekucon.OFA.Play();


//			bool active = NetworkServer.active;
//			if (active)
//			{

//				base.characterBody.AddBuff(Modules.Buffs.ofaBuff45);

//			}


//			AkSoundEngine.PostEvent(3940341776, this.gameObject);
//			AkSoundEngine.PostEvent(2493696431, this.gameObject);
//			base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAstate45.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
//			base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAstate45.secondaryDef, GenericSkill.SkillOverridePriority.Contextual);
//			base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAstate45.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
//			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, OFAstate45.specialDef, GenericSkill.SkillOverridePriority.Contextual);

//            if (NetworkServer.active && base.healthComponent)
//            {
//                DamageInfo damageInfo = new DamageInfo();
//                damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.05f;
//                damageInfo.position = base.characterBody.corePosition;
//                damageInfo.force = Vector3.zero;
//                damageInfo.damageColorIndex = DamageColorIndex.Default;
//                damageInfo.crit = false;
//                damageInfo.attacker = null;
//                damageInfo.inflictor = null;
//                damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
//                damageInfo.procCoefficient = 0f;
//                damageInfo.procChainMask = default(ProcChainMask);
//                base.healthComponent.TakeDamage(damageInfo);
//            }
//        }
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