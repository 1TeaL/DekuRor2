using RoR2;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using EntityStates;
using DekuMod.Modules.Survivors;
using System.Collections.Generic;
using R2API.Networking;

namespace DekuMod.SkillStates
{
    public class Manchester : BaseSkill
    {
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        private GameObject effectPrefab2 = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        public float baseDuration = 1f;
        public float fireTime;
        //private GameObject effectPrefab = Modules.Assets.sEffect;

        public static float blastRadius = 6f;
        public float distance = 7f;
        public float maxWeight;
        public float force = 5f;
        private float duration;
        private BlastAttack blastAttack;
        private bool hasFired;
        public Vector3 theSpot;

        protected DamageType damageType = DamageType.Stun1s;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;
            fireTime = duration / 2f;


            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            float num2 = (num / base.characterBody.baseMoveSpeed - 1f) * 0.67f;
            float num3 = num2 + 1f;

            hasFired = false;
            theSpot = base.transform.position;
            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("shootstyedashvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("shootstyedashsfx", this.gameObject);
            base.StartAimMode(duration, true);


            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayCrossfade("FullBody, Override", "ManchesterFlip", "Attack.playbackRate", fireTime, 0.01f);

            EffectManager.SimpleMuzzleFlash(Modules.Assets.dekuKickEffect, base.gameObject, "DownSwing", true);

            //move up a little
            base.characterMotor.velocity += Vector3.up * distance;
            base.characterMotor.Motor.ForceUnground();
            //get weight, blast attack after
            GetMaxWeight();

            blastAttack = new BlastAttack();
            blastAttack.radius = blastRadius;
            blastAttack.procCoefficient = 1f;
            blastAttack.position = theSpot;
            blastAttack.damageType = DamageType.Shock5s;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchesterDamageCoefficient * num3;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = force * maxWeight;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = damageType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

        
        }


        protected virtual void OnHitEnemyAuthority(BlastAttack.Result result)
        {
            AkSoundEngine.PostEvent("shootstyledashcombosfx", this.gameObject);
            foreach (BlastAttack.HitPoint hitpoint in result.hitPoints)
            {

                if (!hitpoint.hurtBox.healthComponent.body.HasBuff(Modules.Buffs.barrierMark.buffIndex))
                {
                    hitpoint.hurtBox.healthComponent.body.ApplyBuff(Modules.Buffs.barrierMark.buffIndex, 1, -1);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= fireTime && base.isAuthority && !hasFired)
            {
                hasFired = true;


                for (int i = 0; i <= 4; i += 1)
                {
                    Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * (blastRadius * 0.5f));
                    effectPosition.y = base.characterBody.footPosition.y;
                    EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, new EffectData
                    {
                        origin = effectPosition,
                        scale = blastRadius,
                    }, true);
                }
                BlastAttack.Result result = blastAttack.Fire();
                if (result.hitCount > 0)
                {
                    this.OnHitEnemyAuthority(result);
                    base.characterBody.ApplyBuff(Modules.Buffs.manchesterBuff.buffIndex, 1, result.hitCount + 1);
                }
            }

            if ((base.fixedAge >= this.duration && base.isAuthority))
            {
                this.outer.SetNextStateToMain();
                return;
            }


        }

        public void GetMaxWeight()
        {
            Ray aimRay = base.GetAimRay();
            theSpot = base.transform.position;
            BullseyeSearch search = new BullseyeSearch
            {

                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = theSpot,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = blastRadius,
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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}