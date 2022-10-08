using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using EntityStates;
using System.Runtime.CompilerServices;
using AncientScepter;
using EntityStates.Mage;
using System.Diagnostics;
using UnityEngine.Networking;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using System.Linq;
using Random = UnityEngine.Random;

namespace DekuMod.Modules.Survivors
{
    public class DekuController : MonoBehaviour
    {
        public ChildLocator child;
        public CharacterBody body;
        private EnergySystem energySystem;

        //Particles
        public ParticleSystem GOBEYOND;
        public ParticleSystem RARM;
        public ParticleSystem LARM;
        public ParticleSystem LLEG;
        public ParticleSystem RLEG;
        public ParticleSystem OFA;
        public ParticleSystem OFAeye;
        public ParticleSystem FAJIN;
        public ParticleSystem OKLAHOMA;
        public ParticleSystem DANGERSENSE;
        public ParticleSystem WINDRING;
        private int buffCountToApply;
        public GenericSkill specialSkillSlot;
        string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";

        //Fajin
        public bool fajinon;

        //float
        public float stopwatch;

        //danger sense
        private Vector3 randRelPos;
        private BlastAttack blastAttack;
        public float dangersenseBlastRadius = 3f;
        public float dangersense45BlastRadius = 5f;
        public float dangersense100BlastRadius = 7f;
        public static float procCoefficient = 1f;
        public static float force = 300f;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");

        //float
        internal bool endFloat;
        internal bool hasFloatBuff;
        private Stopwatch floatStopwatch;

        //OFA 
        public float ofaHurtTimer;

        //Go Beyond
        public float goBeyondTimer;
        public bool goBeyondUsed;
        public bool goBeyondOFAGiven;
        public float goBeyondBuffTimer;

        //Indicator
        public float maxTrackingDistance = 50f;
        public float maxTrackingAngle = 15f;
        public float trackerUpdateFrequency = 10f;
        private Indicator indicator;
        private HurtBox trackingTarget;
        public HurtBox Target;
        private float trackerUpdateStopwatch;
        private InputBankTest inputBank;
        private readonly BullseyeSearch search = new BullseyeSearch();

        //gobeyond loop sound
        public uint gobeyondLoopID;

        //auras
        public bool halfMeterAuraGiven;
        public bool fullMeterAuraGiven;

        public void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
            child = GetComponentInChildren<ChildLocator>();
            indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
            inputBank = gameObject.GetComponent<InputBankTest>();
            if (child)
            {
                GOBEYOND = child.FindChild("goBeyondAura").GetComponent<ParticleSystem>();
                LARM = child.FindChild("lArmAura").GetComponent<ParticleSystem>();
                RARM = child.FindChild("rArmAura").GetComponent<ParticleSystem>();
                LLEG = child.FindChild("lLegAura").GetComponent<ParticleSystem>();
                RLEG = child.FindChild("rLegAura").GetComponent<ParticleSystem>();
                OFA = child.FindChild("OFAlightning").GetComponent<ParticleSystem>();
                OFAeye = child.FindChild("OFAlightningeye").GetComponent<ParticleSystem>();
                FAJIN = child.FindChild("FAJINaura").GetComponent<ParticleSystem>();
                OKLAHOMA = child.FindChild("Oklahoma").GetComponent<ParticleSystem>();
                DANGERSENSE = child.FindChild("Dangersense").GetComponent<ParticleSystem>();
                WINDRING = child.FindChild("windRing").GetComponent<ParticleSystem>();
            }
            GOBEYOND.Stop();
            LARM.Stop();
            RARM.Stop();
            LLEG.Stop();
            RLEG.Stop();
            OFA.Stop();
            OFAeye.Stop();
            OFA.Stop();
            FAJIN.Stop();
            OKLAHOMA.Stop();
            DANGERSENSE.Stop();
            WINDRING.Stop();

            StopGobeyondLoop();

