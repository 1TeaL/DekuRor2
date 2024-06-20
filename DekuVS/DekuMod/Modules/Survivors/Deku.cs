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

        //Quirk Combo Mode Skills
        internal static SkillDef quirkPrimarySkillDef;
        internal static SkillDef quirkSecondarySkillDef;
        internal static SkillDef quirkUtilitySkillDef;
        internal static SkillDef quirkSpecialSkillDef;

        //Primary skills
        internal static SkillDef fistPrimarySkillDef;
        internal static SkillDef fist45PrimarySkillDef;
        internal static SkillDef fist100PrimarySkillDef;
        internal static SkillDef legPrimarySkillDef;
        internal static SkillDef leg45PrimarySkillDef;
        internal static SkillDef leg100PrimarySkillDef;
        //internal static SkillDef quirkPrimarySkillDef;
        internal static SkillDef quirk45PrimarySkillDef;
        internal static SkillDef quirk100PrimarySkillDef;
        //Secondary skills
        internal static SkillDef fistSecondarySkillDef;
        internal static SkillDef fist45SecondarySkillDef;
        internal static SkillDef fist100SecondarySkillDef;
        internal static SkillDef legSecondarySkillDef;
        internal static SkillDef leg45SecondarySkillDef;
        internal static SkillDef leg100SecondarySkillDef;
        //internal static SkillDef quirkSecondarySkillDef;
        internal static SkillDef quirk45SecondarySkillDef;
        internal static SkillDef quirk100SecondarySkillDef;
        //Utility skills
        internal static SkillDef fistUtilitySkillDef;
        internal static SkillDef fist45UtilitySkillDef;
        internal static SkillDef fist100UtilitySkillDef;
        internal static SkillDef legUtilitySkillDef;
        internal static SkillDef leg45UtilitySkillDef;
        internal static SkillDef leg100UtilitySkillDef;
        //internal static SkillDef quirkUtilitySkillDef;
        internal static SkillDef quirk45UtilitySkillDef;
        internal static SkillDef quirk100UtilitySkillDef;
        //Special skills
        internal static SkillDef fistSpecialSkillDef;
        internal static SkillDef legSpecialSkillDef;
        //internal static SkillDef quirkSpecialSkillDef;

        //Extra skills
        internal static SkillDef fistExtraSkillDef;
        internal static SkillDef legExtraSkillDef;
        internal static SkillDef quirkExtraSkillDef;
        internal static SkillDef cycleExtraSkillDef;
        internal static SkillDef typeExtraSkillDef;

        //GoBeyond skills
        internal static SkillDef goBeyondSkillDef1;
        internal static SkillDef goBeyondSkillDef2;
        internal static SkillDef goBeyondSkillDef3;
        internal static SkillDef goBeyondSkillDef4;
        internal static SkillDef goBeyondSkillDef5;
        internal static SkillDef goBeyondSkillDef6;
        internal static SkillDef goBeyondSkillDef7;
        internal static SkillDef goBeyondSkillDef8;


        internal static SkillDef secondaryboostSkillDef;
        internal static SkillDef utilityboostSkillDef;
        internal static SkillDef primaryboost45SkillDef;
        internal static SkillDef secondaryboost45SkillDef;
        internal static SkillDef utilityboost45SkillDef;
        internal static SkillDef ofadownSkillDef;
        internal static SkillDef primaryfajinSkillDef;
        internal static SkillDef primaryfajinscepterSkillDef;
        internal static SkillDef secondaryfajinSkillDef;
        internal static SkillDef utilityfajinSkillDef;
        internal static SkillDef airforce100SkillDef;
        internal static SkillDef shootstylekick45SkillDef;
        internal static SkillDef shootstylekick100SkillDef;
        internal static SkillDef dangersense45SkillDef;
        internal static SkillDef dangersense100SkillDef;
        internal static SkillDef blackwhip100SkillDef;
        internal static SkillDef manchester45SkillDef;
        internal static SkillDef manchester100SkillDef;
        internal static SkillDef stlouis100SkillDef;
        internal static SkillDef float45SkillDef;
        internal static SkillDef float100SkillDef;
        internal static SkillDef floatcancelSkillDef;
        internal static SkillDef floatcancel45SkillDef;
        internal static SkillDef floatcancel100SkillDef;
        internal static SkillDef floatdelawareSkillDef;
        internal static SkillDef floatdelaware45SkillDef;
        internal static SkillDef oklahoma45SkillDef;
        internal static SkillDef oklahoma100SkillDef;
        internal static SkillDef detroit45SkillDef;
        internal static SkillDef ofacycle1SkillDef;
        internal static SkillDef ofacycle2SkillDef;
        internal static SkillDef ofacycleSkillDef;
        internal static SkillDef ofacycle1scepterSkillDef;
        internal static SkillDef ofacycle2scepterSkillDef;
        internal static SkillDef ofacycledownSkillDef;
        internal static SkillDef ofacycledownscepterSkillDef;
        internal static SkillDef fajinSkillDef;


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
            characterPortrait = Modules.Assets.LoadCharacterIcon("Deku"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 10f,
            healthGrowth = 19f,
            healthRegen = 1f,
            jumpCount = 2,
            maxHealth = 144f,
            moveSpeed = 7f,
            subtitleNameToken = DekuPlugin.developerPrefix + "_DEKU_BODY_SUBTITLE",
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        internal static Material matArmL = Modules.Assets.CreateHopooMaterial("matArmL");
        internal static Material matArmR = Modules.Assets.CreateHopooMaterial("matArmR");
        internal static Material matBelt = Modules.Assets.CreateHopooMaterial("matBelt");
        internal static Material matBody = Modules.Assets.CreateHopooMaterial("matBody");
        internal static Material matEyes = Modules.Assets.CreateHopooMaterial("matEyes");
        internal static Material matEyesBlack = Modules.Assets.CreateHopooMaterial("matEyesBlack");
        internal static Material matFace = Modules.Assets.CreateHopooMaterial("matFace");
        internal static Material matGoldenButtons = Modules.Assets.CreateHopooMaterial("matGoldenButtons");
        internal static Material matGranCape = Modules.Assets.CreateHopooMaterial("matGranCape");
        internal static Material matGranMantle = Modules.Assets.CreateHopooMaterial("matGranMantle");
        internal static Material matHair = Modules.Assets.CreateHopooMaterial("matHair");
        internal static Material matHands = Modules.Assets.CreateHopooMaterial("matHands");
        internal static Material matHood = Modules.Assets.CreateHopooMaterial("matHood");
        internal static Material matLegs = Modules.Assets.CreateHopooMaterial("matLegs");
        internal static Material matMask = Modules.Assets.CreateHopooMaterial("matMask");
        internal static Material matMidGauntlet = Modules.Assets.CreateHopooMaterial("matMidGauntlet");
        internal static Material matShoes = Modules.Assets.CreateHopooMaterial("matShoes");
        internal static Material matWristband = Modules.Assets.CreateHopooMaterial("matWristband");

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
            //Modules.Skills.CreateFirstExtraSkillFamily(bodyPrefab);
            //Modules.Skills.CreateSecondExtraSkillFamily(bodyPrefab);
            //Modules.Skills.CreateThirdExtraSkillFamily(bodyPrefab);
            //Modules.Skills.CreateFourthExtraSkillFamily(bodyPrefab);

            string prefix = DekuPlugin.developerPrefix +"_DEKU_BODY_";

            #region Passive
            SkillLocator skillloc = bodyPrefab.GetComponent<SkillLocator>();
            skillloc.passiveSkill.enabled = true;
            skillloc.passiveSkill.skillNameToken = prefix + "PASSIVE_NAME";
            skillloc.passiveSkill.skillDescriptionToken = prefix + "PASSIVE_DESCRIPTION";
            skillloc.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ultimate");
            skillloc.passiveSkill.keywordToken = prefix + "KEYWORD_PASSIVE";
            #endregion

            #region Might Mode Skills

            mightPrimarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "MIGHTPRIMARY_NAME",
                skillNameToken = prefix + "MIGHTPRIMARY_NAME",
                skillDescriptionToken = prefix + "MIGHTPRIMARY_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(DelawareSmash)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 4F,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Any,
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(CounterSmash)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            mightSpecialSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {

                skillName = prefix + "MIGHTSPECIAL_NAME",
                skillNameToken = prefix + "MIGHTSPECIAL_NAME",
                skillDescriptionToken = prefix + "MIGHTSPECIAL_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("airforce"),
                activationState = new SerializableEntityStateType(typeof(MightSuper)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Any,
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("gobeyondG"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("gobeyondO"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("gobeyondB"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("gobeyondE"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("gobeyondY"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("gobeyondO"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("gobeyondN"),
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
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("gobeyondD"),
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
            //Modules.Skills.AddPrimarySkill(bodyPrefab, fistPrimarySkillDef);
            //Modules.Skills.AddPrimarySkill(bodyPrefab, legPrimarySkillDef);
            //Modules.Skills.AddPrimarySkill(bodyPrefab, quirkPrimarySkillDef);


            Skills.AddSecondarySkills(this.bodyPrefab, new SkillDef[]
            {
                mightSecondarySkillDef,
                //fistSecondarySkillDef,
                //legSecondarySkillDef,
                //quirkSecondarySkillDef,
            });

            Skills.AddUtilitySkills(this.bodyPrefab, new SkillDef[]
            {
                mightUtilitySkillDef,
                //fistUtilitySkillDef,
                //legUtilitySkillDef,
                //quirkUtilitySkillDef,
            });

            Skills.AddSpecialSkills(this.bodyPrefab, new SkillDef[]
            {
                mightSpecialSkillDef,
                //ofacycle1SkillDef,
            });

            //Skills.AddFirstExtraSkill(bodyPrefab, mightPrimarySkillDef);
            ////Skills.AddFirstExtraSkill(bodyPrefab, fistExtraSkillDef);

            //Skills.AddSecondExtraSkill(bodyPrefab, mightPrimarySkillDef);
            ////Skills.AddSecondExtraSkill(bodyPrefab, legExtraSkillDef);

            //Skills.AddThirdExtraSkill(bodyPrefab, mightPrimarySkillDef);
            ////Skills.AddThirdExtraSkill(bodyPrefab, quirkExtraSkillDef);

            //Skills.AddFourthExtraSkill(bodyPrefab, mightPrimarySkillDef);
            //Skills.AddFourthExtraSkill(bodyPrefab, typeExtraSkillDef);
            //Skills.AddFourthExtraSkill(bodyPrefab, fistSpecialSkillDef);
            #endregion

        }

        //internal static Material matArmL = Modules.Assets.CreateHopooMaterial("matArmL");
        //internal static Material matArmR = Modules.Assets.CreateHopooMaterial("matArmR");
        //internal static Material matBelt = Modules.Assets.CreateHopooMaterial("matBelt");
        //internal static Material matBody = Modules.Assets.CreateHopooMaterial("matBody");
        //internal static Material matEyes = Modules.Assets.CreateHopooMaterial("matEyes");
        //internal static Material matEyesBlack = Modules.Assets.CreateHopooMaterial("matEyesBlack");
        //internal static Material matFace = Modules.Assets.CreateHopooMaterial("matFace");
        //internal static Material matGoldenButtons = Modules.Assets.CreateHopooMaterial("matGoldenButtons");
        //internal static Material matGranCape = Modules.Assets.CreateHopooMaterial("matGranCape");
        //internal static Material matGranMantle = Modules.Assets.CreateHopooMaterial("matGranMantle");
        //internal static Material matHair = Modules.Assets.CreateHopooMaterial("matHair");
        //internal static Material matHands = Modules.Assets.CreateHopooMaterial("matHands");
        //internal static Material matHood = Modules.Assets.CreateHopooMaterial("matHood");
        //internal static Material matLegs = Modules.Assets.CreateHopooMaterial("matLegs");
        //internal static Material matMask = Modules.Assets.CreateHopooMaterial("matMask");
        //internal static Material matMidGauntlet = Modules.Assets.CreateHopooMaterial("matMidGauntlet");
        //internal static Material matShoes = Modules.Assets.CreateHopooMaterial("matShoes");
        //internal static Material matWristband = Modules.Assets.CreateHopooMaterial("matWristband");

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
                Assets.mainAssetBundle.LoadAsset<Sprite>("Airforceskin"),
                defaultRendererInfo,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("ArmL"),
                    renderer = defaultRendererInfo[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("ArmR"),
                    renderer = defaultRendererInfo[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Belt"),
                    renderer = defaultRendererInfo[2].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Body"),
                    renderer = defaultRendererInfo[3].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Eyes"),
                    renderer = defaultRendererInfo[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Face"),
                    renderer = defaultRendererInfo[5].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Neck"),
                    renderer = defaultRendererInfo[6].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("GoldenButtons"),
                    renderer = defaultRendererInfo[7].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("GranCape"),
                    renderer = defaultRendererInfo[8].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("GranMantle"),
                    renderer = defaultRendererInfo[9].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Hair"),
                    renderer = defaultRendererInfo[10].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Hands"),
                    renderer = defaultRendererInfo[11].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Hood"),
                    renderer = defaultRendererInfo[12].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Legs"),
                    renderer = defaultRendererInfo[13].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Mask"),
                    renderer = defaultRendererInfo[14].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("MidGauntletL"),
                    renderer = defaultRendererInfo[15].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("MidGauntletR"),
                    renderer = defaultRendererInfo[16].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("MidGauntlets"),
                    renderer = defaultRendererInfo[17].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Shoes"),
                    renderer = defaultRendererInfo[18].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Wristband"),
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
                Assets.mainAssetBundle.LoadAsset<Sprite>("Airforceskin"),
                eyeRendererInfo,
                mainRenderer,
                model);

            eyeSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("ArmL"),
                    renderer = defaultRendererInfo[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("ArmR"),
                    renderer = defaultRendererInfo[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Belt"),
                    renderer = defaultRendererInfo[2].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Body"),
                    renderer = defaultRendererInfo[3].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Eyes"),
                    renderer = defaultRendererInfo[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Face"),
                    renderer = defaultRendererInfo[5].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Neck"),
                    renderer = defaultRendererInfo[6].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("GoldenButtons"),
                    renderer = defaultRendererInfo[7].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("GranCape"),
                    renderer = defaultRendererInfo[8].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("GranMantle"),
                    renderer = defaultRendererInfo[9].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Hair"),
                    renderer = defaultRendererInfo[10].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Hands"),
                    renderer = defaultRendererInfo[11].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Hood"),
                    renderer = defaultRendererInfo[12].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Legs"),
                    renderer = defaultRendererInfo[13].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Mask"),
                    renderer = defaultRendererInfo[14].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("MidGauntletL"),
                    renderer = defaultRendererInfo[15].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("MidGauntletR"),
                    renderer = defaultRendererInfo[16].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("MidGauntlets"),
                    renderer = defaultRendererInfo[17].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Shoes"),
                    renderer = defaultRendererInfo[18].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Wristband"),
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

