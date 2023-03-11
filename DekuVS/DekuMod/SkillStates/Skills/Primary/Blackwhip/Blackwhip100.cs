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
using DekuMod.Modules;

namespace DekuMod.SkillStates
{
    public class Blackwhip100 : BaseQuirk100
    {
        public HurtBox Target;
        public float pullRange = 0f;
        private ChildLocator child;
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        public float chargeTime = 0.1f;
        public float castTime = 0.2f;
        public float duration = 0.3f;
        public bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            hasFired = false;
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

        }

        protected override void DoSkill()
        {
            base.StartAimMode(0.5f + this.duration, false);
            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("blackwhipvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("blackwhipsfx", this.gameObject);


            if (base.isAuthority)
            {
                new SpendHealthNetworkRequest(characterBody.masterObjectId, Modules.StaticValues.blackwhip100HealthCostFraction * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
            }
            //animate blackwhip full
            //base.PlayAnimation("RightArm, Override", "Blackwhip", "Attack.playbackRate", duration);
        }

        protected override void DontDoSkill()
        {
            base.DontDoSkill();
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
                    base.PlayCrossfade("RightArm, Override", "Blackwhip", "Attack.playbackRate", castTime, 0.01f);
                    new PerformBlackwhipPullNetworkRequest(base.characterBody.masterObjectId, base.GetAimRay().origin - GetAimRay().direction, base.GetAimRay().direction, pullRange).Send(NetworkDestination.Clients);

                    if (!dekucon.blackwhipActivated)
                    {
                        if (Target.healthComponent != null)
                        {
                            Target.healthComponent.body.ApplyBuff(Buffs.blackwhipDebuff.buffIndex, 1, 10);
                            dekucon.enemyBody = Target.healthComponent.body;
                            dekucon.blackwhipTimer = StaticValues.blackwhipAttachDuration;
                            dekucon.blackwhipActivated = true;
                            if(!characterBody.HasBuff(Buffs.blackwhipBuff.buffIndex))
                            {
                                characterBody.ApplyBuff(Buffs.blackwhipBuff.buffIndex, 1);
                            }
                        }
                    }
                    else if (dekucon.blackwhipActivated)
                    {
                        dekucon.blackwhipActivated = false;
                        if (characterBody.HasBuff(Buffs.blackwhipBuff.buffIndex))
                        {
                            characterBody.ApplyBuff(Buffs.blackwhipBuff.buffIndex, 0);
                        }
                    }
                }

                if (base.fixedAge > duration && base.isAuthority)
                {
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
