using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.Modules;
using DekuMod.SkillStates.BaseStates;
using EntityStates.Merc;

namespace DekuMod.SkillStates.ShootStyle
{
    public class BlastDash : BaseDekuSkillState
    {
        private string muzzleName = "LFinger";

        public Animator anim;
        public bool consumeStock = false;

        private float duration;
        private float rollSpeed;
        private float initialSpeedCoefficient;
        private float finalSpeedCoefficient;

        private Vector3 dashDirection;
        private BlastAttack blastAttack;
        private float blastRadius;
        private float blastDamage;
        private Vector3 blastPosition;
        private float blastForce;
        private DamageType blastType = DamageType.Generic;

        public override void OnEnter()
        {
            base.OnEnter();
            anim = base.GetModelAnimator();
            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            PlayCrossfade("FullBody, Override", "DelawareSmashBaseCharge", "Attack.playbackRate", 1f, 0.01f);

            blastPosition = characterBody.corePosition;
            blastDamage = StaticValues.blastDashDamageCoefficient * damageStat;
            blastForce = StaticValues.blastDashForce;
            blastRadius = StaticValues.blastDashRadius;
            duration = StaticValues.blastDashDuration / attackSpeedStat;
            blastType = DamageType.Generic;
            dashDirection = base.GetAimRay().direction;

            switch (level)
            {
                case 0:
                    initialSpeedCoefficient = StaticValues.blastDashSpeed;
                    finalSpeedCoefficient = 0f;
                    EffectManager.SpawnEffect(Modules.Asset.delawareEffect, new EffectData
                    {
                        origin = FindModelChild(muzzleName).position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(-base.GetAimRay().direction)

                    }, true);
                    break;
                case 1:
                    initialSpeedCoefficient = StaticValues.blastDashSpeed;
                    finalSpeedCoefficient = 0f;
                    EffectManager.SpawnEffect(Modules.Asset.delawareEffect, new EffectData
                    {
                        origin = FindModelChild(muzzleName).position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(-base.GetAimRay().direction)

                    }, true);
                    break;
                case 2:
                    initialSpeedCoefficient = StaticValues.blastDashSpeed;
                    finalSpeedCoefficient = 0f;
                    EffectManager.SpawnEffect(Modules.Asset.delawareEffect, new EffectData
                    {
                        origin = FindModelChild(muzzleName).position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(-base.GetAimRay().direction)

                    }, true);
                    break;
            }

            //blast attack
            blastAttack = new BlastAttack();
            blastAttack.procCoefficient = 1f;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 1000f;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = blastType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.radius = blastRadius;
            blastAttack.baseDamage = blastDamage;
            blastAttack.position = blastPosition;
            blastAttack.baseForce = blastForce;

            blastAttack.Fire();
            //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", 1f);

        }

        public override void OnExit()
        {
            base.OnExit();
            anim.SetBool("delawareCharged", true);
        }

        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, base.fixedAge / duration);
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(characterBody.characterDirection.forward);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //PlayCrossfade("FullBody, Override", "DelawareSmashBaseCharge", "Attack.playbackRate", 1f, 0.01f);
            //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", duration);

            if(fixedAge <= duration)
            {
                this.RecalculateRollSpeed();
                this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));



                Vector3 normalized = dashDirection.normalized;
                if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                {
                    Vector3 vector = normalized * this.rollSpeed * attackSpeedStat;
                    
                    base.characterMotor.velocity = vector;
                }
            }

            switch (level)
            {
                case 0:

                    if (fixedAge <= duration)
                    {
                    }
                    break;
                case 1:

                    if (fixedAge <= duration)
                    {

                    }
                    break;
                case 2:

                    if (fixedAge <= duration)
                    {

                    }
                break;
            }
            if(fixedAge > duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}