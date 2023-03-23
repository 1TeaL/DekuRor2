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
    public class Manchester45 : BaseSkill45
    {
        public  float basejumpDuration = 0.7f;
        private float jumpDuration;
        public  float dropForce = 20f;

        public  float slamRadius = 7f;
        public  float slamProcCoefficient = 1f;
        public  float slamForce = 10f;

        private bool hasDropped;
        private Vector3 flyVector = Vector3.zero;
        private Transform modelTransform;
        private Transform slamIndicatorInstance;
        private Transform slamCenterIndicatorInstance;
        private Ray downRay;

        protected DamageType damageType = DamageType.Stun1s;
        public DekuController dekucon;
        private float maxWeight;
        private BlastAttack blastAttack;
        private float num3;

        //private NemforcerGrabController grabController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.modelTransform = base.GetModelTransform();
            this.flyVector = Vector3.up;
            this.hasDropped = false; 
            dekucon = base.GetComponent<DekuController>();
            jumpDuration = basejumpDuration/attackSpeedStat;

            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            float num2 = (num / base.characterBody.baseMoveSpeed - 1f) * 0.67f;
            num3 = num2 + 1f;
            dropForce *= num3;
            slamForce *= num3;

            base.characterMotor.disableAirControlUntilCollision = true;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayCrossfade("Body", "Jump", "Attack.playbackRate", jumpDuration, 0.1f);
            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("shootstyedashvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("shootstyedashsfx", this.gameObject);

            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            

            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();


            blastAttack = new BlastAttack();
            blastAttack.radius = slamRadius;
            blastAttack.procCoefficient = slamProcCoefficient;
            blastAttack.position = base.characterBody.footPosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = base.RollCrit();
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchester45DamageCoefficient * num3;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = slamForce;
            blastAttack.teamIndex = base.teamComponent.teamIndex;
            blastAttack.damageType = damageType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

            for (int i = 0; i <= 4; i += 1)
            {
                Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * (slamRadius * 0.5f));
                effectPosition.y = base.characterBody.footPosition.y;
                EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, new EffectData
                {
                    origin = effectPosition,
                    scale = slamRadius,
                }, true);
            }

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuffAuthority(Modules.Buffs.manchesterBuff.buffIndex, Modules.StaticValues.manchester45BuffDuration);
            }
            BlastAttack.Result result = blastAttack.Fire();
            if (result.hitCount > 0)
            {
                this.OnHitEnemyAuthority(result);
            }
        }


        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
        }
        protected virtual void OnHitEnemyAuthority(BlastAttack.Result result)
        {
            AkSoundEngine.PostEvent("delawaresfx", this.gameObject);
            foreach (BlastAttack.HitPoint hitpoint in result.hitPoints)
            {             

                if (!hitpoint.hurtBox.healthComponent.body.HasBuff(Modules.Buffs.barrierMark.buffIndex))
                {
                    hitpoint.hurtBox.healthComponent.body.ApplyBuff(Modules.Buffs.barrierMark.buffIndex, 1, -1);
                }
            }
            //base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 10) * (this.moveSpeedStat / 7));

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();


            if (!this.hasDropped)
            {
                base.characterMotor.rootMotion += this.flyVector * ((dropForce) * EntityStates.Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / jumpDuration) * Time.fixedDeltaTime);
                base.characterMotor.velocity.y = 0f;
            }

            if (base.fixedAge >= (0.25f * jumpDuration) && !this.slamIndicatorInstance)
            {
                this.CreateIndicator();
            }

            if (base.fixedAge >= jumpDuration && !this.hasDropped)
            {
                this.StartDrop();
            }

            if (this.hasDropped && base.isAuthority && !base.characterMotor.disableAirControlUntilCollision)
            {
                this.LandingImpact();
                this.outer.SetNextStateToMain();
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
                this.slamIndicatorInstance.localScale = Vector3.one * slamRadius;

                this.slamCenterIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab).transform;
                this.slamCenterIndicatorInstance.localScale = (Vector3.one * slamRadius) / 3f;
            }
        }

        private void StartDrop()
        {
            this.hasDropped = true;

            base.characterMotor.disableAirControlUntilCollision = true;
            base.characterMotor.velocity.y = -dropForce;
            base.PlayCrossfade("FullBody, Override", "ManchesterFlip", "Attack.playbackRate", jumpDuration, 0.01f);

            //bool active = NetworkServer.active;
            //if (active)
            //{
            //    base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            //}
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

        private void LandingImpact()
        {

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.characterMotor.velocity *= 0.1f;

                blastAttack.position = base.characterBody.footPosition;

                BlastAttack.Result result = blastAttack.Fire();
                if (result.hitCount > 0)
                {
                    this.OnHitEnemyAuthority(result);
                }



                for (int i = 0; i <= 4; i += 1)
                {
                    Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * 8f);
                    effectPosition.y = base.characterBody.footPosition.y;
                    EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, new EffectData
                    {
                        origin = effectPosition,
                        scale = slamRadius / 6,
                    }, true);
                }
            }
        }

        public override void OnExit()
        {
            base.characterMotor.disableAirControlUntilCollision = false;

            if (this.slamIndicatorInstance) EntityState.Destroy(this.slamIndicatorInstance.gameObject);
            if (this.slamCenterIndicatorInstance) EntityState.Destroy(this.slamCenterIndicatorInstance.gameObject);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");


            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;



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