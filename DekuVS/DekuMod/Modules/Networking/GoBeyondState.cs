using EntityStates;
using ExtraSkillSlots;
using UnityEngine;
using DekuMod.Modules.Survivors;
using RoR2;
using UnityEngine.Networking;
using DekuMod.SkillStates;
using R2API.Networking;
using static RoR2.CameraTargetParams;
using System.Linq;
using System.Collections.Generic;
using RoR2.Skills;

namespace DekuMod.Modules.Networking
{
    class GoBeyondState : BaseSkillState
    {
        private float stopwatch;
        const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";
        private ExtraSkillLocator extraskillLocator;

		public SkillDef skilldef1;
        public SkillDef skilldef2;
        public SkillDef skilldef3;
        public SkillDef skilldef4;
        public SkillDef skilldef5;
        public SkillDef skilldef6;
        public SkillDef skilldef7;
        public SkillDef skilldef8;

        private CharacterCameraParamsData emoteCameraParams = new CharacterCameraParamsData()
		{
			maxPitch = 70,
			minPitch = -70,
			pivotVerticalOffset = 1f,
			idealLocalCameraPos = new Vector3(0, 0.0f, -20f),
			wallCushion = 0.1f,
		};

		private CameraParamsOverrideHandle camOverrideHandle;
		public DekuController dekucon;
		public float duration = 8f;

		private Animator animator;
		private BlastAttack blastAttack;
		private float maxWeight;

		public float FOV = 20f;
        private float blastRadius = 20f;

        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            base.characterMotor.velocity = Vector3.zero;
			dekucon = base.GetComponent<DekuController>();
			extraskillLocator = base.GetComponent<ExtraSkillLocator>();
			//AkSoundEngine.PostEvent(189871802, this.gameObject);
			//base.PlayAnimation("FullBody, Override", "FallStun", "Emote.playbackRate", Modules.StaticValues.fallDamageStunTimer - 0.5f);
			//Play Animation here.

			dekucon.PlayGobeyondLoop();
			this.animator = base.GetModelAnimator();
			base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
            //PlayAnimation("Body", "GoBeyond", "Attack.playbackRate", duration);
            PlayAnimation("FullBody, Override", "GoBeyond", "Attack.playbackRate", duration);

            base.characterBody.hideCrosshair = true;
			if (base.GetAimAnimator()) base.GetAimAnimator().enabled = false;

			CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
			{
				cameraParamsData = emoteCameraParams,
				priority = 0,
			};

			camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, duration);

			GetMaxWeight();

			blastAttack = new BlastAttack();
			blastAttack.radius = blastRadius;
			blastAttack.procCoefficient = 1f;
			blastAttack.position = base.transform.position;
			blastAttack.damageType = DamageType.Stun1s;
			blastAttack.attacker = base.gameObject;
			blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
			blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.gobeyondDamageCoefficient;
			blastAttack.falloffModel = BlastAttack.FalloffModel.None;
			blastAttack.baseForce = 100f * maxWeight;
			blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
			blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

