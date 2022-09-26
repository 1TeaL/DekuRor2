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
        internal static BuffDef ofaBuff;
        internal static BuffDef supaofaBuff;
        internal static BuffDef ofaBuff45;
        internal static BuffDef supaofaBuff45;

        //skills
        internal static BuffDef kickBuff;
        internal static BuffDef floatBuff;
        internal static BuffDef fajinBuff;

        //go beyond
        internal static BuffDef goBeyondBuff;
        internal static BuffDef goBeyondBuffUsed;

        //armor buff 
        internal static BuffDef manchesterBuff;
        internal static BuffDef oklahomaBuff;

        //counter
        internal static BuffDef dangersenseBuff;
        internal static BuffDef dangersense45Buff;
        internal static BuffDef dangersense100Buff;
        //debuffs
        internal static BuffDef delayAttackDebuff;

        internal static void RegisterBuffs()
        {
            oklahomaBuff = Buffs.AddNewBuff("DekuArmorBuff", Assets.shieldBuffIcon, Color.green, false, false);
            manchesterBuff = Buffs.AddNewBuff("DekuArmorBuff", Assets.shieldBuffIcon, Color.green, false, false);
            ofaBuff = Buffs.AddNewBuff("DekuOFABuff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightninggreen"), Color.white, false, false);
            supaofaBuff = Buffs.AddNewBuff("DekuInfiniteOFABuff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningwhitegreen"), Color.white, false, false);
            kickBuff = Buffs.AddNewBuff("DekuKickBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("kickCount"), Color.white, true, false);
            ofaBuff45 = Buffs.AddNewBuff("DekuOFA45Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningblue"), Color.white, false, false);
            supaofaBuff45 = Buffs.AddNewBuff("DekuInfiniteOFA45Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningwhiteblue"), Color.white, false, false);
            goBeyondBuff = Buffs.AddNewBuff("goBeyondBuff", Assets.healBuffIcon, Color.green, false, false);
            goBeyondBuffUsed = Buffs.AddNewBuff("goBeyondBuffUsed", Assets.healBuffIcon, Color.black, false, false);

            fajinBuff = Buffs.AddNewBuff("FaJinBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("armorgreen"), Color.white, true, false);
            floatBuff = Buffs.AddNewBuff("DekuFloatBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("Float"), Color.white, false, false);
            dangersenseBuff = Buffs.AddNewBuff("DangerSenseBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("Counter"), Color.white, false, false);
            dangersense45Buff = Buffs.AddNewBuff("DangerSense45Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("Counter"), Color.white, false, false);
            dangersense100Buff = Buffs.AddNewBuff("DangerSense100Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("Counter"), Color.white, false, false);
            delayAttackDebuff = Buffs.AddNewBuff("delayAttackDebuff", Assets.crippleBuffIcon, Color.green, true, true);

            //oklahomaBuff = Buffs.AddNewBuff("DekuArmorBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.green, false, false);
            //oklahomaBuff = Buffs.AddNewBuff("DekuArmorBuff", Addressables.LoadAssetAsync<Sprite>(key: "RoR2/Base/Textures/BuffIcons/texBuffGenericShield.prefab").WaitForCompletion(), Color.green, false, false);
            //ofaBuff = Buffs.AddNewBuff("DekuOFABuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.green, false, false);
            //supaofaBuff = Buffs.AddNewBuff("DekuInfiniteOFABuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.white, false, false);
            //kickBuff = Buffs.AddNewBuff("DekuKickBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.cyan, true, false);
            //ofaBuff45 = Buffs.AddNewBuff("DekuOFA45Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.blue, false, false);
            //supaofaBuff45 = Buffs.AddNewBuff("DekuInfiniteOFA45Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.grey, false, false);

            //fajinBuff = Buffs.AddNewBuff("FaJinBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffBodyArmorIcon"), Color.green, true, false);
            //floatBuff = Buffs.AddNewBuff("DekuFloatBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texMovespeedBufficon"), Color.blue, false, false);
            //dangersenseBuff = Buffs.AddNewBuff("DekudangersenseBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffNullifyStackIcon"), Color.green, false, false);
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