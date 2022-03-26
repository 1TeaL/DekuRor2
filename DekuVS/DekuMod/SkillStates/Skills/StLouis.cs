using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Survivors;

namespace DekuMod.SkillStates
{
    public class StLouis : BaseSkillState
    {
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        public float baseDuration = 1f;
        public static float blastRadius = 10f;
        public static float succForce = 4.5f;
        //private GameObject effectPrefab = Modules.Assets.sEffect;

        public float range = 5f;
        public float rangeaddition = 8f;
        public float force = 2000f;
        private float duration;
        private float maxWeight;
        private BlastAttack blastAttack;
        //private bool hasFired;
        public Vector3 theSpot;
        public float whipage;
        public float speedattack;

        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;
            speedattack = attackSpeedStat / 2;
            if (speedattack < 1)
            {
                speedattack = 1;
            }
            dekucon = base.GetComponent<DekuController>();
            dekucon.AddToBuffCount(10);
            if (dekucon.isMaxPower)
            {
                dekucon.RemoveBuffCount(50);
                fajin = 2f;
            }
            else
            {
                fajin = 1f;
            }
            if (dekucon.isMaxPower)
            {
                EffectManager.SpawnEffect(Modules.Assets.impactEffect, new EffectData
                {
                    origin = base.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(aimRay.direction)
                }, false);
            }


            //hasFired = false;
            theSpot = aimRay.origin + range * aimRay.direction;
            AkSoundEngine.PostEvent(3709822086, this.gameObject);
            AkSoundEngine.PostEvent(3062535197, this.gameObject);
            base.StartAimMode(duration, true);

            base.characterMotor.disableAirControlUntilCollision = false;

            //base.PlayCrossfade("Fullbody, Override", "LegSmash", startUp);
            //base.PlayAnimation("Fullbody, Override" "LegSmash", "Attack.playbackRate", startUp);
            base.PlayCrossfade("Fullbody, Override", "LegSmash", duration / 2);

            //EffectManager.SpawnEffect(Modules.Assets.blackwhip, new EffectData
            //{
            //    origin = theSpot,
            //    scale = 1f,       

            //}, true);


            blastAttack = new BlastAttack();
            blastAttack.radius = blastRadius * speedattack * fajin;
            blastAttack.procCoefficient = 0.2f;
            blastAttack.position = theSpot;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.stlouisDamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = force;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = damageType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;


                //EffectData effectData = new EffectData();
                //effectData.origin = theSpot2;
                //effectData.scale = (blastRadius / 5) * this.attackSpeedStat;
                //effectData.rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z));

                //EffectManager.SpawnEffect(this.effectPrefab, effectData, false);

        }

        protected virtual void OnHitEnemyAuthority()
        {
            base.healthComponent.Heal(((healthComponent.fullCombinedHealth / 20) * speedattack * fajin), default(ProcChainMask), true);

        }


        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Ray aimRay = base.GetAimRay();
            theSpot = aimRay.origin + range * aimRay.direction;

            if ((base.fixedAge >= this.duration / 5 * fajin) && base.isAuthority && whipage >= this.duration/5 * fajin)
            {
                //hasFired = true;
                if (dekucon.isMaxPower)
                {

                    blastAttack.damageType = DamageType.BypassArmor | DamageType.Stun1s;
                }
                else
                {
                    blastAttack.damageType = DamageType.Generic;
                }
                blastAttack.position = theSpot;
                range += rangeaddition;
                whipage = 0f;
                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();
                }
                EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
                {
                    origin = theSpot,
                    scale = blastRadius * speedattack * fajin,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

                }, true);
                EffectManager.SpawnEffect(effectPrefab, new EffectData
                {
                    origin = theSpot,
                    scale = blastRadius * speedattack * fajin,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

                }, true);
                //for (int i = 0; i <= 5; i++)
                //{
                //    float num = 60f;
                //    Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
                //    float num2 = 0.01f;
                //    rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                //    rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
                //    EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
                //    {
                //        origin = base.transform.position,
                //        scale = blastRadius,
                //        rotation = rotation
                //    }, false);

                //}

            }
            else this.whipage += Time.fixedDeltaTime;


            if ((base.fixedAge >= this.duration && base.isAuthority))
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
