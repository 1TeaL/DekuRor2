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

        public static float duration = Modules.StaticValues.dangersense45BuffTimer;
        public bool dangersense45;


        public override void OnEnter()
        {
            base.OnEnter();

            dangersense45 = true;

            //base.characterBody.SetAimTimer(duration);
            //this.muzzleString = "LFinger";

            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddBuff(Modules.Buffs.counterBuff);

            }

        }


        public override void OnExit()
        {
            base.OnExit();
            dangersense45 = false;
            bool active = NetworkServer.active;
            if (active && base.characterBody.HasBuff(Modules.Buffs.counterBuff))
            {
                base.characterBody.RemoveBuff(Modules.Buffs.counterBuff);
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