			if (NetworkServer.active)
            {
				base.characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex);

			}


            skilldef1 = extraskillLocator.extraFirst.skillDef;
            skilldef2 = extraskillLocator.extraSecond.skillDef;
            skilldef3 = extraskillLocator.extraThird.skillDef;
            skilldef4 = extraskillLocator.extraFourth.skillDef;
            skilldef5 = characterBody.skillLocator.primary.skillDef;
            skilldef6 = characterBody.skillLocator.secondary.skillDef;
            skilldef7 = characterBody.skillLocator.utility.skillDef;
            skilldef8 = characterBody.skillLocator.special.skillDef;


            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, skilldef5, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, skilldef6, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, skilldef7, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);

            base.skillLocator.special.UnsetSkillOverride(base.skillLocator.utility, skilldef8, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef8, GenericSkill.SkillOverridePriority.Contextual);

            extraskillLocator.extraFirst.UnsetSkillOverride(extraskillLocator.extraFirst, skilldef1, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFirst.SetSkillOverride(extraskillLocator.extraFirst, Deku.goBeyondSkillDef1, GenericSkill.SkillOverridePriority.Contextual);

            extraskillLocator.extraSecond.UnsetSkillOverride(extraskillLocator.extraSecond, skilldef2, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, Deku.goBeyondSkillDef2, GenericSkill.SkillOverridePriority.Contextual);

            extraskillLocator.extraThird.UnsetSkillOverride(extraskillLocator.extraThird, skilldef3, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, Deku.goBeyondSkillDef3, GenericSkill.SkillOverridePriority.Contextual);

            extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, skilldef4, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, Deku.goBeyondSkillDef4, GenericSkill.SkillOverridePriority.Contextual);
		}

		public void GetMaxWeight()
		{
			Ray aimRay = base.GetAimRay();
			BullseyeSearch search = new BullseyeSearch
			{

				teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
				filterByLoS = false,
				searchOrigin = base.transform.position,
				searchDirection = UnityEngine.Random.onUnitSphere,
				sortMode = BullseyeSearch.SortMode.Distance,
				maxDistanceFilter = blastRadius,
				maxAngleFilter = 360f
			};

			search.RefreshCandidates();
			search.FilterOutGameObject(base.gameObject);



			List<HurtBox> target = search.GetResults().ToList<HurtBox>();
			foreach (HurtBox singularTarget in target)
			{
				if (singularTarget)
				{
					if (singularTarget.healthComponent && singularTarget.healthComponent.body)
					{
						if (singularTarget.healthComponent.body.characterMotor)
						{
							if (singularTarget.healthComponent.body.characterMotor.mass > maxWeight)
							{
								maxWeight = singularTarget.healthComponent.body.characterMotor.mass;
							}
						}
						else if (singularTarget.healthComponent.body.rigidbody)
						{
							if (singularTarget.healthComponent.body.rigidbody.mass > maxWeight)
							{
								maxWeight = singularTarget.healthComponent.body.rigidbody.mass;
							}
						}
					}
				}
			}
		}


		public override void OnExit()
        {
            base.OnExit();

			base.characterBody.inputBank.enabled = true;
			blastAttack.Fire();


			if (base.GetAimAnimator()) base.GetAimAnimator().enabled = true;
			base.characterBody.hideCrosshair = false;
			base.cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.5f);
			if (NetworkServer.active)
			{
				base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
				base.characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 5);
				base.characterBody.ApplyBuff(Modules.Buffs.goBeyondBuff.buffIndex, 1, Modules.StaticValues.goBeyondBuffDuration);
			}

			EffectManager.SpawnEffect(Modules.Asset.gobeyondPulseEffect, new EffectData
			{
				origin = base.transform.position,
				scale = blastRadius,
				rotation = Quaternion.LookRotation(Vector3.up)
			}, true);


			dekucon.GOBEYOND.Play();

			extraskillLocator.extraFirst.UnsetSkillOverride(extraskillLocator.extraFirst, Deku.goBeyondSkillDef1, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraSecond.UnsetSkillOverride(extraskillLocator.extraSecond, Deku.goBeyondSkillDef2, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraThird.UnsetSkillOverride(extraskillLocator.extraThird, Deku.goBeyondSkillDef3, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, Deku.goBeyondSkillDef4, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.goBeyondSkillDef8, GenericSkill.SkillOverridePriority.Contextual);


			extraskillLocator.extraFirst.SetSkillOverride(extraskillLocator.extraFirst, skilldef1, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, skilldef2, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, skilldef3, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, skilldef4, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, skilldef5, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, skilldef6, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, skilldef7, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, skilldef8, GenericSkill.SkillOverridePriority.Contextual);

		}

		public override void FixedUpdate()
        {
            base.FixedUpdate();

			if (base.fixedAge > duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
