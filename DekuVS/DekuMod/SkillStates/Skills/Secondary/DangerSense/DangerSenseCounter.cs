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
        public float duration = 0.25f;
        private float stopwatch;
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
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            AkSoundEngine.PostEvent(2548270042, this.gameObject);


            //characterBody.RemoveBuff(Modules.Buffs.dangersenseBuff.buffIndex);

            dekucon = base.GetComponent<DekuController>();

            this.animator = base.GetModelAnimator();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayAnimation("Body", "ShootStyleFlip", "Attack.playbackRate", duration);

            RecalculateRollSpeed();
            Ray aimray = base.GetAimRay();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity = -(aimray.direction).normalized * this.rollSpeed;
                //base.characterMotor.velocity = (characterBody.transform.position - enemyPosition).normalized * this.rollSpeed;
            }

        }
        private void RecalculateRollSpeed()
        {
            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            this.rollSpeed = num * Mathf.Lerp(initialspeedCoefficient, 0, base.fixedAge / duration);
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

            //GET OUTTA HERE
            if (base.fixedAge >= this.duration)
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
