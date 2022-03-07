//using EntityStates;
//using EntityStates.Mage.Weapon;
//using RoR2;
//using RoR2.Orbs;
//using System.Collections.Generic;
//using DekuMod.SkillStates.Orbs;
//using UnityEngine;
//using UnityEngine.Networking;

//namespace DekuMod.SkillStates
//{
//    public class BlackwhipRicochet : BaseSkillState
//    {
//        public static int maxRicochetCount = 20;
//        public static bool resetBouncedObjects = true;
//        public static float damageCoefficient = 2.0f;
//        public static float procCoefficient = 1f;
//        public static float baseDuration = 0.6f;
//        public static float force = 800f;
//        public static float recoil = 3f;
//        public static float range = 256f;
//        // public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

//        private float duration;
//        private string muzzleString;
//        private Ray initialAimRay;

//        public static GameObject tracerEffectPrefab = Modules.Projectiles.blackwhipTracer;

//        public override void OnEnter()
//        {
//            base.OnEnter();
//            this.duration = BlackwhipRicochet.baseDuration / this.attackSpeedStat;
//            base.characterBody.SetAimTimer(2f);
//            this.muzzleString = "Chest";
//            base.PlayAnimation("Gesture, Override", "ShootRifle", "ShootRifle.playbackRate", 2f * this.duration);
//            base.characterBody.AddSpreadBloom(1.5f);
//            //EffectManager.SimpleMuzzleFlash(Modules.Assets.yokoRifleMuzzleSmallEffect, base.gameObject, this.muzzleString, false);
//            Util.PlaySound("TTGLTokoRifleFire", base.gameObject);
//            base.AddRecoil(-1f * BlackwhipRicochet.recoil, -2f * BlackwhipRicochet.recoil, -0.5f * BlackwhipRicochet.recoil, 0.5f * BlackwhipRicochet.recoil);
//            if (base.isAuthority)
//            {
//                this.initialAimRay = base.GetAimRay();
//            }
//            if (NetworkServer.active)
//            {
//                this.FireServer(this.initialAimRay);
//            }
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();
//            if (base.fixedAge >= this.duration && base.isAuthority)
//            {
//                this.outer.SetNextStateToMain();
//                return;
//            }
//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//        }

//        private void FireServer(Ray aimRay)
//        {
//            Vector3 hitPoint = Vector3.zero;
//            float hitDistance = 0f;
//            HealthComponent hitHealthComponent = null;
//            var bulletAttack = new BulletAttack
//            {
//                bulletCount = 1,
//                aimVector = aimRay.direction,
//                origin = aimRay.origin,
//                damage = BlackwhipRicochet.damageCoefficient * this.damageStat,
//                damageColorIndex = DamageColorIndex.Default,
//                damageType = DamageType.Stun1s | DamageType.BypassArmor,
//                falloffModel = BulletAttack.FalloffModel.DefaultBullet,
//                maxDistance = BlackwhipRicochet.range,
//                force = BlackwhipRicochet.force,
//                hitMask = LayerIndex.CommonMasks.bullet,
//                minSpread = 0f,
//                maxSpread = 0f,
//                isCrit = base.RollCrit(),
//                owner = base.gameObject,
//                muzzleName = muzzleString,
//                smartCollision = false,
//                procChainMask = default(ProcChainMask),
//                procCoefficient = procCoefficient,
//                radius = 0.75f,
//                sniper = false,
//                stopperMask = LayerIndex.CommonMasks.bullet,
//                weapon = null,
//                tracerEffectPrefab = tracerEffectPrefab,
//                spreadPitchScale = 0f,
//                spreadYawScale = 0f,
//                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
//                hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
//            };
//            if (maxRicochetCount > 0)
//            {
//                bulletAttack.hitCallback = delegate (ref BulletAttack.BulletHit hitInfo)
//                {
//                    var result = bulletAttack.DefaultHitCallback(ref hitInfo);
//                    if (hitInfo.hitHurtBox)
//                    {
//                        hitPoint = hitInfo.point;
//                        hitDistance = hitInfo.distance;
//                        hitHealthComponent = hitInfo.hitHurtBox.healthComponent;
//                    }
//                    return result;
//                };
//            }
//            bulletAttack.filterCallback = delegate (ref BulletAttack.BulletHit info)
//            {
//                return (!info.entityObject || info.entityObject != bulletAttack.owner) && bulletAttack.DefaultFilterCallback(ref info);
//            };
//            bulletAttack.Fire();
//            if (hitHealthComponent != null)
//            {
//                CritRicochetOrb critRicochetOrb = new CritRicochetOrb();
//                critRicochetOrb.bouncesRemaining = maxRicochetCount - 1;
//                critRicochetOrb.resetBouncedObjects = resetBouncedObjects;
//                critRicochetOrb.damageValue = bulletAttack.damage;
//                critRicochetOrb.isCrit = base.RollCrit();
//                critRicochetOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
//                critRicochetOrb.attacker = base.gameObject;
//                critRicochetOrb.attackerBody = base.characterBody;
//                critRicochetOrb.procCoefficient = bulletAttack.procCoefficient;
//                critRicochetOrb.duration = 0.1f;
//                critRicochetOrb.bouncedObjects = new List<HealthComponent>();
//                critRicochetOrb.range = Mathf.Max(30f, hitDistance);
//                critRicochetOrb.tracerEffectPrefab = tracerEffectPrefab;
//                critRicochetOrb.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab;
//                //critRicochetOrb.hitSoundString = "TTGLTokoRifleCrit";
//                critRicochetOrb.origin = hitPoint;
//                critRicochetOrb.bouncedObjects.Add(hitHealthComponent);
//                var nextTarget = critRicochetOrb.PickNextTarget(hitPoint);
//                if (nextTarget)
//                {
//                    critRicochetOrb.target = nextTarget;
//                    OrbManager.instance.AddOrb(critRicochetOrb);
//                }
//            }
//        }

//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.PrioritySkill;
//        }

//        public override void OnSerialize(NetworkWriter writer)
//        {
//            base.OnSerialize(writer);
//            Vector3 origin = this.initialAimRay.origin;
//            Vector3 direction = this.initialAimRay.direction;
//            writer.Write(origin);
//            writer.Write(direction);
//        }

//        public override void OnDeserialize(NetworkReader reader)
//        {
//            base.OnDeserialize(reader);
//            Vector3 origin = reader.ReadVector3();
//            Vector3 direction = reader.ReadVector3();
//            this.initialAimRay = new Ray(origin, direction);
//        }
//    }
//}