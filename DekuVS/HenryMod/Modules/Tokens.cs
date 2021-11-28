using R2API;
using System;

namespace DekuMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Deku
            string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";

            string desc = "Deku is high risk survivor that can boost his stats and abilities but with a health cost.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Airforce  is a simple ranged attack " + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > ShootStyle is a dash, damaging enemies behind him. " + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Blackwhip pulls enemies to a point, granting barrier on hit. " + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > One for All 100% grants increased stats at a cost of health regen and self-damage with his skills." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > | Delaware Smash is an AOE, launching you backwards." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > | Detroit smash is a chargeable punch that gains AOE as you charge." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > | StLouis Smash is a dash with invincibility, htiting nearby enemies multiple times." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, continuing his journey to become the greatest hero.";
            string outroFailure = "..goodbye..One For All.";

            LanguageAPI.Add(prefix + "NAME", "Deku");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Ninth One For All User");
            LanguageAPI.Add(prefix + "LORE", "I forgot to mention this, but this is the story of how I became the world's greatest hero");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Deku passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
             #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_NAME", "Airforce");
            LanguageAPI.Add(prefix + "PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot a bullet, dealing <style=cIsDamage>{100f * StaticValues.airforceDamageCoefficient}% damage twice</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY_NAME", "StLouis Smash");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY_DESCRIPTION", $"Dash and disappear, hitting enemies in the vicinity for <style=cIsDamage>{100f * StaticValues.shootattackDamageCoefficient}% damage twice</style>");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY_NAME", "Detroit Smash");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Charge a Detroit Smash, instantly dashing and dealing <style=cIsDamage>{100f * StaticValues.detroitDamageCoefficient}% increasing infinitely</style>.");
            LanguageAPI.Add(prefix + "SECONDARY_NAME", "Blackwhip");
            LanguageAPI.Add(prefix + "SECONDARY_DESCRIPTION", $"<style=cIsDamage>Stunning</style>. Blackwhip enemies, pulling, stunning and dealing <style=cIsDamage>{100f * StaticValues.blackwhipDamageCoefficient}%</style>, hitting them gains barrier.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_NAME", "Shoot Style");
            LanguageAPI.Add(prefix + "UTILITY_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Dash through enemies, stunning and dealing <style=cIsDamage>{100f * StaticValues.shootbulletDamageCoefficient}% damage twice</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY_NAME", "Delaware Smash");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Delaware Smash, dealing <style=cIsDamage>{100f * StaticValues.delawareDamageCoefficient}% damage in an AOE, sending yourself backwards</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_NAME", "OFA 100%");
            LanguageAPI.Add(prefix + "SPECIAL_DESCRIPTION", $"Push your body to its limits, boosting Armor, Movespeed, Damage, Attackspeed, <style=cIsDamage>gaining negative Regen and Self-Damage from every move</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_NAME", "OFA down");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_DESCRIPTION", $"Return yourself back to your limits.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_NAME", $"Infinite 100%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_DESCRIPTION", $"Unlock the true power of One For All, gaining the same boosts as well as lifesteal.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Deku: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Deku, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Deku: Mastery");
            #endregion
            #endregion


        }
    }
}
