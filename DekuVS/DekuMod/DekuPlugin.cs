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
using EmotesAPI;
using R2API.Networking.Interfaces;
using static UnityEngine.ParticleSystem.PlaybackState;
using static RoR2.Console;
using R2API;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace DekuMod
{
    //neeed this!
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.sound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.language", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.prefab", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.networking", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.damagetype", BepInDependency.DependencyFlags.HardDependency)]

    //don't need 
    //[BepInDependency("com.bepis.r2api.loadout", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.artifactcode", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.commandhelper", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.content_management", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.damagetype", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.deployable", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.difficulty", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.director", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.dot", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.elites", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.items", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.lobbyconfig", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.orb", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.lobbyconfig", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.recalculatestats", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.sceneasset", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.tempvisualeffect", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.bepis.r2api.unlockable", BepInDependency.DependencyFlags.HardDependency)]


    [BepInDependency("com.KingEnderBrine.ExtraSkillSlots", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]
    //[BepInDependency("com.ThinkInvisible.ClassicItems", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    //[R2APISubmoduleDependency(new string[]
    //{
    //    "PrefabAPI",
    //    "LanguageAPI",
    //    "SoundAPI",
    //    "NetworkingAPI",
    //    "SkinAPI",
    //    "LoadoutAPI",
    //    "DamageAPI"
    //})]

    public class DekuPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said

        //ancient scepters
        //public static bool scepterInstalled = false;
        //public static bool fallbackScepter = false;

        public const string MODUID = "com.TeaL.DekuMod";
        public const string MODNAME = "DekuMod";
        public const string MODVERSION = "5.0.0";
        public const float passiveRegenBonus = 0.035f;

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "TEAL";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        public static DekuPlugin instance;
        public static CharacterBody DekuCharacterBody;

        private void Awake()
        {
            Log.Init(Logger);
            instance = this;
            DekuCharacterBody = null;
            DekuPlugin.instance = this;
            
            // load assets and read config
            Modules.Assets.Initialize();
            Modules.Config.ReadConfig();
            Modules.Damage.SetupModdedDamage(); //setup modded damage
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new Deku().Initialize();

            //networking
            NetworkingAPI.RegisterMessageType<SpendHealthNetworkRequest>();
            NetworkingAPI.RegisterMessageType<HealNetworkRequest>();

            NetworkingAPI.RegisterMessageType<ServerForceGoBeyondStateNetworkRequest>();

            NetworkingAPI.RegisterMessageType<PerformDetroitSmashNetworkRequest>();
            NetworkingAPI.RegisterMessageType<PeformShootStyleKickAttackNetworkRequest>();
            NetworkingAPI.RegisterMessageType<PerformStLouisSmashNetworkRequest>();
            NetworkingAPI.RegisterMessageType<PerformBlackwhipPullNetworkRequest>();
            NetworkingAPI.RegisterMessageType<ForceCounterState>();
            NetworkingAPI.RegisterMessageType<ForceDangerSenseState>();
            NetworkingAPI.RegisterMessageType<TakeDamageForceRequest>();
            NetworkingAPI.RegisterMessageType<SetDodgeStateMachine>();
            NetworkingAPI.RegisterMessageType<BlackwhipImmobilizeRequest>();

            NetworkingAPI.RegisterMessageType<PerformDetroitDelawareNetworkRequest>();
            NetworkingAPI.RegisterMessageType<PerformFinalSmashNetworkRequest>();


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
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_OnDamageDealt;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            //On.RoR2.HealthComponent.Awake += HealthComponent_Awake;
            if (Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
        }

        //EMOTES
        private void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();
            foreach (var item in SurvivorCatalog.allSurvivorDefs)
            {
                Debug.Log(item.bodyPrefab.name);
                if (item.bodyPrefab.name == "DekuBody")
                {
                    CustomEmotesAPI.ImportArmature(item.bodyPrefab, Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("humanoidDeku"));
                }
            }
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

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig.Invoke(self, damageInfo, victim);

            var attacker = damageInfo.attacker;
            if (attacker)
            {
                var body = attacker.GetComponent<CharacterBody>();
                var victimBody = victim.GetComponent<CharacterBody>();
                if (body && victimBody)
                {
                    if (body && victimBody && damageInfo.damage > 0 && (damageInfo.damageType & DamageType.DoT) != DamageType.DoT)
                    {
                        //deku mark system
                        if(body.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME")
                        {

                            //vulnerability modded damage
                            if (DamageAPI.HasModdedDamageType(damageInfo, Modules.Damage.blackwhipImmobilise))
                            {
                                victimBody.ApplyBuff(Buffs.blackwhipDebuff.buffIndex, 1, victimBody.GetBuffCount(Buffs.blackwhipDebuff) + StaticValues.blackwhipDebuffDuration);
                            }

                            EnergySystem energySys = body.GetComponent<EnergySystem>();
                            energySys.plusUltraBoostTimer = 2f;

                            //gearshift buff
                            //if (body.HasBuff(Buffs.gearshiftBuff))
                            //{
                            //    float Weight = 1f;

                            //    if (victimBody.characterMotor)
                            //    {
                            //        Weight = victimBody.characterMotor.mass;
                            //    }
                            //    else if (victimBody.rigidbody)
                            //    {
                            //        Weight = victimBody.rigidbody.mass;
                            //    }

                            //    victimBody.healthComponent.TakeDamageForce(body.inputBank.aimDirection * StaticValues.gearshiftForceBoost * (Weight), true, true);
                            //}
                            ////gearshift45 buff
                            //if (damageInfo.procCoefficient > 0 && body.HasBuff(Buffs.gearshift45Buff))
                            //{
                            //    var bulletAttack = new BulletAttack
                            //    {
                            //        bulletCount = 1,
                            //        aimVector = body.characterDirection.forward,
                            //        origin = damageInfo.position,
                            //        damage = StaticValues.gearshift45DamageCoefficient * damageInfo.damage,
                            //        damageColorIndex = DamageColorIndex.Fragile,
                            //        damageType = damageInfo.damageType |= DamageType.SlowOnHit,
                            //        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                            //        maxDistance = 10f,
                            //        force = 500f,
                            //        hitMask = LayerIndex.CommonMasks.bullet,
                            //        minSpread = 0f,
                            //        maxSpread = 0f,
                            //        isCrit = body.RollCrit(),
                            //        owner = body.gameObject,
                            //        smartCollision = false,
                            //        procChainMask = default(ProcChainMask),
                            //        procCoefficient = 0,
                            //        radius = 1.5f,
                            //        sniper = true,
                            //        stopperMask = LayerIndex.world.collisionMask,
                            //        weapon = null,
                            //        spreadPitchScale = 0f,
                            //        spreadYawScale = 0f,
                            //        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                            //        hitEffectPrefab = Modules.Assets.dekuHitImpactEffect,

                            //    };
                            //    bulletAttack.Fire();

                            //    EffectManager.SpawnEffect(Modules.Assets.gearshiftPierceEffect, new EffectData
                            //    {
                            //        origin = damageInfo.position,
                            //        scale = 1f,
                            //        rotation = Quaternion.LookRotation(body.characterDirection.forward) 

                            //    }, true);

                            //}
                            ////gearshift100 buff
                            //if (damageInfo.procCoefficient > 0 && body.HasBuff(Buffs.gearshift100Buff))
                            //{
                            //    int gearshiftBuffCount = body.GetBuffCount(Buffs.gearshift100Buff.buffIndex);
                            //    body.ApplyBuff(Buffs.gearshift100Buff.buffIndex, gearshiftBuffCount - 1);

                            //    var damageInfo2 = new DamageInfo();

                            //    damageInfo2.damage = damageInfo.damage;
                            //    damageInfo2.position = victimBody.transform.position;
                            //    damageInfo2.force = damageInfo.force;
                            //    damageInfo2.damageColorIndex = DamageColorIndex.WeakPoint;
                            //    damageInfo2.crit = body.RollCrit();
                            //    damageInfo2.attacker = body.gameObject;
                            //    damageInfo2.inflictor = body.gameObject;
                            //    damageInfo2.damageType = damageInfo.damageType;
                            //    damageInfo2.procCoefficient = 0f;
                            //    damageInfo2.procChainMask = default(ProcChainMask);

                            //    float number = 0f;
                            //    float threshold = StaticValues.gearshift100Threshold;
                            //    while(body.moveSpeed >= threshold)
                            //    {
                            //        AkSoundEngine.PostEvent("detroitexitsfx", body.gameObject);

                            //        if (victimBody.gameObject.GetComponent<CharacterBody>().baseNameToken
                            //            != DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && body != null)
                            //        {
                            //            victimBody.healthComponent.TakeDamage(damageInfo2);
                            //        }

                            //        Vector3 enemyPos = victimBody.transform.position;
                            //        EffectManager.SpawnEffect(Modules.Assets.impactEffect, new EffectData
                            //        {
                            //            origin = enemyPos + ((body.transform.position - enemyPos) * number),
                            //            scale = 1f,
                            //            rotation = Quaternion.LookRotation((enemyPos - body.transform.position))

                            //        }, true);
                            //        number++;
                            //        threshold += StaticValues.gearshift100Threshold;
                            //    }
                            //}
                            //blackwhip debuff
                            //if (damageInfo.damageType == DamageType.ClayGoo)
                            //{
                            //    victimBody.ApplyBuff(Buffs.blackwhipDebuff.buffIndex, 1, StaticValues.blackwhipDebuffDuration);
                                
                            //}
                            //heal and armor mark for freeze
                            //if (damageInfo.damageType == DamageType.Freeze2s)
                            //{
                            //    if (!victimBody.HasBuff(Buffs.healMark.buffIndex))
                            //    {
                            //        victimBody.ApplyBuff(Buffs.healMark.buffIndex, 1, -1);
                            //    }
                            //    if (!victimBody.HasBuff(Buffs.barrierMark.buffIndex))
                            //    {
                            //        victimBody.ApplyBuff(Buffs.barrierMark.buffIndex, 1, -1);
                            //    }
                            //}
                            ////heal and armor mark for ignite
                            //if (damageInfo.damageType == DamageType.IgniteOnHit)
                            //{
                            //    if (!victimBody.HasBuff(Buffs.healMark.buffIndex))
                            //    {
                            //        victimBody.ApplyBuff(Buffs.healMark.buffIndex, 1, -1);
                            //    }
                            //    if (!victimBody.HasBuff(Buffs.barrierMark.buffIndex))
                            //    {
                            //        victimBody.ApplyBuff(Buffs.barrierMark.buffIndex, 1, -1);
                            //    }
                            //}
                            ////heal mark for shock
                            //if (damageInfo.damageType == DamageType.Shock5s)
                            //{
                            //    if (!victimBody.HasBuff(Buffs.healMark.buffIndex))
                            //    {
                            //        victimBody.ApplyBuff(Buffs.healMark.buffIndex, 1, -1);
                            //    }
                            //}
                            ////armor mark for stun
                            //if (damageInfo.damageType == DamageType.Stun1s)
                            //{
                            //    if (!victimBody.HasBuff(Buffs.barrierMark.buffIndex))
                            //    {
                            //        victimBody.ApplyBuff(Buffs.barrierMark.buffIndex, 1, -1);
                            //    }
                            //}

                            ////heal each hit based on damage, after buffcount is 4 remove
                            //if (victimBody.HasBuff(Buffs.healMark.buffIndex))
                            //{
                            //    int buffCount = victimBody.GetBuffCount(Buffs.healMark.buffIndex);
                            //    if (buffCount < 4)
                            //    {
                            //        victimBody.ApplyBuff(Buffs.healMark.buffIndex, buffCount + 1);
                            //        body.healthComponent.Heal(damageInfo.damage * StaticValues.healMarkCoefficient, default(ProcChainMask), true);
                            //    }
                            //    else if (buffCount >= 4)
                            //    {
                            //        victimBody.ApplyBuff(Buffs.healMark.buffIndex, 0);
                            //    }
                            //}

                            ////gain barrier each hit based on maxhealth, after buffcount is 4 remove
                            //if (victimBody.HasBuff(Buffs.barrierMark.buffIndex))
                            //{
                            //    int buffCount = victimBody.GetBuffCount(Buffs.barrierMark.buffIndex);
                            //    if (buffCount < 4)
                            //    {
                            //        victimBody.ApplyBuff(Buffs.barrierMark.buffIndex, buffCount + 1);
                            //        body.healthComponent.AddBarrierAuthority(body.healthComponent.combinedHealth * StaticValues.barrierMarkCoefficient);
                            //    }
                            //    else if (buffCount >= 4)
                            //    {
                            //        victimBody.ApplyBuff(Buffs.barrierMark.buffIndex, 0);
                            //    }
                            //}

                        }
                    }
                }
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self)
            {
                if (damageInfo != null && damageInfo.attacker && damageInfo.attacker.GetComponent<CharacterBody>())
                {
                    if (self.body.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME")
                    {
                        DekuController dekucon = self.gameObject.GetComponent<DekuController>();
                        EnergySystem energysys = self.gameObject.GetComponent<EnergySystem>();

                        bool flag = (damageInfo.damageType & DamageType.BypassArmor) > DamageType.Generic;
                        if (!flag
                            && damageInfo.damage > self.body.healthComponent.health
                            && energysys.currentPlusUltra > StaticValues.goBeyondThreshold
                            && !self.body.HasBuff(Buffs.goBeyondBuffUsed))
                        {
                            energysys.currentPlusUltra -= energysys.currentPlusUltra;
                            damageInfo.rejected = true;
                            self.body.healthComponent.health = 1f;
                            new ServerForceGoBeyondStateNetworkRequest(self.body.masterObjectId).Send(NetworkDestination.Clients);
                            
                        }
                    }

                }
            }
            orig.Invoke(self, damageInfo);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            //regen 
            orig.Invoke(self);

            if (self)
            {
                if (self.HasBuff(Buffs.blackwhipDebuff))
                {
                    self.moveSpeed *= StaticValues.blackwhipDebuffMultiplier;
                    self.attackSpeed *= StaticValues.blackwhipDebuffMultiplier;
                }

                if (self.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME")
                {
                    if (self.HasBuff(Buffs.counterAttackBuff))
                    {
                        self.attackSpeed *= StaticValues.counterBuffAttackspeed;
                    }
                    if (self.HasBuff(Buffs.counterBuff))
                    {
                        self.armor += StaticValues.counterBuffArmor;
                    }
                    if (self.HasBuff(Buffs.mightBuff))
                    {
                        self.damage *= StaticValues.mightBuffMultiplier;
                    }
                    if (self.HasBuff(Buffs.blackwhipCDBuff))
                    {
                        self.skillLocator.secondary.cooldownScale *= StaticValues.blackwhipCDMultiplier;
                        self.skillLocator.utility.cooldownScale *= StaticValues.blackwhipCDMultiplier;
                    }

                    //if (self.HasBuff(Buffs.gearshiftBuff))
                    //{
                    //    self.moveSpeed *= StaticValues.gearshiftMovespeedBoost;
                    //}

                    //if (self.HasBuff(Buffs.manchesterBuff))
                    //{
                    //    self.armor += StaticValues.manchesterArmor;
                    //}

                    //bool floatbuff = self.HasBuff(Buffs.floatBuff);
                    //if (floatbuff)
                    //{
                    //    self.moveSpeed *= 1.5f;
                    //    self.acceleration *= 2f;

                    //}

                    bool goBeyond = self.HasBuff(Buffs.goBeyondBuff);
                    if (goBeyond)
                    {
                        self.armor *= 5f;
                        self.moveSpeed *= 1.5f;
                    }

                    //bool fajin = self.HasBuff(Modules.Buffs.fajinBuff);
                    //if (fajin)
                    //{
                    //    self.damage *= StaticValues.fajinDamageMultiplier;
                    //    self.moveSpeed *= StaticValues.fajinDamageMultiplier;

                    //}

                    //bool ofa = self.HasBuff(Modules.Buffs.ofaBuff);

                    //if (ofa)
                    //{
                    //    self.armor *= 5f;
                    //    self.moveSpeed *= 1.5f;
                    //    self.regen -= (self.levelRegen * (self.level - 1));              
                    //}


                    //bool supaofa = self.HasBuff(Modules.Buffs.supaofaBuff);
                    //if (supaofa)
                    //{
                    //    self.armor *= 5f;
                    //    self.moveSpeed *= 1.5f;
                    //}
                         

                    //bool ofa45 = self.HasBuff(Modules.Buffs.ofaBuff45);
                    //if (ofa45)
                    //{
                    //    self.armor *= 2.5f;
                    //    self.moveSpeed *= 1.2f;
                    //    self.regen -= (1 + (self.levelRegen * (self.level - 1)));
                    //}

                    //bool supaofa45 = self.HasBuff(Modules.Buffs.supaofaBuff45);
                    //if (supaofa45)
                    //{
                    //    self.armor *= 2.5f;
                    //    self.moveSpeed *= 1.25f;
                    //    self.regen -= (1 + (self.levelRegen * (self.level - 1)));
                    //}


                    //if (self.HasBuff(Modules.Buffs.oklahomaBuff))
                    //{
                    //    self.armor *= 3f;
                    //}

                }

                
            }

        }
        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            orig(self);
            if (self.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME")
            {
                AkSoundEngine.PostEvent("dekudeath", this.gameObject);
            }

        }
        //lifesteal
        private void GlobalEventManager_OnDamageDealt(DamageReport report)
        {
            bool flag = !report.attacker || !report.attackerBody;
            //if (!flag && report.attackerBody.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && report.attackerBody.HasBuff(Modules.Buffs.supaofaBuff))
            //{
            //    CharacterBody attackerBody = report.attackerBody;
            //    attackerBody.healthComponent.Heal(report.damageDealt * 0.1f, default(ProcChainMask), true);

            //}
            //if (!flag && report.attackerBody.baseNameToken == DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && report.attackerBody.HasBuff(Modules.Buffs.supaofaBuff45))
            //{
            //    CharacterBody attackerBody = report.attackerBody;
            //    attackerBody.healthComponent.Heal(report.damageDealt * 0.05f, default(ProcChainMask), true);

            //}
        }


        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("DekuDisplay"))
            {
                AkSoundEngine.PostEvent("dekuEntrance", self.gameObject);
            }

        }

        private void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self)
        {
            orig(self);

            if (self)
            {
                if (self.body)
                {
                    this.OverlayFunction(Modules.Assets.blackwhipDebuffMaterial, self.body.HasBuff(Modules.Buffs.blackwhipDebuff), self);
                    this.OverlayFunction(Modules.Assets.redblinkingMaterial, self.body.HasBuff(Modules.Buffs.counterAttackBuff), self);
                    this.OverlayFunction(Modules.Assets.whiteblinkingMaterial, self.body.HasBuff(Modules.Buffs.overlayBuff), self);
                    this.OverlayFunction(Modules.Assets.yellowblinkingMaterial, self.body.HasBuff(Modules.Buffs.mightBuff), self);
                    this.OverlayFunction(Modules.Assets.purpleblinkingMaterial, self.body.HasBuff(Modules.Buffs.blackwhipCDBuff), self);
                }
            }
        }


        private void OverlayFunction(Material overlayMaterial, bool condition, CharacterModel model)
        {
            if (model.activeOverlayCount >= CharacterModel.maxOverlays)
            {
                return;
            }
            if (condition)
            {
                Material[] array = model.currentOverlays;
                int num = model.activeOverlayCount;
                model.activeOverlayCount = num + 1;
                array[num] = overlayMaterial;
            }
        }

    }
}