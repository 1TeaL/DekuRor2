//using DekuMod.Modules.Survivors;
//using EntityStates;
//using RoR2.Skills;
//using RoR2;
//using UnityEngine.Networking;
//using UnityEngine;
//using EntityStates.Merc;
//using System.Collections.Generic;
//using System.Linq;
//using DekuMod.Modules.Networking;
//using R2API.Networking;
//using R2API.Networking.Interfaces;
//using System;

//namespace DekuMod.SkillStates
//{

//	public class LegSuper : BaseSpecial
//    {
//        public float timer;
//        public float baseFireInterval = 0.2f;
//        public float fireTime = 0.5f;
//        private float duration = 4.5f;
//        public static float exitDuration = 4f;
//        public static float baseBlastRadius = Modules.StaticValues.finalsmashBlastRadius;
//        public static float blastRadius;

//        public float fireInterval;
//        public float previousMass;
//        public Vector3 direction;

//        private Transform modelTransform;
//        private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
//        private float basespeedCoefficient = 3f;
//        private float speedCoefficient;
//        private string muzzleString;

//        private BlastAttack blastAttack;
//        private float maxWeight;
//        public bool animChange;
//        private BullseyeSearch search;
//        private List<HurtBox> target;

//        public override void OnEnter()
//		{
//			base.OnEnter();
//            dekucon = base.GetComponent<DekuController>();
//            energySystem = base.GetComponent<EnergySystem>();

//            float num = this.moveSpeedStat;
//            bool isSprinting = base.characterBody.isSprinting;
//            if (isSprinting)
//            {
//                num /= base.characterBody.sprintingSpeedMultiplier;
//            }
//            float num2 = (1f + (num / (base.characterBody.baseMoveSpeed) - 1f));

//            timer = 0f;
//            blastRadius = baseBlastRadius * num2;
//            fireInterval = baseFireInterval;
//            speedCoefficient = basespeedCoefficient * moveSpeedStat;
//            animChange = false;
//            Ray aimRay = base.GetAimRay();
//            direction = aimRay.direction;
//            base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
//            this.muzzleString = "LFoot";

//            blastAttack = new BlastAttack();
//            blastAttack.radius = blastRadius;
//            blastAttack.procCoefficient = 1f;
//            blastAttack.attacker = base.gameObject;
//            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
//            blastAttack.baseDamage = damageStat * Modules.StaticValues.finalsmashSmashDamageCoefficient * attackSpeedStat;
//            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
//            blastAttack.baseForce = maxWeight * 10f;
//            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
//            blastAttack.damageType = DamageType.IgniteOnHit;
//            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

//            if (energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
//            {
//                energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);

//                base.StartAimMode(0.5f + this.duration, false);

//                if (NetworkServer.active)
//                {
//                    base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration);
//                }

//                EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
//                EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, this.muzzleString, false);


//                if (base.isAuthority)
//                {
//                    new PerformFinalSmashNetworkRequest(base.characterBody.masterObjectId).Send(NetworkDestination.Clients);


//                    base.characterMotor.useGravity = false;
//                    this.previousMass = base.characterMotor.mass;
//                    base.characterMotor.mass = 0f;
//                    base.characterMotor.Motor.ForceUnground();
//                    base.characterMotor.disableAirControlUntilCollision = true;

//                    dekucon.WINDRING.Play();
//                    AkSoundEngine.PostEvent("finalsmashsfxvoice", this.gameObject);
                    
//                    PlayCrossfade("FullBody, Override", "FinalSmashBegin", "Attack.playbackRate", fireTime, 0.01f);
//                }

//            }
//            else
//            {
//                if (base.isAuthority)
//                {
//                    Chat.AddMessage($"You need {Modules.StaticValues.specialPlusUltraSpend} plus ultra.");
//                    this.outer.SetNextStateToMain();
//                    return;

//                }
//            }


//        }


//        private void CreateBlinkEffect(Vector3 origin)
//        {
//            EffectData effectData = new EffectData();
//            effectData.rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward);
//            effectData.origin = origin;
//            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
//        }

//        public override void FixedUpdate()
//		{
//			base.FixedUpdate();

//            if (timer > fireInterval && base.isAuthority)
//            {
//                timer = 0;
//                new PerformFinalSmashNetworkRequest(base.characterBody.masterObjectId).Send(NetworkDestination.Clients);

//            }
//            else
//            {
//                timer += Time.fixedDeltaTime;
//            }

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


//            if (base.fixedAge >= fireTime && base.fixedAge < exitDuration && base.isAuthority)
//            {
//                PlayCrossfade("FullBody, Override", "FinalSmashDash", "Attack.playbackRate",fireTime, 0.1f);
//                this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));

//                Ray aimRay = base.GetAimRay();

                
//                if (base.characterMotor)
//                {
//                    Vector3 vector = direction * this.speedCoefficient;
//                    //float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
//                    //vector = aimRay.direction;
//                    //vector.y = 0f;

//                    base.characterMotor.velocity = vector;
//                }


//            } 
            
//            if(base.fixedAge > exitDuration && base.isAuthority)
//            {
//                if (!animChange)
//                {
//                    PlayAnimation("FullBody, Override", "FinalSmashSmash", "Attack.playbackRate", fireTime);
//                    animChange = true;
//                }
//            }


//            if (base.fixedAge > duration && base.isAuthority)
//			{
//                blastAttack.position = base.transform.position;
//                new PerformFinalSmashNetworkRequest(base.characterBody.masterObjectId).Send(NetworkDestination.Clients);

//                blastAttack.Fire();

//                AkSoundEngine.PostEvent("impactsfx", this.gameObject);
//                dekucon.WINDRING.Stop();


//                for (int i = 0; i <= 4; i += 1)
//                {
//                    Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * (blastRadius * 0.5f));
//                    effectPosition.y = base.characterBody.footPosition.y;
//                    EffectManager.SpawnEffect(Modules.Assets.elderlemurianexplosionEffect, new EffectData
//                    {
//                        origin = effectPosition,
//                        scale = blastRadius,
//                    }, true);
//                }

//                this.outer.SetNextStateToMain();
//                return;
//			}

//		}

//        public override void OnExit()
//        {
//            base.OnExit();

//            base.characterMotor.disableAirControlUntilCollision = false;
//            base.characterMotor.mass = this.previousMass;
//            base.characterMotor.useGravity = true;
//            base.characterMotor.velocity = Vector3.zero;

//            base.characterMotor.disableAirControlUntilCollision = false;
//            base.characterMotor.velocity.y = 0;
//        }

//        public override InterruptPriority GetMinimumInterruptPriority()
//		{
//			return InterruptPriority.Death;
//		}
//	}
//}