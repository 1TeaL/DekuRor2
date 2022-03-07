using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class OFAcycle2 : BaseSkillState
	{
		public static float baseDuration = 0.05f;
		public static SkillDef airforceDef = Deku.airforce100SkillDef;
		public static SkillDef shootstylekickDef = Deku.shootstylekick100SkillDef;
		public static SkillDef blackwhipDef = Deku.blackwhip100SkillDef;
		public static SkillDef manchesterDef = Deku.manchester100SkillDef;
		public static SkillDef shootstyleDef = Deku.shootstyle100SkillDef;
		public static SkillDef shootstylefullcowlingDef = Deku.shootstylefullcowling100SkillDef;
		public static SkillDef detroitDef = Deku.secondaryboostSkillDef;
		public static SkillDef specialDef = Deku.ofacycledownSkillDef;
		public DekuController dekucon;
		const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";

		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			dekucon = base.GetComponent<DekuController>();
			dekucon.OFAeye.Play();

			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.RemoveBuff(Modules.Buffs.ofaBuff45);
				base.characterBody.AddBuff(Modules.Buffs.ofaBuff);
			}
			base.PlayAnimation("FullBody, Override", "OFA", "Attack.playbackRate", 0.05f);


			AkSoundEngine.PostEvent(3940341776, this.gameObject);
			AkSoundEngine.PostEvent(2493696431, this.gameObject);

			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, OFAcycle1.specialDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, OFAcycle2.specialDef, GenericSkill.SkillOverridePriority.Contextual);


			switch (base.skillLocator.primary.skillNameToken)
			{
				case prefix + "BOOSTEDPRIMARY2_NAME":
                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAcycle1.airforceDef, GenericSkill.SkillOverridePriority.Contextual);
                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAcycle2.airforceDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDPRIMARY6_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAcycle1.shootstylekickDef, GenericSkill.SkillOverridePriority.Contextual);
					//base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAcycle1.altshootstylekickDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAcycle2.shootstylekickDef, GenericSkill.SkillOverridePriority.Contextual);
					break;

			}
			switch (base.skillLocator.secondary.skillNameToken)
			{
				case prefix + "BOOSTEDSECONDARY2_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, OFAcycle1.blackwhipDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAcycle2.blackwhipDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDSECONDARY5_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, OFAcycle1.manchesterDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAcycle2.manchesterDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (base.skillLocator.utility.skillNameToken)
			{
				case prefix + "BOOSTEDUTILITY4_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAcycle1.shootstyleDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAcycle2.shootstyleDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDUTILITY6_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAcycle1.shootstylefullcowlingDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAcycle2.shootstylefullcowlingDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDUTILITY8_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAcycle1.detroitDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAcycle2.detroitDef, GenericSkill.SkillOverridePriority.Contextual);
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