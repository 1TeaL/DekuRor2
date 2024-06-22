using R2API;
using System;
using System.Collections.Generic;
using System.Text;

namespace DekuMod.Modules
{
    public static class Damage
    {
        internal static DamageAPI.ModdedDamageType blackwhipImmobilise;

        internal static void SetupModdedDamage()
        {
            blackwhipImmobilise = DamageAPI.ReserveDamageType();
        }
    }
}
