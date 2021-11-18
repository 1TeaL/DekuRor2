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

            string desc = "Deku is high risk survivor that hurts himself to power up his skills.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Delaware Smash  is a simple ranged attack; when powered up increases its AOE and piercing enemies." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Detroit smash is a chargeable punch that gains AOE as you charge; when powered up turns it into St. Louis Smash, increasing its AOE and piercing enemies." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Blackwhip allows him to move through the environment and grapple on to enemies; when powered up lets him pull multiple enemies towards him." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > One for All 100% enables Deku to use one powered up skill at the cost of a percentage of his health." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

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
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
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
//            string outro = "..and so he left, searching for a new world to save.";
//            string outroFailure = "..and so he vanished, continuing his journey to become the greatest Hero.";

//            LanguageAPI.Add(prefix + "NAME", "Deku");
//            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
//            LanguageAPI.Add(prefix + "SUBTITLE", "Ninth One For All User");
//            LanguageAPI.Add(prefix + "LORE", "I forgot to mention this, but this is the story of how I became the world's greatest hero");
//            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
//            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

//            #region Skins
//            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
//            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
//            #endregion

//            #region Passive
//            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Deku passive");
//            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
//            #endregion

//            #region Primary
//            LanguageAPI.Add(prefix + "DEKU_PRIMARY_NAME", "Delaware Smash Air Force");
//            LanguageAPI.Add(prefix + "DEKU_PRIMARY_DESCRIPTION", Helpers.agilePrefix + "Shoot a blast of compressed air, doing <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
//            #endregion

//            #region Secondary
//            LanguageAPI.Add(prefix + "DEKU_SECONDARY_NAME", "Detroit Smash");
//            LanguageAPI.Add(prefix + "DEKU_SECONDARY_DESCRIPTION", Helpers.agilePrefix + "Charge up One For All's power into a fist, dealing up to <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
//            #endregion

//            #region Utility
//            LanguageAPI.Add(prefix + "DEKU_UTILITY_NAME", "Blackwhip");
//            LanguageAPI.Add(prefix + "DEKU_UTILITY_DESCRIPTION", "Fire Blackwhip forward, dealing <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage. Pulling light targets and moving towards heavy targets</style>");
//            #endregion

//            #region Special
//            LanguageAPI.Add(prefix + "DEKU_SPECIAL_NAME", "OFA 100%");
//            LanguageAPI.Add(prefix + "DEKU_SPECIAL_DESCRIPTION", "Use One For All at full power, <style=cIsUtility>boosting the next ability cast</style>.");