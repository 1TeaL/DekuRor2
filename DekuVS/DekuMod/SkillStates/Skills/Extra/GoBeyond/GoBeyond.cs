using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using ExtraSkillSlots;
using UnityEngine;
using static RoR2.CameraTargetParams;
using UnityEngine.Networking;
using R2API.Networking;

namespace DekuMod.SkillStates
{

	public class GoBeyond : BaseSkillState
	{
		private CharacterCameraParamsData emoteCameraParams = new CharacterCameraParamsData()
		{
			maxPitch = 70,
			minPitch = -70,
			pivotVerticalOffset = 1f,
			idealLocalCameraPos = new Vector3(0, 0.0f, -35f),
			wallCushion = 0.1f,
		};

		private CameraParamsOverrideHandle camOverrideHandle;
		public DekuController dekucon;
		const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";
		private ExtraSkillLocator extraskillLocator;
		public float duration = 5f;

        private Animator animator;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();
			extraskillLocator = base.GetComponent<ExtraSkillLocator>();

			bool active = NetworkServer.active;
			if (active)
			{
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


			this.animator = base.GetModelAnimator();
			base.characterBody.hideCrosshair = true;
			if (base.GetAimAnimator()) base.GetAimAnimator().enabled = false;

			CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
			{
				cameraParamsData = emoteCameraParams,
				priority = 0,
			};

			camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, duration/2);

		}

        public override void OnExit()
        {
            base.OnExit();

			if (base.GetAimAnimator()) base.GetAimAnimator().enabled = true;
			base.characterBody.hideCrosshair = false;
			base.cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.5f);
			if (NetworkServer.active)
			{
				base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
				base.characterBody.ApplyBuff(Modules.Buffs.goBeyondBuff.buffIndex, 1, 60f);
			}
		}

        public override void FixedUpdate()
		{
			base.FixedUpdate();
			if(base.fixedAge > duration && base.isAuthority)
            {
				this.outer.SetNextStateToMain();
				return;
			}
			
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}