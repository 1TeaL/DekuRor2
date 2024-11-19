using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using DekuMod.Modules.Survivors;
using RoR2.UI;
using DekuMod.Modules;
using static UnityEngine.ParticleSystem.PlaybackState;
using DekuMod.SkillStates.BaseStates;
using System.Linq;

namespace DekuMod.SkillStates.Might
{
    public class DelawareSmash : BaseDekuSkillState
    {
        private GameObject effectPrefab = DekuAssets.muzzleflashMageLightningLargePrefab;
        private string muzzleName = "RFinger";

        public bool hasFired;
        public Animator anim;
        //public bool consumeStock = false;

        private float recoilAmplitude = 4f;
        private float force;
        private float radius;
        private float damage;
        private float range;
        private DamageType damageType;
        private float chargePercent;
        private float maxCharge;
        private float baseMaxCharge = StaticValues.delawareMaxCharge;

        public override void OnEnter()
        {
            base.OnEnter();
            anim = base.GetModelAnimator();
            anim.SetBool("delawareCharged", false);
            hasFired = false;

            Ray aimRay = GetAimRay();
            characterBody.SetAimTimer(1f);
            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);


            float[] source = new float[]
            {
                this.attackSpeedStat,
                4f
            };
            this.maxCharge = baseMaxCharge / source.Min();

            //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", 1f);

        }
        public override void Level1()
        {
            damage = StaticValues.delawareDamageCoefficient;
            force = damage;
            radius = StaticValues.delawareRadius;
            damageType = DamageType.Generic;
            PlayCrossfade("FullBody, Override", "DelawareWeakCharge", "Attack.playbackRate", 0.5f, 0.01f);
        }
        public override void Level2()
        {
            damage = StaticValues.delawareDamageCoefficient * StaticValues.delaware2DamageMultiplier;
            radius = StaticValues.delawareRadius;
            damageType = DamageType.Generic;
            PlayCrossfade("FullBody, Override", "DelawareWeakCharge", "Attack.playbackRate", 0.5f, 0.01f);
        }
        public override void Level3()
        {
            damage = StaticValues.delawareDamageCoefficient * StaticValues.delaware3DamageMultiplier;
            radius = StaticValues.delaware3Radius;
            damageType = DamageType.Stun1s;
            PlayCrossfade("FullBody, Override", "DelawareSmashCharge", "Attack.playbackRate", 0.5f, 0.01f);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (dekucon.RARM.isPlaying)
            {
                dekucon.RARM.Stop();
            }
            base.PlayCrossfade("FullBody, Override", "BufferEmpty", 0.1f);
            base.PlayCrossfade("UpperBody, Override", "BufferEmpty", 0.1f);
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsKeyDownAuthority() || inputBank.skill2.down)
            {

                if (base.fixedAge <= this.maxCharge)
                {
                    this.chargePercent = base.fixedAge / this.maxCharge;


                    //base.characterMotor.walkSpeedPenaltyCoefficient = 1f - this.chargePercent / maxCharge;
                }
                else if (base.fixedAge > this.maxCharge)
                {
                    if (chargePercent > 1f)
                    {
                        chargePercent = 1f;
                    }
                    if (dekucon.RARM.isStopped)
                    {
                        dekucon.RARM.Play();
                    }

                }
            }
            else
            {
                if(!IsKeyDownAuthority() && !inputBank.skill2.down)
                {
                    if(!hasFired)
                    {
                        anim.SetBool("delawareCharged", true);
                        hasFired = true;

                        float angle = Vector3.Angle(new Vector3(0, -1, 0), base.GetAimRay().direction);
                        switch (level)
                        {
                            case 0:
                                damage += (this.chargePercent * Modules.StaticValues.delawareDamageMultiplier);
                                
                                Fire();
                                if (angle < 60)
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareWeakEndUp");
                                }
                                else if (angle > 120)
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareWeakEndDown");
                                }
                                else
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareWeakEnd");
                                }
                                break;
                            case 1:
                                damage += (this.chargePercent * Modules.StaticValues.delaware2DamageMultiplier);
                                
                                Fire();
                                if (angle < 60)
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareWeakEndUp");
                                }
                                else if (angle > 120)
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareWeakEndDown");
                                }
                                else
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareWeakEnd");
                                }
                                break;
                            case 2:
                                damage += (this.chargePercent * Modules.StaticValues.delaware3DamageMultiplier);
                                
                                Fire();
                                if (angle < 60)
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareSmashChargeEndUp");
                                }
                                else if (angle > 120)
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareSmashChargeEndDown");
                                }
                                else
                                {
                                    base.PlayAnimation("FullBody, Override", "DelawareSmashChargeEnd");
                                }
                                break;
                        }
                        

                        if (isAuthority && Config.allowVoice.Value)
                        {
                            AkSoundEngine.PostEvent("delawarevoice", gameObject);
                        }

                        AkSoundEngine.PostEvent("delawaresfx", gameObject);

                        if(characterMotor.isGrounded)
                        {
                            base.characterMotor.velocity = StaticValues.delawareDistance * (-base.GetAimRay().direction) * moveSpeedStat;

                        }
                        else 
                        if (!characterMotor.isGrounded)
                        {
                            base.characterMotor.velocity = StaticValues.delawareDistance * (-base.GetAimRay().direction) * moveSpeedStat * 0.5f;

                        }

                        this.outer.SetNextStateToMain();
                        return;
                    }

                }
            }
            if (characterBody)
            {
                characterBody.SetAimTimer(1f);
            }
        }

        public void Fire()
        {
            //AddRecoil(-3f * recoilAmplitude, -4f * recoilAmplitude, -0.5f * recoilAmplitude, 0.5f * recoilAmplitude);

            BlastAttack blastAttack = new BlastAttack();

            blastAttack.position = base.characterBody.corePosition;
            blastAttack.baseDamage = this.damageStat * this.damage;
            blastAttack.baseForce = force * this.damage;
            blastAttack.radius = this.radius;
            blastAttack.attacker = base.gameObject;
            blastAttack.inflictor = base.gameObject;
            blastAttack.teamIndex = base.teamComponent.teamIndex;
            blastAttack.crit = base.RollCrit();
            blastAttack.procChainMask = default(ProcChainMask);
            blastAttack.procCoefficient = 1f;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.damageColorIndex = DamageColorIndex.Default;
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            blastAttack.Fire();

            if (effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(effectPrefab, gameObject, muzzleName, false);
            }

            if (isAuthority)
            {


                switch (level)
                {
                    case 0:
                        EffectManager.SpawnEffect(Modules.DekuAssets.delawareBullet, new EffectData
                        {
                            origin = characterBody.footPosition,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                        }, true);
                        break;
                    case 1:
                        EffectManager.SpawnEffect(Modules.DekuAssets.delawareBullet, new EffectData
                        {
                            //origin = FindModelChild(muzzleName).position,
                            origin = characterBody.footPosition,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                        }, true);
                        break;
                    case 2:

                        EffectManager.SpawnEffect(Modules.DekuAssets.delawareEffect, new EffectData
                        {
                            origin = characterBody.footPosition,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                        }, true);
                        break;
                }

            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}