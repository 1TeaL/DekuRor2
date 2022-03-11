using RoR2;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using EntityStates;
using DekuMod.Modules.Survivors;
using System.Collections.Generic;
using RoR2.Skills;

namespace DekuMod.SkillStates
{
    public class Float45 : BaseSkillState
    {
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        public static float basejumpDuration = 1f;
        public static float jumpDuration;
        public static float dropForce = 80f;

        public static float slamRadius = 15f;
        public static float slamProcCoefficient = 1f;
        public static float slamForce = 1000f;

        private bool hasFloated;
        private Vector3 flyVector = Vector3.zero;
        private Transform modelTransform;
        private Transform slamIndicatorInstance;
        private Transform slamCenterIndicatorInstance;
        private Ray downRay;

        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;
        private float maxWeight;

        public static SkillDef utilityDef = Deku.floatcancelSkillDef;
        public static SkillDef specialDef = Deku.floatdelaware45SkillDef;

        //private NemforcerGrabController grabController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.modelTransform = base.GetModelTransform();
            this.flyVector = Vector3.up;
            this.hasFloated = false;
            dekucon = base.GetComponent<DekuController>();

            base.PlayAnimation("FullBody, Override", "FloatBegin", "Attack.playbackRate", Float45.jumpDuration);
            AkSoundEngine.PostEvent(687990298, this.gameObject);
            AkSoundEngine.PostEvent(1918362945, this.gameObject);

            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            base.characterMotor.useGravity = false;

            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();

            EffectManager.SpawnEffect(Modules.Assets.impactEffect, new EffectData
            {
                origin = base.transform.position,
                scale = slamRadius,
                rotation = Util.QuaternionSafeLookRotation(Vector3.down),
            }, false);

            for (int i = 0; i <= 20; i++)
            {
                float num = 60f;
                Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
                float num2 = 0.01f;
                rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
                EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
                {
                    origin = base.transform.position,
                    scale = slamRadius,
                    rotation = rotation
                }, true);

            }

            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddBuff(Modules.Buffs.floatBuff);
            }

        }

        protected virtual void OnHitEnemyAuthority()
        {
            base.healthComponent.AddBarrierAuthority(this.damageStat * (this.moveSpeedStat / 7));

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!this.hasFloated)
            {
                base.characterMotor.rootMotion += this.flyVector * ((0.6f * this.moveSpeedStat * fajin) * EntityStates.Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / Float.jumpDuration) * Time.fixedDeltaTime);
                base.characterMotor.velocity.y = 0f;
            }

            if (base.fixedAge >= Float45.jumpDuration)
            {
                base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Float45.utilityDef, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.special.SetSkillOverride(base.skillLocator.special, Float45.specialDef, GenericSkill.SkillOverridePriority.Contextual);

                this.outer.SetNextStateToMain();
            }
        }




        public override void OnExit()
        {
            dekucon.RemoveBuffCount(50);

            if (this.slamIndicatorInstance) EntityState.Destroy(this.slamIndicatorInstance.gameObject);
            if (this.slamCenterIndicatorInstance) EntityState.Destroy(this.slamCenterIndicatorInstance.gameObject);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");


            //base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;

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