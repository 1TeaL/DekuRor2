using BepInEx;
using DekuMod.Modules.Survivors;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using EntityStates.Mage;
using EntityStates;
using BepInEx.Bootstrap;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace DekuMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]

    public class DekuPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said

        public static bool scepterInstalled = false;

        public const string MODUID = "com.TeaL.DekuMod";
        public const string MODNAME = "DekuMod";
        public const string MODVERSION = "1.1.0";
        public const float passiveRegenBonus = 0.025f;

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "TEAL";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        public static DekuPlugin instance;
        

        private void Awake()
        {
            instance = this;

            DekuPlugin.instance = this;
            bool flag = Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
            if (flag)
            {
                DekuPlugin.scepterInstalled = true;
            }

            // load assets and read config
            Modules.Assets.Initialize();
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new Deku().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;

            Hook();
        }

        private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            // have to set item displays later now because they require direct object references..
            Modules.Survivors.Deku.instance.SetItemDisplays();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            //On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
            //On.RoR2.HealthComponent.TakeDamage += BlackwhipPull;            
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            //regen 
            orig.Invoke(self);
            bool flag2 = self.HasBuff(Modules.Buffs.ofaBuff);
            if (flag2)
            {
                self.armor *= 5f;
                self.moveSpeed *= 1.5f;
                self.attackSpeed *= 1.5f;
                self.regen *= -8f;
                self.damage *= 2f;
            }
            

            if (self.baseNameToken == DekuPlugin.developerPrefix + "DEKU")                
            {


                if (!flag2)
                {

                    HealthComponent hp = self.healthComponent;
                    float regenValue = hp.fullCombinedHealth * DekuPlugin.passiveRegenBonus;
                    float regen = Mathf.SmoothStep(regenValue, 0, hp.combinedHealth / hp.fullCombinedHealth);
                    self.regen += regen;
                    //Chat.AddMessage("hpregen activated");
                }
                

            }
        }
        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            orig(self);
            if (self.baseNameToken == DekuPlugin.developerPrefix + "DEKU")
            {
                AkSoundEngine.PostEvent(779278001, this.gameObject);
            }

        }

        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("DekuDisplay"))
            {
                AkSoundEngine.PostEvent(2656882895, this.gameObject);
            }

        }

        //private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        //{


        //}

    }
}