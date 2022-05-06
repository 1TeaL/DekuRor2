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

namespace DekuMod.Modules.Survivors
{
    public class DekuController : MonoBehaviour
    {
        public ChildLocator child;
        public CharacterBody body;
        public ParticleSystem OFA;
        public ParticleSystem OFAeye;
        public ParticleSystem FAJIN;
        public ParticleSystem OKLAHOMA;
        public ParticleSystem DANGERSENSE;
        private int buffCountToApply;
        public GenericSkill specialSkillSlot;
        string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";
        public bool fajinon;
        public bool fajinscepteron;
        public Animator anim;
        public float stopwatch;
        public static float fajinscepterrate = 2f;
        public float fajinrate = 4f;
        public bool isMaxPower;
        public float oklahomacount;

        public bool kickBuff;
        public bool kickon;

        public bool countershouldflip;
        public bool dangersensefreeze;

        internal bool endFloat;
        internal bool hasFloatBuff;
        private Stopwatch floatStopwatch;
        private CharacterBody characterBody;

        public void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
            child = GetComponentInChildren<ChildLocator>();
            if (child)
            {
                OFA = child.FindChild("OFAlightning").GetComponent<ParticleSystem>();
                OFAeye = child.FindChild("OFAlightningeye").GetComponent<ParticleSystem>();
                FAJIN = child.FindChild("FAJINaura").GetComponent<ParticleSystem>();
                OKLAHOMA = child.FindChild("Oklahoma").GetComponent<ParticleSystem>();
                DANGERSENSE = child.FindChild("Dangersense").GetComponent<ParticleSystem>();
            }
            OFAeye.Stop();
            OFA.Stop();
            FAJIN.Stop();
            OKLAHOMA.Stop();
            DANGERSENSE.Stop();
            anim = GetComponentInChildren<Animator>();
            stopwatch = 0f;

        }


        public void IncrementBuffCount()
        {
            buffCountToApply++;
            if (buffCountToApply >= Modules.StaticValues.fajinMaxStack)
            {
                buffCountToApply = Modules.StaticValues.fajinMaxStack;
            }
        }

        public void RemoveBuffCount(int numbertominus)
        {
            buffCountToApply -= numbertominus;
            if (buffCountToApply < 0)
            {
                buffCountToApply = 0;
            }
        }
        public void AddToBuffCount(int numbertoadd)
        {
            buffCountToApply += numbertoadd;
            if (buffCountToApply >= Modules.StaticValues.fajinMaxStack)
            {
                buffCountToApply = Modules.StaticValues.fajinMaxStack;
            }
        }

        public bool CheckIfMaxPowerStacks()
        {
            if (buffCountToApply >= Modules.StaticValues.fajinMaxPower)
            {
                isMaxPower = true;
            }
            else
            {
                isMaxPower = false;
            }
            return isMaxPower;
        }

        public int GetBuffCount()
        {
            if (buffCountToApply > Modules.StaticValues.fajinMaxStack)
            {
                return Modules.StaticValues.fajinMaxStack;
            }
            return buffCountToApply;
        }

        public bool CheckIfMaxKickPowerStacks()
        {
            if (buffCountToApply >= Modules.StaticValues.kickMaxStack)
            {
                kickBuff = true;
            }
            else
            {
                kickBuff = false;
            }
            return kickBuff;
        }

        public void IncrementKickBuffCount()
        {
            buffCountToApply++;
            if (buffCountToApply >= Modules.StaticValues.kickMaxStack)
            {
                buffCountToApply = Modules.StaticValues.kickMaxStack;
            }
        }

        public void RemoveKickBuffCount(int numbertominus)
        {
            buffCountToApply -= numbertominus;
            if (buffCountToApply < 0)
            {
                buffCountToApply = 0;
            }
        }

        public int GetKickBuffCount()
        {
            if (buffCountToApply > Modules.StaticValues.kickMaxStack)
            {
                return Modules.StaticValues.kickMaxStack;
            }
            return buffCountToApply;
        }

        public void FixedUpdate()
        {

            CheckIfMaxKickPowerStacks();

            if (fajinon)
            {
                CheckIfMaxPowerStacks();
                if (isMaxPower)
                {
                    FAJIN.Play();
                }
                else
                {
                    FAJIN.Stop();
                }
                if (fajinscepteron)
                {
                    if (anim.GetBool("isMoving") && stopwatch >= fajinscepterrate / body.moveSpeed)
                    {
                        IncrementBuffCount();
                        stopwatch = 0f;
                    }
                }
                else
                {
                    if (anim.GetBool("isMoving") && stopwatch >= fajinrate / body.moveSpeed)
                    {
                        IncrementBuffCount();
                        stopwatch = 0f;
                    }
                }

            }
            stopwatch += Time.fixedDeltaTime;

            
            //if (body.HasBuff(Modules.Buffs.counterBuff))
            //{
            //    DANGERSENSE.Play();
            //}
            //if (!body.HasBuff(Modules.Buffs.counterBuff))
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
    }
}

