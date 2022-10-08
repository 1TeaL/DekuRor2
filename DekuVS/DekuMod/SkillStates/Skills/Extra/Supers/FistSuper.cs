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
		public static float baseDuration = 4f;
		private float duration;
		private float exitDuration;
		private float fireTime = 0.5f;
		private float baseBlastRadius = Modules.StaticValues.finalsmashBlastRadius;
		private float blastRadius;
		private float FOV = 120f;

		private BlastAttack blastAttack;
		public Vector3 theSpot;
        private float maxWeight;
        private float timer;

		private bool animChange;
		private string muzzleString;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");

		public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();
			energySystem = base.GetComponent<EnergySystem>();
			if (energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
			{
				energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);
				this.duration = baseDuration;
				exitDuration = duration - fireTime;
				blastRadius = baseBlastRadius * attackSpeedStat;
				animChange = false;

				Ray aimRay = base.GetAimRay();
				base.StartAimMode(0.5f + this.duration, false);
				theSpot = baseBlastRadius * aimRay.direction;


				if (NetworkServer.active)
				{
					base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, baseDuration);
				}
				//intial pull
				if (base.isAuthority)
				{
					new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId,
						base.transform.position,
						base.GetAimRay().direction,
						Modules.StaticValues.detroitdelawareDamageCoefficient).Send(NetworkDestination.Clients);

					this.muzzleString = "RFinger";
					EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
					EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, this.muzzleString, false);

					//blast attack
					blastAttack = new BlastAttack();
					blastAttack.radius = blastRadius;
					blastAttack.procCoefficient = 1f;
					blastAttack.position = theSpot;
					blastAttack.attacker = base.gameObject;
					blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
					blastAttack.baseDamage = damageStat * Modules.StaticValues.detroitdelawareDamageCoefficient;
					blastAttack.falloffModel = BlastAttack.FalloffModel.None;
					blastAttack.baseForce = maxWeight * 10f;
					blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
					blastAttack.damageType = DamageType.Freeze2s;
					blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

					EffectManager.SpawnEffect(Modules.Projectiles.delawareTracer, new EffectData
					{
						origin = theSpot,
						scale = 1f,
						rotation = Quaternion.LookRotation(GetAimRay().direction)
					}, true);

					base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
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

		protected virtual void OnHitEnemyAuthority()
		{
			AkSoundEngine.PostEvent("impactsfx", this.gameObject);
			//base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 30) * this.attackSpeedStat);
		}

		public void GetMaxWeight()
		{
			Ray aimRay = base.GetAimRay();
			BullseyeSearch search = new BullseyeSearch
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



			List<HurtBox> target = search.GetResults().ToList<HurtBox>();
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
					}
				}
			}
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();


			if (base.fixedAge > fireTime)
			{
				if (base.isAuthority && timer > 0.2f)
				{
					timer = 0f;
					new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId,
						base.transform.position,
						base.GetAimRay().direction,
						0f).Send(NetworkDestination.Clients);
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
				if (blastAttack.Fire().hitCount > 0)
				{
					this.OnHitEnemyAuthority();
				}

				EffectManager.SpawnEffect(Modules.Assets.detroitEffect, new EffectData
				{
					origin = theSpot,
					scale = blastRadius,
					rotation = Quaternion.LookRotation(GetAimRay().direction)
				}, true);

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