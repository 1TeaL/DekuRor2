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
		public static float baseDuration = 5f;
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


		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			hasFired = false;
			dekucon = base.GetComponent<DekuController>();
			Ray aimRay = base.GetAimRay();
			theSpot = aimRay.origin + 0 * aimRay.direction;
            //bool active = NetworkServer.active;
            //if (active)
            //{
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, duration);
			base.characterBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed.buffIndex, duration);
            //}

			Util.PlaySound(StealthMode.enterStealthSound, base.gameObject);
			//base.PlayAnimation("FullBody, Override", "OFA","Attack.playbackRate", 1f);

			if (dekucon.isMaxPower)
            {
				fajin = 2f;
				if (base.isAuthority)
				{
					Vector3 effectPosition = base.characterBody.corePosition;
					effectPosition.y = base.characterBody.corePosition.y;
					EffectManager.SpawnEffect(this.smokebigprefab, new EffectData
					{
						origin = effectPosition,
						scale = radius * fajin,
						rotation = Quaternion.LookRotation(Vector3.up)
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



        public override void OnExit()
        {
			//dekucon.wardTrue = false;
			dekucon.RemoveBuffCount(50);
			//UnityEngine.Object.Destroy(this.affixHauntedWard);
			//this.affixHauntedWard = null;
			Util.PlaySound(StealthMode.exitStealthSound, base.gameObject);

			//base.PlayCrossfade("FullBody, Override", "BufferEmpty", 0f);
			base.OnExit();
		}
        public override void FixedUpdate()
		{
			
			if (dekucon.CheckIfMaxPowerStacks())
			{
                if (base.isAuthority)
                {
					SmokescreenSearch();
                }
			}
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
		
		//public void OnHitEnemyAuthority()
  //      {
		//	base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, duration);
		//}

		//public void ApplySmokeScreen()
		//{
		//	if (!NetworkServer.active)
		//	{
		//		return;
		//	}
		//	if (!dekucon.wardTrue)
		//	{
		//		dekucon.wardTrue = true;

		//		this.affixHauntedWard = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/AffixHauntedWard"));
		//		this.affixHauntedWard.GetComponent<TeamFilter>().teamIndex = this.body.teamComponent.teamIndex;
		//		this.affixHauntedWard.GetComponent<BuffWard>().Networkradius = Smokescreen.radius * fajin;
		//		this.affixHauntedWard.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(this.gameObject);
		//	}
		//}
		public void SmokescreenSearch()
		{
			Ray aimRay = base.GetAimRay();
			BullseyeSearch search = new BullseyeSearch
			{

				teamMaskFilter = TeamMask.AllExcept(TeamIndex.Monster),
				filterByLoS = false,
				searchOrigin = base.transform.position,
				searchDirection = UnityEngine.Random.onUnitSphere,
				sortMode = BullseyeSearch.SortMode.Distance,
				maxDistanceFilter = radius * fajin,
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
                        if (NetworkServer.active)
                        {
							singularTarget.healthComponent.body.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, duration);

							singularTarget.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.CloakSpeed.buffIndex, duration);
						}
					}
				}
			}
		}
	}
}
