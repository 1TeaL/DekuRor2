using System;
using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using UnityEngine;

namespace DekuMod.SkillStates.BaseStates
{
    internal class DetroitRelease : BaseSkillState
    {
        internal float damageMult;
        internal float radius;
        private GameObject muzzlePrefab = Resources.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        //private string lMuzzleString = "LFinger";
        private string rMuzzleString = "RShoulder";
        internal Vector3 moveVec;
		//private GameObject explosionPrefab = Resources.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
		private GameObject explosionPrefab = Modules.Projectiles.detroitweakTracer;
		private float baseForce = 600f;

		public GameObject blastEffectPrefab = Resources.Load<GameObject>("Prefabs/effects/SonicBoomEffect");


		public override void OnEnter()
        {
			
			base.OnEnter();
            base.characterMotor.velocity = Vector3.zero;
			base.PlayAnimation("FullBody, Override", "SmashChargeAttack", "Attack.playbackRate", 0.3f);
			//Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
			AkSoundEngine.PostEvent(3289116818, this.gameObject);
			//EffectManager.SimpleMuzzleFlash(this.muzzlePrefab, base.gameObject, this.lMuzzleString, false);
			EffectManager.SimpleMuzzleFlash(this.muzzlePrefab, base.gameObject, this.rMuzzleString, false);
            base.characterMotor.rootMotion += this.moveVec;
            //base.characterMotor.velocity += this.moveVec * 2;

        }
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
		public override void OnExit()
        {

            Ray aimRay = base.GetAimRay();

			EffectData effectData = new EffectData
			{
				scale = this.radius * 2f,
				origin = base.characterBody.corePosition,
				rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
			};
			EffectManager.SpawnEffect(this.explosionPrefab, effectData, true);

			for (int i = 0; i <= 20; i++)
			{
				float num = 60f;
				Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
				float num2 = 0.01f;
				rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
				rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
				EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
				{
					origin = base.characterBody.corePosition,
					scale = this.radius * 2,
					rotation = rotation
				}, false);
			}

			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				new BlastAttack
				{
					position = base.characterBody.corePosition,
					baseDamage = this.damageStat * this.damageMult,
					baseForce = this.baseForce * this.damageMult,
					radius = this.radius,
					attacker = base.gameObject,
					inflictor = base.gameObject,
					teamIndex = base.teamComponent.teamIndex,
					crit = base.RollCrit(),
					procChainMask = default(ProcChainMask),
					procCoefficient = 3f,
					falloffModel = BlastAttack.FalloffModel.None,
					damageColorIndex = DamageColorIndex.Default,
					damageType = DamageType.Stun1s,
					attackerFiltering = AttackerFiltering.Default
					

				}.Fire();
			}
			base.OnExit();
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= 0.1f && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextStateToMain();
			}
		}
	}


}