using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using UnityEngine.AddressableAssets;
using static RoR2.Console;

namespace DekuMod.Modules
{
    internal static class Assets
    {
        // the assetbundle to load assets from
        internal static AssetBundle mainAssetBundle;

        //buffs
        public static Sprite healBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Croco/bdCrocoRegen.asset").WaitForCompletion().iconSprite;
        //public static Sprite warcryBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/WarCryOnMultiKill/bdWarCryBuff.asset").WaitForCompletion().iconSprite;
        public static Sprite shieldBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdArmorBoost.asset").WaitForCompletion().iconSprite;
        //public static Sprite tarBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdClayGoo.asset").WaitForCompletion().iconSprite;
        public static Sprite crippleBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdCripple.asset").WaitForCompletion().iconSprite;
        public static Sprite speedBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Bandit2/bdCloakSpeed.asset").WaitForCompletion().iconSprite;
        //public static Sprite boostBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/RandomDamageZone/bdPowerBuff.asset").WaitForCompletion().iconSprite;
        //public static Sprite alphashieldonBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/BearVoid/bdBearVoidReady.asset").WaitForCompletion().iconSprite;
        //public static Sprite alphashieldoffBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/BearVoid/bdBearVoidCooldown.asset").WaitForCompletion().iconSprite;
        //public static Sprite decayBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdVoidFogStrong.asset").WaitForCompletion().iconSprite;
        //public static Sprite multiplierBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/PrimarySkillShuriken/bdPrimarySkillShurikenBuff.asset").WaitForCompletion().iconSprite;
        public static Sprite jumpBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/MoveSpeedOnKill/bdKillMoveSpeed.asset").WaitForCompletion().iconSprite;
        //public static Sprite sprintBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/SprintOutOfCombat/bdWhipBoost.asset").WaitForCompletion().iconSprite;
        //public static Sprite spikeBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Grandparent/bdOverheat.asset").WaitForCompletion().iconSprite;
        //public static Sprite mortarBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/GainArmor/bdElephantArmorBoost.asset").WaitForCompletion().iconSprite;
        //public static Sprite attackspeedBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/EnergizedOnEquipmentUse/bdEnergized.asset").WaitForCompletion().iconSprite;
        //public static Sprite gravityBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/KillEliteFrenzy/bdNoCooldowns.asset").WaitForCompletion().iconSprite;
        //public static Sprite bleedBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdBleeding.asset").WaitForCompletion().iconSprite;
        //public static Sprite skinBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/OutOfCombatArmor/bdOutOfCombatArmorBuff.asset").WaitForCompletion().iconSprite;
        //public static Sprite orbreadyBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/ElementalRings/bdElementalRingsReady.asset").WaitForCompletion().iconSprite;
        //public static Sprite orbdisableBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/ElementalRings/bdElementalRingsCooldown.asset").WaitForCompletion().iconSprite;
        //public static Sprite blazingBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdOnFire.asset").WaitForCompletion().iconSprite;
        public static Sprite lightningBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/ShockNearby/bdTeslaField.asset").WaitForCompletion().iconSprite;
        //public static Sprite resonanceBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/LaserTurbine/bdLaserTurbineKillCharge.asset").WaitForCompletion().iconSprite;
        public static Sprite critBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/CritOnUse/bdFullCrit.asset").WaitForCompletion().iconSprite;
        //public static Sprite claygooBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdClayGoo.asset").WaitForCompletion().iconSprite;
        //public static Sprite predatorBuffIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/AttackSpeedOnCrit/bdAttackSpeedOnCrit.asset").WaitForCompletion().iconSprite;



        // particle effects
        public static GameObject bisonEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bison/BisonChargeStep.prefab").WaitForCompletion();
        public static GameObject banditmuzzleEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/MuzzleflashBandit2.prefab").WaitForCompletion();
        public static GameObject bandittracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracerbandit2rifle");
        public static GameObject banditimpactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/HitsparkBandit2Pistol.prefab").WaitForCompletion();
        
