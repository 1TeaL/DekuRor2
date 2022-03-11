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
    public class Oklahoma100 : BaseSkillState
    {

        public float spinage;
        public float baseduration = 1f;
        public float duration;
        public float speedattack;
        public static float blastRadius = 3f;

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
            blastAttack.radius = Oklahoma100.blastRadius * fajin;
            blastAttack.procCoefficient = 0.25f;
            blastAttack.position = base.characterBody.corePosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.oklahoma100DamageCoefficient * fajin;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 100f;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.BypassArmor | DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            //base.PlayCrossfade("RightArm, Override", "SmashCharge", 0.2f);
            base.PlayAnimation("RightArm, Override", "SmashCharge", "Attack.playbackRate", duration);
            //Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);
            AkSoundEngine.PostEvent(3842300745, this.gameObject);
            AkSoundEngine.PostEvent(573664262, this.gameObject);

            if (NetworkServer.active && base.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.1f;
                damageInfo.position = base.characterBody.corePosition;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.Default;
                damageInfo.crit = false;
                damageInfo.attacker = null;
                damageInfo.inflictor = null;
                damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
                damageInfo.procCoefficient = 0f;
                damageInfo.procChainMask = default(ProcChainMask);
                base.healthComponent.TakeDamage(damageInfo);
            }

            base.characterMotor.walkSpeedPenaltyCoefficient = 0.5f;
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddBuff(Modules.Buffs.oklahomaBuff);
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
        public override void OnExit()
        {
            dekucon.RemoveBuffCount(50);
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
 
                    spinage = 0f;
                    blastAttack.Fire();
                    
                    EffectManager.SpawnEffect(Modules.Assets.blackwhip, new EffectData
                    {
                        origin = base.characterBody.corePosition,
                        scale = 1f,

                    }, true);
                 
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
