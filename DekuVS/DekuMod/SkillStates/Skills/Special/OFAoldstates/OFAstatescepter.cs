using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class OFAstatescepter : BaseSkillState
	{
		public static float baseDuration = 0.05f;
		public static SkillDef primaryDef = Deku.primaryboostSkillDef;
		public static SkillDef secondaryDef = Deku.secondaryboostSkillDef;
		public static SkillDef utilityDef = Deku.utilityboostSkillDef;
		public static SkillDef specialDef = Deku.ofadownSkillDef;
		public DekuController dekucon;


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
				base.characterBody.AddBuff(Modules.Buffs.supaofaBuff);		
				
			}


			AkSoundEngine.PostEvent(3940341776, this.gameObject);
			AkSoundEngine.PostEvent(2493696431, this.gameObject);
			base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAstate.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAstate.secondaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAstate.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, OFAstate.specialDef, GenericSkill.SkillOverridePriority.Contextual);
			if (NetworkServer.active && base.healthComponent)
			{
				DamageInfo damageInfo = new DamageInfo();
				damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.1f;
				damageInfo.position = base.characterBody.corePosition;
				damageInfo.force = Vector3.zero;
				damageInfo.damageColorIndex = DamageColorIndex.Default;
				damageInfo.crit = false;
				damageInfo.attacker = null;
				damageInfo.inflictor = null;
				damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
				damageInfo.procCoefficient = 0f;
				damageInfo.procChainMask = default(ProcChainMask);
				base.healthComponent.TakeDamage(damageInfo);
			}
		}
        public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextStateToMain();
			}
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}