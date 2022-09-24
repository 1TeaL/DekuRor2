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
    public class DangerSense100 : BaseQuirk100
    {

        public static float duration = 0.5f;
        public bool dangersense;

        public override void OnEnter()
        {
            base.OnEnter();
            dangersense = true;

            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddTimedBuffAuthority(Modules.Buffs.counterBuff.buffIndex, Modules.StaticValues.dangersenseBuffTimer);

            }

        }


        public override void OnExit()
        {
            dangersense = false;
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