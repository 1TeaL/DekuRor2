using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EntityStates.Bandit2;
using System;

namespace DekuMod.SkillStates
{

	public class Smokescreen45 : BaseQuirk45  
	{
		public static float radius = 15f;

		public Vector3 theSpot;
		public CharacterBody body;
		public bool hasFired;
		private BlastAttack blastAttack;
		private GameObject smokeprefab = Modules.Assets.smokeEffect;
		private GameObject smokebigprefab = Modules.Assets.smokebigEffect;
        private float maxWeight;

        public override void OnEnter()
		{
			base.OnEnter();
			hasFired = false;
			theSpot = base.transform.position;
		}

		protected override void DoSkill()
		{
			base.DoSkill();
			Ray aimRay = base.GetAimRay();
			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreen45Duration);
				base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreen45Duration);
			}

			Util.PlaySound(StealthMode.enterStealthSound, base.gameObject);
			//base.PlayAnimation("FullBody, Override", "OFA","Attack.playbackRate", 1f);

			if (base.isAuthority)
			{
				Vector3 effectPosition = base.characterBody.corePosition;
				effectPosition.y = base.characterBody.corePosition.y;
				EffectManager.SpawnEffect(this.smokeprefab, new EffectData
				{
					origin = effectPosition,
					scale = radius,
					rotation = Quaternion.LookRotation(Vector3.up)
				}, true);

			}

		}

		protected override void DontDoSkill()
		{
			base.DontDoSkill();
			skillLocator.utility.AddOneStock();
		}


		public override void OnExit()
        {
			base.OnExit();
		}
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!hasFired && base.fixedAge > 0.1f && base.isAuthority)
            {
				hasFired = true;
				Util.PlaySound(StealthMode.exitStealthSound, base.gameObject);
				blastAttack.position = base.transform.position;
				blastAttack.Fire();
				this.outer.SetNextStateToMain();
                return;

            }

		}


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
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
