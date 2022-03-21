using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using EntityStates;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class DangerSense100 : BaseSkillState
    {

        public static float procCoefficient = 2f;
        public static float baseDuration = 1f;
        public static float force = 300f;

        private float duration;
        private float fireTime;

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

        public enum DangerState { STARTBUFF, CHECKFLIP, END };
        public DangerState state;

        public override void OnEnter()
        {
            base.OnEnter();
            state = DangerState.STARTBUFF;

            dekucon = base.GetComponent<DekuController>();
            this.duration = baseDuration;

            //base.characterBody.SetAimTimer(duration);
            //this.muzzleString = "LFinger";

            counteron = false;
            dekucon.countershouldflip = false;

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

            this.fireTime = duration / (4f * attackSpeedStat * fajin);
            if (this.fireTime < 0.1f)
            {
                fireTime = 0.1f;
            }

            if (NetworkServer.active && base.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.05f;
                damageInfo.position = base.transform.position;
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

            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self.body.HasBuff(Modules.Buffs.counterBuff.buffIndex))
            {
                damageInfo.rejected = true;

                //Debug.Log("hookhasbuff"+self.body.HasBuff(Modules.Buffs.counterBuff.buffIndex));

                var dekucon = self.body.gameObject.GetComponent<DekuController>();
                dekucon.countershouldflip = true;

                var damageInfo2 = new DamageInfo();

                damageInfo2.damage = self.body.damage * Modules.StaticValues.counterDamageCoefficient;
                damageInfo2.position = damageInfo.attacker.transform.position;
                damageInfo2.force = Vector3.zero;
                damageInfo2.damageColorIndex = DamageColorIndex.Default;
                damageInfo2.crit = Util.CheckRoll(self.body.crit, self.body.master);
                damageInfo2.attacker = self.gameObject;
                damageInfo2.inflictor = null;
                damageInfo2.damageType = DamageType.BypassArmor | DamageType.WeakOnHit;
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


                new ForceCounterState(self.body.masterObjectId, enemyPos).Send(R2API.Networking.NetworkDestination.Clients);

                //if (self.body.characterMotor && self.body.characterDirection)
                //{
                //    Debug.Log("pluginmove");
                //    self.body.characterMotor.rootMotion += (self.body.transform.position - enemyPos).normalized * self.body.moveSpeed;
                //}

                //self.body.characterMotor.rootMotion += (self.body.transform.position - enemyPos).normalized * self.body.moveSpeed;

                //DangerSenseCounter dangersenseCounter = new DangerSenseCounter();
                //dangersenseCounter.enemyPosition = enemyPos;
                //self.body.gameObject.GetComponent<EntityStateMachine>().SetNextState(dangersenseCounter);






                blastAttack = new BlastAttack();
                blastAttack.radius = blastRadius;
                blastAttack.procCoefficient = procCoefficient;
                blastAttack.position = base.characterBody.corePosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.counterDamageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = force;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.damageType = DamageType.Freeze2s;
                blastAttack.attackerFiltering = AttackerFiltering.Default;


                blastAttack.Fire();

                for (int i = 0; i <= 5; i++)
                {
                    this.randRelPos = new Vector3((float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f);
                    float num = 60f;
                    Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
                    float num2 = 0.01f;
                    rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                    rotation.y += UnityEngine.Random.Range(-num2, num2) * num;

                    EffectData effectData = new EffectData
                    {
                        scale = 1f,
                        origin = base.characterBody.corePosition + this.randRelPos,
                        rotation = rotation

                    };
                    EffectManager.SpawnEffect(this.effectPrefab, effectData, true);
                }


                self.body.RemoveBuff(Modules.Buffs.counterBuff.buffIndex);

                //self.body.gameObject.GetComponent<EntityStateMachine>().SetInterruptState(new FrozenState(), InterruptPriority.Frozen);


                //EntityStateMachine[] stateMachines = self.body.gameObject.GetComponents<EntityStateMachine>();
                //foreach (EntityStateMachine stateMachine in stateMachines)
                //{
                //    if (stateMachine.customName == "Body")
                //    {
                //        Debug.Log("bodystatechange");
                //        self.body.gameObject.GetComponent<EntityStateMachine>().SetState(new DangerSenseCounter
                //        {
                //            enemyPosition = enemyPos
                //        });


                //    }

                //}

            }
            orig.Invoke(self, damageInfo);
        }


        public override void OnExit()
        {
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
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
            //Debug.Log(state +" "+ base.fixedAge);

            switch (state)
            {
                case DangerState.STARTBUFF:
                    if (base.fixedAge >= fireTime && base.fixedAge < (duration - fireTime)
                && !base.characterBody.HasBuff(Modules.Buffs.counterBuff.buffIndex))
                    {
                        //Debug.Log(fireTime);
                        //Debug.Log("counterstart");
                        dekucon.DANGERSENSE.Play();
                        bool active = NetworkServer.active;
                        if (active)
                        {
                            base.characterBody.AddBuff(Modules.Buffs.counterBuff);

                        }
                        AkSoundEngine.PostEvent(573664262, this.gameObject);
                        state = DangerState.CHECKFLIP;
                    }
                    break;
                case DangerState.CHECKFLIP:
                  
                    if (base.fixedAge > duration - fireTime)
                    {
                        state = DangerState.END;
                    }
                    break;
                case DangerState.END:
                    if (base.fixedAge >= (duration - (fireTime)) && !counteron)
                    {
                        counteron = true;
                        //Debug.Log(duration - fireTime);
                        //Debug.Log("counterend");
                        dekucon.DANGERSENSE.Stop();
                        bool active = NetworkServer.active;
                        if (active && base.characterBody.HasBuff(Modules.Buffs.counterBuff))
                        {
                            base.characterBody.RemoveBuff(Modules.Buffs.counterBuff);
                        }

                    }

                    if (base.fixedAge >= this.duration && base.isAuthority)
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break;
            }

            //if (base.fixedAge >= fireTime && base.fixedAge < (duration - fireTime) 
            //    && !base.characterBody.HasBuff(Modules.Buffs.counterBuff.buffIndex) 
            //    && !counteron)
            //{
            //    Debug.Log(fireTime);
            //    counteron = true;
            //    Debug.Log("counterstart");
            //    dekucon.DANGERSENSE.Play();
            //    bool active = NetworkServer.active;
            //    if (active)
            //    {
            //        base.characterBody.AddBuff(Modules.Buffs.counterBuff);
            //        //dangercon = base.characterBody.gameObject.GetComponent<DangerSenseComponent>();
            //        //characterBody.gameObject.AddComponent<DangerSenseComponent>();

            //    }
            //    //dekucon.countershouldflip = true;
            //    AkSoundEngine.PostEvent(573664262, this.gameObject);
            //}



            //if (base.fixedAge > fireTime && base.fixedAge < (duration - fireTime))
            //{



            //    Debug.Log("buffmiddle" + base.characterBody.HasBuff(Modules.Buffs.counterBuff.buffIndex));
            //    Debug.Log("counteronmiddle"+counteron);

            //    if (!base.characterBody.HasBuff(Modules.Buffs.counterBuff.buffIndex) 
            //        && counteron)
            //    {
            //        On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            //        counteron = false;
            //        Debug.Log("counter");


            //        DangerSenseCounter dangersenseCounter = new DangerSenseCounter();
            //        this.outer.SetState(dangersenseCounter);
            //    }               


            //}



            //if (base.fixedAge >= (duration - (fireTime)) && counteron)
            //{
            //    Debug.Log(duration-fireTime);
            //    counteron = false;
            //    Debug.Log("counterend");
            //    dekucon.countershouldflip = false;
            //    dekucon.DANGERSENSE.Stop();
            //    bool active = NetworkServer.active;
            //    if (active && base.characterBody.HasBuff(Modules.Buffs.counterBuff))
            //    {
            //        base.characterBody.RemoveBuff(Modules.Buffs.counterBuff);
            //    }

            //    //base.PlayCrossfade("Gesture, Override", "CounterEnd", "Attack.playbackRate", this.duration / (4 * attackSpeedStat * fajin), this.duration / (4 * attackSpeedStat * fajin));
            //}



            //if (base.fixedAge >= this.duration && base.isAuthority)
            //{
            //    this.outer.SetNextStateToMain();
            //    return;
            //}
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}