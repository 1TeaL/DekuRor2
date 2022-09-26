using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class QuirkSuper : BaseSpecial
	{
		public static float baseDuration = 0.5f;

		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			base.StartAimMode(0.5f + this.duration, false);

			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.AddTimedBuffAuthority(Modules.Buffs.fajinBuff.buffIndex, Modules.StaticValues.fajinDuration);
			}



		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.fixedAge > baseDuration)
			{
				this.outer.SetNextStateToMain();
			}

		}
		public override void OnExit()
		{
			base.OnExit();

		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}