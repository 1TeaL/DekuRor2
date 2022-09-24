using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class BaseQuirk : BaseSkillState
	{
		public DekuController dekucon;
		public EnergySystem energySystem;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();
			energySystem = base.GetComponent<EnergySystem>();
			energySystem.currentPlusUltra -= Modules.StaticValues.skillPlusUltraSpend;


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