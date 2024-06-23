using RoR2;
using System.Collections.Generic;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine.AddressableAssets;

namespace DekuMod.Modules
{
    public static class Buffs
    {
        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        //ofa
        internal static BuffDef mightBuff;
        //internal static BuffDef supaofaBuff;
        //internal static BuffDef ofaBuff45;
        //internal static BuffDef supaofaBuff45;

        ////skills
        //internal static BuffDef kickBuff;
        //internal static BuffDef floatBuff;
        //internal static BuffDef fajinBuff;

        //go beyond
        internal static BuffDef goBeyondBuff;
        internal static BuffDef goBeyondBuffUsed;

        //blackwhip
        internal static BuffDef overlayBuff;

        //armor buff 
        //internal static BuffDef manchesterBuff;
        //internal static BuffDef oklahomaBuff;

        //dangersense
        internal static BuffDef counterAttackBuff;
        internal static BuffDef counterBuff;
        internal static BuffDef dangersenseBuff;
        //internal static BuffDef dangersense45Buff;
        //internal static BuffDef dangersense100Buff;

        //gearshift
        //internal static BuffDef gearshiftBuff;
        //internal static BuffDef gearshift45Buff;
        //internal static BuffDef gearshift100Buff;
        //internal static BuffDef gearshift100MovespeedBuff;

        //debuffs
        internal static BuffDef delayAttackDebuff;
        internal static BuffDef dangersenseDebuff;
        internal static BuffDef blackwhipDebuff;

        //marks
        //internal static BuffDef healMark;
        //internal static BuffDef barrierMark;

        internal static void RegisterBuffs()
        {
            //oklahomaBuff = Buffs.AddNewBuff("DekuArmor Buff", Assets.shieldBuffIcon, Color.green, false, false);
            //manchesterBuff = Buffs.AddNewBuff("DekuArmor Buff", Assets.shieldBuffIcon, Color.green, false, false);
            mightBuff = Buffs.AddNewBuff("DekuOFA Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightninggreen"), Color.white, false, false);
            //supaofaBuff = Buffs.AddNewBuff("DekuInfiniteOFA Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningwhitegreen"), Color.white, false, false);
            //kickBuff = Buffs.AddNewBuff("DekuKick Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("kickCount"), Color.white, true, false);
            //blackwhipBuff = Buffs.AddNewBuff("blackwhip Buff", Assets.crippleBuffIcon, Color.white, false, false);
            //ofaBuff45 = Buffs.AddNewBuff("DekuOFA45 Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningblue"), Color.white, false, false);
            //supaofaBuff45 = Buffs.AddNewBuff("DekuInfiniteOFA45 Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningwhiteblue"), Color.white, false, false);
            goBeyondBuff = Buffs.AddNewBuff("goBeyond Buff", Assets.healBuffIcon, Color.green, false, false);
            goBeyondBuffUsed = Buffs.AddNewBuff("goBeyondBuffUsed", Assets.healBuffIcon, Color.black, false, false);

            //fajinBuff = Buffs.AddNewBuff("FaJin Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("armorgreen"), Color.white, false, false);
            //floatBuff = Buffs.AddNewBuff("DekuFloat Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("Float"), Color.white, false, false);
            //gearshiftBuff = Buffs.AddNewBuff("gearshift Buff", Assets.speedBuffIcon, Color.white, false, false);
            //gearshift45Buff = Buffs.AddNewBuff("gearshift45 Buff", Assets.speedBuffIcon, Color.blue, false, false);
            //gearshift100Buff = Buffs.AddNewBuff("gearshift100 Buff", Assets.jumpBuffIcon, Color.white, true, false);
            //gearshift100MovespeedBuff = Buffs.AddNewBuff("gearshift100Movespeed Buff", Assets.jumpBuffIcon, Color.blue, true, false);

            overlayBuff = Buffs.AddNewBuff("Blackwhip Overlay Buff", Assets.jumpBuffIcon, Color.black, false, false);
            counterBuff = Buffs.AddNewBuff("Counter Buff", Assets.shieldBuffIcon, Color.cyan, false, false);
            counterAttackBuff = Buffs.AddNewBuff("Counter Buff", Assets.shieldBuffIcon, Color.cyan, false, false);

            blackwhipDebuff = Buffs.AddNewBuff("blackwhip DeBuff", Assets.crippleBuffIcon, Color.black, false, true);
            delayAttackDebuff = Buffs.AddNewBuff("delayAttack Debuff", Assets.crippleBuffIcon, Color.green, true, true);

            //dangersenseBuff = Buffs.AddNewBuff("DangerSense Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("DangerSense"), Color.white, false, false);
            //dangersenseDebuff = Buffs.AddNewBuff("dangersenseDe Buff", Assets.lightningBuffIcon, Color.black, false, true);
            ////dangersense45Buff = Buffs.AddNewBuff("DangerSense45 Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("Counter"), Color.white, false, false);
            ////dangersense100Buff = Buffs.AddNewBuff("DangerSense100 Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("Counter"), Color.white, false, false);
            //healMark = Buffs.AddNewBuff("healMark", Assets.critBuffIcon, Color.green, true, true);
            //barrierMark = Buffs.AddNewBuff("goBeyond BuffUsed", Assets.critBuffIcon, Color.yellow, true, true);

            //oklahomaBuff = Buffs.AddNewBuff("DekuArmor Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.green, false, false);
            //oklahomaBuff = Buffs.AddNewBuff("DekuArmor Buff", Addressables.LoadAssetAsync<Sprite>(key: "RoR2/Base/Textures/BuffIcons/texBuffGenericShield.prefab").WaitForCompletion(), Color.green, false, false);
            //ofaBuff = Buffs.AddNewBuff("DekuOFA Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.green, false, false);
            //supaofaBuff = Buffs.AddNewBuff("DekuInfiniteOFA Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.white, false, false);
            //kickBuff = Buffs.AddNewBuff("DekuKick Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.cyan, true, false);
            //ofaBuff45 = Buffs.AddNewBuff("DekuOFA45 Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.blue, false, false);
            //supaofaBuff45 = Buffs.AddNewBuff("DekuInfiniteOFA45 Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.grey, false, false);

            //fajinBuff = Buffs.AddNewBuff("FaJin Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffBodyArmorIcon"), Color.green, true, false);
            //floatBuff = Buffs.AddNewBuff("DekuFloat Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texMovespeedBufficon"), Color.blue, false, false);
            //dangersenseBuff = Buffs.AddNewBuff("Dekudangersense Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffNullifyStackIcon"), Color.green, false, false);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Buffs.buffDefs.Add(buffDef);

            return buffDef;
        }

    }
}