using System;
using RoR2;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DekuMod.Modules.Survivors
{
    public class EnergySystem : MonoBehaviour
    {
        public DekuUI dekuUICon;

        public CharacterBody characterBody;

        //Energy system
        public float maxPlusUltra;
        public float currentPlusUltra;
        public float regenPlusUltra;
        public float plusUltraGain;
        public float plusUltraTimer;

        //bools to stop energy regen after skill used
        private bool ifEnergyUsed;
        private float energyDecayTimer;
        private bool ifEnergyRegenAllowed;


        public void Start()
        {
            characterBody = gameObject.GetComponent<CharacterBody>();
            dekuUICon = gameObject.GetComponent<DekuUI>();

            //Energy
            maxPlusUltra = StaticValues.maxPlusUltra;
            currentPlusUltra = 0f;
            regenPlusUltra = StaticValues.regenPlusUltraRate;
            plusUltraGain = StaticValues.basePlusUltraGain;
            ifEnergyRegenAllowed = true;



        }


        private void CalculateEnergyStats()
        {
            //Energy updates
            if (characterBody.HasBuff(Buffs.goBeyondBuff.buffIndex))
            {
                currentPlusUltra += StaticValues.goBeyondBuffGain;
            }

            if (ifEnergyRegenAllowed)
            {
                if (characterBody.characterMotor.velocity != Vector3.zero)
                {
                    if (!characterBody.HasBuff(Modules.Buffs.ofaBuff) || !characterBody.HasBuff(Modules.Buffs.ofaBuff) || !characterBody.HasBuff(Modules.Buffs.ofaBuff) || !characterBody.HasBuff(Modules.Buffs.ofaBuff))
                    {
                        plusUltraTimer += Time.fixedDeltaTime;
                        if (plusUltraTimer >= regenPlusUltra / characterBody.moveSpeed)
                        {
                            currentPlusUltra += StaticValues.basePlusUltraGain;
                            plusUltraTimer = 0f;
                        }

                    }
                }

                currentPlusUltra += StaticValues.basePlusUltraGain * Time.fixedDeltaTime;
                
            }

            if (ifEnergyUsed)
            {
                if (energyDecayTimer > 1f)
                {
                    energyDecayTimer = 0f;
                    ifEnergyRegenAllowed = true;
                    ifEnergyUsed = false;
                }
                else
                {
                    ifEnergyRegenAllowed = false;
                    energyDecayTimer += Time.fixedDeltaTime;
                }
            }

            if (currentPlusUltra > maxPlusUltra)
            {
                currentPlusUltra = maxPlusUltra;
            }
            if (currentPlusUltra < 0f)
            {
                currentPlusUltra = 0f;
            }


            dekuUICon.UpdatePlusUltraMeter(currentPlusUltra);
            //Chat.AddMessage($"{currentPlusUltra}/{maxPlusUltra}");
            //particles

        }

        public void FixedUpdate()
        {
            CalculateEnergyStats();
        }

        public void Update()
        {
        }

        public void GainPlusUltra(float Energy)
        {
            if (ifEnergyRegenAllowed)
            {
                currentPlusUltra += Energy;
            }
        }

        public void SpendPlusUltra(float Energy)
        {
            ifEnergyUsed = true;
            currentPlusUltra -= Energy;
        }

    }
}

