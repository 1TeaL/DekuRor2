using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class OFAcycle1scepter : BaseSkillState
	{
		public static float baseDuration = 0.05f;
		public static SkillDef airforceDef = Deku.primaryboost45SkillDef;
		public static SkillDef shootstylekickDef = Deku.shootstylekick45SkillDef;
		public static SkillDef dangersenseDef = Deku.dangersense45SkillDef;
		public static SkillDef blackwhipDef = Deku.secondaryboost45SkillDef;
		public static SkillDef manchesterDef = Deku.manchester45SkillDef;
		public static SkillDef stlouisDef = Deku.utilityboost45SkillDef;
		public static SkillDef floatDef = Deku.float45SkillDef;
		public static SkillDef oklahomaDef = Deku.oklahoma45SkillDef;
		public static SkillDef detroitDef = Deku.detroit45SkillDef;
		public static SkillDef specialDef = Deku.ofacycle2SkillDef;
		public DekuController dekucon;
		const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";

		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			dekucon = base.GetComponent<DekuController>();
			dekucon.OFA.Play();

			bool active = NetworkServer.active;
			if (active)
			{ 				
				base.characterBody.AddBuff(Modules.Buffs.supaofaBuff45);
			}


			AkSoundEngine.PostEvent(3940341776, this.gameObject);
			AkSoundEngine.PostEvent(2493696431, this.gameObject);
            base.skillLocator.special.SetSkillOverride(base.skillLocator.special, OFAcycle1scepter.specialDef, GenericSkill.SkillOverridePriority.Contextual);


			switch (base.skillLocator.primary.skillNameToken)
			{
				case prefix + "PRIMARY_NAME":
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAcycle1.airforceDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "PRIMARY2_NAME":
					//base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.primaryaltSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAcycle1.shootstylekickDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "PRIMARY3_NAME":
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAcycle1.dangersenseDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (base.skillLocator.secondary.skillNameToken)
			{
				case prefix + "SECONDARY_NAME":
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAcycle1.blackwhipDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "SECONDARY2_NAME":
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAcycle1.manchesterDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "SECONDARY3_NAME":
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAcycle1.stlouisDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (base.skillLocator.utility.skillNameToken)
			{
				case prefix + "UTILITY_NAME":
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAcycle1.floatDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "UTILITY2_NAME":
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAcycle1.oklahomaDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "UTILITY3_NAME":
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAcycle1.detroitDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}

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