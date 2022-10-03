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

namespace DekuMod.SkillStates
{

	public class FistSuper : BaseSpecial
	{
		public static float baseDuration = 4f;
		private float duration;
		private float exitDuration;
		private float fireTime = 0.5f;
		private float baseBlastRadius = 20f;
		private float blastRadius;
		private float FOV = 120f;

		private BlastAttack blastAttack;
		public Vector3 theSpot;
		public Vector3 theDirection;
        private float maxWeight;
        private float timer;

		private bool animChange;

		public override void OnEnter()
		{
			base.OnEnter();


		}
		protected override void DoSkill()
		{
			this.duration = baseDuration;
			exitDuration = duration - fireTime;
			blastRadius = baseBlastRadius * attackSpeedStat;
			animChange = false;

			Ray aimRay = base.GetAimRay();
			base.StartAimMode(0.5f + this.duration, false);

			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
			}
			//intial pull
			if (base.isAuthority)
			{
				new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId,
					base.GetAimRay().origin - GetAimRay().direction,
					base.GetAimRay().direction,
					Modules.StaticValues.detroitdelawareDamageCoefficient).Send(NetworkDestination.Clients);
			}

			theSpot = aimRay.origin + blastRadius * aimRay.direction;
			theDirection = aimRay.direction;
			//blast attack
			blastAttack = new BlastAttack();
			blastAttack.radius = blastRadius;
			blastAttack.procCoefficient = 1f;
			blastAttack.position = theSpot;
			blastAttack.attacker = base.gameObject;
			blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
			blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.detroitdelawareDamageCoefficient;
			blastAttack.falloffModel = BlastAttack.FalloffModel.None;
			blastAttack.baseForce = maxWeight * 20f;
			blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
			blastAttack.damageType = DamageType.Freeze2s;
			blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

			PlayCrossfade("FullBody, Override", "DetroitDelawareDelaware", "Attack.playbackRate", fireTime, 0.01f);

            if (base.isAuthority)
			{
				AkSoundEngine.PostEvent("delawaredetroitvoice", this.gameObject);
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
			if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(FOV, 60f, base.fixedAge / duration);

			if (base.fixedAge > fireTime)
			{
				timer += Time.fixedDeltaTime;
                if (base.isAuthority && timer > 0.5f)
				{
					timer = 0f;
					new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId,
						base.GetAimRay().origin - GetAimRay().direction,
						theDirection,
						0f).Send(NetworkDestination.Clients);
				}
                if (!animChange)
				{
					animChange = true;
					PlayCrossfade("FullBody, Override", "DetroitDelawareDetroit", "Attack.playbackRate", exitDuration - fireTime, 0.01f);
				}
			}
			if(base.fixedAge > exitDuration && animChange)
			{
				animChange = false;
				PlayCrossfade("FullBody, Override", "DetroitDelawareSmash", "Attack.playbackRate", fireTime, 0.01f);
			}

			if(base.fixedAge > duration)
			{
				if (blastAttack.Fire().hitCount > 0)
				{
					this.OnHitEnemyAuthority();
				}

				EffectManager.SpawnEffect(Modules.Assets.detroitEffect, new EffectData
				{
					origin = base.transform.position,
					scale = blastRadius,
					rotation = Quaternion.LookRotation(Vector3.up)
				}, true);
				EffectManager.SpawnEffect(Modules.Assets.mageLightningBombEffectPrefab, new EffectData
				{
					origin = base.transform.position,
					scale = blastRadius,
					rotation = Quaternion.LookRotation(Vector3.up)
				}, true);


				this.outer.SetNextStateToMain();
			}
			
		}
        public override void OnExit()
        {
            base.OnExit();

			if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
		}
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}