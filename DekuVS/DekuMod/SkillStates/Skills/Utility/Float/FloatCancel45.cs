﻿//using RoR2;
//using UnityEngine;
//using System.Linq;
//using UnityEngine.Networking;
//using EntityStates;
//using DekuMod.Modules.Survivors;
//using System.Collections.Generic;

//namespace DekuMod.SkillStates
//{
//    public class FloatCancel45 : BaseSkillState
//    {
//        public static float basejumpDuration = 10f;
//        public static float jumpDuration;
//        public static float dropForce = 80f;

//        public static float slamRadius = 15f;
//        public static float slamProcCoefficient = 1f;
//        public static float slamForce = 1000f;

//        private bool hasDropped;
//        private Vector3 flyVector = Vector3.zero;
//        private Transform modelTransform;
//        private Transform slamIndicatorInstance;
//        private Transform slamCenterIndicatorInstance;
//        private Ray downRay;

//        public float fajin;
//        protected DamageType damageType;
//        public DekuController dekucon;
//        private float maxWeight;

//        //private NemforcerGrabController grabController;

//        public override void OnEnter()
//        {
//            base.OnEnter();
//            this.modelTransform = base.GetModelTransform();
//            this.flyVector = Vector3.up;

//            dekucon = base.GetComponent<DekuController>();


//            base.characterMotor.Motor.ForceUnground();
//            base.characterMotor.velocity = Vector3.zero;

            

//            //base.gameObject.layer = LayerIndex.fakeActor.intVal;
//            base.characterMotor.Motor.RebuildCollidableLayers();

//            base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.floatcancel45SkillDef, GenericSkill.SkillOverridePriority.Contextual);
//            base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.float45SkillDef, GenericSkill.SkillOverridePriority.Contextual);
//            base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.floatdelaware45SkillDef, GenericSkill.SkillOverridePriority.Contextual);
//            base.skillLocator.utility.SetSkillOverride(base.skillLocator.special, Deku.ofacycle2SkillDef, GenericSkill.SkillOverridePriority.Contextual);

//            if (NetworkServer.active)
//            {
//                base.characterBody.RemoveBuff(Modules.Buffs.floatBuff);
//            }
//            if (base.isAuthority)
//            {
//                EffectManager.SpawnEffect(Modules.Asset.impactEffect, new EffectData
//                {
//                    origin = base.transform.position,
//                    scale = slamRadius,
//                    rotation = Util.QuaternionSafeLookRotation(Vector3.down),
//                }, false);
//            }
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();

//            if (!this.hasDropped)
//            {
//                this.StartDrop();
//            }

//            if (this.hasDropped && base.isAuthority && !base.characterMotor.disableAirControlUntilCollision)
//            {
//                this.LandingImpact();
//                this.outer.SetNextStateToMain();
//            }

//            if (this.hasDropped && base.isAuthority && base.fixedAge > basejumpDuration)
//            {
//                this.LandingImpact();
//                this.outer.SetNextStateToMain();
//            }
//        }

//        private void StartDrop()
//        {
//            this.hasDropped = true;

//            base.characterMotor.disableAirControlUntilCollision = true;
//            base.characterMotor.velocity.y = -FloatCancel.dropForce;

//            bool active = NetworkServer.active;
//            if (active)
//            {
//                base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
//            }
//        }



//        private void LandingImpact()
//        {

//            if (base.isAuthority)
//            {
//                base.characterMotor.velocity *= 0.1f;
//            }
//        }


//        public override void OnExit()
//        {

//            base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.float45SkillDef, GenericSkill.SkillOverridePriority.Contextual);
//            base.PlayAnimation("FullBody, Override", "BufferEmpty");

//            base.characterMotor.useGravity = true;
//            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;

//            if (NetworkServer.active && base.characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility)) base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);

//            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
//            base.characterMotor.Motor.RebuildCollidableLayers();
//            base.OnExit();
//        }

       

//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.Frozen;
//        }
//    }
//}