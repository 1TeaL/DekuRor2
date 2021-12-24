using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class Fajinstate : BaseSkillState
	{
		public static float baseDuration = 0.05f;
		public static SkillDef primaryDef = Deku.primaryfajinSkillDef;
		public static SkillDef secondaryDef = Deku.secondaryfajinSkillDef;
		public static SkillDef utilityDef = Deku.utilityfajinSkillDef;
		public static SkillDef specialDef = Deku.ofadownSkillDef;
		public DekuController dekucon;


		private float duration;
		public override void OnEnter()
		{
			
			base.OnEnter();
			//this.duration = baseDuration;
			dekucon = base.GetComponent<DekuController>();
			//dekucon.OFA.Play();
			dekucon.fajinon = true;

			//bool active = NetworkServer.active;
			//if (active)
			//{ 				
			//	base.characterBody.AddBuff(Modules.Buffs.ofaBuff);
			//}
            //base.PlayAnimation("FullBody, Override", "OFA","Attack.playbackRate", 0f);


			//AkSoundEngine.PostEvent(3940341776, this.gameObject);
			//AkSoundEngine.PostEvent(2493696431, this.gameObject);

			base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Fajinstate.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Fajinstate.secondaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Fajinstate.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, Fajinstate.specialDef, GenericSkill.SkillOverridePriority.Contextual);

			//if (NetworkServer.active && base.healthComponent)
			//{
			//	DamageInfo damageInfo = new DamageInfo();
			//	damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.1f;
			//	damageInfo.position = base.characterBody.corePosition;
			//	damageInfo.force = Vector3.zero;
			//	damageInfo.damageColorIndex = DamageColorIndex.Default;
			//	damageInfo.crit = false;
			//	damageInfo.attacker = null;
			//	damageInfo.inflictor = null;
			//	damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
			//	damageInfo.procCoefficient = 0f;
			//	damageInfo.procChainMask = default(ProcChainMask);
			//	base.healthComponent.TakeDamage(damageInfo);
			//}
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