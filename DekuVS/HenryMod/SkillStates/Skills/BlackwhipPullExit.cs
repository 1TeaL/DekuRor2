using System.Collections.Generic;
using System.Linq;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.ClayBoss;
using EntityStates;


namespace DekuMod.SkillStates
{

	public class BlackwhipPullExit : BaseSkillState
	{

		public static float exitDuration = 1f;
		private float stopwatch;

		public override void OnEnter()
		{
			base.OnEnter();
			this.stopwatch = 0f;
			base.PlayAnimation("Body", "IdleIn", "1f", BlackwhipPullExit.exitDuration);
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			if (this.stopwatch >= BlackwhipPullExit.exitDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}
	}
}
