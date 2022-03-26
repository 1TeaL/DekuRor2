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
            desc = desc + "< ! > He scales with attackspeed and movespeed on multiple of his skills" + Environment.NewLine + Environment.NewLine;
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
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", Helpers.Passive("Deku has innate increased health regen the lower his health is.") + "<style=cIsUtility> He has a double jump. He can sprint in any direction.</style>");
             #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_NAME", "Airforce");
            LanguageAPI.Add(prefix + "PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot a bullet, dealing <style=cIsDamage>2x{100f * StaticValues.airforceDamageCoefficient}%</style>." + Helpers.Passive(" Fa Jin buff makes the bullets ricochet") + ".");
            LanguageAPI.Add(prefix + "PRIMARY2_NAME", "Shoot Style Kick");
            LanguageAPI.Add(prefix + "PRIMARY2_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Dash and kick, dealing <style=cIsDamage>{100f * StaticValues.shootkickDamageCoefficient}% damage, scaling with movespeed</style>, resetting the cooldown on hit and resetting all cooldowns on kill</style>." + Helpers.Passive(" Fa Jin buff makes shoot style kick freeze enemies and deal an extra hit") + ".");
            LanguageAPI.Add(prefix + "PRIMARY3_NAME", "Danger Sense");
            LanguageAPI.Add(prefix + "PRIMARY3_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Activate Danger Sense, when timed properly, dodge, reset CD, stun and deal <style=cIsDamage>{100f * StaticValues.counterDamageCoefficient}% damage</style>, to the attacker and enemies around Deku. Timing window scales with attackspeed. Total duration is 2 seconds</style>." + Helpers.Passive(" Fa Jin buff freezes and increases the timing window") + ".");

            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY_NAME", "Shoot Style Full Cowling 100%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY_DESCRIPTION", $"Dash through enemies, hitting enemies behind for <style=cIsDamage>{100f * StaticValues.shootbulletDamageCoefficient}% damage scaling by attackspeed</style>." + Helpers.Damage(" Costs 1% of max Health") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY2_NAME", "Airforce 45%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY2_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot 4 bullets with all your fingers, dealing <style=cIsDamage>{100f * StaticValues.airforce45DamageCoefficient}% damage each</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY3_NAME", "Fa Jin");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY3_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Charge up kinetic energy, dealing <style=cIsDamage>{100f* StaticValues.fajinDamageCoefficient}% damage</style>." + Helpers.Passive(" Grants 10 stacks of Fa Jin. Does not consume Fa Jin") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY4_NAME", "Fa Jin Mastered");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY4_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Charge up kinetic energy, dealing <style=cIsDamage>{100f * StaticValues.fajinDamageCoefficient}% damage</style>." + Helpers.Passive(" Grants 10 stacks of Fa Jin. Does not consume Fa Jin") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY5_NAME", "Airforce 100%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY5_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Shoot beams with your fists, stunning and dealing <style=cIsDamage>{100f * StaticValues.airforce100DamageCoefficient}% damage</style>, initially having 20% attackspeed, ramping up to 200%." + Helpers.Damage(" Costs 1% of max Health") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY6_NAME", "Shoot Style Kick 45%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY6_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Dash and kick, dealing <style=cIsDamage>{100f * StaticValues.shootkick45DamageCoefficient}% damage, scaling with movespeed</style>, resetting the cooldown on hit and resetting all cooldowns on kill.");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY7_NAME", "Shoot Style Kick 100%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY7_DESCRIPTION", $"<style=cIsDamage>Freezing. Agile.</style> Dash and kick, freezing and dealing <style=cIsDamage>{100f * StaticValues.shootkick100DamageCoefficient}% damage twice, scaling with movespeed</style>, resetting the cooldown on hit and resetting all cooldowns on kill." + Helpers.Damage(" Costs 1% of max Health") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY8_NAME", "Danger Sense 45%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY8_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Activate Danger Sense, when timed properly, dodge, reset CD, stun and deal <style=cIsDamage>{100f * StaticValues.counterDamageCoefficient}% damage</style> to the attacker and enemies around Deku. Timing window scales with attackspeed. Total duration is 1.5 seconds</style>." );
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY9_NAME", "Danger Sense 100%");
            LanguageAPI.Add(prefix + "BOOSTEDPRIMARY9_DESCRIPTION", $"<style=cIsDamage>Freezing. Agile.</style> Activate Danger Sense, when timed properly, dodge, reset CD, freeze and deal <style=cIsDamage>{100f * StaticValues.counterDamageCoefficient}% damage</style>, to the attacker and enemies around Deku. Timing window scales with attackspeed. Total duration is 1 second</style>." + Helpers.Damage(" Costs 5% of max Health") + ".");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_NAME", "Blackwhip");
            LanguageAPI.Add(prefix + "SECONDARY_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Blackwhip enemies, pulling, stunning and dealing <style=cIsDamage>5x{100f * StaticValues.blackwhipDamageCoefficient}%</style>, gaining barrier on hit, scaling with attackspeed</style>." + Helpers.Passive(" Fa Jin buff makes blackwhip double the barrier gain") + ".");
            LanguageAPI.Add(prefix + "SECONDARY2_NAME", "Manchester Smash");
            LanguageAPI.Add(prefix + "SECONDARY2_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Jump in the air and slam down, dealing <style=cIsDamage>{100f * StaticValues.manchesterDamageCoefficient}%</style> and gaining barrier on hit, scaling with movespeed</style>." + Helpers.Passive(" Fa Jin buff hits and pulls enemies when you jump as well as doubling barrier gain") + ".");
            LanguageAPI.Add(prefix + "SECONDARY3_NAME", "St Louis Smash Airforce");
            LanguageAPI.Add(prefix + "SECONDARY3_DESCRIPTION", $"<style=cIsDamage>Agile.</style> St Louis Smash, kicking multiple blasts of air pressure in front of you, dealing <style=cIsDamage>{100f * StaticValues.stlouisDamageCoefficient}% damage and healing on hit, scaling with attackspeed</style>." + Helpers.Passive(" Fa Jin buff doubles the range and health gain") + ".");

            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY_NAME", "Detroit Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Charge a Detroit Smash, instantly dashing and dealing <style=cIsDamage>{100f * StaticValues.detroit100DamageCoefficient}% increasing infinitely</style>. " + Helpers.Damage("Costs 10% of max Health")+".");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY2_NAME", "Blackwhip 45%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY2_DESCRIPTION", $"<style=cIsDamage>Stunning. </style> Blackwhip enemies, pulling them right in front of you, stunning and dealing <style=cIsDamage>5x{100f * StaticValues.blackwhip45DamageCoefficient}%</style> and gaining barrier on hit, scaling with attackspeed.");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY3_NAME", "Blackwhip Combo");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY3_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Hit enemies in front of you and shoot blackwhip, dealing <style=cIsDamage>{100f * StaticValues.blackwhipshootDamageCoefficient}%</style>. Tapping pulls you forward while Holding pulls enemies towards you</style>." + Helpers.Passive(" Fa Jin buff makes blackwhip stuns, shoot multiple times and have a larger melee hitbox") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY4_NAME", "Blackwhip 100%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY4_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Blackwhip enemies, pulling them right in front of you, stunning and dealing <style=cIsDamage>3x{100f * StaticValues.blackwhip100DamageCoefficient}%</style> and gaining barrier on hit, scaling with attackspeed." + Helpers.Damage("Costs 10% of max Health") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY5_NAME", "Manchester Smash 45%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY5_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Jump in the air and slam down, dealing <style=cIsDamage>{100f * StaticValues.manchester45DamageCoefficient}%</style> and gaining barrier on hit, scaling with movespeed. Radius scales with movespeed.");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY6_NAME", "Manchester Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY6_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Jump in the air and slam down, dealing <style=cIsDamage>2x{100f * StaticValues.manchester100DamageCoefficient}%</style> and gaining barrier on hit, scaling with movespeed. Hits and pulls enemies when you jump as well. Radius scales with movespeed." + Helpers.Damage("Costs 10% of max Health") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY7_NAME", "St Louis Smash Airforce 100%");
            LanguageAPI.Add(prefix + "BOOSTEDSECONDARY7_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> St Louis Smash, kicking multiple blasts of air pressure in front of you, stunning and dealing <style=cIsDamage>{100f * StaticValues.stlouis100DamageCoefficient}% damage. On hit, create an additional blast and heal, scaling with attackspeed</style>." + Helpers.Damage("Costs 10% of max Health") + ".");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_NAME", "Float");
            LanguageAPI.Add(prefix + "UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Jump and float in the air, disabling gravity, changing your special to Delaware Smash 100%. Press the button to cancel Float</style>." + Helpers.Passive(" Fa Jin buff causes a blast of wind to damage enemies around you as you jump as well") + ".");
            LanguageAPI.Add(prefix + "UTILITY2_NAME", "Oklahoma Smash");
            LanguageAPI.Add(prefix + "UTILITY2_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Hold the button to spin around, knocking back and dealing <style=cIsDamage>{100f * StaticValues.oklahomaDamageCoefficient}% damage</style> around you multiple times. While active, triple armor at the cost of movespeed</style>." + Helpers.Passive(" Fa Jin buff doubles the number of hits, speed, and AOE") + ".");
            LanguageAPI.Add(prefix + "UTILITY3_NAME", "Detroit Smash");
            LanguageAPI.Add(prefix + "UTILITY3_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Charge a Detroit Smash, instantly dashing and dealing <style=cIsDamage>{100f * StaticValues.detroitDamageCoefficient}%</style>, range scales based on attackspeed and movespeed</style>." + Helpers.Passive(" Fa Jin buff doubles everything") + ".");

            LanguageAPI.Add(prefix + "BOOSTEDUTILITY_NAME", "Delaware Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Delaware Smash, dealing <style=cIsDamage>{100f * StaticValues.delaware100DamageCoefficient}% damage</style> in an AOE, sending yourself backwards</style>. " + Helpers.Damage("Costs 10% of max Health")+".");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY2_NAME", "St Louis Smash 45%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY2_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> St Louis Smash, stunning enemies in front, dealing <style=cIsDamage>{100f * StaticValues.stlouis45DamageCoefficient}% damage</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY3_NAME", "Smokescreen");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY3_DESCRIPTION", $"<style=cIsDamage>Slowing.</style> Release a smokescreen, going invisible for 4 seconds and dealing <style=cIsDamage>{100f * StaticValues.smokescreenDamageCoefficient}% damage</style> around you</style>." + Helpers.Passive(" Fa Jin buff makes nearby allies invisible as well") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY4_NAME", "Float 45%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY4_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Jump and float in the air, disabling gravity, changing your special to Delaware Smash 100%. Press the button to cancel Float</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY5_NAME", "Float 100%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY5_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Jump, dealing <style=cIsDamage>{100f * StaticValues.floatDamageCoefficient}% damage</style> and float in the air, disabling gravity, changing your special to Delaware Smash 100%. Press the button to cancel Float</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY6_NAME", "Oklahoma Smash 45%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY6_DESCRIPTION", $"<style=cIsDamage>Slowing. Agile.</style> Hold the button to spin around, knocking back and dealing <style=cIsDamage>{100f * StaticValues.oklahoma45DamageCoefficient}% damage around you multiple times. While active, triple armor at the cost of movespeed</style>.");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY7_NAME", "Oklahoma Smash 100%");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY7_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Hold the button to spin around, knocking back and dealing <style=cIsDamage>{100f * StaticValues.oklahoma100DamageCoefficient}% damage around you multiple times. While active, triple armor at the cost of movespeed</style>." + Helpers.Damage("Costs 10% of max Health") + ".");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY8_NAME", "Detroit Smash");
            LanguageAPI.Add(prefix + "BOOSTEDUTILITY8_DESCRIPTION", $"<style=cIsDamage>Stunning. Agile.</style> Charge a Detroit Smash, instantly dashing and dealing <style=cIsDamage>{100f * StaticValues.detroit45DamageCoefficient} to {300f * StaticValues.detroit45DamageCoefficient}% damage</style>, range scales based on attackspeed and movespeed</style>.");
            LanguageAPI.Add(prefix + "FLOATCANCEL_NAME", "Float Cancel"); 
            LanguageAPI.Add(prefix + "FLOATCANCEL_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Float to the ground, enabling gravity, reverting your special</style>.");
            //LanguageAPI.Add(prefix + "FLOATCANCEL45_NAME", "Float 45% Cancel");
            //LanguageAPI.Add(prefix + "FLOATCANCEL45_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Float to the ground, enabling gravity, reverting your special</style>.");
            //LanguageAPI.Add(prefix + "FLOATCANCEL100_NAME", "Float 100% Cancel");
            //LanguageAPI.Add(prefix + "FLOATCANCEL100_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Float to the ground, enabling gravity, reverting your special</style>.");
            LanguageAPI.Add(prefix + "FLOATSPECIAL_NAME", "Delaware Smash 100%");
            LanguageAPI.Add(prefix + "FLOATSPECIAL_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Delaware Smash, dealing <style=cIsDamage>{100f * StaticValues.delawareDamageCoefficient}% damage in an AOE, sending yourself backwards</style>. " + Helpers.Passive(" Fa Jin buff doubles the distance travelled") + "." + Helpers.Damage("Costs 10% of max Health") + ".");
            //LanguageAPI.Add(prefix + "FLOATSPECIAL45_NAME", "Delaware Smash 45%");
            //LanguageAPI.Add(prefix + "FLOATSPECIAL45_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Delaware Smash, dealing <style=cIsDamage>{100f * StaticValues.delaware45DamageCoefficient}% damage in an AOE, sending yourself backwards</style>. ");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_NAME", "One For All");
            LanguageAPI.Add(prefix + "SPECIAL_DESCRIPTION", $"Cycle between One For All 45% and 100%, upgrading your skills and stats. This skill activates 45%.");
            LanguageAPI.Add(prefix + "SPECIAL2_NAME", "OFA 45%");
            LanguageAPI.Add(prefix + "SPECIAL2_DESCRIPTION", $"Push your body to its limits, boosting Armor, Movespeed, Damage, Attackspeed, gain unique 45% moves, " + Helpers.Damage("disabling Health Regen") + ".");
            LanguageAPI.Add(prefix + "SPECIAL3_NAME", "OFA 100%");
            LanguageAPI.Add(prefix + "SPECIAL3_DESCRIPTION", $"Go beyond your limits, boosting Armor, Movespeed, Damage, Attackspeed, gain unique 100% moves, " + Helpers.Damage("gaining negative Regen and Self-Damage from every move") + ".");
            LanguageAPI.Add(prefix + "SPECIAL4_NAME", "OFA Quirks");
            LanguageAPI.Add(prefix + "SPECIAL4_DESCRIPTION", $"Unlock your additional quirks. This skill grants the Fa Jin buff. Moving increases the buff up to 200 stacks. Gain up to 2x damage at 50 stacks. Every move gives 10 stacks and will consume 50 stacks if able. However, if a move uses 50 stacks it has additional effects. <style=cIsUtility>In general all moves will stun and bypass armor, have double the movement, double the radius and range</style>.");
           
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_NAME", "OFA down");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_DESCRIPTION", $"Return yourself back to your limits.");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL2_NAME", "OFA 45%");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL2_DESCRIPTION", $"Push your body to its limits, boosting Armor, Movespeed, Damage, Attackspeed, gain unique 45% moves, " + Helpers.Damage("disabling Health Regen") + ". This skill goes to 100%.");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL3_NAME", "OFA 100%");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL3_DESCRIPTION", $"Go beyond your limits, boosting Armor, Movespeed, Damage, Attackspeed, gain unique 100% moves, " + Helpers.Damage("gaining negative Regen and Self-Damage from every move") + ". This skill returns yourself back to your limits.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_NAME", "One For All Mastered");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_DESCRIPTION", $"Cycle between Mastered One For All 45% and 100%, upgrading your skills and stats as well as granting lifesteal. This skill goes to 45%.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_NAME", $"Infinite 45%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_DESCRIPTION", $"Master OFA 45%, gaining the same effects as well as 5% lifesteal.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL3_NAME", $"Infinite 100%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL3_DESCRIPTION", $"Unlock the true power of One For All, gaining the same effects as well as 10% lifesteal.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL4_NAME", $"OFA Quirks Mastered");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL4_DESCRIPTION", $"Fa Jin Buff stacks are granted at double the rate.");
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
