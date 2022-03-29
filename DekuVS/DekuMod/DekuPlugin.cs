using BepInEx;
using BepInEx.Bootstrap;
using DekuMod.Modules;
using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates;
using R2API.Networking;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

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
        "NetworkingAPI",
        "SkinAPI",
        "LoadoutAPI",
        "DamageAPI"
    })]

    public class DekuPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said

        public static bool scepterInstalled = false;

        public const string MODUID = "com.TeaL.DekuMod";
        public const string MODNAME = "DekuMod";
        public const string MODVERSION = "3.1.2";
        public const float passiveRegenBonus = 0.035f;

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "TEAL";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        public static DekuPlugin instance;
        public static CharacterBody DekuCharacterBody;

        private void Awake()
        {
            instance = this;
            DekuCharacterBody = null;
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

            //networking
            NetworkingAPI.RegisterMessageType<ForceCounterState>();

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
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_OnDamageDealt;
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            //On.RoR2.HealthComponent.Awake += HealthComponent_Awake;
        }

        //private void HealthComponent_Awake(On.RoR2.HealthComponent.orig_Awake orig, HealthComponent healthComponent)
        //{
        //    //if (healthComponent)
        //    //{
        //    //    if (!healthComponent.gameObject.GetComponent<DangerSenseComponent>())
        //    //    {
        //    //        healthComponent.gameObject.AddComponent<DangerSenseComponent>();
        //    //    }
                    
        //    //}


        //    //orig.Invoke(healthComponent);
        //}

        //private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        //{
        //    if (self.body.HasBuff(Modules.Buffs.counterBuff.buffIndex))
        //    {
        //        damageInfo.damage = 0f;
        //        self.body.RemoveBuff(Modules.Buffs.counterBuff.buffIndex);
        //        Debug.Log(self.body.HasBuff(Modules.Buffs.counterBuff.buffIndex));

        //        var dekucon = self.body.gameObject.GetComponent<DekuController>();
        //        dekucon.countershouldflip = true;

        //        var damageInfo2 = new DamageInfo();

        //        damageInfo2.damage = self.body.damage * Modules.StaticValues.counterDamageCoefficient;
        //        damageInfo2.position = damageInfo.attacker.transform.position;
        //        damageInfo2.force = Vector3.zero;
        //        damageInfo2.damageColorIndex = DamageColorIndex.Default;
        //        damageInfo2.crit = Util.CheckRoll(self.body.crit, self.body.master);
        //        damageInfo2.attacker = self.gameObject;
        //        damageInfo2.inflictor = null;
        //        damageInfo2.damageType = DamageType.BypassArmor | DamageType.Stun1s;
        //        damageInfo2.procCoefficient = 2f;
        //        damageInfo2.procChainMask = default(ProcChainMask);

        //        if (damageInfo.attacker.gameObject.GetComponent<CharacterBody>().baseNameToken
        //            != DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && damageInfo.attacker != null)
        //        {
        //            damageInfo.attacker.GetComponent<CharacterBody>().healthComponent.TakeDamage(damageInfo2);
        //        }

        //        Vector3 enemyPos = damageInfo.attacker.transform.position;
        //        EffectManager.SpawnEffect(Modules.Projectiles.airforceTracer, new EffectData
        //        {
        //            origin = self.body.transform.position,
        //            scale = 1f,
        //            rotation = Quaternion.LookRotation(enemyPos - self.body.transform.position)

        //        }, true);


        //        DangerSenseCounter dangersenseCounter = new DangerSenseCounter();
        //        dangersenseCounter.enemyPosition = enemyPos;
        //        self.body.gameObject.GetComponent<EntityStateMachine>().SetState(dangersenseCounter);



        //        if (self.body.characterMotor && self.body.characterDirection)
        //        {
        //            self.body.characterMotor.rootMotion += (self.body.transform.position - damageInfo.attacker.transform.position).normalized * self.body.moveSpeed;
        //        }

        //        EntityStateMachine[] stateMachines = self.body.gameObject.GetComponents<EntityStateMachine>();
        //        foreach (EntityStateMachine stateMachine in stateMachines)
        //        {
        //            if (stateMachine.customName == "Body")
        //            {

        //                self.body.gameObject.GetComponent<EntityStateMachine>().SetNextState(new DangerSenseCounter
        //                {
        //                    enemyPosition = enemyPos
        //                });


        //            }

        //        }

        //    }
        //    orig.Invoke(self, damageInfo);
        //}

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            //regen 
            orig.Invoke(self);

            bool floatbuff = self.HasBuff(Buffs.floatBuff);
            if (floatbuff)
            {
                self.moveSpeed *= 1.5f;
                self.acceleration *= 2f;

            }


            bool fajin = self.HasBuff(Modules.Buffs.fajinBuff);
            if (fajin)
            {
                self.damage *= Mathf.Lerp(1f, Modules.StaticValues.fajinMaxMultiplier, (float)self.GetComponent<DekuController>().GetBuffCount() / (float)(Modules.StaticValues.fajinMaxStack/2));

            }

            bool ofa = self.HasBuff(Modules.Buffs.ofaBuff);

            if (ofa && (self.healthComponent.health - self.healthComponent.barrier) > 1 )
            {

                self.armor *= 5f;
                self.moveSpeed *= 1.5f;
                self.attackSpeed *= 1.5f;
                self.regen = (1 + (self.levelRegen * (self.level-1))) * -4f;
                self.damage *= 2f;
                
            }

            if (ofa && (self.healthComponent.health - self.healthComponent.barrier) <2)
            {

                self.armor *= 5f;
                self.moveSpeed *= 1.5f;
                self.attackSpeed *= 1.5f;
                self.regen = (1 + (self.levelRegen * (self.level - 1))) * 0f;
                self.damage *= 2f;

            }

            bool supaofa = self.HasBuff(Modules.Buffs.supaofaBuff);
            if (supaofa && (self.healthComponent.health - self.healthComponent.barrier) > 1)
            {
                self.armor *= 5f;
                self.moveSpeed *= 1.5f;
                self.attackSpeed *= 1.5f;
                self.regen = (1 + (self.levelRegen * (self.level-1))) * -4f;
                self.damage *= 2f;
            }

            if (supaofa && (self.healthComponent.health - self.healthComponent.barrier) < 1)
            {
                self.armor *= 5f;
                self.moveSpeed *= 1.5f;
                self.attackSpeed *= 1.5f;
                self.regen = (1 + (self.levelRegen * (self.level - 1))) * 0f;
                self.damage *= 2f;
            }                           

            bool ofa45 = self.HasBuff(Modules.Buffs.ofaBuff45);
            if (ofa45)
            {
                self.armor *= 2.5f;
                self.moveSpeed *= 1.2f;
                self.attackSpeed *= 1.2f;
                self.regen *= 0f;
                self.damage *= 1.5f;
            }

            bool supaofa45 = self.HasBuff(Modules.Buffs.supaofaBuff45);
            if (supaofa45)
            {
                self.armor *= 2.5f;
                self.moveSpeed *= 1.25f;
                self.attackSpeed *= 1.25f;
                self.regen *= 0f;
                self.damage *= 1.5f;
            }

            if (self.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME")                
            {


                if (!ofa45 && !supaofa45)
                {

                    HealthComponent hp = self.healthComponent;
                    float regenValue = hp.fullCombinedHealth * DekuPlugin.passiveRegenBonus;
                    float regen = Mathf.SmoothStep(regenValue, 0, hp.combinedHealth / hp.fullCombinedHealth);
                    self.regen += regen;
                    //Chat.AddMessage("hpregen activated");
                }
                

            }

            if (self)
            {
                if (self.HasBuff(Modules.Buffs.oklahomaBuff))
                {
                    self.armor *= 3f;
                }
            }
        }
        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            orig(self);
            if (self.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME")
            {
                AkSoundEngine.PostEvent(779278001, this.gameObject);
            }

        }
        //lifesteal
        private void GlobalEventManager_OnDamageDealt(DamageReport report)
        {
            //orig(self, damageinfo, victim);

            //if(damageinfo.attacker.name.Contains("Deku"))
            //{
            //    if(DekuCharacterBody == null)
            //    {
            //        DekuCharacterBody = damageinfo.attacker.GetComponent<CharacterBody>();
            //    }
            //    if (DekuCharacterBody.HasBuff(Modules.Buffs.supaofaBuff))
            //    {
            //        HealthComponent hp = DekuCharacterBody.healthComponent;
            //        hp.health += damageinfo.damage * 0.1f;
            //    }

            //}
            bool flag = !report.attacker || !report.attackerBody;
            if (!flag && report.attackerBody.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && report.attackerBody.HasBuff(Modules.Buffs.supaofaBuff))
            {
                CharacterBody attackerBody = report.attackerBody;
                attackerBody.healthComponent.Heal(report.damageDealt * 0.1f, default(ProcChainMask), true);

            }
            if (!flag && report.attackerBody.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && report.attackerBody.HasBuff(Modules.Buffs.supaofaBuff45))
            {
                CharacterBody attackerBody = report.attackerBody;
                attackerBody.healthComponent.Heal(report.damageDealt * 0.05f, default(ProcChainMask), true);

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
        
        private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);

            //Update fajin
            if (self.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME")
            {
                DekuController dekucon = self.GetComponent<DekuController>();
                if (dekucon.fajinon)
                {
                    self.SetBuffCount(Modules.Buffs.fajinBuff.buffIndex, dekucon.GetBuffCount());
                }
                if (dekucon.kickon)
                {
                    self.SetBuffCount(Modules.Buffs.kickBuff.buffIndex, dekucon.GetKickBuffCount());
                }

            }

        }

    }
}