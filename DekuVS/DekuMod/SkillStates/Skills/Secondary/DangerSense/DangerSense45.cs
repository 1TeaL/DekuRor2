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
    public class DangerSense45 : BaseQuirk45
    {

        public static float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();

            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddTimedBuffAuthority(Modules.Buffs.dangersenseBuff.buffIndex, Modules.StaticValues.dangersense45BuffTimer);

            }

        }


        public override void OnExit()
        {
            base.OnExit();
            bool active = NetworkServer.active;
            if (active && base.characterBody.HasBuff(Modules.Buffs.dangersenseBuff))
            {
                base.characterBody.RemoveBuff(Modules.Buffs.dangersenseBuff);
            }
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
            return InterruptPriority.Frozen;
        }
    }
}