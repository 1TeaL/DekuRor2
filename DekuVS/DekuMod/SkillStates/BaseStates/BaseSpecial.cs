using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class BaseSpecial : BaseSkillState
	{
		public DekuController dekucon;
		public EnergySystem energySystem;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();
			energySystem = base.GetComponent<EnergySystem>();
			if(energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
			{
				DoSkill();
				energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);
            }
            else
			{
				energySystem.TriggerGlow(0.3f, 0.3f, Color.black);
				this.outer.SetNextStateToMain();
			}


		}

		protected virtual void DoSkill()
		{

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