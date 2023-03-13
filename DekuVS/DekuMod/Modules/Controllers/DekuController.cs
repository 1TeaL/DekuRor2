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
        public ParticleSystem DANGERSENSE;
        public ParticleSystem WINDRING;
        public ParticleSystem BLACKWHIP;
        public ParticleSystem GEARSHIFTIN;
        public ParticleSystem GEARSHIFTOUT;
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
        public static float dangersenseprocCoefficient = 1f;
        public static float force = 300f;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");


        //blackwhip
        public NetworkedBodyAttachment attachment;
        public SiphonNearbyController siphonNearbyController;
        public GameObject blackwhipLineEffect;
        public LineRenderer blackwhipLineRenderer;
        public CharacterBody enemyBody;
        public Vector3 storedPos;
        public bool blackwhipAttachWorld;
        public float blackwhipTimer;
        public bool blackwhipActivated;

        //OFA 
        public float ofaHurtTimer;

        //Go Beyond
        public float goBeyondTimer;
        public bool goBeyondUsed;
        public bool goBeyondOFAGiven;
        public float goBeyondBuffTimer;

        //Indicator
        public float maxTrackingDistance = 70f;
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
        //public bool halfMeterAuraGiven;
        //public bool fullMeterAuraGiven;
        //public bool blackwhipAuraGiven;
        //public bool gearshiftInAuraGiven;
        //public bool gearshiftOutAuraGiven;

        //skill cd
        public float skillCDTimer;

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
                DANGERSENSE = child.FindChild("Dangersense").GetComponent<ParticleSystem>();
                WINDRING = child.FindChild("windRing").GetComponent<ParticleSystem>();
                BLACKWHIP = child.FindChild("blackwhipAura").GetComponent<ParticleSystem>();
                GEARSHIFTIN = child.FindChild("gearshiftAuraIn").GetComponent<ParticleSystem>();
                GEARSHIFTOUT = child.FindChild("gearshiftAuraOut").GetComponent<ParticleSystem>();
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
            DANGERSENSE.Stop();
            WINDRING.Stop();
            BLACKWHIP.Stop();
            GEARSHIFTIN.Stop();
            GEARSHIFTOUT.Stop();

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
                    if (self.body.HasBuff(Buffs.dangersenseBuff.buffIndex) && !self.body.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
                    {
                        if(energySystem.currentPlusUltra > StaticValues.dangersensePlusUltraSpend)
                        {
                            energySystem.SpendPlusUltra(StaticValues.dangersensePlusUltraSpend);

                            self.body.ApplyBuff(Buffs.dangersenseBuff.buffIndex, 0, -1);
                            self.body.ApplyBuff(Buffs.dangersenseDebuff.buffIndex, 1, StaticValues.dangersenseBuffTimer);

                            damageInfo.force = Vector3.zero;
                            damageInfo.damage -= self.body.armor;
                            if (damageInfo.damage < 0f)
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

                            if (!self.body.inputBank.skill1.down && !self.body.inputBank.skill2.down && !self.body.inputBank.skill3.down)
                            {
                                new ForceCounterState(self.body.masterObjectId, enemyPos).Send(NetworkDestination.Clients);

                                blastAttack = new BlastAttack();
                                blastAttack.radius = dangersenseBlastRadius;
                                blastAttack.procCoefficient = dangersenseprocCoefficient;
                                blastAttack.position = self.transform.position;
                                blastAttack.attacker = base.gameObject;
                                blastAttack.crit = Util.CheckRoll(self.body.crit, self.body.master);
                                blastAttack.baseDamage = self.body.damage * Modules.StaticValues.dangersenseDamageCoefficient;
                                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                                blastAttack.baseForce = force;
                                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                                blastAttack.damageType = DamageType.Shock5s;
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
                        else
                        {
                            Chat.AddMessage($"You need {StaticValues.dangersensePlusUltraSpend} plus ultra.");
                            energySystem.TriggerGlow(0.3f, 0.3f, Color.black);
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
            skillCDTimer = 0f;
        }

        public void Update()
        {

            if (blackwhipTimer > 0f)
            {
                if (enemyBody)
                {
                    MakeLine();
                }
                else
                {
                    if (blackwhipAttachWorld)
                    {
                        MakeLine();
                    }
                }
            }
            else
            {
                if (blackwhipLineEffect)
                {
                    Destroy(blackwhipLineEffect);
                }
                blackwhipAttachWorld = false;
            }

            if(blackwhipLineEffect && blackwhipLineRenderer)
            {
                if (blackwhipTimer > 0f)
                {
                    if (enemyBody)
                    {
                        LineVec(enemyBody.transform.position);
                    }
                    else if (blackwhipAttachWorld)
                    {
                        LineVec(storedPos);
                    }
                }
            }
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

            //skill CD timer
            skillCDTimer += Time.fixedDeltaTime;

            //blackwhip timer
            if (!blackwhipActivated)
            {
                blackwhipTimer -= Time.fixedDeltaTime;
            }
            else if(blackwhipActivated)
            {
                energySystem.SpendPlusUltra(StaticValues.blackwhip100EnergyFraction);
                if (energySystem.currentPlusUltra < 5f)
                {

                    if (body.HasBuff(Buffs.blackwhipBuff.buffIndex))
                    {
                        body.ApplyBuff(Buffs.blackwhipBuff.buffIndex, 0);
                    }
                    blackwhipActivated = false;
                    Chat.AddMessage($"Deactivated blackwhip 100%.");

                }
            }
            //blackwhip 100% attach
            if (enemyBody && blackwhipActivated)
            {
                Vector3 startPos = body.transform.position;
                Vector3 endPos = enemyBody.transform.position;
                float Distance = Vector3.Distance(startPos, endPos);


                if (Distance > StaticValues.blackwhip100AttachRange)
                {
                    new PerformBlackwhipPullNetworkRequest(body.masterObjectId, startPos, (endPos - startPos).normalized, 0f).Send(NetworkDestination.Clients);
                }
                
            }
            else if(!enemyBody)
            {
                if (blackwhipLineEffect)
                {
                    Destroy(blackwhipLineEffect);
                }
                blackwhipAttachWorld = false;
                blackwhipActivated = false;

                if (body.HasBuff(Buffs.blackwhipBuff.buffIndex))
                {
                    body.ApplyBuff(Buffs.blackwhipBuff.buffIndex, 0);
                }
            }

            //gearshift
            if (body.HasBuff(Buffs.gearshift100Buff.buffIndex))
            {
                int gearshiftMovespeed = (int)Math.Round(body.moveSpeed);
                if (NetworkServer.active)
                {
                    body.SetBuffCount(Buffs.gearshift100MovespeedBuff.buffIndex, gearshiftMovespeed);
                }


                if (GEARSHIFTIN.isStopped)
                {
                    GEARSHIFTIN.Play();
                }
                if (GEARSHIFTOUT.isStopped)
                {
                    GEARSHIFTOUT.Play();
                }
            }
            else if (!body.HasBuff(Buffs.gearshift100Buff.buffIndex))
            {
                if (NetworkServer.active)
                {
                    body.SetBuffCount(Buffs.gearshift100MovespeedBuff.buffIndex, 0);
                }
                if (body.HasBuff(Buffs.gearshiftBuff.buffIndex))
                {
                    energySystem.SpendPlusUltra(StaticValues.gearshiftEnergyFraction);

                    if (energySystem.currentPlusUltra < 5f)
                    {
                        body.ApplyBuff(Buffs.gearshiftBuff.buffIndex, 0);
                        Chat.AddMessage($"Deactivated gearshift.");
                    }
                    if (GEARSHIFTIN.isStopped)
                    {
                        GEARSHIFTIN.Play();
                    }
                }
                else if (!body.HasBuff(Buffs.gearshiftBuff.buffIndex))
                {
                    if (GEARSHIFTIN.isPlaying)
                    {
                        GEARSHIFTIN.Stop();
                    }
                }

                if (body.HasBuff(Buffs.gearshift45Buff.buffIndex))
                {
                    energySystem.SpendPlusUltra(StaticValues.gearshiftEnergyFraction);

                    if (energySystem.currentPlusUltra < 5f)
                    {
                        body.ApplyBuff(Buffs.gearshift45Buff.buffIndex, 0);
                        Chat.AddMessage($"Deactivated gearshift 45%.");
                    }
                    if (GEARSHIFTOUT.isStopped)
                    {
                        GEARSHIFTOUT.Play();
                    }
                }
                else if (!body.HasBuff(Buffs.gearshift45Buff.buffIndex))
                {
                    if (GEARSHIFTOUT.isPlaying)
                    {
                        GEARSHIFTOUT.Stop();
                    }
                }

            }
            //blackwhip
            if (this.siphonNearbyController)
            {
                this.siphonNearbyController.NetworkmaxTargets = (body.healthComponent.alive ? StaticValues.blackwhipTargets : 0);
            }
            if (body.HasBuff(Buffs.blackwhipBuff.buffIndex))
            {
                if (BLACKWHIP.isStopped)
                {
                    BLACKWHIP.Play();
                }

            }
            else if (!body.HasBuff(Buffs.blackwhipBuff.buffIndex))
            {
                this.DestroyAttachment();
                if (BLACKWHIP.isPlaying)
                {
                    BLACKWHIP.Stop();
                }
            }

            //float
            if (!body.characterMotor.isGrounded)
            {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch > 0.5f)
                {
                    if (energySystem.currentPlusUltra > 5f)
                    {
                        if (!body.inputBank.sprint.justPressed)
                        {
                            if (body.inputBank.jump.down)
                            {
                                if (NetworkServer.active)
                                {
                                    body.ApplyBuff(Modules.Buffs.floatBuff.buffIndex, 1);
                                }

                                if (body.characterMotor.velocity.y <= 0)
                                {
                                    energySystem.SpendPlusUltra(StaticValues.floatForceEnergyFraction);
                                    if (body.inputBank.skill1.down || body.inputBank.skill2.down || body.inputBank.skill3.down)
                                    {
                                        body.characterMotor.velocity.y = 0f;
                                    }
                                    else
                                    {
                                        if(stopwatch < 3f)
                                        {
                                            body.characterMotor.velocity.y += 1f;
                                        }
                                        else
                                        {
                                            body.characterMotor.velocity.y = 0f;
                                        }
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
                                        if (stopwatch < 3f)
                                        {
                                            body.characterMotor.velocity.y = StaticValues.floatSpeed;
                                        }
                                        else
                                        {
                                            body.characterMotor.velocity.y = 0f;
                                        }
                                    }
                                }                                
                            }
                            //else if (!body.inputBank.jump.down)
                            //{
                            //    if (NetworkServer.active)
                            //    {
                            //        body.ApplyBuff(Modules.Buffs.floatBuff.buffIndex, 0);
                            //    }
                            //}
                        }
                        else if (body.inputBank.sprint.wasDown)
                        {
                            energySystem.SpendPlusUltra(StaticValues.floatForceEnergyFraction * 2);
                            Vector3 aimRay = body.inputBank.GetAimRay().direction;
                            body.characterMotor.velocity = aimRay * body.moveSpeed;
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
            if (body.HasBuff(Buffs.ofaBuff) || body.HasBuff(Buffs.supaofaBuff))
            {
                if (ofaHurtTimer > 1f)
                {
                    ofaHurtTimer = 0f;
                    if (body.hasEffectiveAuthority)
                    {
                        new SpendHealthNetworkRequest(body.masterObjectId, StaticValues.ofaHealthCost * body.healthComponent.health).Send(NetworkDestination.Clients);
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
                        new HealNetworkRequest(body.masterObjectId, body.healthComponent.fullCombinedHealth * StaticValues.gobeyondHealCoefficient).Send(NetworkDestination.Clients);
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
                        if (NetworkServer.active)
                        {
                            body.ApplyBuff(Buffs.ofaBuff.buffIndex, 1);
                        }
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
                        if (NetworkServer.active)
                        {
                            body.ApplyBuff(Buffs.ofaBuff.buffIndex, 1, -1);
                        }
                    }
                }
            }
            //danger sense and particle
            if (!body.HasBuff(Buffs.dangersenseDebuff))
            {
                if (NetworkServer.active)
                {
                    body.ApplyBuff(Buffs.dangersenseBuff.buffIndex, 1, -1);
                }
            }
            if (body.HasBuff(Buffs.dangersenseBuff))
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
            if (body.HasBuff(Buffs.ofaBuff) || body.HasBuff(Buffs.ofaBuff45) || body.HasBuff(Buffs.supaofaBuff) || body.HasBuff(Buffs.supaofaBuff45))
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

            if (energySystem.currentPlusUltra >= 90f)
            {
                if (RLEG.isStopped)
                {
                    RLEG.Play();
                }
                if (LLEG.isStopped)
                {
                    LLEG.Play();
                }
            }
            else if (energySystem.currentPlusUltra >= 50f && energySystem.currentPlusUltra < 90f)
            {
                if (RARM.isStopped)
                {
                    RARM.Play();
                }
                if (LARM.isStopped)
                {
                    LARM.Play();
                }
                if (RLEG.isPlaying)
                {
                    RLEG.Stop();
                }
                if (LLEG.isPlaying)
                {
                    LLEG.Stop();
                }
            }
            else if (energySystem.currentPlusUltra < 50f)
            {
                if (RLEG.isPlaying)
                {
                    RLEG.Stop();
                }
                if (LLEG.isPlaying)
                {
                    LLEG.Stop();
                }
                if (RARM.isPlaying)
                {
                    RARM.Stop();
                }
                if (LARM.isPlaying)
                {
                    LARM.Stop();
                }
            }


        }


        //blackwhip line renderer effect
        public void MakeLine()
        {
            if (!blackwhipLineEffect)
            {
                blackwhipLineEffect = UnityEngine.Object.Instantiate(Assets.blackwhipLineRenderer, child.FindChild("RHand").transform);
                blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();
            }
        }
        public void LineVec(Vector3 linkedBodyPos)
        {
            Vector3 startPos = child.FindChild("RHand").transform.position;
            Vector3 endPos = linkedBodyPos;
            int interVal = (int)Mathf.Abs(Vector3.Distance(endPos, startPos));
            if (interVal <= 0)
            {
                interVal = 2;
            }
            Vector3[] numberofpositions = new Vector3[interVal];
            for (int i = 0; i < numberofpositions.Length; i++)
            {
                numberofpositions[i] = Vector3.Lerp(startPos, endPos, (float)i / interVal);
                numberofpositions[i].z = Mathf.Lerp(startPos.z, endPos.z, (float)i / interVal);


            }
            blackwhipLineRenderer.positionCount = interVal;
            blackwhipLineRenderer.SetPositions(numberofpositions);

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
            this.DestroyAttachment();
        }
        private void DestroyAttachment()
        {
            if (this.attachment)
            {
                UnityEngine.Object.Destroy(this.attachment.gameObject);
            }
            this.attachment = null;
            this.siphonNearbyController = null;
        }

        private void OnDestroy()
        {
            StopGobeyondLoop();
        }

    }
}

