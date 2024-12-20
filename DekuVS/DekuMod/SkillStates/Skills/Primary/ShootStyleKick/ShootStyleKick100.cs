﻿//using DekuMod.Modules;
//using DekuMod.Modules.Networking;
//using DekuMod.Modules.Survivors;
//using EntityStates;
//using EntityStates.Merc;
//using R2API.Networking;
//using R2API.Networking.Interfaces;
//using R2API.Utils;
//using RoR2;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.Networking;

//namespace DekuMod.SkillStates
//{
//    [R2APISubmoduleDependency(new string[]
//    {
//        "NetworkingAPI"
//    })]
//    public class ShootStyleKick100 : BaseDekuSkillState
//    {

//        public float previousMass;
//        private string muzzleString;

//        public static float duration;
//        public int numberOfHits; 
//        public static float baseDuration = 0.5f;
//        public static float initialSpeedCoefficient = 8f;
//        public static float finalSpeedCoefficient = 1f;
//        public static float SpeedCoefficient;
//        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;
//        public static float procCoefficient = 1f;
//        private Animator animator;

//        //private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
//        //public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerMageIceLaser");
//        private Transform modelTransform;
//        private CharacterModel characterModel;
//        private float rollSpeed;
//        private Vector3 forwardDirection;
//        private Vector3 previousPosition;

//        //checking location for networking
//        public Vector3 origin;
//        public Vector3 final;
//        private Vector3 theSpot;
//        private float num3;
//        private readonly BullseyeSearch search = new BullseyeSearch();

//        public override void OnEnter()
//        {
//            base.OnEnter();
//            this.animator = base.GetModelAnimator();

//            float move = this.moveSpeedStat;
//            bool isSprinting = base.characterBody.isSprinting;
//            if (isSprinting)
//            {
//                move /= base.characterBody.sprintingSpeedMultiplier;
//            }
//            float move2 = (move / base.characterBody.baseMoveSpeed - 1f) * 0.67f;
//            num3 = move2 + 1f;

//            if (base.isAuthority && base.inputBank && base.characterDirection)
//            {
//                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
//            }

//            Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
//            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

//            float num = Vector3.Dot(this.forwardDirection, rhs);
//            float num2 = Vector3.Dot(this.forwardDirection, rhs2);

//            this.RecalculateRollSpeed();

//            if (base.characterMotor && base.characterDirection)
//            {
//                base.characterMotor.velocity.y = 0f;
//                base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
//            }

//            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
//            this.previousPosition = base.transform.position - b;


//            base.OnEnter();

//            duration = baseDuration;
//            numberOfHits = Mathf.RoundToInt(StaticValues.shootkick100NumberOFHits * num3);


//            if (base.isAuthority)
//            {
//                AkSoundEngine.PostEvent("shootstyledashvoice", this.gameObject);
//            }
//            AkSoundEngine.PostEvent("shootstyledashsfx", this.gameObject);
//            //base.PlayAnimation("FullBody, Override", "ShootStyleDash", "Attack.playbackRate", 0.1f);
//            //base.PlayAnimation("FullBody, Override", "ShootStyleKick", "Attack.playbackRate", 0.1f);
//            this.animator.SetBool("attacking", true);
//            base.PlayCrossfade("FullBody, Override", "ShootStyleKick", "Attack.playbackRate", duration, 0.1f);

//            base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, baseDuration);

//            this.muzzleString = "LFoot";
//            EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
//            EffectManager.SimpleMuzzleFlash(Modules.Asset.muzzleflashMageLightningLargePrefab, base.gameObject, this.muzzleString, false);

//            base.characterMotor.useGravity = false;
//            this.previousMass = base.characterMotor.mass;
//            base.characterMotor.mass = 0f;


//            origin = base.transform.position;
//            if (base.isAuthority)
//            {
//                new SpendHealthNetworkRequest(characterBody.masterObjectId, Modules.StaticValues.shootkick100HealthCostFraction * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
//            }

//        }
//        private void RecalculateRollSpeed()
//        {
//            this.rollSpeed = this.moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, base.fixedAge / duration);
//        }
//        private void CreateBlinkEffect(Vector3 origin)
//        {
//            EffectData effectData = new EffectData();
//            effectData.rotation = Util.QuaternionSafeLookRotation(this.forwardDirection);
//            effectData.origin = origin;
//            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
//        }

