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
    public class GearShift : BaseQuirk
    {

        public static float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();
            duration /= attackSpeedStat;


        }

        protected override void DoSkill()
        {
            base.DoSkill();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayCrossfade("UpperBody, Override", "GearShift", "Attack.playbackRate", duration, 0.01f);
            bool active = NetworkServer.active;
            if (active)
            {
                float num = this.moveSpeedStat;
                bool isSprinting = base.characterBody.isSprinting;
                if (isSprinting)
                {
                    num /= base.characterBody.sprintingSpeedMultiplier;
                }

                base.characterBody.AddTimedBuffAuthority(Modules.Buffs.gearshiftBuff.buffIndex, Modules.StaticValues.gearshiftBuffTimer * num);

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