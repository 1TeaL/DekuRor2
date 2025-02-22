﻿//using DekuMod.Modules.Networking;
//using EntityStates;
//using EntityStates.Huntress;
//using EntityStates.Loader;
//using EntityStates.VagrantMonster;
//using R2API.Networking;
//using R2API.Networking.Interfaces;
//using RoR2;
//using RoR2.Audio;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.Networking;
//using Object = UnityEngine.Object;
//using Random = UnityEngine.Random;

//namespace DekuMod.SkillStates
//{
//    public class Detroit100 : BaseDekuSkillState
//    {
//		private DamageType damageType = DamageType.Generic;
//		private Vector3 idealDirection;

//		private float baseFireTime = 0.2f;
//		private float fireTime;
//		private float totalDuration;
//		private float hitradius = 5f;
//		private float damageCoefficient = Modules.StaticValues.detroit100DamageCoefficient;
//		private float procCoefficient = 1f;
//		private float force = 1000f;
//		private Vector3 direction;
//		private Animator animator;

//		private GameObject areaIndicator;
//		private GameObject explosionPrefab = Modules.Asset.detroitEffect;
//		public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
//		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
//        private GameObject explosionPrefab2 = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
//        private string rMuzzleString = "RHand";

//		public override void OnEnter()
//		{
//			base.OnEnter();
//			Ray aimRay = base.GetAimRay();
//			base.StartAimMode(aimRay, 2f, true);
//			totalDuration = 0f;
//			fireTime = baseFireTime / attackSpeedStat;

//			bool isAuthority = base.isAuthority;
//			Util.PlaySound(BaseChargeFist.startChargeLoopSFXString, base.gameObject);
//			this.animator = base.GetModelAnimator();
//			//PlayAnimation("Body", "Sprint");
//			//Util.PlaySound(EntityStates.Bison.Charge.startSoundString, base.gameObject);
//			PlayCrossfade("FullBody, Override", "DetroitCharge", "Attack.playbackRate", fireTime, 0.01f);

//			if (base.isAuthority)
//			{
//				new SpendHealthNetworkRequest(characterBody.masterObjectId, Modules.StaticValues.detroit100HealthCostFraction * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
//			}

//			//set speed
//			bool flag = isAuthority;
//			if (flag)
//			{
//				base.characterBody.characterMotor.useGravity = false;
//				base.characterBody.baseAcceleration = 420f;
//				base.characterDirection.turnSpeed = 30f;
//				base.characterMotor.walkSpeedPenaltyCoefficient = 0f;
//				base.cameraTargetParams.fovOverride = 100f;
//				bool flag2 = base.inputBank;
//				if (flag2)
//				{
//					this.idealDirection = base.inputBank.aimDirection;
//					base.characterBody.isSprinting = true;
//                    this.UpdateDirection();

//					base.characterBody.inputBank.enabled = false;
//				}
//            }

//			//indicator
//			this.areaIndicator = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
//			this.areaIndicator.SetActive(true);

//		}

//		private void UpdateDirection()
//		{
//            this.idealDirection = base.inputBank.aimDirection;
//			Ray aimRay = base.GetAimRay();
//			base.StartAimMode(aimRay, 2f, true);
//		}

//        private Vector3 GetIdealVelocity()
//		{
//			return idealDirection * base.characterBody.moveSpeed * 3f;
//		}

//		public override void OnExit()
//		{
//			bool flag = this.areaIndicator;
//			if (flag)
//			{
//				this.areaIndicator.SetActive(false);
//				EntityState.Destroy(this.areaIndicator);
//			}

//			base.characterBody.characterMotor.useGravity = true;
//			base.characterBody.inputBank.enabled = true;

//			EffectManager.SimpleMuzzleFlash(this.muzzlePrefab, base.gameObject, this.rMuzzleString, false);
//			bool isAuthority = base.isAuthority;
//			if (isAuthority)
//			{
//				base.characterBody.baseAcceleration = 40f;
//				base.characterDirection.turnSpeed = 720f;
//				base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
//				base.cameraTargetParams.fovOverride = -1f;
//				base.characterBody.isSprinting = false;
//				base.OnExit();
//			}
//			Util.PlaySound(BaseChargeFist.endChargeLoopSFXString, base.gameObject);
//			//Util.PlaySound(EntityStates.Bison.Charge.endSoundString, base.gameObject);
//		}

//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			totalDuration += Time.fixedDeltaTime;

//			if(base.fixedAge > fireTime)
//			{
//				Loop();
//			}

