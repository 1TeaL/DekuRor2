using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;

namespace DekuMod.SkillStates
{
    public class Blackwhip45 : BaseSkillState
    {
        public float baseDuration = 0.5f;
        public static float blastRadius = 15f;
        public static float succForce = 4f;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ImpBossBlink");

        private float duration;
        private float maxWeight;
        private BlastAttack blastAttack;
        //private bool hasFired;
        public Vector3 theSpot;
        public float whipage;


        public float speedattack;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;

            base.PlayAnimation("FullBody, Override", "Blackwhip", "Attack.playbackRate", baseDuration);
            //base.PlayCrossfade("Fullbody, Override", "Blackwhip", duration);
            speedattack = attackSpeedStat / 2;
            if (speedattack < 1)
            {
                speedattack = 1;
            }

            GetMaxWeight();
            theSpot = aimRay.origin + 5 * aimRay.direction;
            AkSoundEngine.PostEvent(3709822086, this.gameObject);
            AkSoundEngine.PostEvent(3062535197, this.gameObject);
            base.StartAimMode(duration, true);

            base.characterMotor.disableAirControlUntilCollision = false;


            //base.PlayCrossfade("Fullbody, Override", "Blackwhip", duration);

            EffectManager.SpawnEffect(Modules.Assets.blackwhipforward, new EffectData
            {
                origin = aimRay.origin,
                scale = 1f,
                rotation = Quaternion.LookRotation(aimRay.direction),

            }, false);


            blastAttack = new BlastAttack();
            blastAttack.radius = Blackwhip45.blastRadius * this.attackSpeedStat;
            blastAttack.procCoefficient = 0.5f;
            blastAttack.position = theSpot;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.blackwhip45DamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = -1.5f * maxWeight * Modules.StaticValues.blackwhipPull;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;


            //EffectData effectData = new EffectData();
            //effectData.origin = theSpot2;
            //effectData.scale = (blastRadius / 5) * this.attackSpeedStat;
            //effectData.rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z));

            //EffectManager.SpawnEffect(this.effectPrefab, effectData, false);

        }

        public void GetMaxWeight()
        {
            Ray aimRay = base.GetAimRay();
            theSpot = aimRay.origin + 5 * aimRay.direction;
            BullseyeSearch search = new BullseyeSearch
            {

                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = theSpot,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = blastRadius * speedattack,
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
            base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 30) * this.attackSpeedStat);

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
            theSpot = aimRay.origin + 5 * aimRay.direction;

            if ((base.fixedAge >= this.duration / 2) && base.isAuthority && whipage >= this.duration / 10)
            {
                //hasFired = true;
                blastAttack.position = theSpot;
                whipage = 0f;
                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();
                }
                //EffectManager.SpawnEffect(Modules.Assets.blackwhipforward, new EffectData
                //{
                //    origin = theSpot,
                //    scale = 1f,

                //}, false);
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
