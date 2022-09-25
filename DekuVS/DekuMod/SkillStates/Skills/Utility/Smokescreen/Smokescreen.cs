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
			dekucon = base.GetComponent<DekuController>();
			Ray aimRay = base.GetAimRay();
			theSpot = aimRay.origin + 0 * aimRay.direction;
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreenDuration);
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreenDuration);
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
				this.outer.SetNextStateToMain();
			}

		}


		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}

	}
}
