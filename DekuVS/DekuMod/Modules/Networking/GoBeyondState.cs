using EntityStates;
using ExtraSkillSlots;
using UnityEngine;
using DekuMod.Modules.Survivors;
using RoR2;
using UnityEngine.Networking;
using DekuMod.SkillStates;
using R2API.Networking;

namespace DekuMod.Modules.Networking
{
    class GoBeyondState : BaseSkillState
    {
        private float stopwatch;
        const string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";
        private ExtraSkillLocator extraskillLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            base.characterMotor.velocity = Vector3.zero;
            //AkSoundEngine.PostEvent(189871802, this.gameObject);
            //base.PlayAnimation("FullBody, Override", "FallStun", "Emote.playbackRate", Modules.StaticValues.fallDamageStunTimer - 0.5f);
            //Play Animation here.
            if (NetworkServer.active)
            {
				base.characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex);
            }

			extraskillLocator = base.GetComponent<ExtraSkillLocator>();
			
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

        public override void OnExit()
        {
            base.OnExit();

			//base.PlayCrossfade("FullBody, Override", "BufferEmpty", "Emote.playbackRate", 0.1f, 0.5f);
			base.characterBody.inputBank.enabled = true;
            //base.characterBody.characterMotor.hasEffectiveAuthority = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //Increment timer
            stopwatch += Time.fixedDeltaTime;

            //GET OUTTA HERE
            if (stopwatch >= 5f)
            {
				//base.outer.SetNextStateToMain();

				bool isAuthority = base.isAuthority;
				if (isAuthority)
				{
					GoBeyond goBeyond = new GoBeyond();
					this.outer.SetNextState(goBeyond);
				}

			}
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
