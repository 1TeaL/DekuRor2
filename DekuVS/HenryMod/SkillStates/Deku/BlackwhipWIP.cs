//using System;
//using RoR2;
//using RoR2.Projectile;
//using UnityEngine;
//using EntityStates;
//using EntityStates.Loader;

//namespace DekuMod.SkillStates
//{

//	public class BlackwhipWIP : BaseSkillState
//	{
		
//		public GameObject projectilePrefab;
//		public static float damageCoefficient;
//		public static GameObject muzzleflashEffectPrefab;
//		public static string fireSoundString;
//		public GameObject hookInstance;
//		protected ProjectileStickOnImpact hookStickOnImpact;
//		private bool isStuck;
//		private bool hadHookInstance;
		
        
//        public override void OnEnter()
//		{
//			base.OnEnter();
//			if (base.isAuthority)
//			{
//				Ray aimRay = base.GetAimRay();
//				FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
//				{
//					position = aimRay.origin,
//					rotation = Quaternion.LookRotation(aimRay.direction),
//					crit = base.characterBody.RollCrit(),
//					damage = this.damageStat * BlackwhipWIP.damageCoefficient,
//					force = 0f,
//					damageColorIndex = DamageColorIndex.Default,
//					procChainMask = default(ProcChainMask),
//					projectilePrefab = this.projectilePrefab,
//					owner = base.gameObject
//				};
//				ProjectileManager.instance.FireProjectile(fireProjectileInfo);
//			}
//			EffectManager.SimpleMuzzleFlash(FireHook.muzzleflashEffectPrefab, base.gameObject, "MuzzleLeft", false);
//			Util.PlaySound(FireHook.fireSoundString, base.gameObject);
//			//base.PlayAnimation("LeftArm, Override", "FingerFlick", "Attack.playbackRate", 0.5f);
//			base.PlayAnimation("Grapple", "FireHookIntro");

//		}


//		public void SetHookReference(GameObject hook)
//		{
//			this.hookInstance = hook;
//			this.hookStickOnImpact = hook.GetComponent<ProjectileStickOnImpact>();
//			this.hadHookInstance = true;
//		}
//		ProjectileGrappleController

//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			if (this.hookStickOnImpact)
//			{
//				if (this.hookStickOnImpact.stuck && !this.isStuck)
//				{
//					base.PlayAnimation("Grapple", "FireHookLoop");
//				}
//				this.isStuck = this.hookStickOnImpact.stuck;
//			}
//			if (base.isAuthority && !this.hookInstance && this.hadHookInstance)
//			{
//				this.outer.SetNextStateToMain();
//			}
//		}


//		public override void OnExit()
//		{
//			base.PlayAnimation("Grapple", "FireHookExit");
//			//base.PlayAnimation("LeftArm, Override", "FingerFlick", "Attack.playbackRate", 0.5f);
//			EffectManager.SimpleMuzzleFlash(FireHook.muzzleflashEffectPrefab, base.gameObject, "MuzzleLeft", false);
//			base.OnExit();
//		}


//		public override InterruptPriority GetMinimumInterruptPriority()
//		{
//			return InterruptPriority.Pain;
//		}



//	}
//}
