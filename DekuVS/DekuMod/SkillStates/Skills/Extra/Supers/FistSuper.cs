using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EntityStates.Merc;

namespace DekuMod.SkillStates
{

	public class FistSuper : BaseSpecial
	{
		public float baseFireInterval = 0.4f;
		public float fireInterval;
		private float duration = 4f;
		private float exitDuration = 3.5f;
		private float fireTime = 0.5f;
		private float baseBlastRadius = Modules.StaticValues.detroitdelawareBlastRadius;
		private float blastRadius;
		private float FOV = 120f;

		private BlastAttack blastAttack;
		public Vector3 theSpot;
        private float maxWeight;
        private float timer;

		private bool animChange;
		private string muzzleString;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        private EffectData effectData1;
		private EffectData effectData2;
		private BullseyeSearch search;
        private List<HurtBox> target;

        public override void OnEnter()
		{
			base.OnEnter();					
			dekucon = base.GetComponent<DekuController>();
			energySystem = base.GetComponent<EnergySystem>();
			blastRadius = baseBlastRadius * attackSpeedStat;
			fireInterval = baseFireInterval / attackSpeedStat;
			animChange = false;
			base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
			this.muzzleString = "RHand";

			Ray aimRay = base.GetAimRay();
			theSpot = base.transform.position;

			//blast attack
			blastAttack = new BlastAttack();
			blastAttack.radius = blastRadius;
			blastAttack.procCoefficient = 1f;
			blastAttack.position = theSpot;
			blastAttack.attacker = base.gameObject;
			blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
			blastAttack.baseDamage = base.damageStat * Modules.StaticValues.detroitdelawareSmashDamageCoefficient * attackSpeedStat;
			blastAttack.falloffModel = BlastAttack.FalloffModel.None;
			blastAttack.baseForce = maxWeight * 10f;
			blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
			blastAttack.damageType = DamageType.Freeze2s;
			blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

			effectData1 = new EffectData
			{
				scale = 1f,
				origin = base.characterBody.corePosition,
				rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
			};

			effectData2 = new EffectData
			{
				scale = 1f,
				origin = base.characterBody.corePosition,
				rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
			};

			if (energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
			{
				energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);

				if (NetworkServer.active)
				{
					base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration);
				}


				if (base.isAuthority)
				{
					base.StartAimMode(0.5f + this.duration, false);
					GetMaxWeight();


					EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
					EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, this.muzzleString, false);


					EffectManager.SpawnEffect(Modules.Projectiles.delawareTracer, effectData2, true);


					PlayCrossfade("FullBody, Override", "DetroitDelawareFull", "Attack.playbackRate", fireTime, 0.01f);

					AkSoundEngine.PostEvent("delawaredetroitvoice", this.gameObject);
					
				}

			}
			else
			{
                if (base.isAuthority)
				{
					Chat.AddMessage($"You need {Modules.StaticValues.specialPlusUltraSpend} plus ultra.");
					energySystem.TriggerGlow(0.3f, 0.3f, Color.black);
					this.outer.SetNextStateToMain();
					return;

				}
			}

		}


		public void GetMaxWeight()
		{
			Ray aimRay = base.GetAimRay();
			search = new BullseyeSearch
			{

				teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
				filterByLoS = false,
				searchOrigin = theSpot,
				searchDirection = UnityEngine.Random.onUnitSphere,
				sortMode = BullseyeSearch.SortMode.Distance,
				maxDistanceFilter = blastRadius,
				maxAngleFilter = 360f
			};

			search.RefreshCandidates();
			search.FilterOutGameObject(base.gameObject);


			target = search.GetResults().ToList<HurtBox>();
			foreach (HurtBox singularTarget in target)
			{
				if (singularTarget)
				{
					if (singularTarget.healthComponent && singularTarget.healthComponent.body)
					{
						if (singularTarget.healthComponent.body.characterMotor)
						{
							if (singularTarget.healthComponent.body.characterMotor.mass > maxWeight)
							{
								maxWeight = singularTarget.healthComponent.body.characterMotor.mass;
							}
						}
						else if (singularTarget.healthComponent.body.rigidbody)
						{
							if (singularTarget.healthComponent.body.rigidbody.mass > maxWeight)
							{
								maxWeight = singularTarget.healthComponent.body.rigidbody.mass;
							}
						}

						new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId,
						singularTarget.healthComponent.body.masterObjectId).Send(NetworkDestination.Clients);
						
					}
				}
			}
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();


			if (base.fixedAge > fireTime)
			{
				if (base.isAuthority && timer > fireInterval)
				{
					timer = 0f;
					GetMaxWeight();
				}
				else
				{
					timer += Time.fixedDeltaTime;
				}

			}
            if (base.fixedAge > exitDuration && !animChange && base.isAuthority)
			{
				animChange = true;
				AkSoundEngine.PostEvent("shootstyedashcomboimpact", this.gameObject);
				EffectManager.SpawnEffect(Modules.Assets.mageLightningBombEffectPrefab, new EffectData
				{
					origin = theSpot,
					scale = blastRadius,
					rotation = Quaternion.LookRotation(GetAimRay().direction)
				}, true);
			}

            if (base.fixedAge > duration && base.isAuthority)
			{
				GetMaxWeight();
				blastAttack.Fire();
				AkSoundEngine.PostEvent("impactsfx", this.gameObject);

				EffectManager.SpawnEffect(Modules.Assets.detroitEffect, effectData2, true);

				this.outer.SetNextStateToMain();
				return;
			}
			
		}
        public override void OnExit()
        {
            base.OnExit();

			//if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
		}
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}