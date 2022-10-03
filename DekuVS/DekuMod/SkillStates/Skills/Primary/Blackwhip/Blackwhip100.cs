using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Networking;
using R2API.Networking.Interfaces;
using R2API.Networking;

namespace DekuMod.SkillStates
{
    public class Blackwhip100 : BaseQuirk100
    {
        private BlastAttack blastAttack;

        public float baseDuration = 0.5f;
        private float duration;
        public float fireTime;
        public float timer;
        public bool hasFired;

        public static float blastRadius = 15f;
        public static float succForce = 5f;

        public Vector3 theSpot;
        public float whipage;


        public float speedattack;

        public override void OnEnter()
        {
            base.OnEnter();
            

        }

        protected override void DoSkill()
        {
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;
            fireTime = duration / 2f;
            timer = 0f;
            hasFired = false;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            //base.PlayCrossfade("Fullbody, Override", "Blackwhip", duration);

            theSpot = aimRay.origin + 0.5f * attackSpeedStat * blastRadius * aimRay.direction;
            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("blackwhipvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("blackwhipsfx", this.gameObject);
            base.StartAimMode(duration, true);

            base.characterMotor.disableAirControlUntilCollision = false;



            EffectManager.SpawnEffect(Modules.Assets.blackwhipforward, new EffectData
            {
                origin = aimRay.origin,
                scale = 1f,
                rotation = Quaternion.LookRotation(aimRay.direction),

            }, true);


            blastAttack = new BlastAttack();
            blastAttack.radius = blastRadius * this.attackSpeedStat;
            blastAttack.procCoefficient = 1f;
            blastAttack.position = theSpot;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.blackwhip100DamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 0f;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;


            if (base.isAuthority)
            {
                new SpendHealthNetworkRequest(characterBody.masterObjectId, Modules.StaticValues.blackwhip100HealthCostFraction * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
            }

        }


        public override void OnExit()
        {

            //base.PlayAnimation("RightArm, Override", "SmashCharge", "this.duration", 0.2f);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= fireTime && base.isAuthority && hasFired)
            {
                base.PlayCrossfade("FullBody, Override", "Blackwhip", "Attack.playbackRate", fireTime, 0.05f);
                hasFired = true;
                if (base.isAuthority)
                {
                    new PerformBlackwhip100NetworkRequest(base.characterBody.masterObjectId,
                        theSpot,
                        base.GetAimRay().direction,
                        Modules.StaticValues.blackwhip100DamageCoefficient).Send(NetworkDestination.Clients);

                    EffectManager.SpawnEffect(Modules.Assets.blackwhip, new EffectData
                    {
                        origin = theSpot,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(base.GetAimRay().direction),

                    }, true);
                }
            }

            
            if ((base.fixedAge >= this.duration && base.isAuthority))
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
