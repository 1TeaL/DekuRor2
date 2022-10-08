using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using DekuMod.Modules.Survivors;
using RoR2.UI;

namespace DekuMod.SkillStates
{
    public class DelawareSmash : BaseSkill
    {
        public float baseDuration = 0.5f;
        public float duration;
        public HurtBox Target;
        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;
        public GameObject crosshairOverridePrefab = Modules.Assets.banditCrosshair;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.characterBody.SetAimTimer(this.duration);
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            PlayCrossfade("FullBody, Override", "DelawareSmashBaseCharge", "Attack.playbackRate", duration, 0.01f);


            //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", 1f);
            if (this.crosshairOverridePrefab)
            {
                this.crosshairOverrideRequest = CrosshairUtils.RequestOverrideForBody(base.characterBody, this.crosshairOverridePrefab, CrosshairUtils.OverridePriority.Skill);
            }
        }

        public override void OnExit()
        {
            CrosshairUtils.OverrideRequest overrideRequest = this.crosshairOverrideRequest;
            if (overrideRequest != null)
            {
                overrideRequest.Dispose();
            }
            base.OnExit();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.IsKeyDownAuthority())
            {
                PlayCrossfade("FullBody, Override", "DelawareSmashBaseCharge", "Attack.playbackRate", duration, 0.01f);
                //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", duration);
            }
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.duration);
            }
            if (base.fixedAge >= this.duration && base.isAuthority && !base.IsKeyDownAuthority())
            {
                this.outer.SetNextState(new DelawareSmashFire());
                return;
            }
        }




        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}