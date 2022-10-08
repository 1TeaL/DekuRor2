using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using EntityStates;
using EntityStates.Huntress;
using EntityStates.VagrantMonster;
using R2API.Networking;
using R2API.Networking.Interfaces;
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
    public class Detroit : BaseSkillState
    {
        public DekuController dekucon;
        public EnergySystem energySystem;

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
            PlayCrossfade("RightArm, Override", "DetroitCharge", "Attack.playbackRate", fireTime, 0.01f);
            dekucon = base.GetComponent<DekuController>();
            energySystem = base.GetComponent<EnergySystem>();
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            if(!Target)
            {
                return;
            }

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

                if (base.fixedAge > this.fireTime && !hasFired && base.isAuthority)
                {
                    energySystem.currentPlusUltra += Modules.StaticValues.skillPlusUltraGain;
                    

                    if (!hasTeleported)
                    {
                        hasTeleported = true;
                        base.characterMotor.velocity = Vector3.zero;
                        base.characterMotor.Motor.SetPositionAndRotation(Target.healthComponent.body.transform.position + Vector3.up, Target.healthComponent.body.transform.rotation, true);
                        //new PerformDetroitTeleportNetworkRequest(base.characterBody.masterObjectId, Target.gameObject).Send(NetworkDestination.Clients);
                        if (base.isAuthority)
                        {
                            AkSoundEngine.PostEvent("detroitexitvoice", this.gameObject);
                        }
                        AkSoundEngine.PostEvent("detroitexitsfx", this.gameObject);
                    }

                    hasFired = true;
                    new PerformDetroitSmashNetworkRequest(base.characterBody.masterObjectId, Target.healthComponent.body.masterObjectId).Send(NetworkDestination.Clients);

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
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                    if (Target.healthComponent.body.characterMotor.isGrounded)
                    {
                        PlayCrossfade("RightArm, Override", "DetroitSmashUp", "Attack.playbackRate", fireTime, 0.01f);
                        blastAttack.bonusForce = Vector3.up * 50f;
                        blastAttack.Fire();

                    }
                    if (!Target.healthComponent.body.characterMotor.isGrounded)
                    {
                        PlayCrossfade("RightArm, Override", "DetroitSmashDown", "Attack.playbackRate", fireTime, 0.01f);
                        blastAttack.bonusForce = Vector3.down * 50f;
                        blastAttack.Fire();
                    }

                    this.outer.SetNextStateToMain();
                    return;


                }

            }
            else
            {
                base.skillLocator.secondary.AddOneStock();
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
