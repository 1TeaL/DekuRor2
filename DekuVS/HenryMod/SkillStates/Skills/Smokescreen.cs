using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace DekuMod.SkillStates
{

	public class Smokescreen : BaseSkillState
	{
		public static float baseDuration = 5f;
		public static float radius = 30f;
		public DekuController dekucon;

		public Vector3 theSpot;
		private GameObject affixHauntedWard;
		public CharacterBody body;
		public int stack;
	

	private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			dekucon = base.GetComponent<DekuController>();
			Ray aimRay = base.GetAimRay();
			theSpot = aimRay.origin + 0 * aimRay.direction;
			//bool active = NetworkServer.active;
			//if (active)
			//{
			//	base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.AffixHauntedRecipient.buffIndex, duration);
			//}
			base.PlayAnimation("FullBody, Override", "OFA","Attack.playbackRate", duration);

            BlastAttack blastAttack = new BlastAttack();

            blastAttack.position = theSpot;
            blastAttack.baseDamage = this.damageStat;
            blastAttack.baseForce = 800f;
            blastAttack.radius = Smokescreen.radius;
            blastAttack.attacker = base.gameObject;
            blastAttack.inflictor = base.gameObject;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.crit = base.RollCrit();
            blastAttack.procChainMask = default(ProcChainMask);
            blastAttack.procCoefficient = 1f;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.damageColorIndex = DamageColorIndex.Count;
            blastAttack.damageType = DamageType.SlowOnHit;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            //if (blastAttack.Fire().hitCount > 0)
            //{
            //	this.OnHitEnemyAuthority();
            //}
        }
        public override void FixedUpdate()
		{
			ApplySmokeScreen();
			this.outer.SetNextStateToMain();
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
		
		public void OnHitEnemyAuthority()
        {
			base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, duration);
		}

		public void ApplySmokeScreen()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			bool flag = this.stack > 0;
			if (this.affixHauntedWard != flag)
			{
				if (flag)
				{
					this.affixHauntedWard = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/AffixHauntedWard"));
					this.affixHauntedWard.GetComponent<TeamFilter>().teamIndex = this.body.teamComponent.teamIndex;
					this.affixHauntedWard.GetComponent<BuffWard>().Networkradius = 30f + Smokescreen.radius;
					this.affixHauntedWard.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(this.body.gameObject);
					return;
				}
				UnityEngine.Object.Destroy(this.affixHauntedWard);
				this.affixHauntedWard = null;
			}
		}
		private void OnDisable()
		{
			if (this.affixHauntedWard)
			{
				UnityEngine.Object.Destroy(this.affixHauntedWard);
			}
		}
	}
}
