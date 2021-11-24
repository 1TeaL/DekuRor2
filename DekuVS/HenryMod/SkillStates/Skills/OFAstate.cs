using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class OFAstate : BaseSkillState
	{
		public static float baseDuration = 0.05f;
		public static SkillDef primaryDef = Deku.primaryboostSkillDef;
		public static SkillDef secondaryDef = Deku.secondaryboostSkillDef;
		public static SkillDef utilityDef = Deku.utilityboostSkillDef;
		public static SkillDef specialDef = Deku.ofadownSkillDef;


		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			//this.henryController.hasBazookaReady = true;

			bool active = NetworkServer.active;
			if (active)
			{ 				
				base.characterBody.AddBuff(Modules.Buffs.ofaBuff);
			}
            base.PlayAnimation("FullBody, Override", "OFA", "Atack.playbackRate", 0.05f;

			AkSoundEngine.PostEvent(3940341776, this.gameObject);
			AkSoundEngine.PostEvent(2493696431, this.gameObject);
			//Util.PlaySound("HenryBazookaEquip", base.gameObject);
			base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, OFAstate.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, OFAstate.secondaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, OFAstate.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, OFAstate.specialDef, GenericSkill.SkillOverridePriority.Contextual);
            //bool flag = base.cameraTargetParams;
            //if (flag)
            //{
            //    base.cameraTargetParams.aimMode = CameraTargetParams.AimType.OverTheShoulder;
            //}
            //this.DekuController.UpdateCrosshair();
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