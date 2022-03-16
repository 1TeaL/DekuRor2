using System;
using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using UnityEngine;

namespace DekuMod.SkillStates
{
    internal class Detroit45Release : BaseSkillState
    {
        internal float damageMult;
        internal float radius;
        private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        //private string lMuzzleString = "LFinger";
        private string rMuzzleString = "RHand";
        internal Vector3 moveVec;
		//private GameObject explosionPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
		private GameObject explosionPrefab = Modules.Projectiles.detroitTracer;
		private float baseForce = 600f;
		public float procCoefficient = 2f;

		public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");



		public override void OnEnter()
        {
			
			base.OnEnter();
            base.characterMotor.velocity = Vector3.zero;
			base.PlayAnimation("FullBody, Override", "SmashChargeAttack", "Attack.playbackRate", 1f);
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
				BlastAttack blastAttack = new BlastAttack();

				blastAttack.position = base.characterBody.corePosition;
				blastAttack.baseDamage = this.damageStat * this.damageMult;
				blastAttack.baseForce = this.baseForce * this.damageMult;
				blastAttack.radius = this.radius;
				blastAttack.attacker = base.gameObject;
				blastAttack.inflictor = base.gameObject;
				blastAttack.teamIndex = base.teamComponent.teamIndex;
				blastAttack.crit = base.RollCrit();
				blastAttack.procChainMask = default(ProcChainMask);
				blastAttack.procCoefficient = procCoefficient;
				blastAttack.falloffModel = BlastAttack.FalloffModel.None;
				blastAttack.damageColorIndex = DamageColorIndex.Default;
				blastAttack.damageType = DamageType.Stun1s;
				blastAttack.attackerFiltering = AttackerFiltering.Default;

				if (blastAttack.Fire().hitCount > 0)
				{
					this.OnHitEnemyAuthority();
				}
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
		protected virtual void OnHitEnemyAuthority()
		{
			Ray aimRay = base.GetAimRay();

			EffectData effectData = new EffectData
			{
				scale = this.radius * 2f,
				origin = base.characterBody.corePosition,
				rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z)),
			};
			EffectManager.SpawnEffect(this.explosionPrefab, effectData, true);
		}
	}


}