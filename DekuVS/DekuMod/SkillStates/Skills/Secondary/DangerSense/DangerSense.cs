using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using EntityStates;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class DangerSense : BaseQuirk
    {

        public static float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();


        }

        protected override void DoSkill()
        {
            base.DoSkill();
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddTimedBuffAuthority(Modules.Buffs.dangersenseBuff.buffIndex, Modules.StaticValues.dangersenseBuffTimer);

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