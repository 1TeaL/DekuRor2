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
		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			//this.henryController.hasBazookaReady = false;
			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.RemoveBuff(Modules.Buffs.ofaBuff);
			}
			//base.PlayAnimation("FullBody, Override", "OFA", "Attack.playbackRate", this.duration);
			//base.PlayAnimation("Bazooka, Override", "BazookaExit", "Bazooka.playbackRate", this.duration);
			//Util.PlaySound("HenryBazookaUnequip", base.gameObject);
			//bool flag = base.cameraTargetParams;
			//if (flag)
			//{
			//	base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
			//}
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
		public override void OnExit()
		{
			base.OnExit();
			base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, OFAstate.primaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, OFAstate.secondaryDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, OFAstate.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, OFAstate.specialDef, GenericSkill.SkillOverridePriority.Contextual);
			//bool flag = base.cameraTargetParams;
			//this.henryController.UpdateCrosshair();
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}