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
        private int buffCountToApply;
        private static bool scepterActive;
        public GenericSkill specialSkillSlot;
        string prefix = DekuPlugin.developerPrefix + "_DEKU_BODY_";
        public bool fajinon;
        public Animator anim;
        public float stopwatch;
        public float fajinrate = 10f;



        public void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
            child = GetComponentInChildren<ChildLocator>();
            if (child)
            {
                OFA = child.FindChild("OFAlightning").GetComponent<ParticleSystem>();
            }
            OFA.Stop();

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

        public void AddToBuffCount(int numbertoadd)
        {
            buffCountToApply += numbertoadd;
            if (buffCountToApply >= Modules.StaticValues.fajinMaxStack)
            {
                buffCountToApply = Modules.StaticValues.fajinMaxStack;
            }
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
                
                if (anim.GetBool("isMoving") && stopwatch >= fajinrate/body.moveSpeed)
                {
                    IncrementBuffCount();
                    stopwatch = 0f;
                }
            }

            stopwatch += Time.fixedDeltaTime;
            
        }

    }
}