            //anim = GetComponentInChildren<Animator>();
            //stopwatch = 0f;

            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }


        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            //dangersense
            if (damageInfo != null && damageInfo.attacker && 
                damageInfo.attacker.GetComponent<CharacterBody>() && 
                damageInfo.attacker.gameObject.GetComponent<CharacterBody>().baseNameToken != DekuPlugin.developerPrefix + "_DEKU_BODY_NAME")
            {
                bool flag = (damageInfo.damageType & DamageType.BypassArmor) > DamageType.Generic;
                if (!flag && damageInfo.damage > 0f)
                {
                    //dangersense base
                    if (self.body.HasBuff(Modules.Buffs.dangersenseBuff.buffIndex))
                    {
                        damageInfo.force = Vector3.zero;
                        damageInfo.damage -= self.body.level * 5f;
                        if(damageInfo.damage < 0f)
                        {
                            self.Heal(Mathf.Abs(damageInfo.damage), default(RoR2.ProcChainMask), true);
                            damageInfo.damage = 0f;
                        }

                        //Debug.Log("hookhasbuff"+self.body.HasBuff(Modules.Buffs.dangersenseBuff.buffIndex));

                        var dekucon = self.body.gameObject.GetComponent<DekuController>();
                        //dekucon.countershouldflip = true;

                        var damageInfo2 = new DamageInfo();

                        damageInfo2.damage = self.body.damage * Modules.StaticValues.dangersenseDamageCoefficient;
                        damageInfo2.position = damageInfo.attacker.transform.position;
                        damageInfo2.force = Vector3.zero;
                        damageInfo2.damageColorIndex = DamageColorIndex.Default;
                        damageInfo2.crit = Util.CheckRoll(self.body.crit, self.body.master);
                        damageInfo2.attacker = self.body.gameObject;
                        damageInfo2.inflictor = self.body.gameObject;
                        damageInfo2.damageType = DamageType.Freeze2s;
                        damageInfo2.procCoefficient = 1f;
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

                        //new ForceCounterState(self.body.masterObjectId, enemyPos).Send(NetworkDestination.Clients);



                        //blastAttack = new BlastAttack();
                        //blastAttack.radius = dangersenseBlastRadius;
                        //blastAttack.procCoefficient = procCoefficient;
                        //blastAttack.position = self.transform.position;
                        //blastAttack.attacker = base.gameObject;
                        //blastAttack.crit = Util.CheckRoll(self.body.crit, self.body.master);
                        //blastAttack.baseDamage = self.body.damage * Modules.StaticValues.dangersenseDamageCoefficient;
                        //blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                        //blastAttack.baseForce = force;
                        //blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                        //blastAttack.damageType = DamageType.Shock5s;
                        //blastAttack.attackerFiltering = AttackerFiltering.Default;


                        //blastAttack.Fire();

                        //for (int i = 0; i <= 5; i++)
                        //{
                        //    this.randRelPos = new Vector3((float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f);
                        //    float num = 60f;
                        //    Quaternion rotation = Util.QuaternionSafeLookRotation(self.body.characterDirection.forward.normalized);
                        //    float num2 = 0.01f;
                        //    rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                        //    rotation.y += UnityEngine.Random.Range(-num2, num2) * num;

                        //    EffectData effectData = new EffectData
                        //    {
                        //        scale = 1f,
                        //        origin = self.body.corePosition + this.randRelPos,
                        //        rotation = rotation

                        //    };
                        //    EffectManager.SpawnEffect(this.effectPrefab, effectData, true);
                        //}


                    }
                    //dangersense 45
                    if (self.body.HasBuff(Modules.Buffs.dangersense45Buff.buffIndex))
                    {
                        damageInfo.damage *= StaticValues.dangersense45DamageReduction;


                        //Debug.Log("hookhasbuff"+self.body.HasBuff(Modules.Buffs.dangersenseBuff.buffIndex));

                        var dekucon = self.body.gameObject.GetComponent<DekuController>();
                        //dekucon.countershouldflip = true;

                        var damageInfo2 = new DamageInfo();

                        damageInfo2.damage = self.body.damage * Modules.StaticValues.dangersense45DamageCoefficient;
                        damageInfo2.position = damageInfo.attacker.transform.position;
                        damageInfo2.force = Vector3.zero;
                        damageInfo2.damageColorIndex = DamageColorIndex.Default;
                        damageInfo2.crit = Util.CheckRoll(self.body.crit, self.body.master);
                        damageInfo2.attacker = self.body.gameObject;
                        damageInfo2.inflictor = self.body.gameObject;
                        damageInfo2.damageType = DamageType.Shock5s;
                        damageInfo2.procCoefficient = 1f;
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

                        new ForceCounterState(self.body.masterObjectId, enemyPos).Send(NetworkDestination.Clients);



                        blastAttack = new BlastAttack();
                        blastAttack.radius = dangersenseBlastRadius;
                        blastAttack.procCoefficient = procCoefficient;
                        blastAttack.position = self.transform.position;
                        blastAttack.attacker = self.body.gameObject;
                        blastAttack.crit = Util.CheckRoll(self.body.crit, self.body.master);
                        blastAttack.baseDamage = self.body.damage * Modules.StaticValues.dangersense45DamageCoefficient;
                        blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                        blastAttack.baseForce = force;
                        blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                        blastAttack.damageType = DamageType.Generic;
                        blastAttack.attackerFiltering = AttackerFiltering.Default;


                        blastAttack.Fire();

                        for (int i = 0; i <= 5; i++)
                        {
                            this.randRelPos = new Vector3((float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f);
                            float num = 60f;
                            Quaternion rotation = Util.QuaternionSafeLookRotation(self.body.characterDirection.forward.normalized);
                            float num2 = 0.01f;
                            rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                            rotation.y += UnityEngine.Random.Range(-num2, num2) * num;

                            EffectData effectData = new EffectData
                            {
                                scale = 1f,
                                origin = self.body.corePosition + this.randRelPos,
                                rotation = rotation

                            };
                            EffectManager.SpawnEffect(this.effectPrefab, effectData, true);
                        }


                    }
                    //dangersense 100
                    if (self.body.HasBuff(Modules.Buffs.dangersense100Buff.buffIndex))
                    {
                        damageInfo.force = Vector3.zero;
                        damageInfo.damage -= self.body.armor;
                        if (damageInfo.damage < 0f)
                        {
                            //self.Heal(Mathf.Abs(damageInfo.damage), default(RoR2.ProcChainMask), true);
                            damageInfo.damage = 0f;
                        }


                        //Debug.Log("hookhasbuff"+self.body.HasBuff(Modules.Buffs.dangersenseBuff.buffIndex));

                        var dekucon = self.body.gameObject.GetComponent<DekuController>();
                        //dekucon.countershouldflip = true;

                        var damageInfo2 = new DamageInfo();

                        damageInfo2.damage = self.body.damage * Modules.StaticValues.dangersense100DamageCoefficient;
                        damageInfo2.position = damageInfo.attacker.transform.position;
                        damageInfo2.force = Vector3.zero;
                        damageInfo2.damageColorIndex = DamageColorIndex.Default;
                        damageInfo2.crit = Util.CheckRoll(self.body.crit, self.body.master);
                        damageInfo2.attacker = self.body.gameObject;
                        damageInfo2.inflictor = self.body.gameObject;
                        damageInfo2.damageType = DamageType.Generic;
                        damageInfo2.procCoefficient = 1f;
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

                        new ForceCounterState(self.body.masterObjectId, enemyPos).Send(NetworkDestination.Clients);



                        blastAttack = new BlastAttack();
                        blastAttack.radius = dangersense100BlastRadius;
                        blastAttack.procCoefficient = procCoefficient;
                        blastAttack.position = self.transform.position;
                        blastAttack.attacker = base.gameObject;
                        blastAttack.crit = Util.CheckRoll(self.body.crit, self.body.master);
                        blastAttack.baseDamage = self.body.damage * Modules.StaticValues.dangersense100DamageCoefficient;
                        blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                        blastAttack.baseForce = force;
                        blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                        blastAttack.damageType = DamageType.Generic;
                        blastAttack.attackerFiltering = AttackerFiltering.Default;


                        blastAttack.Fire();

                        for (int i = 0; i <= 5; i++)
                        {
                            this.randRelPos = new Vector3((float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f);
                            float num = 60f;
                            Quaternion rotation = Util.QuaternionSafeLookRotation(self.body.characterDirection.forward.normalized);
                            float num2 = 0.01f;
                            rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                            rotation.y += UnityEngine.Random.Range(-num2, num2) * num;

                            EffectData effectData = new EffectData
                            {
                                scale = 1f,
                                origin = self.body.corePosition + this.randRelPos,
                                rotation = rotation

                            };
                            EffectManager.SpawnEffect(this.effectPrefab, effectData, true);
                        }


                    }

                }


            }
            orig.Invoke(self, damageInfo);
        }

        public void Start()
        {
            energySystem = gameObject.AddComponent<EnergySystem>();

            goBeyondUsed = false;
            goBeyondOFAGiven = false;
            halfMeterAuraGiven = false;
            fullMeterAuraGiven = false;
        }


        public void FixedUpdate()
        {
            //indicator
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
                Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
                this.SearchForTarget(aimRay);
                HurtBox hurtBox = this.trackingTarget;
                this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);


            }