//        public void ApplyComponent()
//        {
//            theSpot = Vector3.Lerp(origin,final, 0.5f);

//            search.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
//            search.filterByLoS = false;
//            search.searchOrigin = theSpot;
//            search.searchDirection = UnityEngine.Random.onUnitSphere;
//            search.sortMode = BullseyeSearch.SortMode.Distance;
//            search.maxDistanceFilter = (final-origin).magnitude/2;
//            search.maxAngleFilter = 360f;
            

//            search.RefreshCandidates();
//            search.FilterOutGameObject(base.gameObject);



//            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
//            foreach (HurtBox singularTarget in target)
//            {
//                if (singularTarget.healthComponent.body && singularTarget.healthComponent)
//                {
//                    int buffcount = singularTarget.healthComponent.body.GetBuffCount(Modules.Buffs.delayAttackDebuff.buffIndex);
//                    if (NetworkServer.active)
//                    {
//                        singularTarget.healthComponent.body.ApplyBuff(Modules.Buffs.delayAttackDebuff.buffIndex, numberOfHits + buffcount);
//                    }
//                    ShootStyleKickComponent shootStyleKickComponent = singularTarget.healthComponent.body.gameObject.GetComponent<ShootStyleKickComponent>();
                    
//                    if (shootStyleKickComponent)
//                    {
//                        shootStyleKickComponent.numberOfHits += numberOfHits;
//                        shootStyleKickComponent.timer = 0;
//                    }
//                    if (!shootStyleKickComponent)
//                    {
//                        shootStyleKickComponent = singularTarget.healthComponent.body.gameObject.AddComponent<ShootStyleKickComponent>();
//                        shootStyleKickComponent.charbody = singularTarget.healthComponent.body;
//                        shootStyleKickComponent.dekucharbody = characterBody;
//                        shootStyleKickComponent.numberOfHits = numberOfHits;
//                        shootStyleKickComponent.damage = base.damageStat * Modules.StaticValues.shootkick100DamageCoefficient * num3 * attackSpeedStat;
//                    }

                    
                    
//                }
//            }
//        }

//        public override void OnExit()
//        {
//            Ray aimRay = base.GetAimRay();
//            this.animator.SetBool("attacking", false);
//            base.PlayCrossfade("FullBody, Override", "ShootStyleKickAuto", 0.01f);
//            Util.PlaySound(EvisDash.endSoundString, base.gameObject);

//            base.characterMotor.mass = this.previousMass;
//            base.characterMotor.useGravity = true;
//            base.characterMotor.velocity = Vector3.zero;

//            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
//            base.characterMotor.disableAirControlUntilCollision = false;
//            base.characterMotor.velocity.y = 0;

//            final = base.transform.position;
//            ApplyComponent();

//            base.OnExit();
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();

//            this.RecalculateRollSpeed();
//            this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));


//            if (base.characterDirection) base.characterDirection.forward = this.forwardDirection;
//            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, base.fixedAge / duration);

//            Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
//            if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
//            {
//                Vector3 vector = normalized * this.rollSpeed;
//                float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
//                vector = this.forwardDirection * d;
//                vector.y = 0f;

//                base.characterMotor.velocity = vector;
//            }
//            this.previousPosition = base.transform.position;

//            if (this.modelTransform)
//            {
//                TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
//                temporaryOverlay.duration = 0.6f;
//                temporaryOverlay.animateShaderAlpha = true;
//                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
//                temporaryOverlay.destroyComponentOnEnd = true;
//                temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
//                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
//                TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
//                temporaryOverlay2.duration = 0.7f;
//                temporaryOverlay2.animateShaderAlpha = true;
//                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
//                temporaryOverlay2.destroyComponentOnEnd = true;
//                temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
//                temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
//            }

//            if (base.isAuthority && base.fixedAge >= duration)
//            {
//                this.outer.SetNextStateToMain();
//                return;
//            }
//        }

        

//        public override void OnSerialize(NetworkWriter writer)
//        {
//            base.OnSerialize(writer);
//            writer.Write(this.forwardDirection);
//        }

//        public override void OnDeserialize(NetworkReader reader)
//        {
//            base.OnDeserialize(reader);
//            this.forwardDirection = reader.ReadVector3();
//        }

//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.Skill;
//        }


//    }
//}
