using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EntityStates.Bandit2;

namespace DekuMod.SkillStates
{

	public class Smokescreen100 : BaseQuirk100
	{
		public static float baseDuration = 4f;
		public static float radius = 15f;

		public Vector3 theSpot;
		public CharacterBody body;
		private float duration;
		public bool hasFired;
		private BlastAttack blastAttack;
		private GameObject smokeprefab = Modules.Assets.smokeEffect;
		private GameObject smokebigprefab = Modules.Assets.smokebigEffect;
        private float maxWeight;

        public override void OnEnter()
		{
			base.OnEnter();

		}

		protected override void DoSkill()
		{
			this.duration = baseDuration;
			hasFired = false;
			dekucon = base.GetComponent<DekuController>();
			Ray aimRay = base.GetAimRay();
			theSpot = aimRay.origin + 0 * aimRay.direction;
			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreen100Duration);
				base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreen100Duration);
			}

			Util.PlaySound(StealthMode.enterStealthSound, base.gameObject);
			//base.PlayAnimation("FullBody, Override", "OFA","Attack.playbackRate", 1f);

			if (base.isAuthority)
			{
				Vector3 effectPosition = base.characterBody.corePosition;
				effectPosition.y = base.characterBody.corePosition.y;
				EffectManager.SpawnEffect(this.smokebigprefab, new EffectData
				{
					origin = effectPosition,
					scale = radius,
					rotation = Quaternion.LookRotation(Vector3.down)
				}, true);

			}


			float radiusSqr = radius * radius;
			Vector3 position = base.transform.position;

			if (NetworkServer.active)
			{
				this.BuffTeam(TeamComponent.GetTeamMembers(TeamIndex.Player), radiusSqr, position);
			}

			GetMaxWeight();

			blastAttack = new BlastAttack();

			blastAttack.position = base.transform.position;
			blastAttack.baseDamage = this.damageStat * Modules.StaticValues.smokescreenDamageCoefficient;
			blastAttack.baseForce = 100f * maxWeight;
			blastAttack.damageType = DamageType.SlowOnHit;
			blastAttack.radius = radius;
			blastAttack.attacker = base.gameObject;
			blastAttack.inflictor = base.gameObject;
			blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
			blastAttack.crit = base.RollCrit();
			blastAttack.procChainMask = default(ProcChainMask);
			blastAttack.procCoefficient = 1f;
			blastAttack.falloffModel = BlastAttack.FalloffModel.None;
			blastAttack.damageColorIndex = DamageColorIndex.Default;
			blastAttack.attackerFiltering = AttackerFiltering.Default;

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
				maxDistanceFilter = radius,
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

		private void BuffTeam(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
		{
			bool flag = !NetworkServer.active;
			if (!flag)
			{
				foreach (TeamComponent teamComponent in recipients)
				{
					bool flag2 = (teamComponent.transform.position - currentPosition).sqrMagnitude <= radiusSqr;
					if (flag2)
					{
						CharacterBody body = teamComponent.body;
						bool flag3 = body;
						if (flag3)
						{
							body.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreen100Duration);
							body.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreen100Duration);

						}
					}
				}
			}
		}

		public override void OnExit()
        {
			Util.PlaySound(StealthMode.exitStealthSound, base.gameObject);

			base.OnExit();
		}
        public override void FixedUpdate()
		{

            if (!hasFired)
            {
				hasFired = true;
				blastAttack.position = base.transform.position;
				blastAttack.Fire();
				this.outer.SetNextStateToMain();
			}

		}


		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}

		
	}
}
