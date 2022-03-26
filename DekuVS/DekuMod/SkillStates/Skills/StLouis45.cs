using System;
using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class StLouis45 : BaseSkillState
	{

		public static Vector3 CameraPosition = new Vector3(1.8f, -2.4f, -8f);
		public static float Force = 3000f;
		public static float ProcCoefficient = 1f;
		public static float baseRadius = 24f;
		public static float Radius;
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        //public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/GrandparentPreSpawnEffect");
        public float duration;
		protected Animator animator;
		protected float baseDuration = 1f;
		protected float EarlyExitTime = 0.6f;
		protected float startUp = 0.4f;
		protected float stopwatch;
		private bool hasFired;
		private Transform slamIndicatorInstance;
		public float speedattack;

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			speedattack = attackSpeedStat / 2;
			if (speedattack < 1)
			{
				speedattack = 1;
			}

			if (!this.slamIndicatorInstance)
			{
				this.CreateIndicator();
			}
			this.UpdateSlamIndicator();
			if (this.stopwatch >= this.startUp && !this.hasFired)
			{
				this.hasFired = true;

				AkSoundEngine.PostEvent(1918362945, this.gameObject);
				if (NetworkServer.active)
				{
					this.Fire();
				}
                for (int i = 0; i <= 20; i++)
                {
                    float num = 60f;
                Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
                float num2 = 0.01f;
                rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
                EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
                {
                    origin = base.transform.position,
                    scale = Radius * 2,
                    rotation = rotation
                }, false);

                }
            }
			if (this.stopwatch >= this.EarlyExitTime && base.isAuthority && this.hasFired)
			{
				this.outer.SetNextStateToMain();
				return;
			}

			




		}


		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}


		public override void OnEnter()
		{
			base.OnEnter();
			base.StartAimMode(0.5f + this.duration, false);
			this.animator = base.GetModelAnimator();
			this.hasFired = false;
			this.duration = this.baseDuration;
			base.characterMotor.velocity = Vector3.zero;
			Radius = baseRadius * this.attackSpeedStat/2;

			//base.PlayCrossfade("Fullbody, Override", "LegSmash", startUp);
			//base.PlayAnimation("Fullbody, Override" "LegSmash", "Attack.playbackRate", startUp);
			base.PlayCrossfade("Fullbody, Override", "LegSmash", duration / 2);

			AkSoundEngine.PostEvent(687990298, this.gameObject);
			if (!this.slamIndicatorInstance)
			{
				this.CreateIndicator();
			}
		}

		public override void OnExit()
		{
			//base.PlayCrossfade("Fullbody, Override", "LegSmashExit", 0.1f);
            //base.PlayAnimation("Fullbody, Override", "LegSmashExit", "Attack.playbackRate", 0.1f);
            //base.PlayAnimation("Body, Override", "IdleIn", "Attack.playbackRate", 0.01f);

			if (this.slamIndicatorInstance)
			{
				EntityState.Destroy(this.slamIndicatorInstance.gameObject);
			}
			base.OnExit();
		}


		protected virtual void OnHitEnemyAuthority()
		{
			base.healthComponent.Heal(((healthComponent.health / 20) * speedattack), default(ProcChainMask), true);
		}

		private void CreateIndicator()
		{
			if (ArrowRain.areaIndicatorPrefab)
			{
				Vector3 position = base.transform.position + base.characterDirection.forward * (Radius - 2);
				this.slamIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab).transform;
				this.slamIndicatorInstance.transform.position = position;
				this.slamIndicatorInstance.localScale = Vector3.one * Radius;
			}
		}

		private void Fire()
		{

            //base.GetAimRay();
            Collider[] array = Physics.OverlapSphere(base.transform.position + base.characterDirection.forward * Radius, (Radius + 2));
			int num = 0;
			int num2 = 0;
			while (num < array.Length && (float)num2 < 1000000f)
			{
				HealthComponent component = array[num].GetComponent<HealthComponent>();
				if (component)
				{
					TeamComponent component2 = component.GetComponent<TeamComponent>();
					bool flag = false;
					if (component2)
					{
						flag = (component2.teamIndex == base.GetTeam());
					}
					if (!flag)
					{
						
						DamageInfo damageInfo = new DamageInfo();
						damageInfo.damage = this.damageStat * Modules.StaticValues.stlouis45DamageCoefficient;
						damageInfo.attacker = base.gameObject;
						damageInfo.inflictor = base.gameObject;
						damageInfo.force = base.GetAimRay().direction.normalized * Force;
						damageInfo.crit = base.RollCrit();
						damageInfo.procCoefficient = ProcCoefficient;
						damageInfo.position = component.transform.position;
						damageInfo.damageType = DamageType.Stun1s;
						component.TakeDamage(damageInfo);
						GlobalEventManager.instance.OnHitEnemy(damageInfo, component.gameObject);
						GlobalEventManager.instance.OnHitAll(damageInfo, component.gameObject);
						num2++;
					}
				}
				num++;
			}
		}

		private void UpdateSlamIndicator()
		{
			if (this.slamIndicatorInstance)
			{
				Vector3 position = base.transform.position + base.characterDirection.forward * Radius;
				this.slamIndicatorInstance.transform.position = position;
			}
		}


	}
}
