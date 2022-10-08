using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using EntityStates;
using ExtraSkillSlots;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class DangerSense100 : BaseQuirk100
    {

        public static float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();

        }

        protected override void DoSkill()
        {
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddTimedBuffAuthority(Modules.Buffs.dangersense100Buff.buffIndex, Modules.StaticValues.dangersense100BuffTimer);

            }

            if (base.isAuthority)
            {
                new SpendHealthNetworkRequest(characterBody.masterObjectId, Modules.StaticValues.dangersense100HealthCostFraction * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
            }

        }

        protected override void DontDoSkill()
        {
            base.DontDoSkill();
            skillLocator.secondary.AddOneStock();
        }
        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}