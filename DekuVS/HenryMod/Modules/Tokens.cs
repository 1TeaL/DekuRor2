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
            desc = desc + "< ! > OFA 100% focuses on close range but greater mobility while OFA 45% focuses on mid-range attacks." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > OFA 100%'s negative health regen can be mitigated with regen items and it stops at 1-2Hp while OFA 45%'s can't." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Ancient Scepter will give 10% Lifesteal for OFA 100% and 5% Lifesteal for OFA 45%." + Environment.NewLine + Environment.NewLine;



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
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Ninth One For All User");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Deku has innate increased health regen the lower his health is. He has a double jump. He can sprint in any direction.");
             #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_NAME", "Airforce");
            LanguageAPI.Add(prefix + "PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot a bullet, dealing <style=cIsDamage>2x{100f * StaticValues.airforceDamageCoefficient}%</style>.<style=cIsUtility> Fa Jin buff makes the bullets ricochet</style>.");
            LanguageAPI.Add(prefix + "PRIMARY2_NAME", "Shoot Style Kick");
            LanguageAPI.Add(prefix + "PRIMARY2_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Dash and kick, dealing <style=cIsDamage>{100f * StaticValues.shootkickDamageCoefficient}% damage, scaling with movespeed</style>, resetting the cooldown on hit and resetting all cooldowns on kill. <style=cIsUtility>Fa Jin buff makes shoot style kick freeze enemies</style>.");
            
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY_NAME", "StLouis Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY_DESCRIPTION", $"Dash through enemies, hitting enemies behind for <style=cIsDamage>{100f * StaticValues.shootbulletDamageCoefficient}% damage scaling by attack speed</style>." );
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY2_NAME", "Airforce 45%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY2_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot 4 bullets with all your fingers, dealing <style=cIsDamage>{100f * StaticValues.airforce45DamageCoefficient}% damage each</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY3_NAME", "Fa Jin");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY3_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Charge up kinetic energy, dealing <style=cIsDamage>{100f* StaticValues.fajinDamageCoefficient}% damage</style>, granting 10 stacks of Fa Jin. Does not consume Fa Jin.");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY4_NAME", "Fa Jin Mastered");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY4_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Charge up kinetic energy, dealing <style=cIsDamage>{100f * StaticValues.fajinDamageCoefficient}% damage</style>, granting 20 stacks of Fa Jin. Does not consume Fa Jin.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_NAME", "Blackwhip");
            LanguageAPI.Add(prefix + "SECONDARY_DESCRIPTION", $"<style=cIsDamage>Stunning</style>. Blackwhip enemies, pulling, stunning and dealing <style=cIsDamage>5x{100f * StaticValues.blackwhipDamageCoefficient}%</style>, gaining barrier on hit, scaling with attack speed. <style=cIsUtility>Fa Jin buff makes blackwhip double the barrier gain</style>.");
            LanguageAPI.Add(prefix + "SECONDARY2_NAME", "Manchester Smash");
            LanguageAPI.Add(prefix + "SECONDARY2_DESCRIPTION", $"<style=cIsDamage>Stunning</style>. Jump in the air and slam down, dealing <style=cIsDamage>{100f * StaticValues.manchesterDamageCoefficient}%</style> and gaining barrier on hit, scaling with movespeed. <style=cIsUtility>Fa Jin buff hits and pulls enemies when you jump as well as doubling barrier gain</style>.");

            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY_NAME", "Detroit Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Charge a Detroit Smash, instantly dashing and dealing <style=cIsDamage>{100f * StaticValues.detroit100DamageCoefficient}% increasing infinitely</style>. " + Helpers.Damage("Costs 10% of max Health")+".");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY2_NAME", "Blackwhip 45%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY2_DESCRIPTION", $"<style=cIsDamage>Stunning</style>. Blackwhip enemies, pulling them right in front of you, stunning and dealing <style=cIsDamage>5x{100f * StaticValues.blackwhip45DamageCoefficient}%</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY3_NAME", "Blackwhip Combo");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY3_DESCRIPTION", $"Hit enemies in front of you and shoot blackwhip, dealing <style=cIsDamage>{100f * StaticValues.blackwhipshootDamageCoefficient}%</style>. Tapping pulls you forward while Holding pulls enemies towards you. <style=cIsUtility>Fa Jin buff makes blackwhip to shoot multiple times and have a larger melee hitbox</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_NAME", "Shoot Style");
            LanguageAPI.Add(prefix + "UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Dash and go invincible, hitting enemies multiple times for <style=cIsDamage>{100f * StaticValues.shootattackDamageCoefficient}% damage</style>. <style=cIsUtility>Fa Jin buff doubles the number of hits</style>.");
            LanguageAPI.Add(prefix + "UTILITY2_NAME", "Shoot Style Full Cowling");
            LanguageAPI.Add(prefix + "UTILITY2_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Dash through enemies, stunning enemies and dealing <style=cIsDamage>{100f * StaticValues.shootbulletstunDamageCoefficient}% damage scaling by attack speed</style>. <style=cIsUtility>Fa Jin buff doubles the number of hits</style>.");
            LanguageAPI.Add(prefix + "UTILITY3_NAME", "Detroit Smash");
            LanguageAPI.Add(prefix + "UTILITY3_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Charge a Detroit Smash, instantly dashing and dealing <style=cIsDamage>{100f * StaticValues.detroitDamageCoefficient}%</style>, range scales based on attack speed and movespeed. <style=cIsUtility>Fa Jin buff doubles everything</style>.");

            LanguageAPI.Add(prefix + "BOOSTEDUTILITY_NAME", "Delaware Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Delaware Smash, dealing <style=cIsDamage>{100f * StaticValues.delawareDamageCoefficient}% damage in an AOE, sending yourself backwards</style>. " + Helpers.Damage("Costs 10% of max Health")+".");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY2_NAME", "St Louis Smash 45%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY2_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> St Louis Smash, stunning enemies in front, dealing <style=cIsDamage>{100f * StaticValues.StLouis45DamageCoefficient}% damage</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY3_NAME", "Smokescreen");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY3_DESCRIPTION", $"<style=cIsDamage>Slowing.</style> Release a smokescreen, going invisible for 4 seconds and dealing <style=cIsDamage>{100f * StaticValues.smokescreenDamageCoefficient}% damage</style> around you. <style=cIsUtility>Fa Jin buff makes nearby allies invisible as well</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_NAME", "OFA 100%");
            LanguageAPI.Add(prefix + "SPECIAL_DESCRIPTION", $"Go beyond your limits, boosting Armor, Movespeed, Damage, Attackspeed, gain unique 100% moves, " + Helpers.Damage("gaining negative Regen and Self-Damage from every move")+".");
            LanguageAPI.Add(prefix + "SPECIAL2_NAME", "OFA 45%");
            LanguageAPI.Add(prefix + "SPECIAL2_DESCRIPTION", $"Push your body to its limits, boosting Armor, Movespeed, Damage, Attackspeed, gain unique 45% moves, " + Helpers.Damage("disabling Health Regen") + ".");
            LanguageAPI.Add(prefix + "SPECIAL3_NAME", "OFA Quirks");
            LanguageAPI.Add(prefix + "SPECIAL3_DESCRIPTION", $"Unlock your additional quirks. This skill grants the Fa Jin buff. Moving increases the buff up to 100 stacks. Gain up to 2x damage at 50 stacks. Every move consumes 50 stacks. However, if a move uses 50 stacks it has additional effects. <style=cIsUtility>In general all moves will stun and bypass armor, have double the movement, double the radius and range</style>.");

            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_NAME", "OFA down");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_DESCRIPTION", $"Return yourself back to your limits.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_NAME", $"Infinite 100%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_DESCRIPTION", $"Unlock the true power of One For All, gaining the same effects as well as 10% lifesteal.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_NAME", $"Infinite 45%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_DESCRIPTION", $"Master OFA 45%, gaining the same effects as well as 5% lifesteal.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_NAME", $"OFA Quirks Mastered");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_DESCRIPTION", $"Fa Jin Buff stacks are granted at double the rate.");
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
