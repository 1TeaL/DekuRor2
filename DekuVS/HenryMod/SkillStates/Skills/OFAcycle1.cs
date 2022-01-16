using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class OFAcycle1 : BaseSkillState
	{
		public static float baseDuration = 0.05f;
		public static SkillDef primaryDef = Deku.primaryboost45SkillDef;
		public static SkillDef primaryDef2 = Deku.primaryboostSkillDef;
		public static SkillDef secondaryDef = Deku.secondaryboost45SkillDef;
		public static SkillDef secondaryDef2 = Deku.secondaryboostSkillDef;
		public static SkillDef utilityDef = Deku.utilityboostSkillDef;
		public static SkillDef utilityDef2 = Deku.utilityboostSkillDef;
		public static SkillDef utilityDef3 = Deku.utilityboostSkillDef;
		public static SkillDef specialDef = Deku.ofadownSkillDef;
		public DekuController dekucon;
		string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";

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
				base.characterBody.AddBuff(Modules.Buffs.ofaBuff);
			}
            base.PlayAnimation("FullBody, Override", "OFA","Attack.playbackRate", 0.05f);


			AkSoundEngine.PostEvent(3940341776, this.gameObject);
			AkSoundEngine.PostEvent(2493696431, this.gameObject);
			//base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAstate.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
			//base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAstate.secondaryDef, GenericSkill.SkillOverridePriority.Contextual);
			//base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAstate.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
			//base.skillLocator.special.SetSkillOverride(base.skillLocator.special, OFAstate.specialDef, GenericSkill.SkillOverridePriority.Contextual);

			if(base.skillLocator.primary.skillNameToken == prefix + "PRIMARY_NAME")
            {
				base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAcycle1.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
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