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

	public class Smokescreen : BaseQuirk
	{
		public static float radius = 15f;

		public Vector3 theSpot;
		public CharacterBody body;
		public bool hasFired;
		private BlastAttack blastAttack;
		private GameObject smokeprefab = Modules.Assets.smokeEffect;
		private GameObject smokebigprefab = Modules.Assets.smokebigEffect;


		public override void OnEnter()
		{
			base.OnEnter();
            hasFired = false;
			theSpot = base.transform.position;
		}

		protected override void DoSkill()
		{
			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreenDuration);
				base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreenDuration);
			}

			float radiusSqr = radius * radius;
			Vector3 position = base.transform.position;

			if (NetworkServer.active)
			{
				this.BuffTeam(TeamComponent.GetTeamMembers(TeamIndex.Player), radiusSqr, position);
			}


			Util.PlaySound(StealthMode.enterStealthSound, base.gameObject);

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
							body.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreenDuration);
							body.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreenDuration);

						}
					}
				}
			}
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
				this.outer.SetNextStateToMain();
                return;
            }

		}


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
