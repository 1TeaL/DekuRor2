using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class QuirkSuper : BaseSkillState
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
				base.characterBody.AddBuff(Modules.Buffs.ofaBuff45);
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