		public static GameObject boostJumpEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/BoostJumpEffect");
        public static GameObject muzzleflashMageLightningLargePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashMageLightningLarge");
        public static GameObject lightningNovaEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        public static GameObject mageLightningBombEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
        public static GameObject sonicboomEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        public static GameObject GupSpikeEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GupExplosion.prefab").WaitForCompletion();
        public static GameObject lemfireBall = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Lemurian/Fireball.prefab").WaitForCompletion();
        public static GameObject greaterwispBall = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/GreaterWisp/WispCannon.prefab").WaitForCompletion();
        public static GameObject chargegreaterwispBall = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/GreaterWisp/OmniExplosionVFXGreaterWisp.prefab").WaitForCompletion();
        public static GameObject larvajumpEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/AcidLarva/AcidLarvaLeapExplosion.prefab").WaitForCompletion();
        public static GameObject elderlemurianexplosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LemurianBruiser/OmniExplosionVFXLemurianBruiserFireballImpact.prefab").WaitForCompletion();
        public static GameObject parentslamEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Parent/ParentSlamEffect.prefab").WaitForCompletion();
        public static GameObject claydunestriderEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ClayBruiser/ClayShockwaveEffect.prefab").WaitForCompletion();
        public static GameObject voidjailerEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidJailer/VoidJailerDeathBombExplosion.prefab").WaitForCompletion();
        public static GameObject voidjailermuzzleEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamTracer.prefab").WaitForCompletion();
        public static GameObject xiconstructbeamEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/MajorAndMinorConstruct/MajorConstructInitialMuzzleFlash.prefab").WaitForCompletion();
        public static GameObject xiconstructexplosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/MajorAndMinorConstruct/MajorConstructSecondMuzzleFlash.prefab").WaitForCompletion();
        public static RoR2.Audio.LoopSoundDef xiconstructsound = Addressables.LoadAssetAsync<RoR2.Audio.LoopSoundDef>("RoR2/DLC1/MajorAndMinorConstruct/lsdMajorConstructLaser.asset").WaitForCompletion();
        //public static GameObject grandparentSunPrefab;
        //public static GameObject grandparentSunSpawnPrefab;

        //public static GameObject artificerfireEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageFlamethrowerEffect.prefab").WaitForCompletion();
        //public static GameObject artificerlightningorbchargeEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MuzzleflashMageLightning.prefab").WaitForCompletion();
        //public static GameObject artificerlightningorbMuzzleEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MuzzleflashMageLightning.prefab").WaitForCompletion();
        //public static GameObject artificerlightningorbprojectileEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion();
        //public static GameObject artificerCrosshair = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageCrosshair.prefab").WaitForCompletion();
        //public static RoR2.Audio.LoopSoundDef artificerlightningsound = Addressables.LoadAssetAsync<RoR2.Audio.LoopSoundDef>("RoR2/Base/Vagrant/lsdVagrantTrackingBombFlight.asset").WaitForCompletion();
        //public static GameObject engiTurret = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiTurretMaster.prefab").WaitForCompletion();
        //public static GameObject voidfiendblinkVFX = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidBlinkVfx.prefab").WaitForCompletion();
        //public static GameObject voidfiendblinkmuzzleEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidBlinkMuzzleflash.prefab").WaitForCompletion();
        //public static GameObject voidfiendblinktrailEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/PodVoidGroundImpact.prefab").WaitForCompletion();
        //public static Material voidfiendblinkMaterial = Addressables.LoadAssetAsync<Material>("RoR2/DLC1/VoidSurvivor/matVoidBlinkBodyOverlay.mat").WaitForCompletion();
        //public static GameObject railgunnercryoCrosshair = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerCryochargeCrosshair.prefab").WaitForCompletion();
        //public static GameObject railgunnercryochargeCrosshair = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerCryochargeUI.prefab").WaitForCompletion();
        //public static GameObject railgunnercryoreloadCrosshair = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerReloadUI.prefab").WaitForCompletion();
        //public static GameObject railgunnercryoTracer = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/TracerRailgunCryo.prefab").WaitForCompletion();
        //public static RoR2.Audio.LoopSoundDef railgunnercryochargingSound = Addressables.LoadAssetAsync<RoR2.Audio.LoopSoundDef>("RoR2/DLC1/Railgunner/lsdRailgunnerBackpackCharging.asset").WaitForCompletion();
        //public static RoR2.Audio.LoopSoundDef railgunnercryoofflineSound = Addressables.LoadAssetAsync<RoR2.Audio.LoopSoundDef>("RoR2/DLC1/Railgunner/lsdRailgunnerBackpackOffline.asset").WaitForCompletion();
        //public static GameObject multEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/OmniExplosionVFXToolbotQuick.prefab").WaitForCompletion();

