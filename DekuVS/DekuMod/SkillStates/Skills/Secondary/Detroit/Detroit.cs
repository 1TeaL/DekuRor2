using DekuMod.Modules.Survivors;
using EntityStates;
using EntityStates.Huntress;
using EntityStates.VagrantMonster;
using RoR2;
using RoR2.Audio;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class Detroit : BaseSkill
    {
        public float duration = 1f;
        protected Animator animator;

        protected DamageType damageType;

        public override void OnEnter()
        {
            base.OnEnter();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayAnimation("RightArm, Override", "SmashCharge", "Attack.playbackRate", 1f);
            AkSoundEngine.PostEvent(3806074874, this.gameObject);

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        public override void OnExit()
        {


            base.OnExit();
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}
