using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using EntityStates;
using System.Runtime.CompilerServices;
using DekuMod.SkillStates.Might;
using UnityEngine.XR;
using DekuMod.SkillStates.ShootStyle;
using DekuMod.SkillStates;
using DekuMod.SkillStates.BlackWhip;

namespace DekuMod.Modules.Survivors
{


    internal class Deku : SurvivorBase
    {

        public static bool scepterInstalled = false;

        internal override string bodyName { get; set; } = "Deku";

        //Might Mode Skills
        internal static SkillDef mightPrimarySkillDef;
        internal static SkillDef mightSecondarySkillDef;
        internal static SkillDef mightUtilitySkillDef;
        internal static SkillDef mightSpecialSkillDef;

        //Shoot Style Mode Skills
        internal static SkillDef shootPrimarySkillDef;
        internal static SkillDef shootSecondarySkillDef;
        internal static SkillDef shootUtilitySkillDef;
        internal static SkillDef shootSpecialSkillDef;

        //Black Whip Mode Skills
        internal static SkillDef blackwhipPrimarySkillDef;
        internal static SkillDef blackwhipSecondarySkillDef;
        internal static SkillDef blackwhipUtilitySkillDef;
        internal static SkillDef blackwhipSpecialSkillDef;


        //Extra skills
        internal static SkillDef mightModeSkillDef;
        internal static SkillDef shootstyleModeSkillDef;
        internal static SkillDef blackwhipModeSkillDef;
        internal static SkillDef blackwhipPullSkillDef;

        //GoBeyond skills
        internal static SkillDef goBeyondSkillDef1;
        internal static SkillDef goBeyondSkillDef2;
        internal static SkillDef goBeyondSkillDef3;
        internal static SkillDef goBeyondSkillDef4;
        internal static SkillDef goBeyondSkillDef5;
        internal static SkillDef goBeyondSkillDef6;
        internal static SkillDef goBeyondSkillDef7;
        internal static SkillDef goBeyondSkillDef8;



        internal override GameObject bodyPrefab { get; set; }
        internal override GameObject displayPrefab { get; set; }

        internal override float sortPosition { get; set; } = 100f;

        internal override ConfigEntry<bool> characterEnabled { get; set; }

        internal override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 15f,
            armorGrowth = 0.5f,
            bodyName = "DekuBody",
            bodyNameToken = DekuPlugin.developerPrefix + "_DEKU_BODY_NAME",
            bodyColor = Color.green,
            characterPortrait = Modules.DekuAssets.LoadCharacterIcon("Deku"),
            crosshair = Modules.DekuAssets.LoadCrosshair("Standard"),
            damage = 10f,
            healthGrowth = 19f,
            healthRegen = 1f,
            jumpCount = 2,
            maxHealth = 144f,
            moveSpeed = 7f,
            subtitleNameToken = DekuPlugin.developerPrefix + "_DEKU_BODY_SUBTITLE",
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        internal static Material matArmL = Modules.DekuAssets.CreateHopooMaterial("matArmL");
        internal static Material matArmR = Modules.DekuAssets.CreateHopooMaterial("matArmR");
        internal static Material matBelt = Modules.DekuAssets.CreateHopooMaterial("matBelt");
        internal static Material matBody = Modules.DekuAssets.CreateHopooMaterial("matBody");
        internal static Material matEyes = Modules.DekuAssets.CreateHopooMaterial("matEyes");
        internal static Material matEyesBlack = Modules.DekuAssets.CreateHopooMaterial("matEyesBlack");
        internal static Material matFace = Modules.DekuAssets.CreateHopooMaterial("matFace");
        internal static Material matGoldenButtons = Modules.DekuAssets.CreateHopooMaterial("matGoldenButtons");
        internal static Material matGranCape = Modules.DekuAssets.CreateHopooMaterial("matGranCape");
        internal static Material matGranMantle = Modules.DekuAssets.CreateHopooMaterial("matGranMantle");
        internal static Material matHair = Modules.DekuAssets.CreateHopooMaterial("matHair");
        internal static Material matHands = Modules.DekuAssets.CreateHopooMaterial("matHands");
        internal static Material matHood = Modules.DekuAssets.CreateHopooMaterial("matHood");
        internal static Material matLegs = Modules.DekuAssets.CreateHopooMaterial("matLegs");
        internal static Material matMask = Modules.DekuAssets.CreateHopooMaterial("matMask");
        internal static Material matMidGauntlet = Modules.DekuAssets.CreateHopooMaterial("matMidGauntlet");
        internal static Material matShoes = Modules.DekuAssets.CreateHopooMaterial("matShoes");
        internal static Material matWristband = Modules.DekuAssets.CreateHopooMaterial("matWristband");

