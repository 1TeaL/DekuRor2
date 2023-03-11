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

            string desc = "Deku is a multi-option, high risk survivor that can change his skills and boost his stats but with detrimental health regen and health costs.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > He scales with attackspeed and movespeed on multiple of his skills" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > The Plus Ultra Meter in the middle increases as you walk and use base skills, it is used for specific skills and for Going Beyond." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Base form focuses on control, as well as allowing him to build up Plus Ultra Meter." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > OFA 100% focuses on pure power at the cost of self-damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > OFA 45% is a mid ground between 100% and base, with no health regeneration." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Deku's can switch his skills between 3 loadouts- Power, Shoot Style, Quirks." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Deku's 4th extra skill depends on his loadout, and costs 50 Plus Ultra Meter." + Environment.NewLine + Environment.NewLine;
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
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", 
            Helpers.Passive("[Plus Ultra Meter] [Marks] [Float] [Danger Sense]") +"." + Environment.NewLine
            + "<style=cIsUtility>He has a double jump. He can sprint in any direction. </style>");
             #endregion

            #region Primary
            LanguageAPI.Add(prefix + "FISTPRIMARY_NAME", "Airforce");
            LanguageAPI.Add(prefix + "FISTPRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot a bullet, dealing <style=cIsDamage>2x{100f * StaticValues.airforceDamageCoefficient}% damage. On Crit, ricochet to {StaticValues.airforceMaxRicochet} additional enemies</style>" + " .");
            LanguageAPI.Add(prefix + "LEGPRIMARY_NAME", "Shoot Style");
            LanguageAPI.Add(prefix + "LEGPRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile. Shocking.</style> Kick forward, dealing <style=cIsDamage>{100f * StaticValues.shootkickDamageCoefficient}% damage</style>, with every 3 hits reducing cooldowns by 1 second and shocking. Dash towards distant targets" + ".");
            LanguageAPI.Add(prefix + "QUIRKPRIMARY_NAME", "Blackwhip");
            LanguageAPI.Add(prefix + "QUIRKPRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile. </style> Activate blackwhip for {StaticValues.blackwhipDebuffDuration} seconds, restricting their movements for {StaticValues.blackwhipDebuffDuration} seconds and dealing <style=cIsDamage>{100f * StaticValues.blackwhipDamageCoefficient}% damage</style> per tick, healing based on your damage dealt." 
                + Helpers.Passive($" Costs {StaticValues.skillPlusUltraSpend} of plus ultra") + ".");
            
            LanguageAPI.Add(prefix + "FIST45PRIMARY_NAME", "Airforce 45%");
            LanguageAPI.Add(prefix + "FIST45PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot 4 bullets with all your fingers, dealing <style=cIsDamage>{100f * StaticValues.airforce45DamageCoefficient}% damage</style> each" + ".");
            LanguageAPI.Add(prefix + "LEG45PRIMARY_NAME", "Shoot Style 45%");
            LanguageAPI.Add(prefix + "LEG45PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Dash and kick, dealing  <style=cIsDamage>{100f * StaticValues.shootkick45DamageCoefficient}% damage, scaling with movespeed</style>, resetting the cooldown on hit and resetting all cooldowns on kill" + ".");
            LanguageAPI.Add(prefix + "QUIRK45PRIMARY_NAME", "Blackwhip 45%");
            LanguageAPI.Add(prefix + "QUIRK45PRIMARY_DESCRIPTION", $"<style=cIsDamage>Stunning. </style> Blackwhip towards the target, dashing towards them, stunning and dealing <style=cIsDamage>{100f * StaticValues.blackwhip45DamageCoefficient}% damage</style>." +
                $" Enemies hit have their movement restricted for {StaticValues.blackwhipDebuffDuration} seconds. " 
                + Helpers.Passive($" Costs {StaticValues.skill45PlusUltraSpend} of plus ultra") + ".");
            
            LanguageAPI.Add(prefix + "FIST100PRIMARY_NAME", "Airforce 100%");
            LanguageAPI.Add(prefix + "FIST100PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Shoot beams with your fists, stunning and dealing <style=cIsDamage>{100f * StaticValues.airforce100DamageCoefficient}% damage</style>, initially having 20% attackspeed, ramping up to 200%." 
                + Helpers.Damage($" Costs {100f * StaticValues.airforce100HealthCostFraction}% of max health") + ".");
            LanguageAPI.Add(prefix + "LEG100PRIMARY_NAME", "Shoot Style 100%");
            LanguageAPI.Add(prefix + "LEG100PRIMARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Instantly dash through enemies, after a delay, deal <style=cIsDamage>5x{100f * StaticValues.shootkick100DamageCoefficient}% damage, scaling with movespeed</style>." +
                $" Number of hits scales based on movespeed." 
                + Helpers.Damage($" Costs {100f * StaticValues.shootkick100HealthCostFraction}% of max health") + ".");
            LanguageAPI.Add(prefix + "QUIRK100PRIMARY_NAME", "Blackwhip 100%");
            LanguageAPI.Add(prefix + "QUIRK100PRIMARY_DESCRIPTION", $"<style=cIsDamage>Stunning.</style> Blackwhip enemies towards you for, restricting their movements for {StaticValues.blackwhipDebuffDuration} seconds, stunning and dealing <style=cIsDamage>{100f * StaticValues.blackwhip100DamageCoefficient}% damage</style>. " +
                $"The Target will be tethered to you. Separating more than {StaticValues.blackwhip100AttachRange}m away will result in Blackwhip 100% being used again in the Target's direction." 
                + Helpers.Passive($" Constantly drains plus ultra until reactivated.")  
                + Helpers.Damage($" Costs {100f * StaticValues.blackwhip100HealthCostFraction}% of max health.") 
                + Helpers.Passive($" Costs {StaticValues.skill100PlusUltraSpend} of plus ultra") + ".");
            
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "FISTSECONDARY_NAME", "Detroit Smash");
            LanguageAPI.Add(prefix + "FISTSECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile. Shocking.</style> Teleport and punch the target and enemies around, dealing <style=cIsDamage>{100f * StaticValues.detroitDamageCoefficient}% damage</style>. Uppercut them if they are grounded and smash them down if they are in the air" + ".");
            LanguageAPI.Add(prefix + "LEGSECONDARY_NAME", "Manchester Smash");
            LanguageAPI.Add(prefix + "LEGSECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile. Stunning.</style> Manchester Smash, damaging nearby enemies for <style=cIsDamage>{100f * StaticValues.manchesterDamageCoefficient}% damage, scaling with movespeed</style>, each enemy hit gives an extra second of a {StaticValues.manchesterArmor} armor buff" + ".");
            LanguageAPI.Add(prefix + "QUIRKSECONDARY_NAME", "Gear Shift");
            LanguageAPI.Add(prefix + "QUIRKSECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Activate gear shift for {StaticValues.gearshiftBuffTimer} seconds. <style=cIsUtility> Movespeed is increased by {StaticValues.gearshiftMovespeedBoost}x</style>. <style=cIsDamage>Attacks apply additional knockback based on your aim</style>."
                + Helpers.Passive($" Constantly drains plus ultra until reactivated.")
                + Helpers.Passive($" Costs {StaticValues.skillPlusUltraSpend} of plus ultra") + ".");
            
            LanguageAPI.Add(prefix + "FIST45SECONDARY_NAME", "Detroit Smash 45%");
            LanguageAPI.Add(prefix + "FIST45SECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Hold to charge a Detroit Smash. When the skill key is released or after some time, teleport and smash nearby enemies, dealing <style=cIsDamage>{100f * StaticValues.detroit45DamageCoefficient}% - {3 * 100f * StaticValues.detroit45DamageCoefficient}% damage</style>" + ".");
            LanguageAPI.Add(prefix + "LEG45SECONDARY_NAME", "Manchester Smash 45%");
            LanguageAPI.Add(prefix + "LEG45SECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile. Stunning.</style> Manchester Smash, jumping and slamming down, stunning and damaging nearby enemies for <style=cIsDamage>2x{100f * StaticValues.manchester45DamageCoefficient}% damage, scaling with movespeed</style>. Gain {StaticValues.manchesterArmor} armor for {StaticValues.manchester45BuffDuration} seconds" + ".");
            LanguageAPI.Add(prefix + "QUIRK45SECONDARY_NAME", "Gear Shift 45%");
            LanguageAPI.Add(prefix + "QUIRK45SECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Activate gear shift transmission for {StaticValues.gearshift45BuffTimer} seconds. <style=cIsDamage>Attacks slow enemies and pierce through them for {100f * StaticValues.gearshift45DamageCoefficient}% of the damage</style>."
                + Helpers.Passive($" Constantly drains plus ultra until reactivated.")
                + Helpers.Passive($" Costs {StaticValues.skill45PlusUltraSpend} of plus ultra") + ".");

            LanguageAPI.Add(prefix + "FIST100SECONDARY_NAME", "Detroit Smash 100%");
            LanguageAPI.Add(prefix + "FIST100SECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Hold to jump forward while charging a Detroit Smash. When the skill key is released or you collide with an enemy, smash nearby enemies, dealing <style=cIsDamage>{100f * StaticValues.detroit100DamageCoefficient}% damage, multiplied by flight time and attackspeed</style>." 
                + Helpers.Damage($" Costs {100f * StaticValues.detroit100HealthCostFraction}% of max health") + ".");
            LanguageAPI.Add(prefix + "LEG100SECONDARY_NAME", "Manchester Smash 100%");
            LanguageAPI.Add(prefix + "LEG100SECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Manchester Smash, rapidly drop down from the sky. Have {StaticValues.manchesterArmor} armor while falling. Upon landing on the ground, deal <style=cIsDamage>3x{100f * StaticValues.manchester100DamageCoefficient}% damage</style>, multiplied by falling time."
                + Helpers.Damage($" Costs {100f * StaticValues.manchester100HealthCostFraction}% of max health") + ".");
            LanguageAPI.Add(prefix + "QUIRK100SECONDARY_NAME", "Gear Shift 100%");
            LanguageAPI.Add(prefix + "QUIRK100SECONDARY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Overdrive gear shift for the next {StaticValues.gearshift100BuffAttacks} attacks. <style=cIsDamage>Attacks bend the laws of physics, dealing multiple times their damage based on your movespeed</style>." +
                $" <style=cIsUtility>The thresholds are in increments of {StaticValues.gearshift100Threshold} movespeed</style>."
                + Helpers.Damage($" Costs {100f * StaticValues.gearshift100HealthCostFraction}% of max health")
                + Helpers.Passive($" Costs {StaticValues.skill100PlusUltraSpend} of plus ultra") + ".");

            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "FISTUTILITY_NAME", "Delaware Smash");
            LanguageAPI.Add(prefix + "FISTUTILITY_DESCRIPTION", $"<style=cIsDamage>Agile. Shocking.</style> Hold the button to aim, then release to shoot a Delaware smash, shocking and dealing <style=cIsDamage>{100f * StaticValues.delawareDamageCoefficient}% damage</style>" + ".");
            LanguageAPI.Add(prefix + "LEGUTILITY_NAME", "St Louis Smash");
            LanguageAPI.Add(prefix + "LEGUTILITY_DESCRIPTION", $"<style=cIsDamage>Agile. Shocking.</style> Teleport and kick the target and enemies around, dealing <style=cIsDamage>{100f * StaticValues.stlouisDamageCoefficient}% damage, scaling with movespeed</style>, sending them forward" + ".");
            LanguageAPI.Add(prefix + "QUIRKUTILITY_NAME", "Smokescreen");
            LanguageAPI.Add(prefix + "QUIRKUTILITY_DESCRIPTION", $"<style=cIsDamage>Agile.</style>. Release a smokescreen, making teammates and yourself go invisible for {StaticValues.smokescreenDuration} seconds" + "." 
                + Helpers.Passive($" Costs {StaticValues.skillPlusUltraSpend} of plus ultra") + ".");
            
            LanguageAPI.Add(prefix + "FIST45UTILITY_NAME", "Delaware Smash 45%");
            LanguageAPI.Add(prefix + "FIST45UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile. Stunning.</style> Flick your fingers, releasing a blast of wind in front, stunning and dealing <style=cIsDamage>{100f * StaticValues.delaware45DamageCoefficient}% damage</style>" + ".");
            LanguageAPI.Add(prefix + "LEG45UTILITY_NAME", "St Louis Smash 45%");
            LanguageAPI.Add(prefix + "LEG45UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile. Stunning.</style> St Louis Smash, kicking multiple blasts of air pressure in front of you, dealing <style=cIsDamage>{100f * StaticValues.stlouis45DamageCoefficient}% damage, scaling with movespeed</style>" + ".");
            LanguageAPI.Add(prefix + "QUIRK45UTILITY_NAME", "Smokescreen 45%");
            LanguageAPI.Add(prefix + "QUIRK45UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile.</style>. Release a smokescreen, make yourself go invisible for {StaticValues.smokescreen45Duration} seconds" + "." 
                + Helpers.Passive($" Costs {StaticValues.skill45PlusUltraSpend} of plus ultra") + ".");
            
            LanguageAPI.Add(prefix + "FIST100UTILITY_NAME", "Delaware Smash 100%");
            LanguageAPI.Add(prefix + "FIST100UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Flick your fingers, releasing an intense blast of wind in front, sending you flying backwards and dealing <style=cIsDamage>{100f * StaticValues.delaware100DamageCoefficient}% damage</style>." 
                + Helpers.Damage($" Costs {100f * StaticValues.delaware100HealthCostFraction}% of max health") + ".");
            LanguageAPI.Add(prefix + "LEG100UTILITY_NAME", "St Louis Smash 100%");
            LanguageAPI.Add(prefix + "LEG100UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Hold to prep a physics breaking St Louis Smash. Release the skill key to deal a blast of wind pressure and instantaneously teleport up. The blast deals <style=cIsDamage>{100f * StaticValues.stlouis100DamageCoefficient}% damage, scaling with movespeed</style>."
                + Helpers.Damage($" Costs {100f * StaticValues.stlouis100HealthCostFraction}% of max health") + ".");             
            LanguageAPI.Add(prefix + "QUIRK100UTILITY_NAME", "Smokescreen 100%");
            LanguageAPI.Add(prefix + "QUIRK100UTILITY_DESCRIPTION", $"<style=cIsDamage>Agile. Crippling.</style> Release a blinding smokescreen, making yourself invisible for {StaticValues.smokescreen100Duration} seconds. The smokescreen also deals <style=cIsDamage>{100f * StaticValues.smokescreenDamageCoefficient}% damage</style> to enemies as well." 
                + Helpers.Damage($" Costs {100f * StaticValues.smokescreen100HealthCostFraction}% of max health.")
                + Helpers.Passive($" Costs {StaticValues.skill100PlusUltraSpend} of plus ultra") + ".");
          
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_NAME", "One For All");
            LanguageAPI.Add(prefix + "SPECIAL_DESCRIPTION", $"Cycle between One For All 45% and 100%, upgrading your skills and stats. This skill activates 45%.");
            LanguageAPI.Add(prefix + "SPECIAL2_NAME", "OFA 45%");
            LanguageAPI.Add(prefix + "SPECIAL2_DESCRIPTION", $"Push your body to its limits, boosting Armor, Movespeed, gain unique 45% moves, " 
                + Helpers.Damage("disabling Health Regen") + ".");
            LanguageAPI.Add(prefix + "SPECIAL3_NAME", "OFA 100%");
            LanguageAPI.Add(prefix + "SPECIAL3_DESCRIPTION", $"Go beyond your limits, boosting Armor, Movespeed, gain unique 100% moves, " + Helpers.Damage($"taking {100f * StaticValues.ofaHealthCost}% of CURRENT health as damage every second and Self-Damage from every move") + ".");
            LanguageAPI.Add(prefix + "SPECIAL4_NAME", "OFA Quirks");
            LanguageAPI.Add(prefix + "SPECIAL4_DESCRIPTION", $"Unlock your additional quirks. This skill grants the Fa Jin buff. Moving increases the buff up to 200 stacks. Gain up to 2x damage at 50 stacks. Every move gives 10 stacks and will consume 50 stacks if able. However, if a move uses 50 stacks it has additional effects. <style=cIsUtility>In general all moves will stun and bypass armor, have double the movement, double the radius and range</style>.");
          

            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_NAME", "OFA down");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL_DESCRIPTION", $"Return yourself back to your limits.");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL2_NAME", "OFA 45%");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL2_DESCRIPTION", $"Push your body to its limits, boosting Armor, Movespeed, gain unique 45% moves, " 
                + Helpers.Damage("disabling Health Regen") + ". This skill goes to 100%.");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL3_NAME", "OFA 100%");
            LanguageAPI.Add(prefix + "BOOSTEDSPECIAL3_DESCRIPTION", $"Go beyond your limits, boosting Armor, Movespeed,  gain unique 100% moves, " 
                + Helpers.Damage($"taking {100f * StaticValues.ofaHealthCost}% of CURRENT health as damage every second and Self-Damage from every move") + ". This skill returns yourself back to your limits.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_NAME", "One For All Mastered");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL_DESCRIPTION", $"Cycle between Mastered One For All 45% and 100%, upgrading your skills and stats as well as granting lifesteal. This skill goes to 45%.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_NAME", $"Infinite 45%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL2_DESCRIPTION", $"Master OFA 45%, gaining the same effects as well as 5% lifesteal. " +
                $"This skill goes to Infinite 100%.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL3_NAME", $"Infinite 100%");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL3_DESCRIPTION", $"Unlock the true power of One For All, gaining the same effects as well as 10% lifesteal. " +
                $"This skill returns yourself back to your limits.");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL4_NAME", $"OFA Quirks Mastered");
            LanguageAPI.Add(prefix + "SCEPTERSPECIAL4_DESCRIPTION", $"Fa Jin Buff stacks are granted at double the rate.");
            #endregion

            #region Extra
            //modes
            LanguageAPI.Add(prefix + "FISTEXTRA_NAME", "Power mode");
            LanguageAPI.Add(prefix + "FISTEXTRA_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Change your skills to Power based, dealing more damage. Changes your 4th extra slot ability as well." );
            LanguageAPI.Add(prefix + "LEGEXTRA_NAME", "Shoot Style mode");
            LanguageAPI.Add(prefix + "LEGEXTRA_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Change your skills to Shoot Style based, focused on mobility with movespeed increasing damage for most skills. Changes your 4th extra slot ability as well." );
            LanguageAPI.Add(prefix + "QUIRKEXTRA_NAME", "Quirk mode");
            LanguageAPI.Add(prefix + "QUIRKEXTRA_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Change your skills to Quirk based, focused on buffs and utility. Changes your 4th extra slot ability as well." );
            LanguageAPI.Add(prefix + "CYCLEEXTRA_NAME", "Cycle mode");
            LanguageAPI.Add(prefix + "CYCLEEXTRA_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Cycle your skills from fist to leg to quirk based.");
            LanguageAPI.Add(prefix + "TYPEEXTRA_NAME", "Type mode");
            LanguageAPI.Add(prefix + "TYPEEXTRA_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Cycle your skills to another skill of the same type.");
            //supers
            LanguageAPI.Add(prefix + "FISTSPECIAL_NAME", "1,000,000% Detroit Delaware Smash");
            LanguageAPI.Add(prefix + "FISTSPECIAL_DESCRIPTION", $"<style=cIsDamage>Agile. Freezing.</style> Delaware Smash enemies, stunning and grouping them in front of you, dealing <style=cIsDamage>{100f * StaticValues.detroitdelawareSmashDamageCoefficient}% damage</style>, then Detroit Smash all of them, freezing and dealing <style=cIsDamage>{100f * StaticValues.detroitdelawareSmashDamageCoefficient}% damage</style>." 
                + Helpers.Passive($" Costs {StaticValues.modePlusUltraSpend} of plus ultra") + ".");
            LanguageAPI.Add(prefix + "LEGSPECIAL_NAME", "Final Smash");
            LanguageAPI.Add(prefix + "LEGSPECIAL_DESCRIPTION", $"<style=cIsDamage>Agile. Igniting.</style> Dash forward, stunning and grouping enemies to your front, dealing <style=cIsDamage>{100f * StaticValues.finalsmashDamageCoefficient}% damage multiple times</style>, then Final Smash all of them,<style=cIsDamage> igniting and dealing {100f * StaticValues.finalsmashSmashDamageCoefficient}% damage</style>." 
                + Helpers.Passive($" Costs {StaticValues.modePlusUltraSpend} of plus ultra") + ".");
            LanguageAPI.Add(prefix + "QUIRKSPECIAL_NAME", "Fa Jin");
            LanguageAPI.Add(prefix + "QUIRKSPECIAL_DESCRIPTION", $"<style=cIsDamage>Agile.</style> Use your stored plus ultra with Fa Jin, granting {StaticValues.fajinDamageMultiplier}x damage and disabling all health costs for {StaticValues.fajinDuration} seconds." 
                + Helpers.Passive($" Costs {StaticValues.modePlusUltraSpend} of plus ultra") + ".");
            //gobeyond
            LanguageAPI.Add(prefix + "GOBEYOND_NAME", "GO BEYOND!");
            LanguageAPI.Add(prefix + "GOBEYOND_DESCRIPTION", Helpers.Passive($" Go Beyond your limits! Plus Ultra! For {StaticValues.goBeyondBuffDuration} seconds, disable health costs, and heal for {100f * StaticValues.gobeyondHealCoefficient}% max health per second") + ".");
            #endregion

            #region Keywords
            LanguageAPI.Add(prefix + "KEYWORD_PASSIVE", $"<style=cKeywordName>Plus Ultra Meter</style>" 
                + "Deku has a" + Helpers.Passive(" meter that builds up through movement and using base form skills. Changing modes, using quirk skills and using supers cost plus ultra. At Max Plus Ultra, once a stage, prevent death and Go Beyond, negating health costs as well as heal over time for 60 seconds") + "." 
                + Environment.NewLine
                + Environment.NewLine
            + $"<style=cKeywordName>Marks</style>" 
                + "Shocking attacks apply a heal mark that on hit," + Helpers.Passive($" heals {100f * StaticValues.healMarkCoefficient} of the damage dealt") + "."  
                + "Stunning attacks apply a barrier mark that on hit," + Helpers.Passive($" gain barrier based on {100f * StaticValues.barrierMarkCoefficient} of your max health") + "." 
                + "Freezing and Igniting attacks apply both. Both are removed after 3 hits."
                + Environment.NewLine
                + Environment.NewLine
            + $"<style=cKeywordName>Float</style>" 
                + "<style=cIsUtility>Holding jump in the air after 0.5 seconds let's him Float, flying up or slowing his descent while using a skill</style>." 
                + Helpers.Passive(" Drains plus ultra")
                + Environment.NewLine
                + Environment.NewLine
            + $"<style=cKeywordName>Danger Sense</style>"
                + $"<style=cIsUtility>Take reduced damage based on your armor every 10 seconds</style>, <style=cIsDamage>freezing the attacker for {100f * StaticValues.dangersenseDamageCoefficient}% damage</style>." 
                + Helpers.Passive(" If the damage is fully negated, heal for the damage dealt.")
                + $" If not using a skill, <style=cIsUtility>dodge out of the way</style> and <style=cIsDamage>shock enemies around you for {100f * StaticValues.dangersenseDamageCoefficient}% more damage</style> as well." 
                + Helpers.Passive($" Costs {StaticValues.dangersensePlusUltraSpend} plus ultra") + ".");

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
