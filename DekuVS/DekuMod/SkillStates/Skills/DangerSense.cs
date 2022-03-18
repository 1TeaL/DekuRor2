using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.SkillStates.Orbs;
using System.Collections.Generic;
using RoR2.Orbs;
using static RoR2.BulletAttack;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class DangerSense : BaseSkillState
    {

        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        public static float force = 300f;
        public static float recoil = 1f;


        private GameObject smokeprefab = Modules.Assets.smokeEffect;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/tracerhuntresssnipe");
        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;

        public bool counteron;
        private BlastAttack blastAttack;
        public float blastRadius = 7f;


        private Vector3 randRelPos;
        private int randFreq;
        private bool reducerFlipFlop;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");

        public DangerSenseComponent dangercon;

        public override void OnEnter()
        {
            base.OnEnter();
            dekucon = base.GetComponent<DekuController>();
            this.duration = baseDuration;
            this.fireTime = duration / (4f * attackSpeedStat * fajin);
            if(this.fireTime < 0.1f)
            {
                fireTime = 0.1f;
            }
            //base.characterBody.SetAimTimer(duration);
            //this.muzzleString = "LFinger";

            counteron = false;
            dekucon.countershouldflip = false;
            hasFired = false;


            if (dekucon.isMaxPower)
            {
                dekucon.RemoveBuffCount(50);
                fajin = 2f;
            }
            else
            {
                fajin = 1f;
            }
            dekucon.AddToBuffCount(10);

            blastAttack = new BlastAttack();
            blastAttack.radius = blastRadius * fajin;
            blastAttack.procCoefficient = procCoefficient;
            blastAttack.position = base.characterBody.corePosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.counterDamageCoefficient * fajin;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = force;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            //base.PlayCrossfade("LeftArm, Override", "FingerFlick", "Attack.playbackRate", this.duration, this.fireTime);
            //base.PlayCrossfade("Fullbody, Override", "CounterStart", "Attack.playbackRate", this.duration, this.fireTime);
            //base.PlayCrossfade("Gesture, Override", "CounterStart", "Attack.playbackRate", this.duration / (2 * attackSpeedStat * fajin), this.duration / (4 * attackSpeedStat * fajin));
            
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self.body.HasBuff(Modules.Buffs.counterBuff.buffIndex))
            {
                damageInfo.damage = 0f;
                self.body.RemoveBuff(Modules.Buffs.counterBuff.buffIndex);

                var damageInfo2 = new DamageInfo();

                damageInfo2.damage = self.body.damage * Modules.StaticValues.counterDamageCoefficient;
                damageInfo2.position = damageInfo.attacker.transform.position;
                damageInfo2.force = Vector3.zero;
                damageInfo2.damageColorIndex = DamageColorIndex.Default;
                damageInfo2.crit = Util.CheckRoll(self.body.crit, self.body.master);
                damageInfo2.attacker = self.gameObject;
                damageInfo2.inflictor = null;
                damageInfo2.damageType = DamageType.BypassArmor | DamageType.Stun1s;
                damageInfo2.procCoefficient = 2f;
                damageInfo2.procChainMask = default(ProcChainMask);

                if (damageInfo.attacker.gameObject.GetComponent<CharacterBody>().baseNameToken
                    != DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && damageInfo.attacker != null)
                {
                    damageInfo.attacker.GetComponent<CharacterBody>().healthComponent.TakeDamage(damageInfo2);
                }

                Vector3 enemyPos = damageInfo.attacker.transform.position;
                EffectManager.SpawnEffect(Modules.Projectiles.airforceTracer, new EffectData
                {
                    origin = self.body.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(enemyPos - self.body.transform.position)

                }, true);


                DangerSenseCounter dangersenseCounter = new DangerSenseCounter();
                dangersenseCounter.enemyPosition = enemyPos;
                this.outer.SetState(dangersenseCounter);     

            }
            orig.Invoke(self, damageInfo);
        }


        //public interface IOnIncomingDamageServerReceiver
        //{
        //    void OnIncomingDamageServer(DamageInfo damageInfo);
        //    DamageInfo damageInfo1 
              
        //    void OnOutgoingDamageServer(DamageInfo damageInfo);


        //}

    
        public override void OnExit()
        {
            //On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            bool active = NetworkServer.active;
            if (active && base.characterBody.HasBuff(Modules.Buffs.counterBuff))
            {
                base.characterBody.RemoveBuff(Modules.Buffs.counterBuff);
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration / (fireTime) && !base.characterBody.HasBuff(Modules.Buffs.counterBuff.buffIndex) && !counteron)
            {
                counteron = true;
                dekucon.DANGERSENSE.Play();
                bool active = NetworkServer.active;
                if (active)
                {
                    base.characterBody.AddBuff(Modules.Buffs.counterBuff);
                    //dangercon = base.characterBody.gameObject.GetComponent<DangerSenseComponent>();
                    //characterBody.gameObject.AddComponent<DangerSenseComponent>();

                }
                dekucon.countershouldflip = true;
                AkSoundEngine.PostEvent(573664262, this.gameObject);
            }

            

            //if (base.fixedAge >= (duration / fireTime) && base.fixedAge < (baseDuration - (fireTime)))
            //{
            //    if (dekucon.countershouldflip)
            //    {
            //        Debug.Log("counter");
            //        bool isAuthority = base.isAuthority;
            //        if (isAuthority)
            //        {

            //            Debug.Log("counterauthority");
            //            DangerSenseCounter dangersenseCounter = new DangerSenseCounter();
                        
            //            this.outer.SetState(dangersenseCounter);
            //        }
            //    }
            //    //bool isAuthority = base.isAuthority;
            //    //if (isAuthority)
            //    //{
            //    //    DangerSenseCounter dangersenseCounter= new DangerSenseCounter();
            //    //    this.outer.SetNextState(dangersenseCounter);
            //    //}
            //    //hasFired = true;
            //    ////base.PlayCrossfade("LeftArm, Override", "FingerFlick", "Attack.playbackRate", this.duration, this.fireTime);
            //    //if (base.isAuthority)
            //    //{
            //    //    blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.counterDamageCoefficient * fajin;
            //    //    blastAttack.position = base.characterBody.corePosition;
            //    //    blastAttack.Fire();
            //    //    base.PlayAnimation("Fullbody, Override", "ShootStyleFullFlip", "Attack.playbackRate", duration / 2);
            //    //    Ray aimRay = base.GetAimRay();

            //    //    //EffectManager.SpawnEffect(Modules.Assets.airforce100impactEffect, new EffectData
            //    //    //{
            //    //    //    origin = aimRay.origin + 5 * aimRay.direction,
            //    //    //    scale = 1f,
            //    //    //    rotation = Quaternion.LookRotation(aimRay.direction)

            //    //    //}, true);


            //    //    for (int i = 0; i <= 5; i++)
            //    //    {
            //    //        this.randRelPos = new Vector3((float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f);
            //    //        float num = 60f;
            //    //        Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
            //    //        float num2 = 0.01f;
            //    //        rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
            //    //        rotation.y += UnityEngine.Random.Range(-num2, num2) * num;

            //    //        EffectData effectData = new EffectData
            //    //        {
            //    //            scale = 1f,
            //    //            origin = base.characterBody.corePosition + this.randRelPos,
            //    //            rotation = rotation

            //    //        };
            //    //        EffectManager.SpawnEffect(this.effectPrefab, effectData, true);

            //    //    }


            //}


            {
                if (base.fixedAge >= (baseDuration - (duration / fireTime)))
                {

                    Debug.Log("counterend");
                    dekucon.countershouldflip = false;
                    dekucon.DANGERSENSE.Stop();
                    bool active = NetworkServer.active;
                    if (active && base.characterBody.HasBuff(Modules.Buffs.counterBuff))
                    {
                        base.characterBody.RemoveBuff(Modules.Buffs.counterBuff);
                    }

                    //base.PlayCrossfade("Gesture, Override", "CounterEnd", "Attack.playbackRate", this.duration / (4 * attackSpeedStat * fajin), this.duration / (4 * attackSpeedStat * fajin));
                }
            }


            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}