﻿using RoR2;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using EntityStates;

namespace DekuMod.SkillStates
{
    public class Manchester : BaseSkillState
    {
        //public static float basejumpDuration = 1f;
        public static float jumpDuration= 1f;
        public static float dropForce = 80f;

        public static float slamRadius = 15f;
        public static float slamProcCoefficient = 1f;
        public static float slamForce = 1000f;

        private bool hasDropped;
        private Vector3 flyVector = Vector3.zero;
        private Transform modelTransform;
        private Transform slamIndicatorInstance;
        private Transform slamCenterIndicatorInstance;
        private Ray downRay;


        //private NemforcerGrabController grabController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.modelTransform = base.GetModelTransform();
            this.flyVector = Vector3.up;
            this.hasDropped = false;
            //jumpDuration = basejumpDuration /(this.attackSpeedStat/2);


            base.PlayAnimation("FullBody, Override", "ManchesterBegin", "Attack.playbackRate", Manchester.jumpDuration);
            AkSoundEngine.PostEvent(687990298, this.gameObject);
            AkSoundEngine.PostEvent(1918362945, this.gameObject);

            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            

            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();

        }

        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
        }
        protected virtual void OnHitEnemyAuthority()
        {
            base.healthComponent.AddBarrierAuthority(Modules.StaticValues.manchesterDamageCoefficient * this.damageStat * (this.moveSpeedStat/14));

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!this.hasDropped)
            {
                base.characterMotor.rootMotion += this.flyVector * ((0.6f * this.moveSpeedStat) * EntityStates.Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / Manchester.jumpDuration) * Time.fixedDeltaTime);
                base.characterMotor.velocity.y = 0f;
            }

            if (base.fixedAge >= (0.25f * Manchester.jumpDuration) && !this.slamIndicatorInstance)
            {
                this.CreateIndicator();
            }

            if (base.fixedAge >= Manchester.jumpDuration && !this.hasDropped)
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
            base.characterMotor.velocity.y = -Manchester.dropForce;

            base.PlayAnimation("Fullbody, Override", "ManchesterSmash", "Attack.playbackRate", jumpDuration/3);
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            }
            //this.AttemptGrab(10f);
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
                this.slamIndicatorInstance.localScale = Vector3.one * Manchester.slamRadius;

                this.slamCenterIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab).transform;
                this.slamCenterIndicatorInstance.localScale = (Vector3.one * Manchester.slamRadius) / 3f;
            }
        }

        private void LandingImpact()
        {
            //if (this.grabController) this.grabController.Release();

            //base.PlayCrossfade("Fullbody, Override", "ManchesterSmashExit", 0.1f);
            if (base.isAuthority)
            {

                base.characterMotor.velocity *= 0.1f;

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = Manchester.slamRadius;
                blastAttack.procCoefficient = Manchester.slamProcCoefficient;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchesterDamageCoefficient * (moveSpeedStat/7);
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = Manchester.slamForce;
                blastAttack.teamIndex = base.teamComponent.teamIndex; 
                blastAttack.damageType = DamageType.Stun1s;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHit;

                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();
                }


                //AkSoundEngine.SetRTPCValue("M2_Charge", 100f);
                //Util.PlaySound(EnforcerPlugin.Sounds.NemesisSmash, base.gameObject);

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

            //if (this.grabController) this.grabController.Release();

            if (this.slamIndicatorInstance) EntityState.Destroy(this.slamIndicatorInstance.gameObject);
            if (this.slamCenterIndicatorInstance) EntityState.Destroy(this.slamCenterIndicatorInstance.gameObject);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");


            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;


            if (NetworkServer.active && base.characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility)) base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);

            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
            base.OnExit();
        }

        //private void AttemptGrab(float grabRadius)
        //{
        //    if (this.grabController) return;

        //    Ray aimRay = base.GetAimRay();

        //    BullseyeSearch search = new BullseyeSearch
        //    {
        //        teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
        //        filterByLoS = false,
        //        searchOrigin = base.transform.position,
        //        searchDirection = Random.onUnitSphere,
        //        sortMode = BullseyeSearch.SortMode.Distance,
        //        maxDistanceFilter = grabRadius,
        //        maxAngleFilter = 360f
        //    };

        //    search.RefreshCandidates();
        //    search.FilterOutGameObject(base.gameObject);

        //    HurtBox target = search.GetResults().FirstOrDefault<HurtBox>();
        //    if (target)
        //    {
        //        if (target.healthComponent && target.healthComponent.body)
        //        {
        //            if (BodyMeetsGrabConditions(target.healthComponent.body))
        //            {
        //                this.grabController = target.healthComponent.body.gameObject.AddComponent<NemforcerGrabController>();
        //                this.grabController.pivotTransform = this.FindModelChild("HandL");
        //            }

        //            if (NetworkServer.active)
        //            {
        //                base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
        //            }
        //        }
        //    }
        //}

        //private bool BodyMeetsGrabConditions(CharacterBody targetBody)
        //{
        //    bool meetsConditions = true;

        //    //if (targetBody.hullClassification == HullClassification.BeetleQueen) meetsConditions = false;

        //    return meetsConditions;
        //}

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}