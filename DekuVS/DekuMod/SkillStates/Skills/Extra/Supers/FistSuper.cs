using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace DekuMod.SkillStates
{

	public class FistSuper : BaseSpecial
	{
		public static float baseDuration = 5f;
		private float duration;
		private float fireTime = 0.5f;
		private float blastRadius = 20f;

		private BlastAttack blastAttack;
		public Vector3 theSpot;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			Ray aimRay = base.GetAimRay();
			base.StartAimMode(0.5f + this.duration, false);

			bool active = NetworkServer.active;
			if (active)
			{ 				
				base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
			}
			//intial pull
			new PerformBlackwhipNetworkRequest(base.characterBody.masterObjectId, 
				base.GetAimRay().origin - GetAimRay().direction, 
				base.GetAimRay().direction, 
				Modules.StaticValues.detroitdelawareDamageCoefficient).Send(NetworkDestination.Clients);


			theSpot = aimRay.origin + blastRadius * aimRay.direction;
			//blast attack
			blastAttack = new BlastAttack();
			blastAttack.radius = blastRadius * this.attackSpeedStat;
			blastAttack.procCoefficient = 1f;
			blastAttack.position = theSpot;
			blastAttack.attacker = base.gameObject;
			blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
			blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.detroitdelawareDamageCoefficient;
			blastAttack.falloffModel = BlastAttack.FalloffModel.None;
			blastAttack.baseForce = 5000f;
			blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
			blastAttack.damageType = DamageType.Freeze2s;
			blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;


		}

		protected virtual void OnHitEnemyAuthority()
		{
			//base.healthComponent.AddBarrierAuthority((healthComponent.fullCombinedHealth / 30) * this.attackSpeedStat);

		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if(base.fixedAge > fireTime)
			{
				new PerformBlackwhipNetworkRequest(base.characterBody.masterObjectId,
					base.GetAimRay().origin - GetAimRay().direction,
					base.GetAimRay().direction,
					0f).Send(NetworkDestination.Clients);
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
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}