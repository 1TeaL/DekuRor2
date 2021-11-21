//using RoR2;
//using UnityEngine;
//using UnityEngine.Networking;
//using EntityStates;
//using EntityStates.Treebot;
//using System.Collections.Generic;
//using RoR2.Projectile;
//using RoR2.Orbs;
//using EntityStates.Treebot.TreebotFlower;

//namespace DekuMod.SkillStates
//{ 
//	public class BlackwhipProjectile : BaseSkillState
//	{

//		public static float yankIdealDistance;
//		public static AnimationCurve yankSuitabilityCurve;
//		public static float healthFractionYieldPerHit;
//		public static float radius;
//		public static float healPulseCount;
//		public static float duration;
//		public static float rootPulseCount;
//		public static string enterSoundString;
//		public static string exitSoundString;
//		public static GameObject enterEffectPrefab;
//		public static GameObject exitEffectPrefab;
//		private List<CharacterBody> rootedBodies;
//		private float healTimer;
//		private float rootPulseTimer;
//		private GameObject owner;
//		private ProcChainMask procChainMask;
//		private float procCoefficient;
//		private TeamIndex teamIndex = TeamIndex.None;
//		private float damage;
//		private DamageType damageType;
//		private bool crit;
//		private float healPulseHealthFractionValue;

