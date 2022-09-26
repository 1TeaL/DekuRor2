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
        public float fireInterval;
        public float timer;

        public static float blastRadius = 15f;
        public static float succForce = 5f;

        public Vector3 theSpot;
        public float whipage;


        public float speedattack;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;
            fireTime = duration / 2f;
            fireInterval = fireTime / 2f;
            timer = 0f;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayAnimation("FullBody, Override", "Blackwhip", "Attack.playbackRate", baseDuration);
            //base.PlayCrossfade("Fullbody, Override", "Blackwhip", duration);

            theSpot = aimRay.origin + 0.5f * attackSpeedStat * blastRadius * aimRay.direction;
            AkSoundEngine.PostEvent(3709822086, this.gameObject);
            AkSoundEngine.PostEvent(3062535197, this.gameObject);
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
                new SpendHealthNetworkRequest(characterBody.masterObjectId, 0.1f * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
            }

            //pull
            if (NetworkServer.active)
            {
                Collider[] array = Physics.OverlapSphere(theSpot, blastRadius * attackSpeedStat, LayerIndex.defaultLayer.mask);
                for (int i = 0; i < array.Length; i++)
                {
                    HealthComponent healthComponent = array[i].GetComponent<HealthComponent>();
                    if (healthComponent)
                    {
                        TeamComponent component2 = healthComponent.GetComponent<TeamComponent>();
                        if (component2.teamIndex != TeamIndex.Player)
                        {
                            var charb = healthComponent.body;
                            if (charb)
                            {
                                Vector3 pushForce = (theSpot - charb.corePosition) * succForce;
                                var motor = charb.GetComponent<CharacterMotor>();
                                var rb = charb.GetComponent<Rigidbody>();

                                float mass = 1;
                                if (motor) mass = motor.mass;
                                else if (rb) mass = rb.mass;
                                if (mass < 100) mass = 100;

                                pushForce *= mass;

                                DamageInfo info = new DamageInfo
                                {
                                    attacker = base.gameObject,
                                    inflictor = base.gameObject,
                                    damage = 0,
                                    damageColorIndex = DamageColorIndex.Default,
                                    damageType = DamageType.Generic,
                                    crit = false,
                                    dotIndex = DotController.DotIndex.None,
                                    force = pushForce,
                                    position = base.transform.position,
                                    procChainMask = default(ProcChainMask),
                                    procCoefficient = 0
                                };

                                charb.healthComponent.TakeDamageForce(info, true, true);
                            }
                        }
                    }
                }
            }

        }

        protected virtual void OnHitEnemyAuthority()
        {
            //base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 30) * this.attackSpeedStat);

        }


        public override void OnExit()
        {

            //base.PlayAnimation("RightArm, Override", "SmashCharge", "this.duration", 0.2f);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime;

            if (timer >= fireTime && base.isAuthority)
            {
                timer -= fireInterval;
                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();
                }
                EffectManager.SpawnEffect(Modules.Assets.blackwhip, new EffectData
                {
                    origin = theSpot,
                    scale = 1f,

                }, true);
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
