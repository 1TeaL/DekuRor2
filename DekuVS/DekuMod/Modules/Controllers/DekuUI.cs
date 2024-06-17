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
        public CharacterBody characterBody;

        //UI plusUltraMeter
        public GameObject CustomUIObject;
        public RectTransform plusUltraMeter;
        public RectTransform plusUltraMeterGlowRect;
        public Image plusUltraMeterGlowBackground;
        public HGTextMeshProUGUI plusUltraNumber;
        public Animator anim;

        //Energy system
        public float maxPlusUltra;
        public float currentPlusUltra;
        public float regenPlusUltra;
        public float plusUltraGain;
        public bool SetActiveTrue;
        public float plusUltraTimer;

        //Energy bar glow
        private enum GlowState
        {
            STOP,
            FLASH,
            DECAY
        }
        private float decayConst;
        private float flashConst;
        private float glowStopwatch;
        private Color targetColor;
        private Color originalColor;
        private Color currentColor;
        private GlowState state;
        //bools to stop energy regen after skill used
        private bool ifEnergyUsed;
        private float energyDecayTimer;
        private bool ifEnergyRegenAllowed;

        public void Awake()
        {
            characterBody = gameObject.GetComponent<CharacterBody>();
            anim = GetComponentInChildren<Animator>();
        }

        public void Start()
        {

            //Energy
            maxPlusUltra = StaticValues.maxPlusUltra;
            currentPlusUltra = 0f;
            regenPlusUltra = StaticValues.regenPlusUltraRate;
            plusUltraGain = StaticValues.basePlusUltraGain;
            ifEnergyRegenAllowed = true;

            //UI objects 
            CustomUIObject = UnityEngine.Object.Instantiate(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("dekuCustomUI"));
            CustomUIObject.SetActive(false);
            SetActiveTrue = false;

            plusUltraMeter = CustomUIObject.transform.GetChild(0).GetComponent<RectTransform>();
            plusUltraMeterGlowBackground = CustomUIObject.transform.GetChild(1).GetComponent<Image>();
            plusUltraMeterGlowRect = CustomUIObject.transform.GetChild(1).GetComponent<RectTransform>();

            //setup the UI element for the min/max
            plusUltraNumber = this.CreateLabel(CustomUIObject.transform, "plusUltraNumber", $"{(int)currentPlusUltra} / {maxPlusUltra}", new Vector2(0, -110), 24f);


            // Start timer on 1f to turn off the timer.
            state = GlowState.STOP;
            decayConst = 1f;
            flashConst = 1f;
            glowStopwatch = 1f;
            originalColor = new Color(1f, 1f, 1f, 0f);
            targetColor = new Color(1f, 1f, 1f, 1f);
            currentColor = originalColor;

        }

        //Creates the label.
        private HGTextMeshProUGUI CreateLabel(Transform parent, string name, string text, Vector2 position, float textScale)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.parent = parent;
            gameObject.AddComponent<CanvasRenderer>();
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            HGTextMeshProUGUI hgtextMeshProUGUI = gameObject.AddComponent<HGTextMeshProUGUI>();
            hgtextMeshProUGUI.enabled = true;
            hgtextMeshProUGUI.text = text;
            hgtextMeshProUGUI.fontSize = textScale;
            hgtextMeshProUGUI.color = new Color(0f, 1f, 0.8f, 1f);
            hgtextMeshProUGUI.alignment = TextAlignmentOptions.Center;
            hgtextMeshProUGUI.enableWordWrapping = false;
            rectTransform.localPosition = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = position;
            return hgtextMeshProUGUI;
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
                if (anim)
                {
                    if (anim.GetBool("isMoving"))
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
                }
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

            if (plusUltraNumber)
            {
                plusUltraNumber.SetText($"{(int)currentPlusUltra} / {maxPlusUltra}");
            }

            if (plusUltraMeter)
            {
                // 2f because meter is too small probably.
                // Logarithmically scale.
                float linear = currentPlusUltra / maxPlusUltra;
                float logVal = Mathf.Log10(((maxPlusUltra / StaticValues.maxPlusUltra) * 10f) + 1) * (currentPlusUltra / maxPlusUltra);
                plusUltraMeter.localScale = new Vector3(2.0f * linear, 0.05f, 1f);
                plusUltraMeterGlowRect.localScale = new Vector3(2.3f * linear, 0.1f, 1f);
            }

            //Chat.AddMessage($"{currentPlusUltra}/{maxPlusUltra}");
            //particles

        }

        public void FixedUpdate()
        {
            CalculateEnergyStats();

            if (characterBody.hasAuthority && !SetActiveTrue)
            {
                CustomUIObject.SetActive(true);
                SetActiveTrue = true;
            }
        }

        public void Update()
        {
            if (state != GlowState.STOP)
            {
                glowStopwatch += Time.deltaTime;
                float lerpFraction;
                switch (state)
                {
                    // Lerp to target color
                    case GlowState.FLASH:

                        lerpFraction = glowStopwatch / flashConst;
                        currentColor = Color.Lerp(originalColor, targetColor, lerpFraction);

                        if (glowStopwatch > flashConst)
                        {
                            state = GlowState.DECAY;
                            glowStopwatch = 0f;
                        }
                        break;

                    //Lerp back to original color;
                    case GlowState.DECAY:
                        //Linearlly lerp.
                        lerpFraction = glowStopwatch / decayConst;
                        currentColor = Color.Lerp(targetColor, originalColor, lerpFraction);

                        if (glowStopwatch > decayConst)
                        {
                            state = GlowState.STOP;
                            glowStopwatch = 0f;
                        }
                        break;
                    case GlowState.STOP:
                        //State does nothing.
                        break;
                }
            }

            plusUltraMeterGlowBackground.color = currentColor;
        }

        public void GainPlusUltra(float Energy)
        {
            if (ifEnergyRegenAllowed)
            {
                currentPlusUltra += Energy;
                TriggerGlow(0.3f, 0.3f, Color.white);
            }
        }

        public void SpendPlusUltra(float Energy)
        {
            ifEnergyUsed = true;
            currentPlusUltra -= Energy;
            TriggerGlow(0.3f, 0.3f, Color.green);
        }

        public void TriggerGlow(float newDecayTimer, float newFlashTimer, Color newStartingColor)
        {
            decayConst = newDecayTimer;
            flashConst = newFlashTimer;
            originalColor = new Color(newStartingColor.r, newStartingColor.g, newStartingColor.b, 0f);
            targetColor = newStartingColor;
            glowStopwatch = 0f;
            state = GlowState.FLASH;
        }


        public void OnDestroy()
        {
            Destroy(CustomUIObject);
        }
    }
}

