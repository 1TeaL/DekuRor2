using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Survivors;

namespace DekuMod.SkillStates
{
    public class BlackwhipFront : BaseSkillState
    {
        public float baseDuration = 0.5f;
        public static float blastRadius = 15f;
        public static float succForce = 4.5f;
        //private GameObject effectPrefab = Modules.Assets.blackwhipEffect;

        private float duration;
        private float maxWeight;
        private BlastAttack blastAttack;
        //private bool hasFired;
        public Vector3 theSpot;
        public float whipage;
        public float speedattack;

        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;
        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;
            speedattack = attackSpeedStat / 2;

            base.PlayAnimation("FullBody, Override", "Blackwhip", "Attack.playbackRate", baseDuration);
            //base.PlayAnimation("RightArm, Override", "Blackwhip", "Attack.playbackRate", duration);
            //base.PlayCrossfade("Fullbody, Override", "Blackwhip", duration);

            if (speedattack < 1)
            {
                speedattack = 1;
            }
            dekucon = base.GetComponent<DekuController>();
            if (dekucon.isMaxPower)
            {
                fajin = 2f;
                dekucon.RemoveBuffCount(50);
            }
            else
            {
                fajin = 1f;
            }
            if (dekucon.isMaxPower)
            {
                EffectManager.SpawnEffect(Modules.Assets.impactEffect, new EffectData
                {
                    origin = base.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(aimRay.direction)
                }, false);
            }


            //hasFired = false;
            GetMaxWeight();
            theSpot = aimRay.origin + 20 * aimRay.direction;
            AkSoundEngine.PostEvent(3709822086, this.gameObject);
            AkSoundEngine.PostEvent(3062535197, this.gameObject);
            base.StartAimMode(duration, true);

            base.characterMotor.disableAirControlUntilCollision = false;



            EffectManager.SpawnEffect(Modules.Assets.blackwhip, new EffectData
            {
                origin = theSpot,
                scale = 1f,       

            }, true);


            blastAttack = new BlastAttack();
            blastAttack.radius = BlackwhipFront.blastRadius * speedattack * fajin;
            blastAttack.procCoefficient = 0.2f;
            blastAttack.position = theSpot;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.blackwhipDamageCoefficient * fajin;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = -maxWeight * Modules.StaticValues.blackwhipPull * fajin;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = damageType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;


            //EffectData effectData = new EffectData();
            //effectData.origin = theSpot2;
            //effectData.scale = (blastRadius / 5) * this.attackSpeedStat;
            //effectData.rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z));

            //EffectManager.SpawnEffect(this.effectPrefab, effectData, false);

            dekucon.AddToBuffCount(10);
        }

        public void GetMaxWeight()
        {
            Ray aimRay = base.GetAimRay(); 
            theSpot = aimRay.origin + 20 * aimRay.direction;
            BullseyeSearch search = new BullseyeSearch
            {
                
                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = theSpot,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = blastRadius*speedattack,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);

            

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
            base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 30) * this.attackSpeedStat * fajin);

        }




        public override void OnExit()
        {
            //base.PlayAnimation("RightArm, Override", "SmashCharge", "this.duration", 0.2f);
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Ray aimRay = base.GetAimRay();
            theSpot = aimRay.origin + 20 * aimRay.direction;

            if ((base.fixedAge >= this.duration / 2) && base.isAuthority && whipage >= this.duration/10)
            {
                //hasFired = true;
                if (dekucon.isMaxPower)
                {

                    blastAttack.damageType = DamageType.BypassArmor | DamageType.Stun1s;
                }
                else
                {
                    blastAttack.damageType = DamageType.Stun1s;
                }
                blastAttack.position = theSpot;
                whipage = 0f;
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
            else this.whipage += Time.fixedDeltaTime;


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
