using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class BaseMode : BaseSkillState
	{
		public DekuController dekucon;
		public EnergySystem energySystem;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();
			energySystem = base.GetComponent<EnergySystem>();


		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

		}

		public override void OnExit()
        {
            base.OnExit();

			base.skillLocator.DeductCooldownFromAllSkillsServer(dekucon.skillCDTimer);
			dekucon.skillCDTimer = 0f;
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}