            //float
            if (!body.characterMotor.isGrounded)
            {
                if(energySystem.currentPlusUltra > 5f)
                {
                    if (body.inputBank.jump.down)
                    {
                        if (NetworkServer.active)
                        {
                            body.ApplyBuff(Modules.Buffs.floatBuff.buffIndex, 1);
                        }
                        stopwatch += Time.fixedDeltaTime;
                        if (stopwatch > 1f)
                        {
                            if (body.characterMotor.velocity.y <= 0)
                            {
                                energySystem.SpendPlusUltra(StaticValues.floatForceEnergyFraction);
                                if(body.inputBank.skill1.down || body.inputBank.skill2.down || body.inputBank.skill3.down)
                                {
                                    body.characterMotor.velocity.y = 0f;
                                }
                                else
                                {
                                    body.characterMotor.velocity.y += StaticValues.floatSpeed;
                                }
                            }
                            else if (body.characterMotor.velocity.y > 0)
                            {
                                energySystem.SpendPlusUltra(StaticValues.floatForceEnergyFraction);
                                if (body.inputBank.skill1.down || body.inputBank.skill2.down || body.inputBank.skill3.down)
                                {
                                    body.characterMotor.velocity.y = 0f;
                                }
                                else
                                {
                                    body.characterMotor.velocity.y += StaticValues.floatSpeed;
                                }
                            }
                        }
                    }
                    else if (!body.inputBank.jump.down)
                    {
                        if (NetworkServer.active)
                        {
                            body.ApplyBuff(Modules.Buffs.floatBuff.buffIndex, 0);
                        }
                    }

                }
            }
            else if (body.characterMotor.isGrounded)
            {
                stopwatch = 0f;
                if (NetworkServer.active)
                {
                    body.ApplyBuff(Modules.Buffs.floatBuff.buffIndex, 0);
                }
            }

