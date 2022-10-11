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
			if(energySystem.currentPlusUltra > Modules.StaticValues.modePlusUltraSpend)
			{
				DoSkill();
				energySystem.SpendPlusUltra(Modules.StaticValues.modePlusUltraSpend);
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
				Chat.AddMessage($"You need {Modules.StaticValues.modePlusUltraSpend} plus ultra.");
				energySystem.TriggerGlow(0.3f, 0.3f, Color.black);
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
			base.skillLocator.DeductCooldownFromAllSkillsServer(dekucon.skillCDTimer);
			dekucon.skillCDTimer = 0f;
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}