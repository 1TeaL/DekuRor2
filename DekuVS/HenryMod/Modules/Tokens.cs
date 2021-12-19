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

            string desc = "Deku is high risk survivor that can boost his stats and abilities but with detrimental health regen and health costs.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > He scales with attack speed and movespeed on multiple of his skills" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > He can also sprint in any direction, enabled by using either of his primary skills once or detroit smashes" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > OFA 100%'s negative health regen can be mitigated with regen items and it stops at 1-2Hp while OFA 45%'s can't." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Ancient Scepter will give 10% Lifesteal for OFA 100% and 5% Lifesteal for OFA 45%" + Environment.NewLine + Environment.NewLine;



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
            LanguageAPI.Add(prefix + "PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot a bullet, dealing <style=cIsDamage>2x</style><style=cIsDamage>{100f * StaticValues.airforceDamageCoefficient}%</style>.");
            LanguageAPI.Add(prefix + "PRIMARY2_NAME", "Shoot Style Kick");
            LanguageAPI.Add(prefix + "PRIMARY2_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Dash and kick, dealing <style=cIsDamage>{100f * StaticValues.shootkickDamageCoefficient}% damage, scaling with movespeed</style>, resetting the cooldown on hit and resetting all cooldowns on kill.");
            
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY_NAME", "StLouis Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY_DESCRIPTION", $"Dash through enemies, hitting enemies behind for <style=cIsDamage>{100f * StaticValues.shootbulletDamageCoefficient}% damage scaling by attack speed</style>." );
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY2_NAME", "Airforce 45%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY2_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot 4 bullets with all your fingers, dealing <style=cIsDamage>{100f * StaticValues.airforce45DamageCoefficient}% damage each</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_NAME", "Blackwhip");
            LanguageAPI.Add(prefix + "SECONDARY_DESCRIPTION", $"<style=cIsDamage>Stunning</style>. Blackwhip enemies, pulling, stunning and dealing <style=cIsDamage>{100f * StaticValues.blackwhipDamageCoefficient}%</style>, gaining barrier on hit, scaling with attack speed.");
            LanguageAPI.Add(prefix + "SECONDARY2_NAME", "Manchester Smash");
            LanguageAPI.Add(prefix + "SECONDARY2_DESCRIPTION", $"<style=cIsDamage>Stunning</style>. Jump in the air and slam down, dealing <style=cIsDamage>{100f * StaticValues.manchesterDamageCoefficient}%</style> and gaining barrier on hit, scaling with movespeed.");

            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY_NAME", "Detroit Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Charge a Detroit Smash, instantly dashing and dealing <style=cIsDamage>{100f * StaticValues.detroit100DamageCoefficient}% increasing infinitely</style>. " + Helpers.Damage("Costs 10% of max Health")+".");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY2_NAME", "Blackwhip 45%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY2_DESCRIPTION", $"<style=cIsDamage>Stunning</style>. Blackwhip enemies, pulling them right in front of you, stunning and dealing <style=cIsDamage>{100f * StaticValues.blackwhip45DamageCoefficient}%</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_NAME", "Shoot Style");
            LanguageAPI.Add(prefix + "UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Dash and go invincible, hitting enemies multiple times for <style=cIsDamage>{100f * StaticValues.shootattackDamageCoefficient}% damage</style>.");
            LanguageAPI.Add(prefix + "UTILITY2_NAME", "Shoot Style Full Cowling");
            LanguageAPI.Add(prefix + "UTILITY2_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Dash through enemies, stunning enemies and dealing <style=cIsDamage>{100f * StaticValues.shootbulletstunDamageCoefficient}% damage scaling by attack speed</style>.");
            LanguageAPI.Add(prefix + "UTILITY3_NAME", "Detroit Smash");
            LanguageAPI.Add(prefix + "UTILITY3_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Charge a Detroit Smash, instantly dashing and dealing <style=cIsDamage>{100f * StaticValues.detroitDamageCoefficient}%</style>, range scales based on attack speed and movespeed.");

            LanguageAPI.Add(prefix + "BOOSTEDUTILITY_NAME", "Delaware Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Delaware Smash, dealing <style=cIsDamage>{100f * StaticValues.delawareDamageCoefficient}% damage in an AOE, sending yourself backwards</style>. " + Helpers.Damage("Costs 10% of max Health")+".");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY2_NAME", "St Louis Smash 45%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY2_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> St Louis Smash, stunning enemies in front, dealing <style=cIsDamage>{100f * StaticValues.StLouis45DamageCoefficient}% damage in an AOE </style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_NAME", "OFA 100%");
            LanguageAPI.Add(prefix + "SPECIAL_DESCRIPTION", $"Go beyond your limits, boosting Armor, Movespeed, Damage, Attackspeed, gain unique 100% moves, " + Helpers.Damage("gaining negative Regen and Self-Damage from every move")+".");
            LanguageAPI.Add(prefix + "SPECIAL2_NAME", "OFA 45%");
            LanguageAPI.Add(prefix + "SPECIAL2_DESCRIPTION", $"Push your body to its limits, boosting Armor, Movespeed, Damage, Attackspeed, gain unique 45% moves, " + Helpers.Damage("disabling Health Regen") + ".");

            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_NAME", "OFA down");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_DESCRIPTION", $"Return yourself back to your limits.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_NAME", $"Infinite 100%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_DESCRIPTION", $"Unlock the true power of One For All, gaining the same effects as well as 10% lifesteal.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_NAME", $"Infinite 45%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_DESCRIPTION", $"Master OFA 45%, gaining the same effects as well as 5% lifesteal.");
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
