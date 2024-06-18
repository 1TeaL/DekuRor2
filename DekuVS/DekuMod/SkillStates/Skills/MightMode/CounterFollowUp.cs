using EntityStates;
using ExtraSkillSlots;
using DekuMod.Modules.Survivors;
using DekuMod.Modules;
using R2API;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using R2API.Networking;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.SkillStates.Might
{
    internal class CounterFollowUp : BaseDekuSkillState
    {
        private float baseDuration = 0.8f;
        private float duration;
        public float radius = StaticValues.counterRadius;
        public Vector3 enemyPos;
        public Vector3 attackPos;
        public DamageType damageType;

        private float baseForce = 600f;
        public float procCoefficient = 1f;
        private Animator animator;
        private CharacterModel characterModel;
        private Transform modelTransform;

        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");

        private float baseattackEndTime = 0.45f;
        private float attackEndTime;
        private bool hasFired;

        public override void OnEnter()
        {

            base.OnEnter();

            hasFired = false;
            this.animator = base.GetModelAnimator();
            this.animator.SetFloat("Attack.playbackRate", 1f);

            this.animator.SetBool("releaseCounter", true);

            //AkSoundEngine.PostEvent("NoctisHitSFX", base.gameObject);
            //AkSoundEngine.PostEvent("SlamSFX", base.gameObject);

            //{
            //    AkSoundEngine.PostEvent("NoctisVoice", this.gameObject);
            //}




            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();

                TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = 0.3f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay2.duration = 0.3f;
                temporaryOverlay2.animateShaderAlpha = true;
                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay2.destroyComponentOnEnd = true;
                temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());

            }

            switch (level)
            {
                case 0:
                    damageType = DamageType.Generic;
                    attackPos = characterBody.corePosition + characterDirection.forward * 2f;
                    //play normal smash animation
                    break;
                case 1:
                    damageType = DamageType.Stun1s;
                    characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);
                    characterBody.ApplyBuff(Buffs.counterAttackBuff.buffIndex, 1, 1);
                    attackPos = characterBody.corePosition + characterDirection.forward * 2f;
                    //play danger sense + smash animation
                    break; 
                case 2:
                    characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);
                    characterBody.ApplyBuff(Buffs.counterAttackBuff.buffIndex, 1, StaticValues.counterBuffDuration);
                    attackPos = enemyPos;
                    damageType = DamageType.Stun1s;

                    characterBody.characterMotor.Motor.SetPositionAndRotation(enemyPos + (enemyPos - characterBody.corePosition).normalized * 2f, Quaternion.LookRotation(enemyPos - characterBody.corePosition).normalized, true);
                    //play animation of deku going nothing personnel enemy behind him style
                    break;
            }

            attackEndTime = baseattackEndTime / attackSpeedStat;
            duration = baseDuration/ attackSpeedStat;


        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge > this.baseDuration * this.attackEndTime)
            {
                if (!hasFired)
                {
                    FireAttack();
                    hasFired = true;
                }
               
            }

            if (base.fixedAge > this.baseDuration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void FireAttack()
        {

            //Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * radius / 2f);
            //effectPosition.y = base.characterBody.footPosition.y;
            EffectManager.SpawnEffect(blastEffectPrefab, new EffectData
            {
                origin = attackPos,
                scale = radius / 2f,
            }, true);
            

            bool isAuthority = base.isAuthority;
            if (isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();

                blastAttack.position = attackPos;
                blastAttack.baseDamage = this.damageStat * StaticValues.counterDamageCoefficient * attackSpeedStat;
                blastAttack.baseForce = this.baseForce;
                blastAttack.radius = this.radius * attackSpeedStat;
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = base.teamComponent.teamIndex;
                blastAttack.crit = base.RollCrit();
                blastAttack.procChainMask = default(ProcChainMask);
                blastAttack.procCoefficient = procCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.damageColorIndex = DamageColorIndex.Default;
                blastAttack.damageType = damageType;
                blastAttack.attackerFiltering = AttackerFiltering.Default;

                blastAttack.Fire();
                
            }

        }

        public override void OnExit()
        {
            base.OnExit();

            if (characterBody.HasBuff(Buffs.counterBuff))
            {
                characterBody.ApplyBuff(Buffs.counterBuff.buffIndex, 0);
            }
        }
    }
}