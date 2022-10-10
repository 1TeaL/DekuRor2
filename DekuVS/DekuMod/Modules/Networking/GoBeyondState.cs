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

namespace DekuMod.Modules.Networking
{
    class GoBeyondState : BaseSkillState
    {
        private float stopwatch;
        const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";
        private ExtraSkillLocator extraskillLocator;

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
				if (base.characterBody.HasBuff(Modules.Buffs.ofaBuff))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.ofaBuff);
				}
				if (base.characterBody.HasBuff(Modules.Buffs.supaofaBuff))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.supaofaBuff);
				}
				if (base.characterBody.HasBuff(Modules.Buffs.ofaBuff45))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.ofaBuff45);
				}
				if (base.characterBody.HasBuff(Modules.Buffs.supaofaBuff45))
				{
					base.characterBody.RemoveBuff(Modules.Buffs.supaofaBuff45);
				}
			}

			
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.ofacycle1SkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.ofacycle2SkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.ofacycledownSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.ofacycle2scepterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.ofacycledownscepterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Deku.ofacycle1scepterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);



			switch (base.skillLocator.primary.skillNameToken)
			{
				case prefix + "FISTPRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fistPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST45PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fist45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST100PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.fist100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEGPRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.legPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG45PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.leg45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG100PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.leg100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRKPRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirkPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRK45PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirk45PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRK100PRIMARY_NAME":
					base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Deku.quirk100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.goBeyondSkillDef5, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (base.skillLocator.secondary.skillNameToken)
			{
				case prefix + "FISTSECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fistSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST45SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fist45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST100SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.fist100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEGSECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.legSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG45SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.leg45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG100SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.leg100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRKSECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirkSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRK45SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirk45SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRK100SECONDARY_NAME":
					base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, Deku.quirk100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.goBeyondSkillDef6, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (base.skillLocator.utility.skillNameToken)
			{
				case prefix + "FISTUTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fistUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST45UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fist45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "FIST100UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.fist100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEGUTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.legUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG45UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.leg45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEG100UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.leg100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRKUTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirkUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRK45UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirk45UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRK100UTILITY_NAME":
					base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Deku.quirk100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
					base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.goBeyondSkillDef7, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}

			switch (extraskillLocator.extraFirst.skillNameToken)
			{
				case prefix + "FISTEXTRA_NAME":
					extraskillLocator.extraFirst.UnsetSkillOverride(extraskillLocator.extraFirst, Deku.fistExtraSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					extraskillLocator.extraFirst.SetSkillOverride(extraskillLocator.extraFirst, Deku.goBeyondSkillDef1, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (extraskillLocator.extraSecond.skillNameToken)
			{
				case prefix + "LEGEXTRA_NAME":
					extraskillLocator.extraSecond.UnsetSkillOverride(extraskillLocator.extraSecond, Deku.legExtraSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, Deku.goBeyondSkillDef2, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (extraskillLocator.extraThird.skillNameToken)
			{
				case prefix + "QUIRKEXTRA_NAME":
					extraskillLocator.extraThird.UnsetSkillOverride(extraskillLocator.extraThird, Deku.quirkExtraSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, Deku.goBeyondSkillDef3, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
			switch (extraskillLocator.extraFourth.skillNameToken)
			{
				case prefix + "FISTSPECIAL_NAME":
					extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, Deku.fistSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, Deku.goBeyondSkillDef4, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "LEGSPECIAL_NAME":
					extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, Deku.legSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, Deku.goBeyondSkillDef4, GenericSkill.SkillOverridePriority.Contextual);
					break;
				case prefix + "QUIRKSPECIAL_NAME":
					extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, Deku.quirkSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);
					extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, Deku.goBeyondSkillDef4, GenericSkill.SkillOverridePriority.Contextual);
					break;
			}
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

			EffectManager.SpawnEffect(Modules.Assets.gobeyondPulseEffect, new EffectData
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


			extraskillLocator.extraFirst.SetSkillOverride(extraskillLocator.extraFirst, Deku.fistExtraSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, Deku.legExtraSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, Deku.quirkExtraSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, Deku.fistSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.fist100PrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.fist100SecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.fist100UtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);
			base.skillLocator.special.SetSkillOverride(base.skillLocator.special, Deku.ofacycledownSkillDef, GenericSkill.SkillOverridePriority.Contextual);

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
            return InterruptPriority.PrioritySkill;
        }
    }
}