        internal override int mainRendererIndex { get; set; } = 0;

        internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {
                new CustomRendererInfo
                {
                    childName = "ArmL",
                    material = matArmL
                },
                new CustomRendererInfo
                {
                    childName = "ArmR",
                    material = matArmR
                },
                new CustomRendererInfo
                {
                    childName = "Belt",
                    material = matBelt
                },
                new CustomRendererInfo
                {
                    childName = "Body",
                    material = matBody
                },
                new CustomRendererInfo
                {
                    childName = "Eyes",
                    material = matEyesBlack
                },
                new CustomRendererInfo
                {
                    childName = "Face",
                    material = matFace      
                },
                new CustomRendererInfo
                {
                    childName = "GoldenButtons",
                    material = matGoldenButtons
                },
                new CustomRendererInfo
                {
                    childName = "GranCape",
                    material = matGranCape
                },
                new CustomRendererInfo
                {
                    childName = "GranMantle",
                    material = matGranMantle
                },
                new CustomRendererInfo
                {
                    childName = "Hair",
                    material = matHair
                },
                new CustomRendererInfo
                {
                    childName = "Hands",
                    material = matHands
                },
                new CustomRendererInfo
                {
                    childName = "Hood",
                    material = matHood
                },
                new CustomRendererInfo
                {
                    childName = "Legs",
                    material = matLegs
                },
                new CustomRendererInfo
                {
                    childName = "Mask",
                    material = matMask
                },
                new CustomRendererInfo
                {
                    childName = "MidGauntletL",
                    material = matMidGauntlet
                },
                new CustomRendererInfo
                {
                    childName = "MidGauntletR",
                    material = matMidGauntlet
                },
                new CustomRendererInfo
                {
                    childName = "MidGauntlets",
                    material = matMidGauntlet
                },
                new CustomRendererInfo
                {
                    childName = "NeckFace",
                    material = matFace
                },
                new CustomRendererInfo
                {
                    childName = "Shoes",
                    material = matShoes
                },
                new CustomRendererInfo
                {
                    childName = "Wristband",
                    material = matWristband
                },
        };


        internal override Type characterMainState { get; set; } = typeof(EntityStates.GenericCharacterMain);

 //item display stuffs
        internal override ItemDisplayRuleSet itemDisplayRuleSet { get; set; }
        internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

        internal override UnlockableDef characterUnlockableDef { get; set; }
        private static UnlockableDef masterySkinUnlockableDef;

        internal override void InitializeCharacter()
        {
            base.InitializeCharacter();

            bodyPrefab.AddComponent<DekuController>();
            bodyPrefab.AddComponent<DekuUI>();
            bodyPrefab.AddComponent<EnergySystem>();
        }

        //internal override void InitializeUnlockables()
        //{
        //    masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
        //}

        internal override void InitializeDoppelganger()
        {
            base.InitializeDoppelganger();
        }



        internal override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;


            Transform KickHitboxTransform = childLocator.FindChild("KickHitbox");
            Modules.Prefabs.SetupHitbox(model, KickHitboxTransform, "KickHitbox");

            Transform SmashRushHitboxTransform = childLocator.FindChild("SmashRushHitbox");
            Modules.Prefabs.SetupHitbox(model, SmashRushHitboxTransform, "SmashRushHitbox");

        }



