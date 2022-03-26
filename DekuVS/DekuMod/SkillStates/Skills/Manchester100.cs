using RoR2;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using EntityStates;
using DekuMod.Modules.Survivors;
using System.Collections.Generic;

namespace DekuMod.SkillStates
{
    public class Manchester100 : BaseSkillState
    {
        public static float basejumpDuration = 0.5f;
        public static float jumpDuration;
        public static float dropForce = 80f;

        public static float baseRadius = 15f;
        public static float slamRadius;
        public static float slamProcCoefficient = 1f;
        public static float slamForce = 1000f;

        private bool hasDropped;
        private Vector3 flyVector = Vector3.zero;
        private Transform modelTransform;
        private Transform slamIndicatorInstance;
        private Transform slamCenterIndicatorInstance;
        private Ray downRay;

        protected DamageType damageType = DamageType.Stun1s;
        public DekuController dekucon;
        private float maxWeight;

        //private NemforcerGrabController grabController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.modelTransform = base.GetModelTransform();
            this.flyVector = Vector3.up;
            this.hasDropped = false;
            slamRadius = baseRadius * attackSpeedStat;
            GetMaxWeight();
            dekucon = base.GetComponent<DekuController>();
            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = Manchester100.slamRadius;
                blastAttack.procCoefficient = Manchester100.slamProcCoefficient;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchesterDamageCoefficient * (moveSpeedStat / 7);
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = -2 * maxWeight;
                blastAttack.teamIndex = base.teamComponent.teamIndex;
                blastAttack.damageType = damageType;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();

                }
            }

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

            jumpDuration = basejumpDuration;


            base.PlayAnimation("FullBody, Override", "ManchesterBegin", "Attack.playbackRate", Manchester100.jumpDuration);
            AkSoundEngine.PostEvent(687990298, this.gameObject);
            AkSoundEngine.PostEvent(1918362945, this.gameObject);

            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            

            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();


            
        }

        public void GetMaxWeight()
        {
            Ray aimRay = base.GetAimRay();
            BullseyeSearch search = new BullseyeSearch
            {

                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = base.transform.position,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = slamRadius,
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
        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
        }
        protected virtual void OnHitEnemyAuthority()
        {
            base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 20) * (this.moveSpeedStat/7));

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!this.hasDropped)
            {
                base.characterMotor.rootMotion += this.flyVector * ((1f * this.moveSpeedStat) * EntityStates.Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / Manchester100.jumpDuration) * Time.fixedDeltaTime);
                base.characterMotor.velocity.y = 0f;
            }

            if (base.fixedAge >= (0.25f * Manchester100.jumpDuration) && !this.slamIndicatorInstance)
            {
                this.CreateIndicator();
            }

            if (base.fixedAge >= Manchester100.jumpDuration && !this.hasDropped)
            {
                this.StartDrop();
            }

            if (this.hasDropped && base.isAuthority && !base.characterMotor.disableAirControlUntilCollision)
            {
                this.LandingImpact();
                this.outer.SetNextStateToMain();
            }
        }

        private void StartDrop()
        {
            this.hasDropped = true;

            base.characterMotor.disableAirControlUntilCollision = true;
            base.characterMotor.velocity.y = -Manchester100.dropForce;

            base.PlayAnimation("Fullbody, Override", "ManchesterSmash", "Attack.playbackRate", jumpDuration/3f);
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            }
        }

        private void CreateIndicator()
        {
            if (EntityStates.Huntress.ArrowRain.areaIndicatorPrefab)
            {
                this.downRay = new Ray
                {
                    direction = Vector3.down,
                    origin = base.transform.position
                };

                this.slamIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab).transform;
                this.slamIndicatorInstance.localScale = Vector3.one * Manchester100.slamRadius;

                this.slamCenterIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab).transform;
                this.slamCenterIndicatorInstance.localScale = (Vector3.one * Manchester100.slamRadius) / 3f;
            }
        }

        private void LandingImpact()
        {

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                if (dekucon.isMaxPower)
                {
                    EffectManager.SpawnEffect(Modules.Assets.impactEffect, new EffectData
                    {
                        origin = base.transform.position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(aimRay.direction)
                    }, true);
                    damageType = DamageType.Stun1s;
                }
                else
                {
                    damageType = DamageType.Stun1s;
                }
                base.characterMotor.velocity *= 0.1f;

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = Manchester100.slamRadius;
                blastAttack.procCoefficient = Manchester100.slamProcCoefficient;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchesterDamageCoefficient * (moveSpeedStat/7f);
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = Manchester100.slamForce;
                blastAttack.teamIndex = base.teamComponent.teamIndex; 
                blastAttack.damageType = damageType;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();

                }



                for (int i = 0; i <= 8; i += 1)
                {
                    Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * 8f);
                    effectPosition.y = base.characterBody.footPosition.y;
                    EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, new EffectData
                    {
                        origin = effectPosition,
                        scale = slamRadius/6,
                    }, true);
                }
            }
        }

        private void UpdateSlamIndicator()
        {
            if (this.slamIndicatorInstance)
            {
                float maxDistance = 250f;

                this.downRay = new Ray
                {
                    direction = Vector3.down,
                    origin = base.transform.position
                };

                RaycastHit raycastHit;
                if (Physics.Raycast(this.downRay, out raycastHit, maxDistance, LayerIndex.world.mask))
                {
                    this.slamIndicatorInstance.transform.position = raycastHit.point;
                    this.slamIndicatorInstance.transform.up = raycastHit.normal;

                    this.slamCenterIndicatorInstance.transform.position = raycastHit.point;
                    this.slamCenterIndicatorInstance.transform.up = raycastHit.normal;
                }
            }
        }

        public override void OnExit()
        {

            if (this.slamIndicatorInstance) EntityState.Destroy(this.slamIndicatorInstance.gameObject);
            if (this.slamCenterIndicatorInstance) EntityState.Destroy(this.slamCenterIndicatorInstance.gameObject);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");


            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;


            if (NetworkServer.active && base.characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility)) base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);

            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
            base.OnExit();
        }

       

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}