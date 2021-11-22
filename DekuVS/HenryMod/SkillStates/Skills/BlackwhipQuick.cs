using System.Collections.Generic;
using RoR2;
using UnityEngine;
using EntityStates;
using EntityStates.Huntress;
using System;
using RoR2.Audio;
using EntityStates.Merc;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class BlackwhipQuick : BaseSkillState
	{

		public static Vector3 CameraPosition = new Vector3(0f, -1.3f, -10f);
		public static float pullForce = 200f;
		public static float pullRadius = 40f;
		public GameObject blastEffectPrefab = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
		public float duration;
		public int maximumPullCount = int.MaxValue;
		public Transform pullOrigin;
		public AnimationCurve pullStrengthCurve;
		protected Animator animator;
		protected float baseDuration = 0.4f;
		protected float damageCoefficient = 3f;
		protected string hitboxName = "BodyHitbox";
		protected NetworkSoundEventIndex impactSound;
		protected float startUp = 0.4f;
		protected float stopwatch;
		private OverlapAttack attack;
		private bool back;
		private Ray downRay;
		private bool front;
		private bool hasFired;
		private bool pulling;
		public string hitSoundString;

		private List<CharacterBody> pullList = new List<CharacterBody>();
		private Transform slamIndicatorInstance;

		public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.stopwatch += Time.fixedDeltaTime;
		if (this.stopwatch >= this.startUp && !this.hasFired)
		{
			this.hasFired = true;
				Util.PlaySound(EvisDash.beginSoundString, base.gameObject);
				//Util.PlaySound("SettEVO", base.gameObject);
				//string animationStateName = "";
				//if (this.front && this.back)
				//{
				//	animationStateName = "Facebreaker_Both";
				//}
				//else if (this.back && !this.front)
				//{
				//	animationStateName = "Facebreaker_Back";
				//}
				//else if (this.front && !this.back)
				//{
				//	animationStateName = "Facebreaker_Front";
				//}
				//else if (!this.front && !this.back)
				//{
				//	animationStateName = "Facebreaker_Miss";
				//}
				base.PlayCrossfade("RightArm, Override", "SmashCharge", "Attack.playbackRate", this.duration, 0.05f);
			}
		if (this.stopwatch <= this.duration)
		{
			this.PullEnemies(Time.fixedDeltaTime);
		}
		if (this.stopwatch >= this.duration * this.startUp && base.isAuthority && this.attack.Fire(null))
		{
			this.OnHitEnemyAuthority();
		}
		if (this.stopwatch >= this.duration && base.isAuthority && this.hasFired)
		{
			this.outer.SetNextStateToMain();
			return;
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000028D1 File Offset: 0x00000AD1
	public override InterruptPriority GetMinimumInterruptPriority()
	{
		return InterruptPriority.Skill;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000028D4 File Offset: 0x00000AD4
	public override void OnEnter()
	{
		base.OnEnter();
		base.characterBody.SetAimTimer(2f);
		this.duration = this.baseDuration / this.attackSpeedStat;
		this.startUp /= this.attackSpeedStat;
		this.hasFired = false;
		this.animator = base.GetModelAnimator();
		base.PlayCrossfade("RightArm, Override", "Blackwhip", "Attack.playbackRate", this.startUp, 0.05f);
		HitBoxGroup hitBoxGroup = Array.Find<HitBoxGroup>(base.GetModelTransform().GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
		this.pullStrengthCurve = AnimationCurve.EaseInOut(0.1f, 0f, 1f, 1f);
		Util.PlaySound(this.hitSoundString, base.gameObject);
		//this.impactSound = Assets.swordHitSoundEvent.index;
		this.attack = this.CreateAttack(hitBoxGroup);
		this.CreateIndicator();
	}

	public override void OnExit()
	{
		if (this.slamIndicatorInstance)
		{
			EntityState.Destroy(this.slamIndicatorInstance.gameObject);
		}
		base.OnExit();
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000029F7 File Offset: 0x00000BF7
	public override void Update()
	{
		base.Update();
		if (this.slamIndicatorInstance)
		{
			this.UpdateSlamIndicator();
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002A14 File Offset: 0x00000C14
	protected OverlapAttack CreateAttack(HitBoxGroup hitBoxGroup)
	{
		return new OverlapAttack
		{
			damageType = (DamageType.Freeze2s),
			attacker = base.gameObject,
			inflictor = base.gameObject,
			teamIndex = base.GetTeam(),
			damage = this.damageCoefficient * this.damageStat,
			procCoefficient = 1f,
			hitEffectPrefab = Evis.hitEffectPrefab,
			forceVector = Vector3.zero,
			pushAwayForce = 0f,
			hitBoxGroup = hitBoxGroup,
			isCrit = base.RollCrit(),
			impactSound = this.impactSound
		};
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002AB0 File Offset: 0x00000CB0
	protected virtual void OnHitEnemyAuthority()
	{
		Util.PlaySound("Hit", base.gameObject);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002AC4 File Offset: 0x00000CC4
	private void AddToPullList(GameObject affectedObject)
	{
		CharacterBody component = affectedObject.GetComponent<CharacterBody>();
		if (!this.pullList.Contains(component))
		{
			this.pullList.Add(component);
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002AF4 File Offset: 0x00000CF4
	private void CreateIndicator()
	{
		if (ArrowRain.areaIndicatorPrefab)
		{
			this.downRay = new Ray
			{
				direction = Vector3.down,
				origin = base.transform.position
			};
			this.slamIndicatorInstance = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab).transform;
			this.slamIndicatorInstance.localScale = Vector3.one * BlackwhipQuick.pullRadius;
			for (int i = 0; i <= 18; i++)
			{
				Vector3 vector = base.characterBody.footPosition + Random.insideUnitSphere * BlackwhipQuick.pullRadius;
				Vector3 normalized = (base.characterBody.footPosition - vector).normalized;
				vector.y = base.characterBody.footPosition.y;
				EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
				{
					origin = vector,
					scale = 1f * BlackwhipQuick.pullRadius,
					rotation = Quaternion.LookRotation(normalized)
				}, false);
			}
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002C08 File Offset: 0x00000E08
	private void InitializePull()
	{
		if (this.pulling)
		{
			return;
		}
		this.pulling = true;
		Collider[] array = Physics.OverlapSphere(this.pullOrigin ? this.pullOrigin.position : base.transform.position, BlackwhipQuick.pullRadius);
		int num = 0;
		int num2 = 0;
		while (num < array.Length && num2 < this.maximumPullCount)
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
					this.AddToPullList(component.gameObject);
					num2++;
				}
			}
			num++;
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002CB8 File Offset: 0x00000EB8
	private void PullEnemies(float deltaTime)
	{
		if (!this.pulling)
		{
			this.InitializePull();
		}
		for (int i = 0; i < this.pullList.Count; i++)
		{
			CharacterBody characterBody = this.pullList[i];
			if (characterBody && characterBody.transform)
			{
				Vector3 vector = (this.pullOrigin ? this.pullOrigin.position : base.transform.position) - characterBody.corePosition;
				float num = this.pullStrengthCurve.Evaluate(vector.magnitude / BlackwhipQuick.pullRadius);
				Vector3 b = vector.normalized * num * deltaTime * BlackwhipQuick.pullForce;
				CharacterMotor component = characterBody.GetComponent<CharacterMotor>();
				Vector3 normalized = base.characterDirection.forward.normalized;
				Vector3 rhs = characterBody.transform.position - (this.pullOrigin ? this.pullOrigin.position : base.transform.position);
				if (component)
				{
					component.rootMotion += b;
					if (component.useGravity)
					{
						CharacterMotor characterMotor = component;
						characterMotor.rootMotion.y = characterMotor.rootMotion.y - Physics.gravity.y * deltaTime * num;
						if (Vector3.Dot(normalized, rhs) < 0f)
						{
							this.back = true;
						}
						else if (Vector3.Dot(normalized, rhs) > 0f)
						{
							this.front = true;
						}
						else if (Vector3.Dot(normalized, rhs) == 0f)
						{
							this.front = true;
						}
					}
				}
				else
				{
					Rigidbody component2 = characterBody.GetComponent<Rigidbody>();
					if (component2)
					{
						component2.velocity += b;
						if (Vector3.Dot(normalized, rhs) < 0f)
						{
							this.back = true;
						}
						else if (Vector3.Dot(normalized, rhs) > 0f)
						{
							this.front = true;
						}
						else if (Vector3.Dot(normalized, rhs) == 0f)
						{
							this.front = true;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002EE0 File Offset: 0x000010E0
	private void UpdateSlamIndicator()
	{
		if (this.slamIndicatorInstance)
		{
			float maxDistance = 250f;
			this.downRay = new Ray
			{
				direction = Vector3.down,
				origin = base.transform.position
			};
			RaycastHit raycastHit;
			if (Physics.Raycast(this.downRay, out raycastHit, maxDistance, LayerIndex.world.mask))
			{
				this.slamIndicatorInstance.transform.position = raycastHit.point;
				this.slamIndicatorInstance.transform.up = raycastHit.normal;
			}
		}
	}


	}
}
