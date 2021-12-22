using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{
    public class OFAdown: BaseSkillState

    {
		public static float baseDuration = 0.05f;
		public DekuController dekucon;


		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();
			dekucon.OFA.Stop();
			this.duration = baseDuration;
			bool active = NetworkServer.active;
			if (active)
			{
				if(base.characterBody.HasBuff(Modules.Buffs.ofaBuff))
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

				if (base.characterBody.HasBuff(Modules.Buffs.ofaBuffweak))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.ofaBuffweak);
				}
				if (base.characterBody.HasBuff(Modules.Buffs.supaofaBuffweak))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.supaofaBuffweak);
				}
			}

		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();

			this.outer.SetNextStateToMain();

		}
		public override void OnExit()
		{

			base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAstate.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, OFAstate.secondaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAstate.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, OFAstate.specialDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAstate45.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, OFAstate45.secondaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAstate45.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, OFAstate45.specialDef, GenericSkill.SkillOverridePriority.Contextual);
			base.OnExit();
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}