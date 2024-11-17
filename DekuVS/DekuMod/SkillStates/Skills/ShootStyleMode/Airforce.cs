using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.SkillStates.Orbs;
using System.Collections.Generic;
using RoR2.Orbs;
using static RoR2.BulletAttack;
using DekuMod.SkillStates.BaseStates;
using DekuMod.Modules;

namespace DekuMod.SkillStates.ShootStyle
{
    public class Airforce : BaseDekuSkillState
    {
        public static float procCoefficient = 0.5f;
        public static float baseDuration = 0.5f;
        public static float force = 300f;
        public static float recoil = 1f;
        public static float range = 200f;

        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/tracerhuntresssnipe");
        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private BulletAttack bulletAttack;
        public uint bulletCount;

        public DamageType damageType = DamageType.Generic;
        public int maxRicochetCount = Modules.StaticValues.airforceMaxRicochet;
        public static bool resetBouncedObjects = true;

        public int punchIndex;
        public int actualshotsFired;
        public int shotsFired = 1;
        public bool hasMoved;
        private BlastAttack blastAttack;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = 0.2f * duration;
            characterBody.SetAimTimer(duration);
            muzzleString = "RFinger";

            hasFired = false;

            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            //base.PlayCrossfade("LeftArm, Override", "FingerFlick","Attack.playbackRate",this.duration, this.fireTime);
            //PlayAnimation("FullBody, Override", "GoBeyond", "Attack.playbackRate", duration);

            switch (level)
            {
                case 0:
                    bulletCount = 1;
                    PlayAnimation("RightArm, Override", "Airforce", "Attack.playbackRate", duration);
                    break;
                case 1:
                    bulletCount = 2;
                    PlayAnimation("RightArm, Override", "Airforce", "Attack.playbackRate", duration);
                    break;
                case 2:
                    bulletCount = 2;

                    //if stand still go into machine gun mode
                    if(base.inputBank.moveVector == Vector3.zero)
                    {
                        hasMoved = false;
                        //if (shotsFired > 20)
                        //{
                        //    shotsFired = 20;
                        //}
                        this.duration = baseDuration / (this.attackSpeedStat * (1 +(float)shotsFired / 10));
                        base.PlayAnimation("FullBody, Override", punchIndex % 2 == 0 ? "DekurapidpunchL" : "DekurapidpunchR", "Attack.playbackRate", this.duration);
                        muzzleString = punchIndex % 2 == 0 ?  "LFinger" :  "RFinger";
                    }
                    else
                    {
                        PlayAnimation("RightArm, Override", "Airforce", "Attack.playbackRate", duration);
                    }


                    break;
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            characterBody.AddSpreadBloom(1f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
            if (isAuthority)
            {
                AkSoundEngine.PostEvent("airforcesfx", gameObject);
            }

            if (isAuthority)
            {
                Ray aimRay = GetAimRay();
                AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);




                bool hasHit = false;
                Vector3 hitPoint = Vector3.zero;
                float hitDistance = 0f;
                HealthComponent hitHealthComponent = null;

                bulletAttack = new BulletAttack();
                bulletAttack.bulletCount = bulletCount;
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.damage = Modules.StaticValues.airforceDamageCoefficient * damageStat;
                bulletAttack.damageColorIndex = DamageColorIndex.Default;
                bulletAttack.damageType = damageType;
                bulletAttack.falloffModel = FalloffModel.DefaultBullet;
                bulletAttack.maxDistance = range;
                bulletAttack.force = force;
                bulletAttack.hitMask = LayerIndex.CommonMasks.bullet;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = 0f;
                bulletAttack.isCrit = RollCrit();
                bulletAttack.owner = gameObject;
                bulletAttack.muzzleName = muzzleString;
                bulletAttack.smartCollision = false;
                bulletAttack.procChainMask = default;
                bulletAttack.procCoefficient = procCoefficient;
                bulletAttack.radius = 0.5f;
                bulletAttack.sniper = false;
                bulletAttack.stopperMask = LayerIndex.CommonMasks.bullet;
                bulletAttack.weapon = null;
                //tracerEffectPrefab = Modules.Projectiles.bulletTracer,
                bulletAttack.spreadPitchScale = 0f;
                bulletAttack.spreadYawScale = 0f;
                bulletAttack.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                bulletAttack.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab;


                if (!hasMoved && level == 3)
                {

                    EffectManager.SpawnEffect(DekuAssets.airforce100Effect, new EffectData
                    {
                        origin = FindModelChild(this.muzzleString).position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(aimRay.direction)

                    }, true);
                    bulletAttack.hitCallback = ApplyBlastAttackOnHit;
                    bulletAttack.Fire();
                }
                else
                {
                    EffectManager.SpawnEffect(DekuAssets.airforceEffect, new EffectData
                    {
                        origin = FindModelChild(this.muzzleString).position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(aimRay.direction)

                    }, true);

                    if (maxRicochetCount > 0 /*&& bulletAttack.isCrit*/)
                    {
                        bulletAttack.hitCallback = delegate (BulletAttack bulletAttackRef, ref BulletHit hitInfo)
                        {
                            var result = defaultHitCallback(bulletAttackRef, ref hitInfo);
                            if (hitInfo.hitHurtBox && (hitInfo.hitHurtBox.healthComponent.body.HasBuff(Buffs.comboDebuff) | bulletAttack.isCrit))
                            {
                                if (!bulletAttack.isCrit)
                                {
                                    if (hitInfo.hitHurtBox.healthComponent.body.HasBuff(Buffs.comboDebuff))
                                    {
                                        int ricochetCount = hitInfo.hitHurtBox.healthComponent.body.GetBuffCount(Buffs.comboDebuff);

                                        maxRicochetCount = ricochetCount;
                                    }
                                }

                                hasHit = true;
                                hitPoint = hitInfo.point;
                                hitDistance = hitInfo.distance;

                                hitHealthComponent = hitInfo.hitHurtBox.healthComponent;
                                //hitHealthComponent.body.AddBuff();                            

                            }
                            return result;
                        };
                    }
                    bulletAttack.filterCallback = delegate (BulletAttack bulletAttackRef, ref BulletHit info)
                    {
                        return (!info.entityObject || info.entityObject != bulletAttack.owner) && defaultFilterCallback(bulletAttackRef, ref info);
                    };
                    bulletAttack.Fire();
                    if (hasHit)
                    {
                        if (hitHealthComponent != null)
                        {
                            CritRicochetOrb critRicochetOrb = new CritRicochetOrb();
                            critRicochetOrb.bouncesRemaining = maxRicochetCount - 1;
                            critRicochetOrb.resetBouncedObjects = resetBouncedObjects;
                            critRicochetOrb.damageValue = bulletAttack.damage;
                            critRicochetOrb.isCrit = RollCrit();
                            critRicochetOrb.teamIndex = TeamComponent.GetObjectTeam(gameObject);
                            critRicochetOrb.damageType = bulletAttack.damageType;
                            critRicochetOrb.attacker = gameObject;
                            critRicochetOrb.attackerBody = characterBody;
                            critRicochetOrb.procCoefficient = bulletAttack.procCoefficient;
                            critRicochetOrb.duration = 0.2f;
                            critRicochetOrb.bouncedObjects = new List<HealthComponent>();
                            critRicochetOrb.range = Mathf.Max(30f, hitDistance);
                            critRicochetOrb.tracerEffectPrefab = tracerEffectPrefab;
                            critRicochetOrb.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab;
                            critRicochetOrb.origin = hitPoint;
                            critRicochetOrb.bouncedObjects.Add(hitHealthComponent);
                            var nextTarget = critRicochetOrb.PickNextTarget(hitPoint);
                            if (nextTarget)
                            {
                                critRicochetOrb.target = nextTarget;
                                OrbManager.instance.AddOrb(critRicochetOrb);
                            }
                        }
                    }
                }

            }

        }

