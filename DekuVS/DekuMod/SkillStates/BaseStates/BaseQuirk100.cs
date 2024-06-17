using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class BaseQuirk100 : BaseSkillState
	{
		public DekuController dekucon;
		public EnergySystem energySystem;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();
			energySystem = base.GetComponent<EnergySystem>();
			if (energySystem.currentPlusUltra > Modules.StaticValues.skill100PlusUltraSpend)
			{
				DoSkill();
				energySystem.SpendPlusUltra(Modules.StaticValues.skill100PlusUltraSpend);
			}
			else
			{
				DontDoSkill();
			}


		}

		protected virtual void DoSkill()
        {

		}
		protected virtual void DontDoSkill()
		{
			if (base.isAuthority)
			{
				Chat.AddMessage($"You need {Modules.StaticValues.skill100PlusUltraSpend} plus ultra.");
				this.outer.SetNextStateToMain();
				return;

			}
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