//		public override void OnEnter()
//		{
//			base.OnEnter();
//			ProjectileController component = base.GetComponent<ProjectileController>();
//			if (component)
//			{
//				this.owner = component.owner;
//				this.procChainMask = component.procChainMask;
//				this.procCoefficient = component.procCoefficient;
//				this.teamIndex = component.teamFilter.teamIndex;
//			}
//			ProjectileDamage component2 = base.GetComponent<ProjectileDamage>();
//			if (component2)
//			{
//				this.damage = component2.damage;
//				this.damageType = component2.damageType;
//				this.crit = component2.crit;
//			}
//			if (NetworkServer.active)
//			{
//				this.rootedBodies = new List<CharacterBody>();
//			}
//			base.PlayAnimation("Base", "SpawnToIdle");
//			Util.PlaySound(TreebotFlower2Projectile.enterSoundString, base.gameObject);
//			if (TreebotFlower2Projectile.enterEffectPrefab)
//			{
//				EffectManager.SimpleEffect(TreebotFlower2Projectile.enterEffectPrefab, base.transform.position, base.transform.rotation, false);
//			}
//			ChildLocator component3 = base.GetModelTransform().GetComponent<ChildLocator>();
//			if (component3)
//			{
//				this.areaIndicator = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
//				Transform transform = component3.FindChild("AreaIndicator");
//				transform.localScale = new Vector3(TreebotFlower2Projectile.radius, TreebotFlower2Projectile.radius, TreebotFlower2Projectile.radius);
//				transform.gameObject.SetActive(true);
//			}
//		}
//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			if (NetworkServer.active)
//			{
//				this.rootPulseTimer -= Time.fixedDeltaTime;
//				this.healTimer -= Time.fixedDeltaTime;
//				if (this.rootPulseTimer <= 0f)
//				{
//					this.rootPulseTimer += TreebotFlower2Projectile.duration / TreebotFlower2Projectile.rootPulseCount;
//					this.RootPulse();
//				}
//				if (this.healTimer <= 0f)
//				{
//					this.healTimer += TreebotFlower2Projectile.duration / TreebotFlower2Projectile.healPulseCount;
//					this.HealPulse();
//				}
//				if (base.fixedAge >= TreebotFlower2Projectile.duration)
//				{
//					EntityState.Destroy(base.gameObject);
//					return;
//				}
//			}
//		}
//		private void RootPulse()
//		{
//			Vector3 position = base.transform.position;
//			foreach (HurtBox hurtBox in new SphereSearch
//			{
//				origin = position,
//				radius = TreebotFlower2Projectile.radius,
//				mask = LayerIndex.entityPrecise.mask
//			}.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(this.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
//			{
//				CharacterBody body = hurtBox.healthComponent.body;
//				if (!this.rootedBodies.Contains(body))
//				{
//					this.rootedBodies.Add(body);
//					body.AddBuff(RoR2Content.Buffs.Entangle);
//					body.RecalculateStats();
//					Vector3 a = hurtBox.transform.position - position;
//					float magnitude = a.magnitude;
//					Vector3 a2 = a / magnitude;
//					Rigidbody component = hurtBox.healthComponent.GetComponent<Rigidbody>();
//					float num = component ? component.mass : 1f;
//					float num2 = magnitude - TreebotFlower2Projectile.yankIdealDistance;
//					float num3 = TreebotFlower2Projectile.yankSuitabilityCurve.Evaluate(num);
//					Vector3 vector = component ? component.velocity : Vector3.zero;
//					if (HGMath.IsVectorNaN(vector))
//					{
//						vector = Vector3.zero;
//					}
//					Vector3 a3 = -vector;
//					if (num2 > 0f)
//					{
//						a3 = a2 * -Trajectory.CalculateInitialYSpeedForHeight(num2, -body.acceleration);
//					}
//					Vector3 force = a3 * (num * num3);
//					DamageInfo damageInfo = new DamageInfo
//					{
//						attacker = this.owner,
//						inflictor = base.gameObject,
//						crit = this.crit,
//						damage = this.damage,
//						damageColorIndex = DamageColorIndex.Default,
//						damageType = this.damageType,
//						force = force,
//						position = hurtBox.transform.position,
//						procChainMask = this.procChainMask,
//						procCoefficient = this.procCoefficient
//					};
//					hurtBox.healthComponent.TakeDamage(damageInfo);
//					HurtBox hurtBoxReference = hurtBox;
//					HurtBoxGroup hurtBoxGroup = hurtBox.hurtBoxGroup;
//					int num4 = 0;
//					while ((float)num4 < Mathf.Min(4f, body.radius * 2f))
//					{
//						EffectData effectData = new EffectData
//						{
//							scale = 1f,
//							origin = position,
//							genericFloat = Mathf.Max(0.2f, TreebotFlower2Projectile.duration - base.fixedAge)
//						};
//						effectData.SetHurtBoxReference(hurtBoxReference);
//						EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/EntangleOrbEffect"), effectData, true);
//						hurtBoxReference = hurtBoxGroup.hurtBoxes[UnityEngine.Random.Range(0, hurtBoxGroup.hurtBoxes.Length)];
//						num4++;
//					}
//				}
//			}
//		}
//		private void HealPulse()
//		{
//			HealthComponent healthComponent = this.owner ? this.owner.GetComponent<HealthComponent>() : null;
//			if (healthComponent && this.rootedBodies.Count > 0)
//			{
//				float num = 1f / TreebotFlower2Projectile.healPulseCount;
//				HealOrb healOrb = new HealOrb();
//				healOrb.origin = base.transform.position;
//				healOrb.target = healthComponent.body.mainHurtBox;
//				healOrb.healValue = num * TreebotFlower2Projectile.healthFractionYieldPerHit * healthComponent.fullHealth * (float)this.rootedBodies.Count;
//				healOrb.overrideDuration = 0.3f;
//				OrbManager.instance.AddOrb(healOrb);
//			}
//		}
//		public override void OnExit()
//		{
//			if (this.rootedBodies != null)
//			{
//				foreach (CharacterBody characterBody in this.rootedBodies)
//				{
//					characterBody.RemoveBuff(RoR2Content.Buffs.Entangle);
//				}
//				this.rootedBodies = null;
//			}
//			Util.PlaySound(TreebotFlower2Projectile.exitSoundString, base.gameObject);
//			if (TreebotFlower2Projectile.exitEffectPrefab)
//			{
//				EffectManager.SimpleEffect(TreebotFlower2Projectile.exitEffectPrefab, base.transform.position, base.transform.rotation, false);
//			}
//			base.OnExit();
//		}
//	}
//}