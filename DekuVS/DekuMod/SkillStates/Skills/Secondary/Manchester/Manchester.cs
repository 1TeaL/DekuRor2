using RoR2;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using EntityStates;
using DekuMod.Modules.Survivors;
using System.Collections.Generic;

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
        public float distance = 2f;
        public float maxWeight;
        public float force = 50f;
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

            hasFired = false;
            theSpot = base.transform.position;
            AkSoundEngine.PostEvent(3709822086, this.gameObject);
            AkSoundEngine.PostEvent(3062535197, this.gameObject);
            base.StartAimMode(duration, true);



            //base.PlayCrossfade("Fullbody, Override", "LegSmash", startUp);
            //base.PlayAnimation("Fullbody, Override" "LegSmash", "Attack.playbackRate", startUp);
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayCrossfade("Fullbody, Override", "ManchesterFlip", "Attack.playbackRate", fireTime, 0.01f);
            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuffAuthority(Modules.Buffs.manchesterBuff.buffIndex, Modules.StaticValues.manchesterBuffDuration);
            }

            //move up a little
            base.characterMotor.rootMotion += Vector3.up * distance;
            //get weight, blast attack after
            GetMaxWeight();

            blastAttack = new BlastAttack();
            blastAttack.radius = blastRadius;
            blastAttack.procCoefficient = 1f;
            blastAttack.position = theSpot;
            blastAttack.damageType = DamageType.Shock5s;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchesterDamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = force * maxWeight;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = damageType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

        
        }

        protected virtual void OnHitEnemyAuthority()
        {
            //base.healthComponent.Heal(((healthComponent.fullCombinedHealth / 20)), default(ProcChainMask), true);

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge < fireTime)
            {
                base.characterMotor.velocity.y = 0f;
            }

            if (base.fixedAge >= fireTime && base.isAuthority && !hasFired)
            {
                hasFired = true;

                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();
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