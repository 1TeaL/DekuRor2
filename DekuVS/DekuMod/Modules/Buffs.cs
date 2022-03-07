using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace DekuMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef armorBuff;

        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static BuffDef ofaBuff;

        internal static BuffDef kickBuff;

        internal static BuffDef floatBuff;

        internal static BuffDef supaofaBuff;

        internal static BuffDef ofaBuff45;

        internal static BuffDef supaofaBuff45;

        internal static BuffDef fajinBuff;

        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("DekuArmorBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.white, false, false);
            ofaBuff = AddNewBuff("DekuOFABuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.green, false, false);
            supaofaBuff = AddNewBuff("DekuInfiniteOFABuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.white, false, false);
            kickBuff = AddNewBuff("DekuKickBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.cyan, true, false);
            ofaBuff45 = AddNewBuff("DekuOFA45Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.blue, false, false);
            supaofaBuff45 = AddNewBuff("DekuInfiniteOFA45Buff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.grey, false, false);

            fajinBuff = AddNewBuff("FaJinBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffBodyArmorIcon"), Color.green, true, false);
            floatBuff = AddNewBuff("floatBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texMovespeedBufficon"), Color.blue, false, true);
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

            buffDefs.Add(buffDef);

            return buffDef;
        }

    }
}