        internal override void InitializeSkills()
        {
            Skills.CreateSkillFamilies(bodyPrefab);
            Modules.Skills.CreateFirstExtraSkillFamily(bodyPrefab);
            Modules.Skills.CreateSecondExtraSkillFamily(bodyPrefab);
            Modules.Skills.CreateThirdExtraSkillFamily(bodyPrefab);
            Modules.Skills.CreateFourthExtraSkillFamily(bodyPrefab);

            string prefix = DekuPlugin.developerPrefix +"_DEKU_BODY_";

            #region Passive
            SkillLocator skillloc = bodyPrefab.GetComponent<SkillLocator>();
            skillloc.passiveSkill.enabled = true;
            skillloc.passiveSkill.skillNameToken = prefix + "PASSIVE_NAME";
            skillloc.passiveSkill.skillDescriptionToken = prefix + "PASSIVE_DESCRIPTION";
            skillloc.passiveSkill.icon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("ultimate");
            skillloc.passiveSkill.keywordToken = prefix + "KEYWORD_PASSIVE";
            #endregion

            #region Might Mode Skills

            mightPrimarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "MIGHTPRIMARY_NAME",
                skillNameToken = prefix + "MIGHTPRIMARY_NAME",
                skillDescriptionToken = prefix + "MIGHTPRIMARY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(SmashRushStart)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            mightSecondarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "MIGHTSECONDARY_NAME",
                skillNameToken = prefix + "MIGHTSECONDARY_NAME",
                skillDescriptionToken = prefix + "MIGHTSECONDARY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("delaware"),
                activationState = new SerializableEntityStateType(typeof(DelawareSmash)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 4F,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            mightUtilitySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "MIGHTUTILITY_NAME",
                skillNameToken = prefix + "MIGHTUTILITY_NAME",
                skillDescriptionToken = prefix + "MIGHTUTILITY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("fajin"),
                activationState = new SerializableEntityStateType(typeof(Fajin)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 30,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 30,
                stockToConsume = 30,
            });

            mightSpecialSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "MIGHTSPECIAL_NAME",
                skillNameToken = prefix + "MIGHTSPECIAL_NAME",
                skillDescriptionToken = prefix + "MIGHTSPECIAL_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("detroit"),
                activationState = new SerializableEntityStateType(typeof(MightSuper)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });
            #endregion

            #region Shoot Style Mode Skills

            shootPrimarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "SHOOTSTYLEPRIMARY_NAME",
                skillNameToken = prefix + "SHOOTSTYLEPRIMARY_NAME",
                skillDescriptionToken = prefix + "SHOOTSTYLEPRIMARY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(Airforce)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            shootSecondarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "SHOOTSTYLESECONDARY_NAME",
                skillNameToken = prefix + "SHOOTSTYLESECONDARY_NAME",
                skillDescriptionToken = prefix + "SHOOTSTYLESECONDARY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("shootstylekick"),
                activationState = new SerializableEntityStateType(typeof(BlastDash)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 4F,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            shootUtilitySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "SHOOTSTYLEUTILITY_NAME",
                skillNameToken = prefix + "SHOOTSTYLEUTILITY_NAME",
                skillDescriptionToken = prefix + "SHOOTSTYLEUTILITY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("Floatactivate"),
                activationState = new SerializableEntityStateType(typeof(OklahomaSmash)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            shootSpecialSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "SHOOTSTYLESPECIAL_NAME",
                skillNameToken = prefix + "SHOOTSTYLESPECIAL_NAME",
                skillDescriptionToken = prefix + "SHOOTSTYLESPECIAL_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(ShootStyleSuper)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });
            #endregion

            #region Black Whip Mode Skills

            blackwhipPrimarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "BLACKWHIPSTYLEPRIMARY_NAME",
                skillNameToken = prefix + "BLACKWHIPSTYLEPRIMARY_NAME",
                skillDescriptionToken = prefix + "BLACKWHIPSTYLEPRIMARY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(BlackwhipStrike)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            blackwhipSecondarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "BLACKWHIPSTYLESECONDARY_NAME",
                skillNameToken = prefix + "BLACKWHIPSTYLESECONDARY_NAME",
                skillDescriptionToken = prefix + "BLACKWHIPSTYLESECONDARY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(PinPointFocus)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 4F,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            blackwhipUtilitySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "BLACKWHIPSTYLEUTILITY_NAME",
                skillNameToken = prefix + "BLACKWHIPSTYLEUTILITY_NAME",
                skillDescriptionToken = prefix + "BLACKWHIPSTYLEUTILITY_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(BlackwhipOverlay)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            blackwhipSpecialSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "BLACKWHIPSTYLESPECIAL_NAME",
                skillNameToken = prefix + "BLACKWHIPSTYLESPECIAL_NAME",
                skillDescriptionToken = prefix + "BLACKWHIPSTYLESPECIAL_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(BlackwhipSuper)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });
            #endregion

            #region Extra Skills

            mightModeSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "MIGHTMODE_NAME",
                skillNameToken = prefix + "MIGHTMODE_NAME",
                skillDescriptionToken = prefix + "MIGHTMODE_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("fistmodeIcon"),
                activationState = new SerializableEntityStateType(typeof(MightMode)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });
            shootstyleModeSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "SHOOTSTYLEMODE_NAME",
                skillNameToken = prefix + "SHOOTSTYLEMODE_NAME",
                skillDescriptionToken = prefix + "SHOOTSTYLEMODE_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("legmodeIcon"),
                activationState = new SerializableEntityStateType(typeof(ShootStyleMode)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });
            blackwhipModeSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "BLACKWHIPMODE_NAME",
                skillNameToken = prefix + "BLACKWHIPMODE_NAME",
                skillDescriptionToken = prefix + "BLACKWHIPMODE_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("Quirks"),
                activationState = new SerializableEntityStateType(typeof(BlackwhipMode)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });
            blackwhipPullSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "BLACKWHIPPULL_NAME",
                skillNameToken = prefix + "BLACKWHIPPULL_NAME",
                skillDescriptionToken = prefix + "BLACKWHIPPULL_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("blackwhip"),
                activationState = new SerializableEntityStateType(typeof(BlackwhipPull)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });
            #endregion

            #region Go Beyond Skills
            goBeyondSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "GOBEYOND_NAME",
                skillNameToken = prefix + "GOBEYOND_NAME",
                skillDescriptionToken = prefix + "GOBEYOND_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("gobeyondG"),
                activationState = new SerializableEntityStateType(typeof(SkillStates.GoBeyondEmptySkill)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            goBeyondSkillDef2 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "GOBEYOND_NAME",
                skillNameToken = prefix + "GOBEYOND_NAME",
                skillDescriptionToken = prefix + "GOBEYOND_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("gobeyondO"),
                activationState = new SerializableEntityStateType(typeof(SkillStates.GoBeyondEmptySkill)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            goBeyondSkillDef3 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "GOBEYOND_NAME",
                skillNameToken = prefix + "GOBEYOND_NAME",
                skillDescriptionToken = prefix + "GOBEYOND_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("gobeyondB"),
                activationState = new SerializableEntityStateType(typeof(SkillStates.GoBeyondEmptySkill)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            goBeyondSkillDef4 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "GOBEYOND_NAME",
                skillNameToken = prefix + "GOBEYOND_NAME",
                skillDescriptionToken = prefix + "GOBEYOND_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("gobeyondE"),
                activationState = new SerializableEntityStateType(typeof(SkillStates.GoBeyondEmptySkill)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            goBeyondSkillDef5 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "GOBEYOND_NAME",
                skillNameToken = prefix + "GOBEYOND_NAME",
                skillDescriptionToken = prefix + "GOBEYOND_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("gobeyondY"),
                activationState = new SerializableEntityStateType(typeof(SkillStates.GoBeyondEmptySkill)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            goBeyondSkillDef6 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "GOBEYOND_NAME",
                skillNameToken = prefix + "GOBEYOND_NAME",
                skillDescriptionToken = prefix + "GOBEYOND_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("gobeyondO"),
                activationState = new SerializableEntityStateType(typeof(SkillStates.GoBeyondEmptySkill)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            goBeyondSkillDef7 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "GOBEYOND_NAME",
                skillNameToken = prefix + "GOBEYOND_NAME",
                skillDescriptionToken = prefix + "GOBEYOND_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("gobeyondN"),
                activationState = new SerializableEntityStateType(typeof(SkillStates.GoBeyondEmptySkill)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            goBeyondSkillDef8 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "GOBEYOND_NAME",
                skillNameToken = prefix + "GOBEYOND_NAME",
                skillDescriptionToken = prefix + "GOBEYOND_DESCRIPTION",
                skillIcon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("gobeyondD"),
                activationState = new SerializableEntityStateType(typeof(SkillStates.GoBeyondEmptySkill)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            #endregion



            #region Adding Skills
            Modules.Skills.AddPrimarySkill(bodyPrefab, mightPrimarySkillDef);


            Skills.AddSecondarySkills(this.bodyPrefab, new SkillDef[]
            {
                mightSecondarySkillDef,
            });

            Skills.AddUtilitySkills(this.bodyPrefab, new SkillDef[]
            {
                mightUtilitySkillDef,
            });

            Skills.AddSpecialSkills(this.bodyPrefab, new SkillDef[]
            {
                mightSpecialSkillDef,
            });

            Skills.AddFirstExtraSkill(bodyPrefab, mightModeSkillDef);

            Skills.AddSecondExtraSkill(bodyPrefab, shootstyleModeSkillDef);

            Skills.AddThirdExtraSkill(bodyPrefab, blackwhipPullSkillDef);

            Skills.AddFourthExtraSkill(bodyPrefab, blackwhipPullSkillDef);
            #endregion

        }

        //internal static Material matArmL = Modules.Asset.CreateHopooMaterial("matArmL");
        //internal static Material matArmR = Modules.Asset.CreateHopooMaterial("matArmR");
        //internal static Material matBelt = Modules.Asset.CreateHopooMaterial("matBelt");
        //internal static Material matBody = Modules.Asset.CreateHopooMaterial("matBody");
        //internal static Material matEyes = Modules.Asset.CreateHopooMaterial("matEyes");
        //internal static Material matEyesBlack = Modules.Asset.CreateHopooMaterial("matEyesBlack");
        //internal static Material matFace = Modules.Asset.CreateHopooMaterial("matFace");
        //internal static Material matGoldenButtons = Modules.Asset.CreateHopooMaterial("matGoldenButtons");
        //internal static Material matGranCape = Modules.Asset.CreateHopooMaterial("matGranCape");
        //internal static Material matGranMantle = Modules.Asset.CreateHopooMaterial("matGranMantle");
        //internal static Material matHair = Modules.Asset.CreateHopooMaterial("matHair");
        //internal static Material matHands = Modules.Asset.CreateHopooMaterial("matHands");
        //internal static Material matHood = Modules.Asset.CreateHopooMaterial("matHood");
        //internal static Material matLegs = Modules.Asset.CreateHopooMaterial("matLegs");
        //internal static Material matMask = Modules.Asset.CreateHopooMaterial("matMask");
        //internal static Material matMidGauntlet = Modules.Asset.CreateHopooMaterial("matMidGauntlet");
        //internal static Material matShoes = Modules.Asset.CreateHopooMaterial("matShoes");
        //internal static Material matWristband = Modules.Asset.CreateHopooMaterial("matWristband");

        internal override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;


            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;
            CharacterModel.RendererInfo[] defaultRendererInfo = SkinRendererInfos(defaultRenderers, new Material[] {
                matArmL,
                matArmR,
                matBelt,
                matBody,
                matEyesBlack,
                matFace,
                matFace,
                matGoldenButtons,
                matGranCape,
                matGranMantle,
                matHair,
                matHands,
                matHood,
                matLegs,
                matMask,
                matMidGauntlet,
                matMidGauntlet,
                matMidGauntlet,
                matShoes,
                matWristband
            });
            
            //black eye material
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(DekuPlugin.developerPrefix + "_DEKU_BODY_DEFAULT_SKIN_NAME",
                DekuAssets.mainAssetBundle.LoadAsset<Sprite>("Airforceskin"),
                defaultRendererInfo,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("ArmL"),
                    renderer = defaultRendererInfo[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("ArmR"),
                    renderer = defaultRendererInfo[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Belt"),
                    renderer = defaultRendererInfo[2].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Body"),
                    renderer = defaultRendererInfo[3].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Eyes"),
                    renderer = defaultRendererInfo[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Face"),
                    renderer = defaultRendererInfo[5].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Neck"),
                    renderer = defaultRendererInfo[6].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("GoldenButtons"),
                    renderer = defaultRendererInfo[7].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("GranCape"),
                    renderer = defaultRendererInfo[8].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("GranMantle"),
                    renderer = defaultRendererInfo[9].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Hair"),
                    renderer = defaultRendererInfo[10].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Hands"),
                    renderer = defaultRendererInfo[11].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Hood"),
                    renderer = defaultRendererInfo[12].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Legs"),
                    renderer = defaultRendererInfo[13].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Mask"),
                    renderer = defaultRendererInfo[14].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("MidGauntletL"),
                    renderer = defaultRendererInfo[15].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("MidGauntletR"),
                    renderer = defaultRendererInfo[16].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("MidGauntlets"),
                    renderer = defaultRendererInfo[17].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Shoes"),
                    renderer = defaultRendererInfo[18].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Wristband"),
                    renderer = defaultRendererInfo[19].renderer
                },
            };

            skins.Add(defaultSkin);
            #endregion

            //eye skin
            #region eye Skin
            CharacterModel.RendererInfo[] eyeRendererInfo = SkinRendererInfos(defaultRenderers, new Material[] {
                matArmL,
                matArmR,
                matBelt,
                matBody,
                matEyes,
                matFace,
                matFace,
                matGoldenButtons,
                matGranCape,
                matGranMantle,
                matHair,
                matHands,
                matHood,
                matLegs,
                matMask,
                matMidGauntlet,
                matMidGauntlet,
                matMidGauntlet,
                matShoes,
                matWristband
            });
            SkinDef eyeSkin = Modules.Skins.CreateSkinDef(DekuPlugin.developerPrefix + "_DEKU_BODY_EYE_SKIN_NAME",
                DekuAssets.mainAssetBundle.LoadAsset<Sprite>("Airforceskin"),
                eyeRendererInfo,
                mainRenderer,
                model);

            eyeSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("ArmL"),
                    renderer = defaultRendererInfo[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("ArmR"),
                    renderer = defaultRendererInfo[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Belt"),
                    renderer = defaultRendererInfo[2].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Body"),
                    renderer = defaultRendererInfo[3].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Eyes"),
                    renderer = defaultRendererInfo[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Face"),
                    renderer = defaultRendererInfo[5].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Neck"),
                    renderer = defaultRendererInfo[6].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("GoldenButtons"),
                    renderer = defaultRendererInfo[7].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("GranCape"),
                    renderer = defaultRendererInfo[8].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("GranMantle"),
                    renderer = defaultRendererInfo[9].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Hair"),
                    renderer = defaultRendererInfo[10].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Hands"),
                    renderer = defaultRendererInfo[11].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Hood"),
                    renderer = defaultRendererInfo[12].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Legs"),
                    renderer = defaultRendererInfo[13].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Mask"),
                    renderer = defaultRendererInfo[14].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("MidGauntletL"),
                    renderer = defaultRendererInfo[15].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("MidGauntletR"),
                    renderer = defaultRendererInfo[16].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("MidGauntlets"),
                    renderer = defaultRendererInfo[17].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Shoes"),
                    renderer = defaultRendererInfo[18].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.DekuAssets.mainAssetBundle.LoadAsset<Mesh>("Wristband"),
                    renderer = defaultRendererInfo[19].renderer
                },
            };

            skins.Add(eyeSkin);

            #endregion

            skinController.skins = skins.ToArray();
        }


        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials)
        {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            for (int i = 0; i < defaultRenderers.Length; i++)
            {
                newRendererInfos[i].defaultMaterial = materials[i];
            }


            return newRendererInfos;
        }

        //newRendererInfos[0].defaultMaterial = materials[0];
        //newRendererInfos[1].defaultMaterial = materials[1];
        //newRendererInfos[instance.mainRendererIndex].defaultMaterial = materials[2];
    }
}