        //own effects
        internal static GameObject blackwhip;
        internal static GameObject blackwhipTrail;
        internal static GameObject blackwhipbullet;
        internal static GameObject blackwhipLineRenderer;
        internal static GameObject blackwhipforward;
        internal static GameObject detroitEffect;
        internal static GameObject detroitweakEffect;
        internal static GameObject delawareEffect;
        internal static GameObject airforceEffect;
        internal static GameObject airforce45Effect;
        internal static GameObject impactEffect;
        internal static GameObject windringEffect;
        internal static GameObject smokeEffect;
        internal static GameObject smokebigEffect;
        internal static GameObject airforce100Effect;
        internal static GameObject airforce100impactEffect;
        internal static GameObject gobeyondPulseEffect;
        internal static GameObject gearshiftPierceEffect;


        //Materials
        public static Material blackwhipDebuffMaterial;


        //sword swing
        internal static GameObject dekuKickEffect;
        internal static GameObject dekuHitImpactEffect;

        //UI
        public static GameObject banditCrosshair = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/Bandit2CrosshairPrepRevolver.prefab").WaitForCompletion();



        // networked hit sounds
        internal static NetworkSoundEventDef kickHitSoundEvent;

        // lists of assets to add to contentpack
        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();
        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        // cache these and use to create our own materials
        internal static Shader hotpoo = RoR2.LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");
        internal static Material commandoMat;
        private static string[] assetNames = new string[0];
        private static List<Material> cachedMaterials = new List<Material>();

        // CHANGE THIS
        private const string assetbundleName = "DekuAssetBundle";

        internal static void Initialize()
        {
            if (assetbundleName == "myassetbundle")
            {
                Debug.LogError("AssetBundle name hasn't been changed- not loading any assets to avoid conflicts");
                return;
            }

            LoadAssetBundle();
            LoadSoundbank();
            PopulateAssets();
        }

        internal static void LoadAssetBundle()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DekuMod." + assetbundleName))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            assetNames = mainAssetBundle.GetAllAssetNames();
        }

