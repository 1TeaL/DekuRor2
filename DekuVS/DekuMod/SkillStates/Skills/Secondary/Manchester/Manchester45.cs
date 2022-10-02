using RoR2;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using EntityStates;
using DekuMod.Modules.Survivors;
using System.Collections.Generic;

namespace DekuMod.SkillStates
{
    public class Manchester45 : BaseSkillState
    {
        public static float basejumpDuration = 0.8f;
        public static float jumpDuration;
        public static float dropForce = 80f;

        public static float slamRadius;
        public static float baseRadius = 3f;
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
            dekucon = base.GetComponent<DekuController>();

            slamRadius = baseRadius * moveSpeedStat;

            jumpDuration = basejumpDuration / attackSpeedStat;

            base.characterMotor.disableAirControlUntilCollision = true;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayCrossfade("Body", "Jump", "Attack.playbackRate", jumpDuration, 0.1f);
            AkSoundEngine.PostEvent(687990298, this.gameObject);
            AkSoundEngine.PostEvent(1918362945, this.gameObject);

            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            

            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();


            BlastAttack blastAttack = new BlastAttack();
            blastAttack.radius = slamRadius;
            blastAttack.procCoefficient = slamProcCoefficient;
            blastAttack.position = base.characterBody.footPosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = base.RollCrit();
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.manchesterDamageCoefficient * (moveSpeedStat / 7);
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = slamForce;
            blastAttack.teamIndex = base.teamComponent.teamIndex;
            blastAttack.damageType = damageType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

            if (blastAttack.Fire().hitCount > 0)
            {
                this.OnHitEnemyAuthority();
            }

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuffAuthority(Modules.Buffs.manchesterBuff.buffIndex, Modules.StaticValues.manchester45BuffDuration);
            }
        }


        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
        }
        protected virtual void OnHitEnemyAuthority()
        {
            //base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 10) * (this.moveSpeedStat / 7));

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterMotor.rootMotion += this.flyVector * ((1f * this.moveSpeedStat) * EntityStates.Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / jumpDuration) * Time.fixedDeltaTime);
            base.characterMotor.velocity.y = 0f;           


            if (base.fixedAge > jumpDuration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
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