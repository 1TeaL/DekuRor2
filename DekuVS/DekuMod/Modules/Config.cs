using BepInEx.Configuration;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions;
using UnityEngine;
using System;

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
        public static ConfigEntry<float> dekuVoice;
        public static ConfigEntry<float> dekuSFX;
        public static ConfigEntry<float> dekuMusic;


        public static void ReadConfig()
        {
            //retainLoadout = DekuPlugin.instance.Config.Bind("General", "Retain loadout across stages", true, "Should you retain your stolen quirks across stages and respawns.");
            //holdButtonAFO = DekuPlugin.instance.Config.Bind("General", "Steal, Give and Remove quirk timer", 0f, "Set how long you want to hold the button.");
            allowVoice = DekuPlugin.instance.Config.Bind("General", "Allow voice", false, "Allow voice lines of Deku. Off by default.");

            //allowAllSkills = DekuPlugin.instance.Config.Bind("General", "Allow all skils to be picked", false, "Should you be allowed to pick all skills in the loadout menu. AFO functionality is not disabled. Will require a Restart.");*

            dekuMusic = DekuPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("Voice/Volume", "Master Music Volume"),
                50f,
                new ConfigDescription("Determines the volume for music for Deku.")
            );
            dekuSFX = DekuPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("Voice/Volume", "Master SFX Volume"),
                50f,
                new ConfigDescription("Determines the volume for music for Deku.")
            );
            dekuVoice = DekuPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("Voice/Volume", "Master Voice Volume"),
                50f,
                new ConfigDescription("Determines the volume for music for Deku.")
            );

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
            Sprite icon = Modules.DekuAssets.mainAssetBundle.LoadAsset<Sprite>("texDekuIcon");
            ModSettingsManager.SetModIcon(icon);

            ModSettingsManager.AddOption(
                new StepSliderOption(
                    dekuMusic,
                    new StepSliderConfig
                    {
                        min = 0,
                        max = 100f,
                        increment = 1f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    dekuVoice,
                    new StepSliderConfig
                    {
                        min = 0,
                        max = 100f,
                        increment = 1f
                    }
                ));
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    dekuSFX,
                    new StepSliderConfig
                    {
                        min = 0,
                        max = 100f,
                        increment = 1f
                    }
                ));
        }

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

        


        public static void OnChangeHooks()
        {
            dekuMusic.SettingChanged += dekuMusic_Changed;
            dekuVoice.SettingChanged += dekuVoice_Changed;
            dekuSFX.SettingChanged += dekuSFX_Changed;
        }

        private static void dekuMusic_Changed(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("Volume_DekuMusic", dekuMusic.Value);
            }
        }
        private static void dekuVoice_Changed(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("Volume_DekuVoice", dekuMusic.Value);
            }
        }
        private static void dekuSFX_Changed(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("Volume_DekuSFX", dekuMusic.Value);
            }
        }


    }
}