using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;

namespace DekuMod.SkillStates
{
    public class BlackwhipFront : BaseSkillState
    {
        public float baseDuration = 0.3f;
        public static float blastRadius = 15f;
        public static float succForce = 4.5f;
        private GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/ImpBossBlink");

        private float duration;
        private float maxWeight;


        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;

            AkSoundEngine.PostEvent(3709822086, this.gameObject);
            AkSoundEngine.PostEvent(3062535197, this.gameObject);
            //base.StartAimMode(0.2f, true);

            base.characterMotor.disableAirControlUntilCollision = false;


            base.PlayAnimation("RightArm, Override", "Blackwhip");
            

            if (base.isAuthority)
            {
                
                Vector3 theSpot = aimRay.origin + 20 * aimRay.direction;

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = BlackwhipFront.blastRadius * this.attackSpeedStat;
                blastAttack.procCoefficient = 1f;
                blastAttack.position = theSpot;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.blackwhipDamageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                blastAttack.baseForce = -maxWeight;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.damageType = DamageType.Stun1s;
                blastAttack.attackerFiltering = AttackerFiltering.Default;


                EffectData effectData = new EffectData();
                effectData.origin = theSpot;
                effectData.scale = (blastRadius / 5) * this.attackSpeedStat;

                EffectManager.SpawnEffect(this.effectPrefab, effectData, false);

                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();
                }

                //base.characterMotor.velocity = -80 * aimRay.direction;


                //succ
                //if (NetworkServer.active)
                //{
                //    Collider[] array = Physics.OverlapSphere(theSpot, BlackwhipFront.blastRadius, LayerIndex.defaultLayer.mask);
                //    for (int i = 0; i < array.Length; i++)
                //    {
                //        HealthComponent healthComponent = array[i].GetComponent<HealthComponent>();
                //        if (healthComponent)
                //        {
                //            TeamComponent component2 = healthComponent.GetComponent<TeamComponent>();
                //            if (component2.teamIndex != TeamIndex.Player)
                //            {
                //                var charb = healthComponent.body;
                //                if (charb)
                //                {
                //                    Vector3 pushForce = (theSpot - charb.corePosition) * BlackwhipFront.succForce;
                //                    var motor = charb.GetComponent<CharacterMotor>();
                //                    var rb = charb.GetComponent<Rigidbody>();

                //                    float mass = 1;
                //                    if (motor) mass = motor.mass;
                //                    else if (rb) mass = rb.mass;
                //                    if (mass < 100) mass = 100;

                //                    pushForce *= mass;

                //                    DamageInfo info = new DamageInfo
                //                    {
                //                        attacker = base.gameObject,
                //                        inflictor = base.gameObject,
                //                        damage = 0,
                //                        damageColorIndex = DamageColorIndex.Default,
                //                        damageType = DamageType.Generic,
                //                        crit = false,
                //                        dotIndex = DotController.DotIndex.None,
                //                        force = pushForce,
                //                        position = base.transform.position,
                //                        procChainMask = default(ProcChainMask),
                //                        procCoefficient = 0
                //                    };

                //                    charb.healthComponent.TakeDamageForce(info, true, true);
                //                }
                //            }
                //        }
                //    }
                //}
            }
        }

        public void GetMaxWeight()
        {
            Ray aimRay = base.GetAimRay(); 
            Vector3 theSpot = aimRay.origin + 20 * aimRay.direction;
            BullseyeSearch search = new BullseyeSearch
            {
                
                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = theSpot,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = blastRadius*this.attackSpeedStat,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);
            maxWeight = Modules.StaticValues.blackwhipPull;


            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget)
                {
                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        if (singularTarget.healthComponent.body.characterMotor)
                        {
                            if (singularTarget.healthComponent.body.characterMotor.mass > maxWeight)
                            {
                                maxWeight = singularTarget.healthComponent.body.characterMotor.mass;
                            }
                        }
                        else if (singularTarget.healthComponent.body.rigidbody)
                        {
                            if (singularTarget.healthComponent.body.rigidbody.mass > maxWeight)
                            {
                                maxWeight = singularTarget.healthComponent.body.rigidbody.mass;
                            }
                        }
                    }
                }
            }
        }
        protected virtual void OnHitEnemyAuthority()
        {
            base.healthComponent.AddBarrierAuthority(this.damageStat * this.attackSpeedStat);

        }




        public override void OnExit()
        {

            base.PlayAnimation("RightArm, Override", "SmashCharge", "this.duration", 0.2f);
            //if (NetworkServer.active)
            //{


            //    //no succ
            //    Ray aimRay = base.GetAimRay();
            //    Vector3 theSpot = aimRay.origin + 15 * aimRay.direction;

            //    Collider[] array = Physics.OverlapSphere(theSpot, BlackwhipFront.blastRadius + 8f, LayerIndex.defaultLayer.mask);
            //    for (int i = 0; i < array.Length; i++)
            //    {
            //        HealthComponent healthComponent = array[i].GetComponent<HealthComponent>();
            //        if (healthComponent)
            //        {
            //            TeamComponent component2 = healthComponent.GetComponent<TeamComponent>();
            //            if (component2.teamIndex != TeamIndex.Player)
            //            {
            //                var charb = healthComponent.body;
            //                if (charb)
            //                {
            //                    var motor = charb.characterMotor;
            //                    var rb = charb.rigidbody;

            //                    if (motor) motor.velocity *= 0.1f;
            //                    if (rb) rb.velocity *= 0.1f;
            //                }
            //            }
            //        }
            //    }
            //}    
            //base.characterMotor.velocity *= 0.1f;



            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
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