        internal static void LoadSoundbank()
        {
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("DekuMod.Deku.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        internal static void PopulateAssets()
        {
            if (!mainAssetBundle)
            {
                Debug.LogError("There is no AssetBundle to load assets from.");
                return;
            }

            airforceEffect = LoadEffect("windbullet");
            airforce45Effect = LoadEffect("windbullet45");
            delawareEffect = LoadEffect("delaware");
            detroitEffect = LoadEffect("detroit");
            detroitweakEffect = LoadEffect("detroitweak");
            blackwhipforward = LoadEffect("blackwhipforward");
            blackwhip = LoadEffect("blackwhipeffect");
            blackwhipTrail = LoadEffect("blackwhipTrail");
            blackwhipLineRenderer = mainAssetBundle.LoadAsset<GameObject>("blackwhipLineRenderer");
            //blackwhip debuff effect
            blackwhipDebuffMaterial = mainAssetBundle.LoadAsset<Material>("blackwhipbullet");
            impactEffect = LoadEffect("impact");
            windringEffect = LoadEffect("windringBig");
            blackwhipbullet = LoadEffect("blackwhipshootGhost");
            smokeEffect = LoadEffect("smoke");
            smokebigEffect = LoadEffect("smokebig");
            airforce100Effect = LoadEffect("airforce100trail");
            airforce100impactEffect= LoadEffect("airforce100impact");
            gobeyondPulseEffect = LoadEffect("goBeyondPulse");
            gearshiftPierceEffect = LoadEffect("gearshiftPierce");

            //sword swing
            dekuKickEffect = Assets.LoadEffect("DekuKickEffect", true);
            dekuHitImpactEffect = Assets.LoadEffect("ImpactDekuKick");            
            kickHitSoundEvent = CreateNetworkSoundEventDef("shootstyedashcomboimpact");

            //blackwhipbullet = LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            //if (bombExplosionEffect)
            //{
            //    ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            //    shakeEmitter.amplitudeTimeDecay = true;
            //    shakeEmitter.duration = 0.5f;
            //    shakeEmitter.radius = 200f;
            //    shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            //    shakeEmitter.wave = new Wave
            //    {
            //        amplitude = 1f,
            //        frequency = 40f,
            //        cycleOffset = 0f
            //    };
            //}
        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            networkSoundEventDefs.Add(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }

        internal static Texture LoadCharacterIcon(string characterName)
        {
            return mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false);
        }

        private static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {
            bool assetExists = false;
            for (int i = 0; i < assetNames.Length; i++)
            {
                if (assetNames[i].Contains(resourceName.ToLower()))
                {
                    assetExists = true;
                    i = assetNames.Length;
                }
            }

            if (!assetExists)
            {
                Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        private static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            effectDefs.Add(newEffectDef);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!commandoMat) commandoMat = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material mat = UnityEngine.Object.Instantiate<Material>(commandoMat);
            Material tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            if (!tempMat)
            {
                Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
                return commandoMat;
            }

            mat.name = materialName;
            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);

            return mat;
        }

        public static Material CreateMaterial(string materialName)
        {
            return Assets.CreateMaterial(materialName, 0f);
        }

        public static Material CreateMaterial(string materialName, float emission)
        {
            return Assets.CreateMaterial(materialName, emission, Color.white);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
        {
            return Assets.CreateMaterial(materialName, emission, emissionColor, 0f);
        }


        public static Material CreateHopooMaterial(string materialName)
        {
            Material tempMat = cachedMaterials.Find(mat =>
            {
                materialName.Replace(" (Instance)", "");
                return mat.name.Contains(materialName);
            });
            if (tempMat)
                return tempMat;

            tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            if (!tempMat)
            {
                Log.Error("Failed to load material: " + materialName + " - Check to see that the material in your Unity project matches this name");
                return new Material(hotpoo);
            }

            return tempMat.SetHopooMaterial();
        }

        public static Material SetHopooMaterial(this Material tempMat)
        {
            if (cachedMaterials.Contains(tempMat))
                return tempMat;

            float? bumpScale = null;
            Color? emissionColor = null;

            //grab values before the shader changes
            if (tempMat.IsKeywordEnabled("_NORMALMAP"))
            {
                bumpScale = tempMat.GetFloat("_BumpScale");
            }
            if (tempMat.IsKeywordEnabled("_EMISSION"))
            {
                emissionColor = tempMat.GetColor("_EmissionColor");
            }

            //set shader
            tempMat.shader = hotpoo;

            //apply values after shader is set
            tempMat.SetColor("_Color", tempMat.GetColor("_Color"));
            tempMat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            tempMat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            tempMat.EnableKeyword("DITHER");
            tempMat.EnableKeyword("CUTOUT");

            if (bumpScale != null)
            {
                tempMat.SetFloat("_NormalStrength", (float)bumpScale);
            }
            if (emissionColor != null)
            {
                tempMat.SetColor("_EmColor", (Color)emissionColor);
                tempMat.SetFloat("_EmPower", 1);
            }

            //set this keyword in unity if you want your model to show backfaces
            //in unity, right click the inspector tab and choose Debug
            if (tempMat.IsKeywordEnabled("NOCULL"))
            {
                tempMat.SetInt("_Cull", 0);
            }
            //set this keyword in unity if you've set up your model for limb removal item displays (eg. goat hoof) by setting your model's vertex colors
            if (tempMat.IsKeywordEnabled("LIMBREMOVAL"))
            {
                tempMat.SetInt("_LimbRemovalOn", 1);
            }
            //cutout
            if (tempMat.IsKeywordEnabled("CUTOUT"))
            {
                tempMat.SetFloat("_EnableCutout", 1f);
            }

            cachedMaterials.Add(tempMat);
            return tempMat;
        }

        /// <summary>
        /// Makes this a unique material if we already have this material cached (i.e. you want an altered version). New material will not be cached
        /// <para>If it was not cached in the first place, simply returns as it is already unique.</para>
        /// </summary>
        public static Material MakeUnique(this Material material)
        {

            if (cachedMaterials.Contains(material))
            {
                return new Material(material);
            }
            return material;
        }

        public static Material SetColor(this Material material, Color color)
        {
            material.SetColor("_Color", color);
            return material;
        }

        public static Material SetNormal(this Material material, float normalStrength = 1)
        {
            material.SetFloat("_NormalStrength", normalStrength);
            return material;
        }

        public static Material SetEmission(this Material material) => SetEmission(material, 1);
        public static Material SetEmission(this Material material, float emission) => SetEmission(material, emission, Color.white);
        public static Material SetEmission(this Material material, float emission, Color emissionColor)
        {
            material.SetFloat("_EmPower", emission);
            material.SetColor("_EmColor", emissionColor);
            return material;
        }
        public static Material SetCull(this Material material, bool cull = false)
        {
            material.SetInt("_Cull", cull ? 1 : 0);
            return material;
        }

    }
}