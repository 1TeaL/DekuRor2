using DekuMod.Modules.Networking;
using DekuMod.Modules;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using EntityStates;
using R2API.Networking;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using R2API.Networking.Interfaces;

namespace DekuMod.SkillStates
{

	public class BaseSpecial : BaseDekuSkillState
	{

		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

		}

        protected virtual void NeutralSuper()
        {
            if (energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
            {
                energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);

            }
            else
            {
                if (base.isAuthority)
                {
                    new SpendHealthNetworkRequest(base.characterBody.masterObjectId, StaticValues.plusUltraHealthCost).Send(NetworkDestination.Clients);

                }
            }
        }
        protected virtual void ForwardSuper()
        {
            if (energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
            {
                energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);

            }
            else
            {
                if (base.isAuthority)
                {
                    new SpendHealthNetworkRequest(base.characterBody.masterObjectId, StaticValues.plusUltraHealthCost).Send(NetworkDestination.Clients);

                }
            }
        }
        protected virtual void BackwardSuper()
        {
            if (energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
            {
                energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);

            }
            else
            {
                if (base.isAuthority)
                {
                    new SpendHealthNetworkRequest(base.characterBody.masterObjectId, StaticValues.plusUltraHealthCost).Send(NetworkDestination.Clients);

                }
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