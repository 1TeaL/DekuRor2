using RoR2;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using EntityStates;
using DekuMod.Modules.Survivors;
using System.Collections.Generic;
using EntityStates.Huntress;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace DekuMod.SkillStates
{
    public class Manchester100 : BaseSkill100
    {
        public static float dropForce = 80f;
        public float dropTimer;
        public static float slamRadius = 8f;
        public static float slamProcCoefficient = 1f;
        public static float slamForce = 1000f;
        private float damageCoefficient = Modules.StaticValues.manchester100DamageCoefficient;

        private bool hasDropped;
        private Vector3 flyVector = Vector3.zero;
        private Transform modelTransform;
        private GameObject slamIndicatorInstance;
        private Ray downRay;

        protected DamageType damageType = DamageType.Generic;
        private Vector3 theSpot;

        //private NemforcerGrabController grabController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.modelTransform = base.GetModelTransform();
            this.flyVector = Vector3.up;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayCrossfade("FullBody, Override", "ManchesterFlip", "Attack.playbackRate", 0.5f, 0.01f);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.dekuKickEffect, base.gameObject, "DownSwing", true);

            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("manchester", this.gameObject);
            }

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
            if (NetworkServer.active)
            {
                base.characterBody.AddBuff(Modules.Buffs.manchesterBuff);
            }
            if (base.isAuthority)
            {
                new SpendHealthNetworkRequest(characterBody.masterObjectId, 0.1f * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
            }
        }


        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
        }
        protected virtual void OnHitEnemyAuthority()
        {
            AkSoundEngine.PostEvent("impactsfx", this.gameObject);
            //base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 20) * (this.moveSpeedStat / 7) * dropTimer);
            //if (characterBody.HasBuff(Modules.Buffs.loaderBuff))
            //{
            //    base.healthComponent.AddBarrierAuthority(healthComponent.fullCombinedHealth / 20);
            //}

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            dropTimer += Time.fixedDeltaTime;
            if (!this.hasDropped)
            {
                this.StartDrop();
                base.PlayCrossfade("FullBody, Override", "ManchesterEnd", "Attack.playbackRate", 0.5f, 0.2f);
            }

            if (!this.slamIndicatorInstance)
            {
                this.CreateIndicator();
            }
            if (this.slamIndicatorInstance)
            {
                this.UpdateSlamIndicator();
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
            base.characterMotor.velocity.y = -dropForce;



        }

        private void CreateIndicator()
        {
            if (EntityStates.Huntress.ArrowRain.areaIndicatorPrefab)
            {
                this.slamIndicatorInstance = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                this.slamIndicatorInstance.SetActive(true);

            }
        }
        private void UpdateSlamIndicator()
        {
            if (this.slamIndicatorInstance)
            {
                this.slamIndicatorInstance.transform.localScale = Vector3.one * slamRadius * (1 + dropTimer/2);
                this.slamIndicatorInstance.transform.localPosition = base.transform.position;
            }
        }
        private void LandingImpact()
        {

            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent(4108468048, base.gameObject);
                Ray aimRay = base.GetAimRay();

                base.characterMotor.velocity *= 0.1f;

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = slamRadius + (1 + dropTimer / 2) * (moveSpeedStat / 7);
                blastAttack.procCoefficient = slamProcCoefficient;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * damageCoefficient * (moveSpeedStat / 7) * (1 + dropTimer);
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = slamForce;
                blastAttack.teamIndex = base.teamComponent.teamIndex;
                blastAttack.damageType = damageType;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();

                }


                for (int i = 0; i <= 4; i += 1)
                {
                    Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * (slamRadius * (1 + dropTimer / 2) * 0.5f));
                    effectPosition.y = base.characterBody.footPosition.y;
                    EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, new EffectData
                    {
                        origin = effectPosition,
                        scale = slamRadius * (1 + dropTimer / 2) * 0.5f * (moveSpeedStat / 7),
                    }, true);
                }

                //EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, new EffectData
                //{
                //    origin = base.characterBody.footPosition,
                //    scale = slamRadius * (1 + dropTimer / 2),
                //}, true);


            }
        }


        public override void OnExit()
        {

            if (this.slamIndicatorInstance)
                this.slamIndicatorInstance.SetActive(false);
            EntityState.Destroy(this.slamIndicatorInstance);



            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;

            if (NetworkServer.active && base.characterBody.HasBuff(Modules.Buffs.manchesterBuff))
            {
                base.characterBody.RemoveBuff(Modules.Buffs.manchesterBuff);
            }

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