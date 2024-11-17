using System;
using System.Reflection;
using Newtonsoft.Json;
using RoR2;
using RoR2.CharacterAI;
using RoR2.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DekuMod.Modules.Survivors
{
    public class DekuUI : MonoBehaviour
    {
        public CharacterBody characterBody;
        private CharacterMaster characterMaster;

        //UI plusUltraMeter
        private GameObject RoRHUDObject;
        public GameObject plusUltraBar;
        public Image plusUltraFilling;
        public GameObject plusUltraFilled1;
        public GameObject plusUltraFilled2;
        public GameObject plusUltraFilled3;
        public GameObject plusUltraGlow;

        private bool Initialized;
        private DekuController dekucon;
        public bool baseAIPresent;

        public Animator anim;


        #region Hook
        public void Hook()
        {
            //On.RoR2.CameraRigController.Update += CameraRigController_Update;
            On.RoR2.UI.HUD.Update += HUD_Update;
        }

        public void Unhook()
        {
            //On.RoR2.CameraRigController.Update -= CameraRigController_Update;
            On.RoR2.UI.HUD.Update -= HUD_Update;
        }

        private void HUD_Update(On.RoR2.UI.HUD.orig_Update orig, HUD self)
        {
            orig(self);
            if (!RoRHUDObject)
            {
                RoRHUDObject = self.gameObject;
            }
        }

        #endregion

        public void Start()
        {
            dekucon = gameObject.GetComponent<DekuController>();
            baseAIPresent = false;
            characterBody = gameObject.GetComponent<CharacterBody>();
            anim = GetComponentInChildren<Animator>();
            characterMaster = characterBody.master;

            //UI objects 
            Initialized = false;
            BaseAI baseAI = characterMaster.GetComponent<BaseAI>();
            baseAIPresent = baseAI;
            Hook();


            //For some reason on goboo's first spawn the master is just not there. However subsequent spawns work.
            // Disable the UI in this event.
            // Besides, there should never be a UI element related to a non-existant master on screen if the attached master/charbody does not exist.
            if (!characterMaster) baseAIPresent = true; // Disable UI Just in case.

            try
            {
                InitializeUI();
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Deku - NRE on UI Initialization, trying again.");
            }
        }

        public void InitializeUI()
        {
            if (!Initialized && !baseAIPresent)
            {
                InitializePlusUltraMeter();
                //Now we need to initialize everything inside the canvas to variables we can control.

                Initialized = true;
            }
        }

        private void InitializePlusUltraMeter()
        {

            if (RoRHUDObject && !plusUltraBar)
            {
                /// 
                HUD hud = RoRHUDObject.GetComponent<HUD>();
                if (hud.healthBar.barContainer)
                {
                    Transform healthbarTransform = hud.healthBar.barContainer.transform;
                    if (healthbarTransform)
                    {
                        plusUltraBar = UnityEngine.GameObject.Instantiate(Modules.DekuAssets.dekuCustomUI, healthbarTransform);
                    }
                }

            }

            plusUltraGlow = plusUltraBar.transform.GetChild(0).gameObject;
            plusUltraFilling = plusUltraBar.transform.GetChild(2).GetComponent<Image>();
            plusUltraFilled1 = plusUltraBar.transform.GetChild(3).gameObject;
            plusUltraFilled2 = plusUltraBar.transform.GetChild(4).gameObject;
            plusUltraFilled3 = plusUltraBar.transform.GetChild(5).gameObject;

            plusUltraGlow.SetActive(false);
            plusUltraFilled1.SetActive(false);
            plusUltraFilled2.SetActive(false);
            plusUltraFilled3.SetActive(false);
        }

        public void FixedUpdate()
        {

        }
                
         
        public void Update()
        {

            if (characterBody.hasEffectiveAuthority)
            {
                if (!Initialized)
                {
                    try
                    {
                        InitializeUI();
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.Log("Deku - NRE on UI Initialization, trying again.");    
                    }
                }

            }
        }        

        public void UpdatePlusUltraMeter(float plusUltraAmount)
        {
            if (!plusUltraFilling)
            {
                return;
            }

            if(plusUltraAmount > StaticValues.maxPlusUltra)
            {
                plusUltraAmount = StaticValues.maxPlusUltra;
            }
            plusUltraFilling.fillAmount = plusUltraAmount/StaticValues.maxPlusUltra;

            if(plusUltraAmount/StaticValues.maxPlusUltra >= 1 / 3f)
            {
                plusUltraFilled1.SetActive(true);
                if (dekucon)
                {
                    if (dekucon.PLUSULTRA1.isStopped)
                    {
                        dekucon.PLUSULTRA1.Play();
                    }

                }
            }
            else
            {
                plusUltraFilled1.SetActive(false);
                if (dekucon)
                {
                    if (dekucon.PLUSULTRA1.isPlaying)
                    {
                        dekucon.PLUSULTRA1.Stop();
                    }

                }
            }

            if (plusUltraAmount / StaticValues.maxPlusUltra >= 2 / 3f)
            {
                plusUltraFilled2.SetActive(true);
                if (dekucon)
                {
                    if (dekucon.PLUSULTRA2.isStopped)
                    {
                        dekucon.PLUSULTRA2.Play();
                    }

                }
            }
            else
            {
                plusUltraFilled2.SetActive(false);
                if (dekucon)
                {
                    if (dekucon.PLUSULTRA2.isPlaying)
                    {
                        dekucon.PLUSULTRA2.Stop();
                    }

                }
            }

            if (plusUltraAmount / StaticValues.maxPlusUltra >= 3 / 3f)
            {
                plusUltraFilled3.SetActive(true);
                if (dekucon)
                {
                    if (dekucon.PLUSULTRA3.isStopped)
                    {
                        dekucon.PLUSULTRA3.Play();
                    }

                }

            }
            else
            {
                plusUltraFilled3.SetActive(false);
                if (dekucon)
                {
                    if (dekucon.PLUSULTRA3.isPlaying)
                    {
                        dekucon.PLUSULTRA3.Stop();
                    }

                }
            }

        }

        public void OnDestroy()
        {
            Destroy(plusUltraBar);
            Unhook();
        }
    }
}

