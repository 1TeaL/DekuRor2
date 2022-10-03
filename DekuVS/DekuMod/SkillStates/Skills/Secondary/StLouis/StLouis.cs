using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Survivors;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using DekuMod.Modules.Networking;

namespace DekuMod.SkillStates
{
    public class StLouis : BaseSkill
    {
        public bool hasTeleported;
        public bool hasFired;
        public float baseDuration = 1f;
        public float duration;
        public float fireTime;
        private DamageType damageType;

        private float radius = 10f;
        private float damageCoefficient = Modules.StaticValues.detroitDamageCoefficient;
        private float procCoefficient = 1f;
        private float force = 1000f;

        private BlastAttack blastAttack;

        //Indicator
        public HurtBox Target;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / base.attackSpeedStat;
            this.fireTime = this.duration / 3f;
            hasFired = false;
            hasTeleported = false;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            if (!Target)
            {
                return;
            }

            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("stlouisvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("stlouissfx", this.gameObject);

        }


        public override void OnExit()
        {
            base.OnExit();
            this.PlayAnimation("Fullbody, Override", "BufferEmpty");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Target)
            {
                if (!hasTeleported)
                {
                    hasTeleported = true;
                    base.characterMotor.velocity = Vector3.zero;
                    base.characterMotor.Motor.SetPositionAndRotation(Target.healthComponent.body.transform.position + Vector3.up, Target.healthComponent.body.transform.rotation, true);
                    //new PerformDetroitTeleportNetworkRequest(base.characterBody.masterObjectId, Target.gameObject).Send(NetworkDestination.Clients);

                }

                if (base.fixedAge > this.fireTime && !hasFired && base.isAuthority)
                {
                    hasFired = true;
                    PlayCrossfade("FullBody, Override", "StLouis", "Attack.playbackRate", duration - fireTime, 0.01f);
                    new PerformStLouisSmashNetworkRequest(base.characterBody.masterObjectId, Target.healthComponent.body.masterObjectId).Send(NetworkDestination.Clients);

                    blastAttack = new BlastAttack();
                    blastAttack.radius = 10f;
                    blastAttack.procCoefficient = 1f;
                    blastAttack.position = base.transform.position;
                    blastAttack.damageType = DamageType.Stun1s;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = base.RollCrit();
                    blastAttack.baseDamage = base.damageStat * Modules.StaticValues.detroitDamageCoefficient;
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    //blastAttack.baseForce = 10f * Weight;
                    blastAttack.bonusForce = GetAimRay().direction * 100f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    

                    blastAttack.Fire();
                }

                if ((base.fixedAge >= this.duration && base.isAuthority))
                {
                    this.outer.SetNextStateToMain();
                    return;
                }

            }
            else
            {
                base.skillLocator.utility.AddOneStock();
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
