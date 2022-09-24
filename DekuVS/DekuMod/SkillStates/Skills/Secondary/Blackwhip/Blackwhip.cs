using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Survivors;
using R2API.Networking;
using R2API.Networking.Interfaces;
using DekuMod.Modules.Networking;

namespace DekuMod.SkillStates
{
    public class Blackwhip : BaseSkill
    {
        public HurtBox Target;
        public float maxTrackingDistance = 100f;
        public float maxTrackingAngle = 30f;
        public float pullRange = 50f;
        private ChildLocator child;
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        public float chargeTime = 0.25f;
        public float castTime = 0.25f;
        public float duration;
        public bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();


            hasFired = false;

            duration = chargeTime + castTime;
            base.StartAimMode(0.5f + this.duration, false);

            AkSoundEngine.PostEvent(3709822086, this.gameObject);
            AkSoundEngine.PostEvent(3062535197, this.gameObject);
            //animate blackwhip ready stance
            base.PlayAnimation("FullBody, Override", "Blackwhip", "Attack.playbackRate", chargeTime);

        }


        public override void OnExit()
        {
            base.OnExit();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge > chargeTime && base.isAuthority)
            {
                if (!hasFired)
                {
                    hasFired = true;
                    //animate blackwhip pulling
                    base.PlayCrossfade("FullBody, Override", "Blackwhip", "Attack.playbackRate", castTime, 0.05f);

                }

                if (base.fixedAge > duration && base.isAuthority)
                {
                    new PerformForceNetworkRequest(base.characterBody.masterObjectId, base.GetAimRay().origin - GetAimRay().direction, base.GetAimRay().direction, pullRange).Send(NetworkDestination.Clients);
                    
                    this.outer.SetNextStateToMain();
                    return;
                }


            }


        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
