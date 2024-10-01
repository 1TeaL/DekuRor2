using EntityStates;
using ExtraSkillSlots;
using DekuMod.Modules;
using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.SkillStates.Might
{
    public class CounterSmash : BaseDekuSkillState
    {
        public ExtraInputBankTest extrainputBankTest;
        private ExtraSkillLocator extraskillLocator;
        public Animator animator;
        private CharacterModel characterModel;
        private Transform modelTransform;
        private bool activateCounter;
        private Vector3 enemyPos;

        public enum DangerState {STARTBUFF, CHECKCOUNTER};
        public DangerState state;
        public DamageType damageType = DamageType.Generic;

        public override void OnEnter()
        {
            base.OnEnter();


            extraskillLocator = characterBody.gameObject.GetComponent<ExtraSkillLocator>();
            extrainputBankTest = characterBody.gameObject.GetComponent<ExtraInputBankTest>();

            this.animator = base.GetModelAnimator();
            this.animator.SetBool("releaseCounter", false);
            base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);

            base.PlayCrossfade("FullBody, Override", "CounterStance", "Attack.playbackRate", 1f, 0.05f);

            state = DangerState.STARTBUFF;

                                             
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;


            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();

                //TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                //temporaryOverlay.duration = 1f;
                //temporaryOverlay.animateShaderAlpha = true;
                //temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                //temporaryOverlay.destroyComponentOnEnd = true;
                //temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                //temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                //TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                //temporaryOverlay2.duration = 1f;
                //temporaryOverlay2.animateShaderAlpha = true;
                //temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                //temporaryOverlay2.destroyComponentOnEnd = true;
                //temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                //temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());

            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {

            if (damageInfo != null && damageInfo.attacker && damageInfo.attacker.GetComponent<CharacterBody>())
            {
                bool flag = (damageInfo.damageType & DamageType.BypassArmor) > DamageType.Generic;
                if (!flag && damageInfo.damage > 0f)
                {
                    if (self.body.HasBuff(Modules.Buffs.counterBuff.buffIndex))
                    {
                        if (damageInfo.attacker != self)
                        {
                            switch (level)
                            {
                                case 0:
                                    break;
                                case 1:
                                    damageInfo.damage *= StaticValues.counterDamageReduction;
                                    break;
                                case 2:
                                    damageInfo.rejected = true;
                                    break;
                            }

                            activateCounter = true;
                            enemyPos = damageInfo.attacker.transform.position;
                        }

                    }

                }
        

            }
            orig.Invoke(self, damageInfo);
        }


        public override void OnExit()
        {
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            this.animator.SetBool("releaseCounter", true);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            switch (state)
            {
                case DangerState.STARTBUFF:
                    if (!base.characterBody.HasBuff(Modules.Buffs.counterBuff.buffIndex))
                    {
                        bool active = NetworkServer.active;
                        if (active)
                        {
                            base.characterBody.ApplyBuff(Modules.Buffs.counterBuff.buffIndex, 1);

                        }
                        state = DangerState.CHECKCOUNTER;
                    }
                    break;
                case DangerState.CHECKCOUNTER:

                    //TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    //temporaryOverlay.duration = 1f;
                    //temporaryOverlay.animateShaderAlpha = true;
                    //temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    //temporaryOverlay.destroyComponentOnEnd = true;
                    //temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                    //temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                    //TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    //temporaryOverlay2.duration = 1f;
                    //temporaryOverlay2.animateShaderAlpha = true;
                    //temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    //temporaryOverlay2.destroyComponentOnEnd = true;
                    //temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                    //temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());

                    if (!base.IsKeyDownAuthority())
                    {
                        base.characterBody.ApplyBuff(Modules.Buffs.counterBuff.buffIndex, 0);
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    if (fixedAge >= StaticValues.counterDuration)
                    {
                        base.characterBody.ApplyBuff(Modules.Buffs.counterBuff.buffIndex, 0);
                        this.animator.SetBool("releaseCounter", true);
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    if (activateCounter)
                    {
                        this.animator.SetBool("releaseCounter", true);
                        new ForceCounterState(characterBody.masterObjectId, level, enemyPos).Send(NetworkDestination.Clients);
                        return;
                    }

                    break; 
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}