        private bool ApplyBlastAttackOnHit(BulletAttack bulletAttackRef, ref BulletAttack.BulletHit hitInfo)
        {

            var hurtbox = hitInfo.hitHurtBox;
            if (hurtbox)
            {
                var healthComponent = hurtbox.healthComponent;
                if (healthComponent)
                {
                    var body = healthComponent.body;
                    if (body)
                    {
                        Ray aimRay = base.GetAimRay();
                        EffectManager.SpawnEffect(Modules.DekuAssets.airforce100impactEffect, new EffectData
                        {
                            origin = healthComponent.body.corePosition,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(aimRay.direction)

                        }, true);

                        blastAttack = new BlastAttack();
                        blastAttack.radius = 5f;
                        blastAttack.procCoefficient = 0.2f;
                        blastAttack.position = healthComponent.body.corePosition;
                        blastAttack.attacker = base.gameObject;
                        blastAttack.crit = base.RollCrit();
                        blastAttack.baseDamage = Modules.StaticValues.airforce3DamageCoefficient * this.damageStat;
                        blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                        blastAttack.baseForce = force;
                        blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                        blastAttack.damageType = damageType;
                        blastAttack.attackerFiltering = AttackerFiltering.Default;

                        blastAttack.Fire();
                    }
                }
            }
            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime && !hasFired)
            {
                hasFired = true;
                Fire();
            }

            //check if any movement from player
            if(base.inputBank.moveVector != Vector3.zero)
            {
                hasMoved = true;
            }

            if (fixedAge >= duration && isAuthority)
            {
                if (inputBank.skill1.down && level == 3 && !hasMoved)
                {
                    this.SetNextState();
                    return;
                }
                else
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }

        protected void SetNextState()
        {
            int index = this.punchIndex;
            if (index == 0) index = 1;
            else index = 0;
            int actualshotsFired = shotsFired + 1;

            this.outer.SetNextState(new Airforce
            {
                punchIndex = index,
                shotsFired = actualshotsFired,
            });

        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}