using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class BaseSkill45 : BaseSkillState
	{
		public DekuController dekucon;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();


		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

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