//        }

//		public override void Update()
//		{
//			base.Update();
//			//indicator update
//			if (this.areaIndicator)
//            {
//                this.areaIndicator.transform.localScale = Vector3.one * this.hitradius * (attackSpeedStat) * (1 + totalDuration);
//                this.areaIndicator.transform.localPosition = base.transform.position;
//            }
//        }

//		public void Loop()
//		{
//			bool isAuthority = base.isAuthority;
//			if (isAuthority)
//			{
//				Ray aimRay = base.GetAimRay();
//				EffectManager.SpawnEffect(Modules.Asset.bisonEffect, new EffectData
//				{
//					origin = base.transform.position,
//					scale = 1f,
//					rotation = Quaternion.LookRotation(aimRay.direction)
//				}, true);

//				//this.direction = base.GetAimRay().direction.normalized;
//				//base.characterDirection.forward = this.direction;


//				base.characterBody.isSprinting = true;
//				base.characterDirection.moveVector = this.idealDirection;
//				base.characterMotor.rootMotion += this.GetIdealVelocity() * Time.fixedDeltaTime;
//				//Vector3 position = base.transform.position + base.characterDirection.forward.normalized * 0.5f;
//				Vector3 position = base.characterBody.corePosition + Vector3.up * 0.5f + aimRay.direction.normalized * 1f;
//				float radius = 0.3f;
//				LayerIndex layerIndex = LayerIndex.world;
//				int num = layerIndex.mask;
//				layerIndex = LayerIndex.entityPrecise;
//				int num2 = Physics.OverlapSphere(position, radius, num | layerIndex.mask).Length;
//				bool flag2 = num2 != 0;
//				if (flag2 || !base.IsKeyDownAuthority())
//				{
//					base.characterMotor.velocity = Vector3.zero;

//					this.animator.SetBool("detroitRelease", true);
//					PlayAnimation("FullBody, Override", "Detroit100Smash");

//					EffectManager.SimpleMuzzleFlash(Modules.Asset.dekuKickEffect, base.gameObject, "DownSwing", true);
//					for (int i = 0; i <= 10; i++)
//					{
//						float rand = 60f;
//						Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
//						float rand2 = 0.01f;
//						rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
//						rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
//						EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
//						{
//							origin = base.characterBody.corePosition,
//							scale = this.hitradius,
//							rotation = rotation
//						}, false);
//					}

//					BlastAttack blastAttack = new BlastAttack();
//					blastAttack.radius = hitradius * totalDuration * (attackSpeedStat);
//                    blastAttack.procCoefficient = procCoefficient;
//					blastAttack.position = base.transform.position;
//					blastAttack.attacker = base.gameObject;
//					blastAttack.crit = base.RollCrit();
//					blastAttack.baseDamage = base.characterBody.damage * damageCoefficient * (attackSpeedStat) * (1+totalDuration);
//					blastAttack.falloffModel = BlastAttack.FalloffModel.None;
//					blastAttack.baseForce = force * (1 + totalDuration);
//					blastAttack.teamIndex = base.teamComponent.teamIndex;
//					blastAttack.damageType = damageType;
//					blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

//					Util.PlaySound(EntityStates.Bison.Headbutt.attackSoundString, base.gameObject);

//					AkSoundEngine.PostEvent("detroitexitsfx", base.gameObject);


//					if (blastAttack.Fire().hitCount > 0)
//					{
//						this.OnHitEnemyAuthority();
//					}
//					this.outer.SetNextStateToMain();
//				}
//                else
//				{
//					PlayAnimation("FullBody, Override", "Detroit100Charging");
//					this.UpdateDirection();
//                }

//            }
//			else
//			{
//				this.outer.SetNextStateToMain();
//			}
//		}

//		protected virtual void OnHitEnemyAuthority()
//		{
//			Ray aimRay = base.GetAimRay();

//			EffectData effectData = new EffectData
//			{
//				scale = this.hitradius * 2f,
//				origin = base.characterBody.corePosition,
//				rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
//			};
//			EffectManager.SpawnEffect(this.explosionPrefab, effectData, true);

//			EffectData effectData2 = new EffectData
//			{
//				scale = this.hitradius,
//				origin = base.characterBody.corePosition,
//				rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
//			};
//			EffectManager.SpawnEffect(this.explosionPrefab2, effectData2, true);
//		}

//		public override InterruptPriority GetMinimumInterruptPriority()
//		{
//			return InterruptPriority.Frozen;
//		}
//	}
//}
