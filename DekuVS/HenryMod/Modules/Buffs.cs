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

        internal static BuffDef floatBuff;

        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("DekuArmorBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.white, false, false);
            ofaBuff = AddNewBuff("DekuOFABuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.green, false, true);
            floatBuff = AddNewBuff("floatBuff", Resources.Load<Sprite>("Textures/BuffIcons/texMovespeedBufficon"), Color.blue, false, true);
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
        internal static void HandleBuffs(CharacterBody body)
        {
            bool flag = body;
            if (flag)
            {
                bool flag2 = body.HasBuff(Buffs.floatBuff);
                if (flag2)
                {
                    body.moveSpeed *= 1.5f;
                    body.acceleration *= 2f;
                }
            }
        }
    }
}