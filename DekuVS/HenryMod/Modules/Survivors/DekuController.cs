using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using EntityStates;
using System.Runtime.CompilerServices;
using AncientScepter;

namespace DekuMod.Modules.Survivors
{
    public class DekuController : MonoBehaviour
    {
        public ChildLocator child;
        public CharacterBody body;
        public ParticleSystem OFA;
        public ParticleSystem FAJIN;
        private int buffCountToApply;
        public GenericSkill specialSkillSlot;
        string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";
        public bool fajinon;
        public bool fajinscepteron;
        public Animator anim;
        public float stopwatch;
        public static float fajinscepterrate = 2.5f;
        public float fajinrate = 5f;
        public bool isMaxPower;
        public bool canPull;

        public void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
            child = GetComponentInChildren<ChildLocator>();
            if (child)
            {
                OFA = child.FindChild("OFAlightning").GetComponent<ParticleSystem>();
                FAJIN = child.FindChild("FAJINaura").GetComponent<ParticleSystem>();
            }
            OFA.Stop();
            FAJIN.Stop();
            anim = GetComponentInChildren<Animator>();
            stopwatch = 0f;
            canPull = false;

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

        

        public void FixedUpdate()
        {
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
            
        }

    }
}