            //ofabuff self damage and eye particle
            if (body.HasBuff(Buffs.ofaBuff))
            {
                if (ofaHurtTimer > 1f)
                {
                    ofaHurtTimer = 0f;
                    if (body.hasEffectiveAuthority)
                    {
                        new SpendHealthNetworkRequest(body.masterObjectId, 0.1f * body.healthComponent.health).Send(NetworkDestination.Clients);
                    }
                }
                else
                {
                    ofaHurtTimer += Time.fixedDeltaTime;
                }

                if (OFAeye.isStopped)
                {
                    OFAeye.Play();
                }
            }
            else
            {
                OFAeye.Stop();
            }
            //go beyond healing and one use and particle
            if (body.HasBuff(Buffs.goBeyondBuff))
            {
                body.skillLocator.special.RemoveAllStocks();
                if (goBeyondTimer > 1f)
                {
                    if (body.hasEffectiveAuthority)
                    {
                        new HealNetworkRequest(body.masterObjectId, body.healthComponent.fullCombinedHealth * 0.05f).Send(NetworkDestination.Clients);
                    }
                    goBeyondTimer = 0f;
                }
                else
                {
                    goBeyondTimer += Time.fixedDeltaTime;
                }

                if(goBeyondBuffTimer > 59f)
                {
                    body.AddBuff(Modules.Buffs.goBeyondBuffUsed);
                    if (!goBeyondUsed)
                    {
                        goBeyondUsed = true;
                        body.ApplyBuff(Buffs.ofaBuff.buffIndex, 1);
                    }
                }
                else
                {
                    goBeyondBuffTimer += Time.fixedDeltaTime;
                }
            }
            else if (!body.HasBuff(Buffs.goBeyondBuff))
            {
                //give ofa after gobeyond is used, once only
                if (body.HasBuff(Buffs.goBeyondBuffUsed))
                {
                    if (!goBeyondOFAGiven)
                    {
                        goBeyondOFAGiven = true;
                        body.ApplyBuff(Buffs.ofaBuff.buffIndex, 1, -1);
                    }
                }
            }
            //danger sense particle
            if (body.HasBuff(Buffs.dangersenseBuff) || body.HasBuff(Buffs.dangersense45Buff) || body.HasBuff(Buffs.dangersense100Buff))
            {
                if (DANGERSENSE.isStopped)
                {
                    DANGERSENSE.Play();
                }
            }
            else 
            {
                DANGERSENSE.Stop();
            }
            //fajin particle
            if (body.HasBuff(Buffs.fajinBuff))
            {
                if (FAJIN.isStopped)
                {
                    FAJIN.Play();
                }
            }
            else
            {
                FAJIN.Stop();
            }
            //ofa particle
            if (body.HasBuff(Buffs.ofaBuff) || body.HasBuff(Buffs.ofaBuff45))
            {
                if (OFA.isStopped)
                {
                    OFA.Play();
                }
            }
            else
            {
                OFA.Stop();
            }

