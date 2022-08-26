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

	public class Smokescreen : BaseSkillState
	{
		public static float baseDuration = 4f;
		public static float radius = 15f;
		public DekuController dekucon;

		public Vector3 theSpot;
		public CharacterBody body;
		public float fajin;
		private float duration;
		public bool hasFired;
		private BlastAttack blastAttack;
		private GameObject smokeprefab = Modules.Assets.smokeEffect;
		private GameObject smokebigprefab = Modules.Assets.smokebigEffect;
		public BuffDef buffdef = RoR2Content.Buffs.Cloak;
		public BuffDef buffdef2 = RoR2Content.Buffs.CloakSpeed;


		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			hasFired = false;
			dekucon = base.GetComponent<DekuController>();
			Ray aimRay = base.GetAimRay();
			theSpot = aimRay.origin + 0 * aimRay.direction;
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, duration);
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, duration);
			}

			Util.PlaySound(StealthMode.enterStealthSound, base.gameObject);
			//base.PlayAnimation("FullBody, Override", "OFA","Attack.playbackRate", 1f);
			dekucon.AddToBuffCount(10);

			if (dekucon.isMaxPower)
			{
				dekucon.RemoveBuffCount(50);
				fajin = 2f;
				if (base.isAuthority)
				{
					Vector3 effectPosition = base.characterBody.corePosition;
					effectPosition.y = base.characterBody.corePosition.y;
					EffectManager.SpawnEffect(this.smokebigprefab, new EffectData
					{
						origin = effectPosition,
						scale = radius * fajin,
						rotation = Quaternion.LookRotation(Vector3.down)
					}, true);

				}
			}
            else
            {
				fajin = 1f;
				if (base.isAuthority)
				{
					Vector3 effectPosition = base.characterBody.corePosition;
					effectPosition.y = base.characterBody.corePosition.y;
					EffectManager.SpawnEffect(this.smokeprefab, new EffectData
					{
						origin = effectPosition,
						scale = radius * fajin,
						rotation = Quaternion.LookRotation(Vector3.up)
					}, true);

				}
			}

			if (dekucon.isMaxPower)
			{
				EffectManager.SpawnEffect(Modules.Assets.impactEffect, new EffectData
				{
					origin = base.transform.position,
					scale = 1f,
					rotation = Quaternion.LookRotation(aimRay.direction)
				}, false);


				float radiusSqr = radius * radius;
				Vector3 position = base.transform.position;

				if (NetworkServer.active)
				{
					this.BuffTeam(TeamComponent.GetTeamMembers(TeamIndex.Player), radiusSqr, position);
				}
			}

			blastAttack = new BlastAttack();

			if (dekucon.isMaxPower)
			{
				blastAttack.damageType = DamageType.Stun1s;
			}
			else
			{
				blastAttack.damageType = DamageType.SlowOnHit;
			}
			blastAttack.position = base.transform.position;
			blastAttack.baseDamage = this.damageStat * Modules.StaticValues.smokescreenDamageCoefficient;
			blastAttack.baseForce = 1600f * fajin;
			blastAttack.radius = Smokescreen.radius * fajin;
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
							body.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, duration);
							body.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, duration);

						}
					}
				}
			}
		}

		public override void OnExit()
        {
			//dekucon.wardTrue = false;
			//UnityEngine.Object.Destroy(this.affixHauntedWard);
			//this.affixHauntedWard = null;
			Util.PlaySound(StealthMode.exitStealthSound, base.gameObject);

			//base.PlayCrossfade("FullBody, Override", "BufferEmpty", 0f);
			base.OnExit();
		}
        public override void FixedUpdate()
		{

            //if (dekucon.isMaxPower)
            //{
            //    SmokescreenSearch();

            //}
            if (!hasFired)
            {
				hasFired = true;
				blastAttack.position = base.transform.position;
				blastAttack.Fire();

            }

			this.outer.SetNextStateToMain();
		}


		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}

		//public void SmokescreenSearch()
		//{
		//	Ray aimRay = base.GetAimRay();
		//	BullseyeSearch search = new BullseyeSearch
		//	{

		//		teamMaskFilter = TeamMask.AllExcept(TeamIndex.Monster),
		//		filterByLoS = false,
		//		searchOrigin = base.transform.position,
		//		searchDirection = UnityEngine.Random.onUnitSphere,
		//		sortMode = BullseyeSearch.SortMode.Distance,
		//		maxDistanceFilter = radius * fajin,
		//		maxAngleFilter = 360f
		//	};

		//	search.RefreshCandidates();
		//	search.FilterOutGameObject(base.gameObject);


		//	, 
		//	List<HurtBox> target = search.GetResults().ToList<HurtBox>();
		//	foreach (HurtBox singularTarget in target)
		//	{
		//		if (singularTarget)
		//		{
		//			if (singularTarget.healthComponent && singularTarget.healthComponent.body)
		//			{
		//				//bool active = NetworkServer.active;
		//				//if (active)
		//				//{
		//					singularTarget.healthComponent.body.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, duration);
		//					singularTarget.healthComponent.body.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, duration);
  //                      //}
  //                  }
  //              }
		//	}
		//}
	}
}
