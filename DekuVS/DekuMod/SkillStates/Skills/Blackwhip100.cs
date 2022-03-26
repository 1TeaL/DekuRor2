using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;

namespace DekuMod.SkillStates
{
    public class Blackwhip100 : BaseSkillState
    {
        public float baseDuration = 0.5f;
        public static float blastRadius = 10f;
        public static float succForce = 4f;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ImpBossBlink");

        private float duration;
        private float maxWeight;
        private BlastAttack blastAttack;
        private bool hasFired;
        private bool hasFired2;
        private bool hasFired3;
        public Vector3 theSpot;
        public Vector3 thecloserSpot;
        public float whipage;


        public float speedattack;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;
            hasFired = false;
            hasFired2 = false;
            hasFired3 = false;
            speedattack = attackSpeedStat / 2;

            base.PlayAnimation("FullBody, Override", "Blackwhip", "Attack.playbackRate", baseDuration);
            //base.PlayCrossfade("Fullbody, Override", "Blackwhip", duration);

            GetMaxWeight();
            theSpot = aimRay.origin + 20 * aimRay.direction;
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
            blastAttack.radius = Blackwhip100.blastRadius * this.attackSpeedStat;
            blastAttack.procCoefficient = 1f;
            blastAttack.position = theSpot;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.blackwhip100DamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = -2f * maxWeight * Modules.StaticValues.blackwhipPull;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

            if (NetworkServer.active && base.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.1f;
                damageInfo.position = base.transform.position;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.Default;
                damageInfo.crit = false;
                damageInfo.attacker = null;
                damageInfo.inflictor = null;
                damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
                damageInfo.procCoefficient = 0f;
                damageInfo.procChainMask = default(ProcChainMask);
                base.healthComponent.TakeDamage(damageInfo);
            }

            //EffectData effectData = new EffectData();
            //effectData.origin = theSpot2;
            //effectData.scale = (blastRadius / 5) * this.attackSpeedStat;
            //effectData.rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z));

            //EffectManager.SpawnEffect(this.effectPrefab, effectData, false);

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

            if (base.fixedAge >= this.duration / 2 && base.isAuthority && !hasFired)
            {
                hasFired = true;
                theSpot = aimRay.origin + 20 * aimRay.direction;
                blastAttack.position = theSpot;
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

            if (base.fixedAge >= this.duration / 1.7 && base.isAuthority && !hasFired2)
            {
                hasFired2 = true;
                theSpot = aimRay.origin + 15 * aimRay.direction;
                blastAttack.position = theSpot;
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

            if (base.fixedAge >= this.duration / 1.5 && base.isAuthority && !hasFired3)
            {
                hasFired3 = true;
                theSpot = aimRay.origin + 10 * aimRay.direction;
                blastAttack.position = theSpot;
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
