using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class OFAcycledownscepter : BaseSkillState
	{
		public static float baseDuration = 0.05f;
		public DekuController dekucon;
		const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";

		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			dekucon = base.GetComponent<DekuController>();
			dekucon.OFAeye.Stop();
			dekucon.OFA.Stop();

			bool active = NetworkServer.active;
			if (active)
			{
				if (base.characterBody.HasBuff(Modules.Buffs.ofaBuff))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.ofaBuff);
				}
				if (base.characterBody.HasBuff(Modules.Buffs.supaofaBuff))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.supaofaBuff);
				}
				if (base.characterBody.HasBuff(Modules.Buffs.ofaBuff45))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.ofaBuff45);
				}
				if (base.characterBody.HasBuff(Modules.Buffs.supaofaBuff45))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.supaofaBuff45);
				}
			}
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, OFAcycle2scepter.specialDef, GenericSkill.SkillOverridePriority.Contextual);

			switch (base.skillLocator.primary.skillNameToken)
			{
				case prefix + "BOOSTEDPRIMARY5_NAME":
                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAcycle2scepter.airforceDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDPRIMARY7_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAcycle2scepter.shootstylekickDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDPRIMARY9_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAcycle2scepter.dangersenseDef, GenericSkill.SkillOverridePriority.Contextual);
					break;

			}
			switch (base.skillLocator.secondary.skillNameToken)
			{
				case prefix + "BOOSTEDSECONDARY4_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, OFAcycle2scepter.blackwhipDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDSECONDARY6_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, OFAcycle2scepter.manchesterDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDSECONDARY7_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, OFAcycle2scepter.stlouisDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (base.skillLocator.utility.skillNameToken)
			{
				case prefix + "BOOSTEDUTILITY5_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAcycle2scepter.floatDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDUTILITY7_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAcycle2scepter.oklahomaDef, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "BOOSTEDSECONDARY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAcycle2scepter.detroitDef, GenericSkill.SkillOverridePriority.Contextual);
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