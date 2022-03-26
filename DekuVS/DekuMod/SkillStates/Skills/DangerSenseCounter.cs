using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    class DangerSenseCounter: BaseSkillState
    {
        public static float procCoefficient = 1f;
        public float duration = 1f;
        private float stopwatch;
        private float fireTime;
        private BlastAttack blastAttack;
        public float blastRadius = 7f;
        public static float force = 300f;
        public DekuController dekucon;
        protected DamageType damageType;
        private Vector3 randRelPos;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");

        public float initialspeedCoefficient = 6f;

        
        public Vector3 enemyPosition;
        public float rollSpeed;

        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            AkSoundEngine.PostEvent(2548270042, this.gameObject);
            this.fireTime = duration / (4 * attackSpeedStat);


            //characterBody.RemoveBuff(Modules.Buffs.counterBuff.buffIndex);

            dekucon = base.GetComponent<DekuController>();
            base.skillLocator.primary.AddOneStock();

            dekucon.countershouldflip = false;
            dekucon.DANGERSENSE.Stop();
            if (dekucon.isMaxPower)
            {
                damageType = DamageType.Freeze2s | DamageType.BypassArmor;
            }
            else
            {
                damageType = DamageType.Stun1s;
                if (dekucon.dangersensefreeze)
                {
                    damageType = DamageType.Freeze2s;
                    dekucon.dangersensefreeze = false;
                }
                else
                {
                    damageType = DamageType.Stun1s;
                }
            }


            //blastAttack = new BlastAttack();
            //blastAttack.radius = blastRadius;
            //blastAttack.procCoefficient = procCoefficient;
            //blastAttack.position = base.characterBody.corePosition;
            //blastAttack.attacker = base.gameObject;
            //blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            //blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.counterDamageCoefficient;
            //blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            //blastAttack.baseForce = force;
            //blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            //blastAttack.damageType = damageType;
            //blastAttack.attackerFiltering = AttackerFiltering.Default;

            //if (base.isAuthority)
            //{
            //    blastAttack.Fire();

            //    for (int i = 0; i <= 5; i++)
            //    {
            //        this.randRelPos = new Vector3((float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f);
            //        float num = 60f;
            //        Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
            //        float num2 = 0.01f;
            //        rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
            //        rotation.y += UnityEngine.Random.Range(-num2, num2) * num;

            //        EffectData effectData = new EffectData
            //        {
            //            scale = 1f,
            //            origin = base.characterBody.corePosition + this.randRelPos,
            //            rotation = rotation

            //        };
            //        EffectManager.SpawnEffect(this.effectPrefab, effectData, true);

            //    }
            //}
            base.PlayAnimation("Fullbody, Override", "ShootStyleFullFlip");

            RecalculateRollSpeed();
            Ray aimray = base.GetAimRay();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity = -(aimray.direction).normalized * this.rollSpeed;
                //base.characterMotor.velocity = (characterBody.transform.position - enemyPosition).normalized * this.rollSpeed;
            }

            //if (base.characterMotor && base.characterDirection)
            //{
            //    Debug.Log("statemove");
            //    //base.characterMotor.rootMotion += -(aimray.direction).normalized * this.rollSpeed;
            //    base.characterMotor.rootMotion += (characterBody.transform.position - enemyPosition).normalized * this.rollSpeed;
            //}
        }
        private void RecalculateRollSpeed()
        {
            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            this.rollSpeed = num * Mathf.Lerp(initialspeedCoefficient, 0, base.fixedAge / fireTime);
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(enemyPosition);
            base.OnSerialize(writer);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            enemyPosition = reader.ReadVector3();
            base.OnDeserialize(reader);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RecalculateRollSpeed(); 

            Ray aimray = base.GetAimRay();
            //base.characterMotor.velocity = Vector3.zero;
            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity = -(aimray.direction).normalized * this.rollSpeed;
                //base.characterMotor.velocity = (characterBody.transform.position - enemyPosition).normalized * this.rollSpeed;
            }
            //base.characterMotor.rootMotion += (characterBody.transform.position - enemyPosition).normalized * this.rollSpeed * Time.fixedDeltaTime;
            //base.characterMotor.rootMotion += -(aimray.direction).normalized * this.rollSpeed * Time.fixedDeltaTime;
            //Increment timer
            stopwatch += Time.fixedDeltaTime;

            //GET OUTTA HERE
            if (stopwatch >= this.fireTime)
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
