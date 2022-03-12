using DekuMod.Modules.Survivors;
using EntityStates;
using EntityStates.Huntress;
using EntityStates.VagrantMonster;
using RoR2;
using RoR2.Audio;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class Oklahoma45 : BaseSkillState
    {

        public float spinage;
        public float baseduration = 1f;
        public float duration;
        public float speedattack;
        public static float blastRadius = 3f;
        public float force = 4000f;

        protected Animator animator;
        private GameObject areaIndicator;
        private RaycastHit raycastHit;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");

        public static float healthCostFraction;
        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;

        private BlastAttack blastAttack;

        public override void OnEnter()
        {
            base.OnEnter();

            dekucon = base.GetComponent<DekuController>();
            if (dekucon.isMaxPower)
            {
                fajin = 2f;
            }
            else
            {
                fajin = 1f;
            }

            duration = baseduration / (this.attackSpeedStat * fajin);

            blastAttack = new BlastAttack();
            blastAttack.radius = Oklahoma45.blastRadius * fajin;
            blastAttack.procCoefficient = 0.25f;
            blastAttack.position = base.characterBody.corePosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.oklahoma45DamageCoefficient * fajin;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 3000f;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.SlowOnHit;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            //base.PlayCrossfade("RightArm, Override", "SmashCharge", 0.2f);
            base.PlayAnimation("Fullbody, Override", "Oklahoma", "Attack.playbackRate", duration);
            //Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);
            AkSoundEngine.PostEvent(3940341776, this.gameObject);

            //if (NetworkServer.active && base.healthComponent)
            //{
            //    DamageInfo damageInfo = new DamageInfo();
            //    damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.1f;
            //    damageInfo.position = base.characterBody.corePosition;
            //    damageInfo.force = Vector3.zero;
            //    damageInfo.damageColorIndex = DamageColorIndex.Default;
            //    damageInfo.crit = false;
            //    damageInfo.attacker = null;
            //    damageInfo.inflictor = null;
            //    damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
            //    damageInfo.procCoefficient = 0f;
            //    damageInfo.procChainMask = default(ProcChainMask);
            //    base.healthComponent.TakeDamage(damageInfo);
            //}

            base.characterMotor.walkSpeedPenaltyCoefficient = 0.2f;
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddBuff(Modules.Buffs.oklahomaBuff);
                
            }
            dekucon.OKLAHOMA.Play();

        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
        public override void OnExit()
        {
            dekucon.OKLAHOMA.Stop();
            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
            if (NetworkServer.active)
            {
                base.characterBody.RemoveBuff(Modules.Buffs.oklahomaBuff);
            }
            base.OnExit();
        }
        
        public override void FixedUpdate()
        {


            base.FixedUpdate();
            bool flag = base.IsKeyDownAuthority();
            if (flag)
            {
                Ray aimRay = base.GetAimRay();

                if (base.isAuthority && spinage >= this.duration/4)
                {
                    blastAttack.position = base.characterBody.corePosition;
                    //hasFired = true;
                    if (dekucon.isMaxPower)
                    {

                        blastAttack.damageType = DamageType.BypassArmor | DamageType.Stun1s;
                    }
                    else
                    {
                        blastAttack.damageType = DamageType.Generic;
                    }
                    spinage = 0f;
                    blastAttack.Fire();

                    //base.PlayAnimation("Fullbody, Override", "Oklahoma", "Attack.playbackRate", duration/4);
                    base.PlayCrossfade("Fullbody, Override", "Oklahoma", duration / 4);

                }
                else this.spinage += Time.fixedDeltaTime;

            }     
                         
            else
            {
                bool isAuthority = base.isAuthority;
                if (isAuthority)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }
    }
}
