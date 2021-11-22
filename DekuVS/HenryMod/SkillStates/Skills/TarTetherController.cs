using RoR2;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates.Skills
{
	// Token: 0x0200042F RID: 1071
	[RequireComponent(typeof(BezierCurveLine))]
	public class TarTetherController : NetworkBehaviour
	{
		[SyncVar]
		public GameObject targetRoot;
		[SyncVar]
		public GameObject ownerRoot;
		public float reelSpeed = 40f;
		[NonSerialized]
		public float mulchDistanceSqr;
		[NonSerialized]
		public float breakDistanceSqr;
		[NonSerialized]
		public float mulchDamageScale;
		[NonSerialized]
		public float mulchTickIntervalScale;
		[NonSerialized]
		public float damageCoefficientPerTick;
		[NonSerialized]
		public float tickInterval;
		[NonSerialized]
		public float tickTimer;
		public float attachTime;
		private float fixedAge;
		private float age;
		private bool beginSiphon;
		private BezierCurveLine bezierCurveLine;
		private HealthComponent targetHealthComponent;
		private HealthComponent ownerHealthComponent;
		private CharacterBody ownerBody;
		private NetworkInstanceId ___targetRootNetId;
		private NetworkInstanceId ___ownerRootNetId;
		private void Awake()
		{
			this.bezierCurveLine = base.GetComponent<BezierCurveLine>();
		}

		public void SetOwnerRoot(GameObject newroot)
        {
			ownerRoot = newroot;
        }


		// Token: 0x06001AB1 RID: 6833 RVA: 0x0006C778 File Offset: 0x0006A978
		private void DoDamageTick(bool mulch)
		{
			if (!this.targetHealthComponent)
			{
				this.targetHealthComponent = this.targetRoot.GetComponent<HealthComponent>();
			}
			if (!this.ownerHealthComponent)
			{
				this.ownerHealthComponent = this.ownerRoot.GetComponent<HealthComponent>();
			}
			if (!this.ownerBody)
			{
				this.ownerBody = this.ownerRoot.GetComponent<CharacterBody>();
			}
			if (this.ownerRoot)
			{
				DamageInfo damageInfo = new DamageInfo
				{
					position = this.targetRoot.transform.position,
					attacker = null,
					inflictor = null,
					damage = (mulch ? (this.damageCoefficientPerTick * this.mulchDamageScale) : this.damageCoefficientPerTick) * this.ownerBody.damage,
					damageColorIndex = DamageColorIndex.Default,
					damageType = DamageType.Generic,
					crit = false,
					force = Vector3.zero,
					procChainMask = default(ProcChainMask),
					procCoefficient = 0f
				};
				this.targetHealthComponent.TakeDamage(damageInfo);
				if (!damageInfo.rejected)
				{
					this.ownerHealthComponent.Heal(damageInfo.damage, default(ProcChainMask), true);
				}
				if (!this.targetHealthComponent.alive)
				{
					this.NetworktargetRoot = null;
				}
			}
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0006C8C0 File Offset: 0x0006AAC0
		private Vector3 GetTargetRootPosition()
		{
			if (this.targetRoot)
			{
				Vector3 result = this.targetRoot.transform.position;
				if (this.targetHealthComponent)
				{
					result = this.targetHealthComponent.body.corePosition;
				}
				return result;
			}
			return base.transform.position;
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x0006C918 File Offset: 0x0006AB18
		private void Update()
		{
			this.age += Time.deltaTime;
			Vector3 position = this.ownerRoot.transform.position;
			if (!this.beginSiphon)
			{
				Vector3 position2 = Vector3.Lerp(position, this.GetTargetRootPosition(), this.age / this.attachTime);
				this.bezierCurveLine.endTransform.position = position2;
				return;
			}
			if (this.targetRoot)
			{
				this.bezierCurveLine.endTransform.position = this.targetRoot.transform.position;
			}
			if (this.ownerRoot)
            {
				this.bezierCurveLine.p0 = this.ownerRoot.transform.position;
            }
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x0006C9AC File Offset: 0x0006ABAC
		private void FixedUpdate()
		{
			this.fixedAge += Time.fixedDeltaTime;
			if (this.targetRoot && this.ownerRoot)
			{
				Vector3 targetRootPosition = this.GetTargetRootPosition();
				if (!this.beginSiphon && this.fixedAge >= this.attachTime)
				{
					this.beginSiphon = true;
					return;
				}
				Vector3 vector = this.ownerRoot.transform.position - targetRootPosition;
				if (NetworkServer.active)
				{
					float sqrMagnitude = vector.sqrMagnitude;
					bool flag = sqrMagnitude < this.mulchDistanceSqr;
					this.tickTimer -= Time.fixedDeltaTime;
					if (this.tickTimer <= 0f)
					{
						this.tickTimer += (flag ? (this.tickInterval * this.mulchTickIntervalScale) : this.tickInterval);
						this.DoDamageTick(flag);
					}
					if (sqrMagnitude > this.breakDistanceSqr)
					{
						UnityEngine.Object.Destroy(base.gameObject);
						return;
					}
				}
				if (Util.HasEffectiveAuthority(this.targetRoot))
				{
					Vector3 b = vector.normalized * (this.reelSpeed * Time.fixedDeltaTime);
					CharacterMotor component = this.targetRoot.GetComponent<CharacterMotor>();
					if (component)
					{
						component.rootMotion += b;
						return;
					}
					Rigidbody component2 = this.targetRoot.GetComponent<Rigidbody>();
					if (component2)
					{
						component2.velocity += b;
						return;
					}
				}
			}
			else if (NetworkServer.active)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00004381 File Offset: 0x00002581
		private void UNetVersion()
		{
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x0006CB40 File Offset: 0x0006AD40
		// (set) Token: 0x06001AB8 RID: 6840 RVA: 0x0006CB53 File Offset: 0x0006AD53
		public GameObject NetworktargetRoot
		{
			get
			{
				return this.targetRoot;
			}
			[param: In]
			set
			{
				base.SetSyncVarGameObject(value, ref this.targetRoot, 1U, ref this.___targetRootNetId);
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x0006CB70 File Offset: 0x0006AD70
		// (set) Token: 0x06001ABA RID: 6842 RVA: 0x0006CB83 File Offset: 0x0006AD83
		public GameObject NetworkownerRoot
		{
			get
			{
				return this.ownerRoot;
			}
			[param: In]
			set
			{
				base.SetSyncVarGameObject(value, ref this.ownerRoot, 2U, ref this.___ownerRootNetId);
			}
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0006CBA0 File Offset: 0x0006ADA0
		public override bool OnSerialize(NetworkWriter writer, bool forceAll)
		{
			if (forceAll)
			{
				writer.Write(this.targetRoot);
				writer.Write(this.ownerRoot);
				return true;
			}
			bool flag = false;
			if ((base.syncVarDirtyBits & 1U) != 0U)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(base.syncVarDirtyBits);
					flag = true;
				}
				writer.Write(this.targetRoot);
			}
			if ((base.syncVarDirtyBits & 2U) != 0U)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(base.syncVarDirtyBits);
					flag = true;
				}
				writer.Write(this.ownerRoot);
			}
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
			}
			return flag;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0006CC4C File Offset: 0x0006AE4C
		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (initialState)
			{
				this.___targetRootNetId = reader.ReadNetworkId();
				this.___ownerRootNetId = reader.ReadNetworkId();
				return;
			}
			int num = (int)reader.ReadPackedUInt32();
			if ((num & 1) != 0)
			{
				this.targetRoot = reader.ReadGameObject();
			}
			if ((num & 2) != 0)
			{
				this.ownerRoot = reader.ReadGameObject();
			}
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x0006CCB4 File Offset: 0x0006AEB4
		public override void PreStartClient()
		{
			if (!this.___targetRootNetId.IsEmpty())
			{
				this.NetworktargetRoot = ClientScene.FindLocalObject(this.___targetRootNetId);
			}
			if (!this.___ownerRootNetId.IsEmpty())
			{
				this.NetworkownerRoot = ClientScene.FindLocalObject(this.___ownerRootNetId);
			}
		}

	}
}
