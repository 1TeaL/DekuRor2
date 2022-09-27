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
		public static float baseDuration = 5f;
		private float duration;
		private float fireTime = 0.5f;
		private float baseBlastRadius = 20f;
		private float blastRadius;
		private float FOV = 120f;

		private BlastAttack blastAttack;
		public Vector3 theSpot;
		public Vector3 theDirection;
        private float maxWeight;

        public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			blastRadius = baseBlastRadius * attackSpeedStat;
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


		}

		protected virtual void OnHitEnemyAuthority()
		{
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
                if (base.isAuthority)
				{
					new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId,
						base.GetAimRay().origin - GetAimRay().direction,
						theDirection,
						0f).Send(NetworkDestination.Clients);
				}
			}

			if(base.fixedAge > duration)
			{
				if (blastAttack.Fire().hitCount > 0)
				{
					this.OnHitEnemyAuthority();
				}

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