using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class Fajinscepter : BaseSkillState
	{
		public static float baseDuration = 0.5f;
		public float stopwatch;
		public DekuController dekucon;
		public bool hasFired;


		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration/this.attackSpeedStat;
			base.GetModelAnimator().SetFloat("attack.playbackRate", 0.1f);
			base.PlayAnimation("Rightarm, Override", "Blackwhip", "attack.playbackRate", duration);
			dekucon = base.GetComponent<DekuController>();
			hasFired = false;
		}

        public override void FixedUpdate()
		{
            base.FixedUpdate();
			if (base.fixedAge >= duration && !hasFired)
            {
				dekucon.AddToBuffCount(10);
				hasFired = true;

				this.outer.SetNextStateToMain();
			}


		}
        public override void OnExit()
        {
            base.OnExit();
            base.PlayCrossfade("RightArm, Override", "BufferEmpty", duration / 2);

			base.GetModelAnimator().SetFloat("attack.playbackRate", 1f);
		}
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

	}
}