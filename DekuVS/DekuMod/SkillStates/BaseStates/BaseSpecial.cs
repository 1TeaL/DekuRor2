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
        public enum superState { SUPER1, SUPER2, SUPER3 };
        public superState state;

        public override void OnEnter()
		{
			base.OnEnter();
            if (base.inputBank.moveVector == Vector3.zero)
            {
                //neutral attack
                NeutralSuper();
                state = superState.SUPER1;
                
            }
            else
            {
                Vector3 moveVector = base.inputBank.moveVector;
                Vector3 aimDirection = base.inputBank.aimDirection;
                Vector3 normalized = new Vector3(aimDirection.x, 0f, aimDirection.z).normalized;

                if (Vector3.Dot(base.inputBank.moveVector, normalized) >= 0.8f)
                {
                    //forward attack
                    ForwardSuper();
                    state = superState.SUPER3;
                }
                else if (Vector3.Dot(base.inputBank.moveVector, normalized) <= -0.8f)
                {
                    //backward attack
                    BackwardSuper();
                    state = superState.SUPER2;
                }

            }
        }

		public override void FixedUpdate()
		{
			base.FixedUpdate();

		}

        protected virtual void NeutralSuper()
        {

            if (characterBody.HasBuff(Buffs.fajinStoredBuff))
            {
                //free- spend the fajin stack
                characterBody.ApplyBuff(Buffs.fajinStoredBuff.buffIndex, characterBody.GetBuffCount(Buffs.fajinStoredBuff)-1);
            }
            else if (energySystem.currentPlusUltra > Modules.StaticValues.super1Cost)
            {
                energySystem.SpendPlusUltra(Modules.StaticValues.super1Cost);

            }
            else
            {
                //check how many segments missing
                float missingMeter = StaticValues.super1Cost - energySystem.currentPlusUltra;
                float barsMissing = Mathf.FloorToInt((float)missingMeter / StaticValues.barCostCheck);


                float healthCost = 0.1f;
                switch (barsMissing)
                {
                    case 1:
                        healthCost = StaticValues.plusUltraHealthCost;
                        break;
                    case 2:
                        healthCost = StaticValues.plusUltraHealthCost2;
                        break;
                    case 3:
                        healthCost = StaticValues.plusUltraHealthCost3;
                        break;
                }


                if (base.isAuthority)
                {
                    new SpendHealthNetworkRequest(base.characterBody.masterObjectId, characterBody.healthComponent.fullHealth * healthCost).Send(NetworkDestination.Clients);

                }
            }
        }
        protected virtual void BackwardSuper()
        {
            if (characterBody.HasBuff(Buffs.fajinStoredBuff))
            {
                //free- spend the fajin stack
                characterBody.ApplyBuff(Buffs.fajinStoredBuff.buffIndex, characterBody.GetBuffCount(Buffs.fajinStoredBuff) - 1);
            }
            else if (energySystem.currentPlusUltra > Modules.StaticValues.super2Cost)
            {
                energySystem.SpendPlusUltra(Modules.StaticValues.super2Cost);

            }
            else
            {
                //check how many segments missing
                float missingMeter = StaticValues.super2Cost - energySystem.currentPlusUltra;
                float barsMissing = Mathf.FloorToInt((float)missingMeter / StaticValues.barCostCheck);

                Chat.AddMessage("bars missing = " + barsMissing);

                float healthCost = 0.1f;
                switch (barsMissing)
                {
                    case 1:
                        healthCost = StaticValues.plusUltraHealthCost;
                        energySystem.SpendPlusUltra(Modules.StaticValues.super1Cost);
                        break;
                    case 2:
                        healthCost = StaticValues.plusUltraHealthCost2;
                        break;
                    case 3:
                        healthCost = StaticValues.plusUltraHealthCost3;
                        break;
                }


                if (base.isAuthority)
                {
                    new SpendHealthNetworkRequest(base.characterBody.masterObjectId, characterBody.healthComponent.fullHealth * healthCost).Send(NetworkDestination.Clients);

                }
            }
        }

        protected virtual void ForwardSuper()
        {
            if (characterBody.HasBuff(Buffs.fajinStoredBuff))
            {
                //free- spend the fajin stack
                characterBody.ApplyBuff(Buffs.fajinStoredBuff.buffIndex, characterBody.GetBuffCount(Buffs.fajinStoredBuff) - 1);
            }
            else if (energySystem.currentPlusUltra >= Modules.StaticValues.super3Cost)
            {
                energySystem.SpendPlusUltra(Modules.StaticValues.super3Cost);

            }
            else
            {
                //check how many segments missing
                float missingMeter = StaticValues.super3Cost - energySystem.currentPlusUltra;
                float barsMissing = Mathf.FloorToInt((float)missingMeter / StaticValues.barCostCheck);

                Chat.AddMessage("bars missing = " + barsMissing);

                float healthCost = 0.1f;
                switch (barsMissing)
                {
                    case 1:
                        healthCost = StaticValues.plusUltraHealthCost;
                        energySystem.SpendPlusUltra(Modules.StaticValues.super2Cost);
                        break;
                    case 2:
                        healthCost = StaticValues.plusUltraHealthCost2;
                        energySystem.SpendPlusUltra(Modules.StaticValues.super1Cost);
                        break;
                    case 3:
                        healthCost = StaticValues.plusUltraHealthCost3;
                        break;
                }


                if (base.isAuthority)
                {
                    new SpendHealthNetworkRequest(base.characterBody.masterObjectId, characterBody.healthComponent.fullHealth * healthCost).Send(NetworkDestination.Clients);

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