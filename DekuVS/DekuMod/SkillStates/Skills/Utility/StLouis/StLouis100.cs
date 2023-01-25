using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Survivors;
using static RoR2.BlastAttack;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates.Huntress;

namespace DekuMod.SkillStates
{
    public class StLouis100 : BaseSkill100
    {
        public float baseDuration = 1f;
        public float fireTime;

        public static float blastRadius = 10f;
        public float distance = 5f;
        public float maxWeight;
        public float force = 20f;
        private float duration;
        private BlastAttack blastAttack;
        private bool hasFired;
        public Vector3 theSpot;


        //teleporting up
        private GameObject aimSphere;
        public float radius = 3f;
        private Ray aimRay;
        private float baseDistance = 5f;
        private float maxDistance;

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

            this.aimSphere = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);

            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            float num2 = (num / base.characterBody.baseMoveSpeed - 1f) * 0.67f;
            float num3 = num2 + 1f;

            maxDistance = baseDistance * moveSpeedStat;
            if (maxDistance > 100f)
            {
                maxDistance = 100f;
            }

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            PlayCrossfade("FullBody, Override", "StLouis100Charge", "Attack.playbackRate",fireTime, 0.01f);

            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("stlouisvoice", this.gameObject);
            }
            //EffectManager.SpawnEffect(Modules.Assets.blackwhip, new EffectData
            //{
            //    origin = theSpot,
            //    scale = 1f,       

            //}, true);


            //get weight, teleport after
            GetMaxWeight();

            blastAttack = new BlastAttack();
            blastAttack.radius = blastRadius * num3;
            blastAttack.procCoefficient = 1f;
            blastAttack.position = theSpot;
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.stlouis100DamageCoefficient * num3;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = force * maxWeight;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

            //base.characterMotor.Motor.SetPositionAndRotation(characterBody.transform.position + Vector3.up * distance * moveSpeedStat, Util.QuaternionSafeLookRotation(aimRay.direction), true);


            if (base.isAuthority)
            {
                new SpendHealthNetworkRequest(characterBody.masterObjectId, Modules.StaticValues.stlouis100HealthCostFraction * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
            }
        }

        protected virtual void OnHitEnemyAuthority()
        {
            //base.healthComponent.Heal(((healthComponent.fullCombinedHealth / 20)), default(ProcChainMask), true);

        }

        public override void OnExit()
        {
            base.OnExit();
            EntityState.Destroy(this.aimSphere.gameObject);
        }
        public override void Update()
        {
            base.Update();
            this.UpdateAreaIndicator();
        }
        private void UpdateAreaIndicator()
        {
            bool isAuthority = base.isAuthority;
            bool flag = isAuthority;
            if (flag)
            {
                this.aimSphere.transform.localScale = new Vector3(this.radius, this.radius, this.radius);
            }
            this.aimRay = base.GetAimRay();
            RaycastHit raycastHit;
            //bool flag2 = Physics.Raycast(base.GetAimRay(), out raycastHit, this.maxDistance, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
            bool flag2 = Physics.Raycast(aimRay.origin, Vector3.up, out raycastHit, this.maxDistance, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
            bool flag3 = flag2;
            if (flag3)
            {
                this.aimSphere.transform.position = raycastHit.point + Vector3.up;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = -this.aimRay.direction;
            }
            else
            {
                Vector3 position = aimRay.origin + this.maxDistance * Vector3.up;
                this.aimSphere.transform.position = position;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = -this.aimRay.direction;
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.IsKeyDownAuthority())
            {
                PlayCrossfade("FullBody, Override", "StLouis100Charge", "Attack.playbackRate", fireTime, 0.01f);
            }

            if (base.fixedAge >= fireTime && base.isAuthority && !base.IsKeyDownAuthority())
            {

                PlayCrossfade("FullBody, Override", "StLouis100Smash", "Attack.playbackRate", duration - fireTime, 0.01f);
                base.characterMotor.rootMotion += this.aimSphere.transform.position - base.characterBody.corePosition;
                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();
                }
                EffectManager.SimpleMuzzleFlash(Modules.Assets.dekuKickEffect, base.gameObject, "Swing1", true);
                EffectManager.SpawnEffect(Modules.Assets.mageLightningBombEffectPrefab, new EffectData
                {
                    origin = theSpot,
                    scale = blastRadius,
                    rotation = Util.QuaternionSafeLookRotation(Vector3.down)

                }, true);
                EffectManager.SpawnEffect(Modules.Assets.detroitEffect, new EffectData
                {
                    origin = theSpot,
                    scale = blastRadius,
                    rotation = Util.QuaternionSafeLookRotation(Vector3.down)

                }, true);
                for (int i = 0; i <= 10; i++)
                {
                    float num = 60f;
                    Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
                    float num2 = 0.01f;
                    rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                    rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
                    EffectManager.SpawnEffect(Modules.Assets.sonicboomEffectPrefab, new EffectData
                    {
                        origin = theSpot,
                        scale = blastRadius,
                        rotation = rotation
                    }, false);

                }

                AkSoundEngine.PostEvent("stlouisexitsfx", this.gameObject);
                
                this.outer.SetNextStateToMain();
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
