using RoR2;
using UnityEngine;
using EntityStates;

namespace DekuMod.SkillStates
{

    public class Airforce45 : BaseSkillState
    {

        public static float procCoefficient = 0.25f;
        public float baseDuration = 0.7f; // the base skill duration. i.e. attack speed
        public static int bulletCount = 3;
        public static float bulletSpread = 1f;
        public static float bulletRecoil = 1f;
        public static float bulletRange = 100;
        public static float bulletwidth = 0.7f;


        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerHuntressSnipe");

        public static bool levelHasChanged;
        private float originalBulletwidth = 0.7f;

        protected float duration;
        protected float fireDuration;
        protected float attackStopDuration;
        protected bool hasFired;
        private Animator animator;
        protected string muzzleString;
        private Quaternion baserotation;

        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(5f);
            animator = GetModelAnimator();
            muzzleString = "LFinger";
            hasFired = false;
            duration = baseDuration / attackSpeedStat;

            base.PlayCrossfade("LeftArm, Override", "FingerFlick", "Attack.playbackRate", this.duration, 0.2f);

            fireDuration = 0.1f * duration;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public virtual void FireBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                AkSoundEngine.PostEvent(1063047365, this.gameObject);

                bool isCrit = RollCrit();


                float recoilAmplitude = bulletRecoil / this.attackSpeedStat;

                base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
                characterBody.AddSpreadBloom(4f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, false);

                if (isAuthority)
                {
                    float damage = Modules.StaticValues.airforce45DamageCoefficient * damageStat;

                    GameObject tracerEffect = Modules.Projectiles.airforce45Tracer;

                    if (levelHasChanged)
                    {
                        levelHasChanged = false;

                        enlargeTracer(ref tracerEffect);
                    }


                    Ray aimRay = GetAimRay();

                    float spread = bulletSpread;
                    float width = bulletwidth;
                    float force = 100;
                    baserotation = Quaternion.LookRotation(aimRay.direction);

                    EffectManager.SpawnEffect(Modules.Projectiles.airforce45Tracer, new EffectData
                    {
                        origin = FindModelChild(muzzleString).position,
                        scale = 1f,
                        rotation = baserotation

                    }, false);
    
                    BulletAttack bulletAttack = new BulletAttack
                    {

                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.Buckshot,
                        maxDistance = bulletRange,
                        force = force,// RiotShotgun.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        isCrit = isCrit,
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = width,
                        sniper = false,
                        stopperMask = LayerIndex.world.collisionMask,
                        weapon = null,
                        //tracerEffectPrefab = tracerEffect,
                        spreadPitchScale = 1f,
                        spreadYawScale = 1f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
                        HitEffectNormal = false
                    };


                    bulletAttack.minSpread = 0;
                    bulletAttack.maxSpread = 0;
                    bulletAttack.bulletCount = 1;
                    bulletAttack.Fire();
                    //bulletAttack.aimVector = aimRay.direction;
                    //tracerEffectPrefab = Modules.Projectiles.airforce45Tracer;
                    //bulletAttack.aimVector = aimRay.direction + 1f * Vector3.right;
                    //tracerEffectPrefab = Modules.Projectiles.airforce45Tracer;
                    //bulletAttack.Fire();

                    //bulletAttack.aimVector = aimRay.direction + 2f * Vector3.left;
                    //tracerEffectPrefab = Modules.Projectiles.airforce45Tracer;
                    //bulletAttack.Fire();
                    //bulletAttack.aimVector = aimRay.direction + 3f * Vector3.right;
                    //tracerEffectPrefab = Modules.Projectiles.airforce45Tracer;
                    //bulletAttack.Fire();

                    uint secondShot = (uint)Mathf.CeilToInt(bulletCount / 2f) - 1;
                    bulletAttack.minSpread = 0;
                    bulletAttack.maxSpread = spread / 1.45f;
                    bulletAttack.bulletCount = secondShot;
                    bulletAttack.Fire();

                    bulletAttack.minSpread = spread / 1.45f;
                    bulletAttack.maxSpread = spread;
                    bulletAttack.bulletCount = (uint)Mathf.FloorToInt(bulletCount / 2f);
                    bulletAttack.Fire();
                }
            }
        }

        private void enlargeTracer(ref GameObject tracerEffect)
        {

            // getcomponents in foreach forgive my insolence
            foreach (LineRenderer i in tracerEffect.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {

                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);
                    float addedBulletwidth = bulletwidth - originalBulletwidth;
                    i.widthMultiplier = (1 + addedBulletwidth) * 0.5f;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();



            //if (isAuthority)
            //{

            //    shieldComponent.beefStop = false;
            //    if (fixedAge > fireDuration && fixedAge < attackStopDuration + fireDuration)
            //    {
            //        if (characterMotor)
            //        {
            //            //animator.speed = 0;
            //            characterMotor.moveDirection = Vector3.zero;
            //            //characterBody.moveSpeed = 0;
            //            //animator.SetFloat(AnimationParameters.walkSpeed, 0);
            //            shieldComponent.beefStop = true;
            //        }
            //    }
            //}

            if (fixedAge >= fireDuration)
            {
                FireBullet();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}