            if (energySystem.currentPlusUltra >= 99f)
            {
                if (!fullMeterAuraGiven)
                {
                    RLEG.Play();
                    LLEG.Play();
                    fullMeterAuraGiven = true;
                }
            }
            else if (energySystem.currentPlusUltra >= 50f)
            {
                if (!halfMeterAuraGiven)
                {
                    RARM.Play();
                    LARM.Play();
                    halfMeterAuraGiven = true;

                    if (fullMeterAuraGiven)
                    {
                        RLEG.Stop();
                        LLEG.Stop();
                        fullMeterAuraGiven = false;
                    }
                }
            }
            else if (energySystem.currentPlusUltra < 50f)
            {
                halfMeterAuraGiven = false;
                fullMeterAuraGiven = false;
                RARM.Stop();
                LARM.Stop();
                RLEG.Stop();
                LLEG.Stop();
            }


            //CheckIfMaxKickPowerStacks();

            //if (fajinon)
            //{
            //    CheckIfMaxPowerStacks();
            //    if (isMaxPower)
            //    {
            //        FAJIN.Play();
            //    }
            //    else
            //    {
            //        FAJIN.Stop();
            //    }
            //    if (fajinscepteron)
            //    {
            //        if (anim.GetBool("isMoving") && stopwatch >= fajinscepterrate / body.moveSpeed)
            //        {
            //            IncrementBuffCount();
            //            stopwatch = 0f;
            //        }
            //    }
            //    else
            //    {
            //        if (anim.GetBool("isMoving") && stopwatch >= fajinrate / body.moveSpeed)
            //        {
            //            IncrementBuffCount();
            //            stopwatch = 0f;
            //        }
            //    }

            //}
            //stopwatch += Time.fixedDeltaTime;


            //if (body.HasBuff(Modules.Buffs.dangersenseBuff))
            //{
            //    DANGERSENSE.Play();
            //}
            //if (!body.HasBuff(Modules.Buffs.dangersenseBuff))
            //{
            //    DANGERSENSE.Stop();
            //}
            //if (this.hasFloatBuff)
            //{
            //    this.floatStopwatch.Start();
            //    bool flag2 = this.floatStopwatch.Elapsed.TotalSeconds >= (double)StaticValues.floatDuration;
            //    if (flag2)
            //    {
            //        this.endFloat = true;
            //    }
            //}
            //else
            //{
            //    bool isGrounded = this.characterBody.characterMotor.isGrounded;
            //    if (isGrounded)
            //    {
            //        this.floatStopwatch.Reset();
            //    }
            //}
        }

        
        public void PlayGobeyondLoop()
        {
            if (body.hasEffectiveAuthority)
            {
                gobeyondLoopID = AkSoundEngine.PostEvent("gobeyondost", body.gameObject);
            }
        }

        
        public void StopGobeyondLoop()
        {
            AkSoundEngine.StopPlayingID(gobeyondLoopID);
        }

        private void SearchForTarget(Ray aimRay)
        {
            this.search.teamMaskFilter = TeamMask.AllExcept(TeamIndex.Player);
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = this.maxTrackingDistance;
            this.search.maxAngleFilter = this.maxTrackingAngle;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();
        }
        public HurtBox GetTrackingTarget()
        {
            return this.trackingTarget;
        }

        private void OnEnable()
        {
            this.indicator.active = true;
        }

        private void OnDisable()
        {
            this.indicator.active = false;
        }

        private void OnDestroy()
        {
            StopGobeyondLoop();
        }

    }
}

