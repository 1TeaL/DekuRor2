using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.Merc;


namespace DekuMod.SkillStates
{
    public class ShootStyle : BaseState
    {
		private CameraTargetParams.AimRequest aimRequest;
		private Animator animator;
		private CharacterModel characterModel;
		private ChildLocator childLocator;
		private HurtBoxGroup hurtboxGroup;
		private OverlapAttack overlapAttack;
		private float stopwatch;
		private bool isDashing;
		private bool inHitPause;
		private float hitPauseTimer;
		private Transform modelTransform;
		private Vector3 dashVector = Vector3.zero;

		public bool hasHit { get; private set; }
		public int dashIndex { private get; set; }
		public override void OnEnter()
		{
			base.OnEnter();
            Util.PlaySound(Assaulter.beginSoundString, base.gameObject);
            this.modelTransform = base.GetModelTransform();
            if (base.cameraTargetParams)
			{
				this.aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
			}
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
                this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
                if (this.childLocator)
				{
					this.childLocator.FindChild("PreDashEffect").gameObject.SetActive(true);
				}
			}
			base.SmallHop(base.characterMotor, Assaulter.smallHopVelocity);
			base.PlayAnimation("FullBody, Override", "AssaulterPrep", "AssaulterPrep.playbackRate", Assaulter.dashPrepDuration);
			this.dashVector = base.inputBank.aimDirection;
			this.overlapAttack = base.InitMeleeOverlap(Assaulter.damageCoefficient, Assaulter.hitEffectPrefab, this.modelTransform, "Assaulter");
			this.overlapAttack.damageType = DamageType.Stun1s;
			if (NetworkServer.active)
			{
				base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex);
			}
		}
		private void CreateDashEffect()
		{
			Transform transform = this.childLocator.FindChild("DashCenter");
			if (transform && Assaulter.dashPrefab)
			{
				UnityEngine.Object.Instantiate<GameObject>(Assaulter.dashPrefab, transform.position, Util.QuaternionSafeLookRotation(this.dashVector), transform);
			}
			if (this.childLocator)
			{
				this.childLocator.FindChild("PreDashEffect").gameObject.SetActive(false);
			}
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.characterDirection.forward = this.dashVector;
			if (this.stopwatch > Assaulter.dashPrepDuration / this.attackSpeedStat && !this.isDashing)
			{
				this.isDashing = true;
				this.dashVector = base.inputBank.aimDirection;
				this.CreateDashEffect();
				base.PlayCrossfade("FullBody, Override", "AssaulterLoop", 0.1f);
				base.gameObject.layer = LayerIndex.fakeActor.intVal;
				base.characterMotor.Motor.RebuildCollidableLayers();
				if (this.modelTransform)
				{
					TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
					temporaryOverlay.duration = 0.7f;
					temporaryOverlay.animateShaderAlpha = true;
					temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
					temporaryOverlay.destroyComponentOnEnd = true;
					temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matMercEnergized");
					temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
				}
			}
			if (!this.isDashing)
			{
				this.stopwatch += Time.fixedDeltaTime;
			}
			else if (base.isAuthority)
			{
				base.characterMotor.velocity = Vector3.zero;
				if (!this.inHitPause)
				{
					bool flag = this.overlapAttack.Fire(null);
					this.stopwatch += Time.fixedDeltaTime;
					if (flag)
					{
						if (!this.hasHit)
						{
							this.hasHit = true;
						}
						this.inHitPause = true;
						this.hitPauseTimer = Assaulter.hitPauseDuration / this.attackSpeedStat;
						if (this.modelTransform)
						{
							TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
							temporaryOverlay2.duration = Assaulter.hitPauseDuration / this.attackSpeedStat;
							temporaryOverlay2.animateShaderAlpha = true;
							temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
							temporaryOverlay2.destroyComponentOnEnd = true;
							temporaryOverlay2.originalMaterial = Resources.Load<Material>("Materials/matMercEvisTarget");
							temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
						}
					}
					base.characterMotor.rootMotion += this.dashVector * this.moveSpeedStat * Assaulter.speedCoefficient * Time.fixedDeltaTime;
				}
				else
				{
					this.hitPauseTimer -= Time.fixedDeltaTime;
					if (this.hitPauseTimer < 0f)
					{
						this.inHitPause = false;
					}
				}
			}
			if (this.stopwatch >= Assaulter.dashDuration + Assaulter.dashPrepDuration / this.attackSpeedStat && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
			}
		}
		public override void OnExit()
		{
			base.gameObject.layer = LayerIndex.defaultLayer.intVal;
			base.characterMotor.Motor.RebuildCollidableLayers();
			Util.PlaySound(Assaulter.endSoundString, base.gameObject);
			if (base.isAuthority)
			{
				base.characterMotor.velocity *= 0.1f;
				base.SmallHop(base.characterMotor, Assaulter.smallHopVelocity);
			}
			CameraTargetParams.AimRequest aimRequest = this.aimRequest;
			if (aimRequest != null)
			{
				aimRequest.Dispose();
			}
			if (this.childLocator)
			{
				this.childLocator.FindChild("PreDashEffect").gameObject.SetActive(false);
			}
			base.PlayAnimation("FullBody, Override", "EvisLoopExit");
			if (NetworkServer.active)
			{
				base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex);
			}
			base.OnExit();
		}
		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write((byte)this.dashIndex);
		}
		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.dashIndex = (int)reader.ReadByte();
		}
	}
}
