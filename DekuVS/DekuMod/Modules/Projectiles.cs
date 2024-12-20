﻿using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.Modules
{
    public static class Projectiles
    {

        internal static void RegisterProjectiles()
        {
            //only separating into separate methods for my sanity

            //CreateBlackwhip();
            //AddProjectile(blackwhipPrefab);

            ////bullet tracers
            //airforceTracer = Modules.Asset.airforceEffect;

            //if (!airforceTracer.GetComponent<EffectComponent>()) airforceTracer.AddComponent<EffectComponent>();
            //if (!airforceTracer.GetComponent<VFXAttributes>()) airforceTracer.AddComponent<VFXAttributes>();
            //if (!airforceTracer.GetComponent<NetworkIdentity>()) airforceTracer.AddComponent<NetworkIdentity>();

            //Material bulletMat = null;

            //foreach (LineRenderer i in airforceTracer.GetComponentsInChildren<LineRenderer>())
            //{
            //    if (i)
            //    {
            //        bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
            //        bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
            //        i.material = bulletMat;
            //        i.startColor = new Color(0.68f, 0.58f, 0.05f);
            //        i.endColor = new Color(0.68f, 0.58f, 0.05f);

            //    }
            //}
            //Modules.Effects.AddEffect(airforceTracer);

            //airforce100Tracer = Modules.Asset.airforce100Effect;

            //if (!airforce100Tracer.GetComponent<EffectComponent>()) airforce100Tracer.AddComponent<EffectComponent>();
            //if (!airforce100Tracer.GetComponent<VFXAttributes>()) airforce100Tracer.AddComponent<VFXAttributes>();
            //if (!airforce100Tracer.GetComponent<NetworkIdentity>()) airforce100Tracer.AddComponent<NetworkIdentity>();


            //foreach (LineRenderer i in airforce100Tracer.GetComponentsInChildren<LineRenderer>())
            //{
            //    if (i)
            //    {
            //        bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
            //        bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
            //        i.material = bulletMat;
            //        i.startColor = new Color(0.68f, 0.58f, 0.05f);
            //        i.endColor = new Color(0.68f, 0.58f, 0.05f);

            //    }
            //}
            //Modules.Effects.AddEffect(airforce100Tracer);

            //delawareBullet = Modules.Asset.airforce45Effect;

            //if (!delawareBullet.GetComponent<EffectComponent>()) delawareBullet.AddComponent<EffectComponent>();
            //if (!delawareBullet.GetComponent<VFXAttributes>()) delawareBullet.AddComponent<VFXAttributes>();
            //if (!delawareBullet.GetComponent<NetworkIdentity>()) delawareBullet.AddComponent<NetworkIdentity>();

            //foreach (LineRenderer i in delawareBullet.GetComponentsInChildren<LineRenderer>())
            //{
            //    if (i)
            //    {
            //        bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
            //        bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
            //        i.material = bulletMat;
            //        i.startColor = new Color(0.68f, 0.58f, 0.05f);
            //        i.endColor = new Color(0.68f, 0.58f, 0.05f);

            //    }
            //}
            //Modules.Effects.AddEffect(delawareBullet);

            //blackwhipTracer = Modules.Asset.blackwhipbullet;

            //if (!blackwhipTracer.GetComponent<EffectComponent>()) blackwhipTracer.AddComponent<EffectComponent>();
            //if (!blackwhipTracer.GetComponent<VFXAttributes>()) blackwhipTracer.AddComponent<VFXAttributes>();
            //if (!blackwhipTracer.GetComponent<NetworkIdentity>()) blackwhipTracer.AddComponent<NetworkIdentity>();

            //foreach (LineRenderer i in blackwhipTracer.GetComponentsInChildren<LineRenderer>())
            //{
            //    if (i)
            //    {
            //        bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
            //        bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
            //        i.material = bulletMat;
            //        i.startColor = new Color(0.68f, 0.58f, 0.05f);
            //        i.endColor = new Color(0.68f, 0.58f, 0.05f);

            //    }
            //}
            //Modules.Effects.AddEffect(blackwhipTracer);

            //delawareTracer = Modules.Asset.delawareEffect;
            //Modules.Effects.AddEffect(delawareTracer);
            //detroitTracer = Modules.Asset.detroitEffect;
            //Modules.Effects.AddEffect(detroitTracer);
            //detroitweakTracer = Modules.Asset.detroitweakEffect;
            //Modules.Effects.AddEffect(detroitweakTracer);

        }



        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }

        //private static void CreateBlackwhip()
        //{
        //    blackwhipPrefab = CloneProjectilePrefab("magefirebolt", "blackwhipProjectile");

        //    ProjectileImpactExplosion blackwhip = blackwhipPrefab.GetComponent<ProjectileImpactExplosion>();
        //    InitializeImpactExplosion(blackwhip);

        //    blackwhip.blastRadius = 3f;
        //    blackwhip.destroyOnEnemy = true;
        //    blackwhip.lifetime = 6f;
        //    //bombImpactExplosion.impactEffect = Modules.Asset.bombExplosionEffect;
        //    //bombImpactExplosion.lifetimeExpiredSound = Modules.Asset.CreateNetworkSoundEventDef("HenryBombExplosion");
        //    blackwhip.timerAfterImpact = false;
        //    blackwhip.lifetimeAfterImpact = 0f;
        //    blackwhip.destroyOnWorld = true;

        //    ProjectileController bombController = blackwhipPrefab.GetComponent<ProjectileController>();
        //    if (Modules.Asset.mainAssetBundle.LoadAsset<GameObject>("blackwhipshootGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("blackwhipshootGhost");
        //    bombController.startSound = "";

        //    blackwhipPrefab.AddComponent<ProjectileImpactEventCaller>();
        //}

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.explosionSoundString = "";
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.hasImpact = true;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeExpiredSoundString = "";
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        //private static void InitializeHookProjectileImpact(HookProjectileImpact hookProjectileImpact)
        //{
        //    hookProjectileImpact.flyTimer = 1f;
        //    hookProjectileImpact.liveTimer = 1f;
        //    hookProjectileImpact.Reel

        //}

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.DekuAssets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.DekuAssets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}