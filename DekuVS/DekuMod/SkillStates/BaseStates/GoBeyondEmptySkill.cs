using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class GoBeyondEmptySkill : BaseSkillState
	{
		public DekuController dekucon;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();

            if (base.isAuthority)
            {
				this.outer.SetNextStateToMain();
				return;
            }

		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
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