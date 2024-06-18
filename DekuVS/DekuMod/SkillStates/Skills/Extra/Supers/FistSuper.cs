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
using EntityStates.Merc;
using DekuMod.Modules;

namespace DekuMod.SkillStates
{

	public class FistSuper : BaseSpecial
	{
		public float baseFireInterval = 0.4f;
		public float fireInterval;
		private float duration = 4f;
		private float exitDuration = 3.5f;
		private float fireTime = 0.5f;
		private float baseBlastRadius = Modules.StaticValues.detroitdelawareBlastRadius;
		private float blastRadius;
		private float FOV = 120f;

		private BlastAttack blastAttack;
		public Vector3 theSpot;
        private float maxWeight;
        private float timer;

		private bool animChange;
		private string muzzleString;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        private EffectData effectData1;
		private EffectData effectData2;

        public override void OnEnter()
		{
			base.OnEnter();					
			dekucon = base.GetComponent<DekuController>();
			energySystem = base.GetComponent<EnergySystem>();
			blastRadius = baseBlastRadius * attackSpeedStat;
			fireInterval = baseFireInterval;
			base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
			this.muzzleString = "RHand";

			Ray aimRay = base.GetAimRay();
			theSpot = base.transform.position;

			//blast attack
			blastAttack = new BlastAttack();
			blastAttack.radius = blastRadius;
			blastAttack.procCoefficient = 1f;
			blastAttack.position = theSpot;
			blastAttack.attacker = base.gameObject;
			blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
			blastAttack.baseDamage = base.damageStat * Modules.StaticValues.detroitdelawareSmashDamageCoefficient * attackSpeedStat;
			blastAttack.falloffModel = BlastAttack.FalloffModel.None;
			blastAttack.baseForce = maxWeight * 10f;
			blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
			blastAttack.damageType = DamageType.Freeze2s;
			blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

			effectData1 = new EffectData
			{
				scale = 1f,
				origin = base.characterBody.corePosition,
				rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
			};

			effectData2 = new EffectData
			{
				scale = 1f,
				origin = base.characterBody.corePosition,
				rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
			};

            if (energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
            {
                energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);

                if (NetworkServer.active)
                {
                    base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration);
                }


                if (base.isAuthority)
                {
                    base.StartAimMode(0.5f + this.duration, false);
                    new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId).Send(NetworkDestination.Clients);


                    EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
                    EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, this.muzzleString, false);


                    //EffectManager.SpawnEffect(Modules.Projectiles.delawareTracer, effectData2, true);


                    PlayCrossfade("FullBody, Override", "DetroitDelawareFull", "Attack.playbackRate", fireTime, 0.01f);

                    AkSoundEngine.PostEvent("delawaredetroitvoice", this.gameObject);

                }

            }
            else
            {
                if (base.isAuthority)
                {
                    Chat.AddMessage($"You need {Modules.StaticValues.specialPlusUltraSpend} plus ultra.");
                    this.outer.SetNextStateToMain();
                    return;

                }
            }


		}

        public override void FixedUpdate()
		{
			base.FixedUpdate();


			if (base.fixedAge > fireTime)
			{
				if (base.isAuthority && timer > fireInterval)
				{
					timer = 0f;

                    new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId).Send(NetworkDestination.Clients);

                }
				else
				{
					timer += Time.fixedDeltaTime;
				}

			}
            if (base.fixedAge > exitDuration && !animChange && base.isAuthority)
			{
				animChange = true;
				AkSoundEngine.PostEvent("shootstyedashcomboimpact", this.gameObject);
				EffectManager.SpawnEffect(Modules.Assets.mageLightningBombEffectPrefab, new EffectData
				{
					origin = theSpot,
					scale = blastRadius,
					rotation = Quaternion.LookRotation(GetAimRay().direction)
				}, true);
			}

            if (base.fixedAge > duration && base.isAuthority)
            {
                new PerformDetroitDelawareNetworkRequest(base.characterBody.masterObjectId).Send(NetworkDestination.Clients);
                blastAttack.Fire();
				AkSoundEngine.PostEvent("impactsfx", this.gameObject);

				EffectManager.SpawnEffect(Modules.Assets.detroitEffect, effectData2, true);

				this.outer.SetNextStateToMain();
				return;
			}
			
		}
        public override void OnExit()
        {
            base.OnExit();

			//if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
		}
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Death;
		}
	}
}