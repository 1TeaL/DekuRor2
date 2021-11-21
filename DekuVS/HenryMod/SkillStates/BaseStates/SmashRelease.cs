using System;
using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using UnityEngine;

namespace DekuMod.SkillStates.BaseStates
{
    internal class SmashRelease : BaseSkillState
    {
        internal float damageMult;
        internal float radius;
        private GameObject muzzlePrefab = Resources.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        //private string lMuzzleString = "LFinger";
        private string rMuzzleString = "RShoulder";
        internal Vector3 moveVec;
		private GameObject explosionPrefab = Resources.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
		private float baseForce = 600f;
		
		

		public override void OnEnter()
        {
			
			base.OnEnter();
            base.characterMotor.velocity = Vector3.zero;
			base.PlayAnimation("FullBody, Override", "SmashChargeAttack", "Attack.playbackRate", 0.3f);
			Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
            //EffectManager.SimpleMuzzleFlash(this.muzzlePrefab, base.gameObject, this.lMuzzleString, false);
            EffectManager.SimpleMuzzleFlash(this.muzzlePrefab, base.gameObject, this.rMuzzleString, false);
            base.characterMotor.rootMotion += this.moveVec;
            //base.characterMotor.velocity += this.moveVec* 4;

        }
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
		public override void OnExit()
		{
			
			base.OnExit();
			EffectData effectData = new EffectData
			{
				scale = this.radius * 2f,
				origin = base.characterBody.corePosition
			};
			EffectManager.SpawnEffect(this.explosionPrefab, effectData, true);
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
					procCoefficient = 1f,
					falloffModel = BlastAttack.FalloffModel.None,
					damageColorIndex = DamageColorIndex.Default,
					damageType = DamageType.Stun1s,
					attackerFiltering = AttackerFiltering.Default
				}.Fire();
			}
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