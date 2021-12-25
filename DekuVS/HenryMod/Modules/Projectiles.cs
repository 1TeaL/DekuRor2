using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.Modules
{
    public static class Projectiles
    {

        internal static GameObject blackwhipPrefab;
        public static GameObject blackwhipTracer;
        public static GameObject airforceTracer;
        public static GameObject delawareTracer;
        public static GameObject detroitTracer;
        public static GameObject detroitweakTracer;
        public static GameObject airforce45Tracer;
        internal static void RegisterProjectiles()
        {
            //only separating into separate methods for my sanity

            CreateBlackwhip();
            AddProjectile(blackwhipPrefab);

            //bullet tracers
            airforceTracer = Modules.Assets.airforceEffect;

            if (!airforceTracer.GetComponent<EffectComponent>()) airforceTracer.AddComponent<EffectComponent>();
            if (!airforceTracer.GetComponent<VFXAttributes>()) airforceTracer.AddComponent<VFXAttributes>();
            if (!airforceTracer.GetComponent<NetworkIdentity>()) airforceTracer.AddComponent<NetworkIdentity>();

            Material bulletMat = null;

            foreach (LineRenderer i in airforceTracer.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
                    bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
                    i.material = bulletMat;
                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);

                }
            }
            Modules.Effects.AddEffect(airforceTracer);
            airforce45Tracer = Modules.Assets.airforce45Effect;

            if (!airforce45Tracer.GetComponent<EffectComponent>()) airforce45Tracer.AddComponent<EffectComponent>();
            if (!airforce45Tracer.GetComponent<VFXAttributes>()) airforce45Tracer.AddComponent<VFXAttributes>();
            if (!airforce45Tracer.GetComponent<NetworkIdentity>()) airforce45Tracer.AddComponent<NetworkIdentity>();

            foreach (LineRenderer i in airforce45Tracer.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
                    bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
                    i.material = bulletMat;
                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);

                }
            }
            Modules.Effects.AddEffect(airforce45Tracer);

            blackwhipTracer = Modules.Assets.blackwhip;

            if (!blackwhipTracer.GetComponent<EffectComponent>()) blackwhipTracer.AddComponent<EffectComponent>();
            if (!blackwhipTracer.GetComponent<VFXAttributes>()) blackwhipTracer.AddComponent<VFXAttributes>();
            if (!blackwhipTracer.GetComponent<NetworkIdentity>()) blackwhipTracer.AddComponent<NetworkIdentity>();

            foreach (LineRenderer i in blackwhipTracer.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
                    bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
                    i.material = bulletMat;
                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);

                }
            }
            Modules.Effects.AddEffect(blackwhipTracer);

            delawareTracer = Modules.Assets.delawareEffect;
            Modules.Effects.AddEffect(delawareTracer);
            detroitTracer = Modules.Assets.detroitEffect;
            Modules.Effects.AddEffect(detroitTracer);
            detroitweakTracer = Modules.Assets.detroitweakEffect;
            Modules.Effects.AddEffect(detroitweakTracer);

        }



        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }

        private static void CreateBlackwhip()
        {
            blackwhipPrefab = CloneProjectilePrefab("magefirebolt", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = blackwhipPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 8f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 6f;
            //bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = false;
            bombImpactExplosion.lifetimeAfterImpact = 0f;
            bombImpactExplosion.destroyOnWorld = true;

            ProjectileController bombController = blackwhipPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("blackwhipshootGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("blackwhipshootGhost");
            bombController.startSound = "";
        }

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
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeExpiredSoundString = "";
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}