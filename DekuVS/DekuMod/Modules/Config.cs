using BepInEx.Configuration;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions;
using UnityEngine;

namespace DekuMod.Modules
{
    public static class Config
    {
        //public static ConfigEntry<bool> retainLoadout;
        //public static ConfigEntry<bool> allowAllSkills;
        public static ConfigEntry<bool> allowVoice;
        //public static ConfigEntry<float> holdButtonAFO;

        //public static ConfigEntry<float> glideSpeed;
        //public static ConfigEntry<float> glideAcceleration;

        public static void ReadConfig()
        {
            //retainLoadout = DekuPlugin.instance.Config.Bind("General", "Retain loadout across stages", true, "Should you retain your stolen quirks across stages and respawns.");
            //holdButtonAFO = DekuPlugin.instance.Config.Bind("General", "Steal, Give and Remove quirk timer", 0f, "Set how long you want to hold the button.");
            allowVoice = DekuPlugin.instance.Config.Bind("General", "Allow voice", false, "Allow voice lines of Deku. Off by default as it does not fit the style.");

            //allowAllSkills = DekuPlugin.instance.Config.Bind("General", "Allow all skils to be picked", false, "Should you be allowed to pick all skills in the loadout menu. AFO functionality is not disabled. Will require a Restart.");*

        }

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return DekuPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return DekuPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }

        public static void SetupRiskOfOptions()
        {
            //Risk of Options intialization
            //ModSettingsManager.AddOption(new KeyBindOption(
            //    AFOHotkey));
            //ModSettingsManager.AddOption(new KeyBindOption(
            //    RemoveHotkey));
            //ModSettingsManager.AddOption(new KeyBindOption(
            //    AFOGiveHotkey));
            //ModSettingsManager.AddOption(new CheckBoxOption(
            //    retainLoadout));
            //ModSettingsManager.AddOption(new CheckBoxOption(
            //    allowAllSkills));
            ModSettingsManager.AddOption(new CheckBoxOption(
                allowVoice));
            //ModSettingsManager.AddOption(new StepSliderOption(
            //    holdButtonAFO, new StepSliderConfig() { min = 0, max = 10, increment = 1f }));
            ModSettingsManager.SetModDescription("Deku Mod");
            Sprite icon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("Deku");
            ModSettingsManager.SetModIcon(icon);

            //ModSettingsManager.AddOption(
            //    new StepSliderOption(
            //        glideSpeed,
            //        new StepSliderConfig
            //        {
            //            min = 0,
            //            max = 100f,
            //            increment = 0.05f
            //        }
            //    ));

            //ModSettingsManager.AddOption(
            //    new StepSliderOption(
            //        glideAcceleration,
            //        new StepSliderConfig
            //        {
            //            min = 0f,
            //            max = 100f,
            //            increment = 0.05f
            //        }
            //    ));

        }
    }
}