using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.Modules;
using DekuMod.SkillStates.BaseStates;
using R2API.Networking;

namespace DekuMod.SkillStates.BlackWhip
{
    public class BlackwhipOverlay : BaseDekuSkillState
    {

        public Animator anim;

        private float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();
            anim = base.GetModelAnimator();
            GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
            PlayCrossfade("FullBody, Override", "DelawareSmashBaseCharge", "Attack.playbackRate", 1f, 0.01f);

            characterBody.ApplyBuff(Buffs.overlayBuff.buffIndex, 1, GetBuffCount(Buffs.overlayBuff) + StaticValues.blackwhipOverlayDuration);


        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(fixedAge > duration)
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