using RoR2;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using EntityStates;
using DekuMod.Modules.Survivors;
using System.Collections.Generic;

namespace DekuMod.SkillStates
{
    public class Manchester : BaseSkillState
    {
        public static float basejumpDuration = 1f;
        public static float jumpDuration;
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

        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;
        private float maxWeight;

        //private NemforcerGrabController grabController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.modelTransform = base.GetModelTransform();
            this.flyVector = Vector3.up;
            this.hasDropped = false;
            dekucon = base.GetComponent<DekuController>();
            if (dekucon.isMaxPower)
            {
                dekucon.RemoveBuffCount(50);
                damageType = DamageType.BypassArmor | DamageType.Stun1s;
                fajin = 2f;
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = Manchester.slamRadius * fajin;
                blastAttack.procCoefficient = Manchester.slamProcCoefficient;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchesterDamageCoefficient * (moveSpeedStat / 7);
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = -1000f;
                blastAttack.teamIndex = base.teamComponent.teamIndex;
                blastAttack.damageType = damageType;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;



                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();

                }
            }
            else
            {
                damageType = DamageType.Stun1s;
                fajin = 1f;
            }
            jumpDuration = basejumpDuration / fajin;


            base.PlayAnimation("FullBody, Override", "ManchesterBegin", "Attack.playbackRate", Manchester.jumpDuration);
            AkSoundEngine.PostEvent(687990298, this.gameObject);
            AkSoundEngine.PostEvent(1918362945, this.gameObject);

            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;


            base.gameObject.layer = LayerIndex.fakeActor.intVal;
            //base.characterMotor.Motor.RebuildCollidableLayers();
            dekucon.AddToBuffCount(10);
;
        }


        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
        }
        protected virtual void OnHitEnemyAuthority()
        {
            base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 10) * (this.moveSpeedStat/7));

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!this.hasDropped)
            {
                base.characterMotor.rootMotion += this.flyVector * ((0.6f * this.moveSpeedStat * fajin) * EntityStates.Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / Manchester.jumpDuration) * Time.fixedDeltaTime);
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

            base.PlayAnimation("Fullbody, Override", "ManchesterSmashExit", "Attack.playbackRate", jumpDuration/3f);
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
                this.slamIndicatorInstance.localScale = Vector3.one * Manchester.slamRadius;

                this.slamCenterIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab).transform;
                this.slamCenterIndicatorInstance.localScale = (Vector3.one * Manchester.slamRadius) / 3f;
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
                    damageType = DamageType.BypassArmor | DamageType.Stun1s;
                }
                else
                {
                    damageType = DamageType.Stun1s;
                }
                base.characterMotor.velocity *= 0.1f;

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = Manchester.slamRadius * fajin;
                blastAttack.procCoefficient = Manchester.slamProcCoefficient;
                blastAttack.position = base.characterBody.footPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchesterDamageCoefficient * (moveSpeedStat/7);
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = Manchester.slamForce;
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
            //base.characterMotor.Motor.RebuildCollidableLayers();
            base.OnExit();
        }

       

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}