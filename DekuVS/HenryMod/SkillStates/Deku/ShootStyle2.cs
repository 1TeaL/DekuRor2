//using EntityStates;
//using RoR2;
//using UnityEngine;
//using UnityEngine.Networking;
//using RoR2.Audio;
//using System;
//using EntityStates.Merc;
//using HenryMod.SkillStates.BaseStates;

//namespace DekuMod.SkillStates
//{
//    public class ShootStyle : BaseMeleeAttack
//    {
//        public float speedCoefficientOnExit;
//        public float speedCoefficient;
//        public string endSoundString;
//        public float exitSmallHop;
//        public float delayedDamageCoefficient;
//        public float delayedProcCoefficient;
//        public float delay;
//        public string enterAnimationLayerName = "FullBody, Override";
//        public string enterAnimationStateName = "LegSmash";
//        public float enterAnimationCrossfadeDuration = 0.1f;
//        public string exitAnimationLayerName = "Body";
//        public string exitAnimationStateName = "IdleIn";
//        public Material enterOverlayMaterial;
//        public float enterOverlayDuration = 0.7f;
//        public GameObject delayedEffectPrefab;
//        public GameObject orbEffect;
//        public float delayPerHit;
//        public GameObject selfOnHitOverlayEffectPrefab;
//        private Transform modelTransform;
//        private Vector3 dashVector;
//        private int currentHitCount;


//        //protected string hitboxName = "Sword";

//        //protected DamageType damageType = DamageType.Generic;
//        //protected float damageCoefficient = 3.5f;
//        //protected float procCoefficient = 1f;
//        //protected float pushForce = 300f;
//        //protected Vector3 bonusForce = Vector3.zero;
//        //protected float baseDuration = 1f;
//        //protected float attackStartTime = 0.2f;
//        //protected float attackEndTime = 0.4f;
//        //protected float baseEarlyExitTime = 0.4f;
//        //protected float hitStopDuration = 0.012f;
//        //protected float attackRecoil = 0.75f;
//        //protected float hitHopVelocity = 4f;
//        //protected bool cancelled = false;

//        //protected string swingSoundString = "";
//        //protected string hitSoundString = "";
//        //protected string muzzleString = "RShoulder";
//        //protected GameObject swingEffectPrefab;
//        //protected GameObject hitEffectPrefab;
//        //protected NetworkSoundEventIndex impactSound;

//        //private float earlyExitTime;
//        //public float duration;
//        //private bool hasFired;
//        //private float hitPauseTimer;
//        //private OverlapAttack attack;
//        //protected bool inHitPause;
//        //private bool hasHopped;
//        //protected float stopwatch;
//        //protected Animator animator;
//        //private BaseState.HitStopCachedState hitStopCachedState;
//        //private Vector3 storedVelocity;


//        private Vector3 dashVelocity
//        {
//            get
//            {
//                return this.dashVector * this.moveSpeedStat * this.speedCoefficient;
//            }
//        }


//        public override void OnEnter()
//        {
//            base.OnEnter();
//            this.dashVector = base.inputBank.aimDirection;
//            base.gameObject.layer = LayerIndex.fakeActor.intVal;
//            base.characterMotor.Motor.RebuildCollidableLayers();
//            base.characterMotor.Motor.ForceUnground();
//            base.characterMotor.velocity = Vector3.zero;
//            this.modelTransform = base.GetModelTransform();
//            if (this.modelTransform)
//            {
//                TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
//                temporaryOverlay.duration = this.enterOverlayDuration;
//                temporaryOverlay.animateShaderAlpha = true;
//                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
//                temporaryOverlay.destroyComponentOnEnd = true;
//                temporaryOverlay.originalMaterial = this.enterOverlayMaterial;
//                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
//            }
//            //base.PlayCrossfade(this.enterAnimationLayerName, this.enterAnimationStateName, this.enterAnimationCrossfadeDuration);
//            //base.characterDirection.forward = base.characterMotor.velocity.normalized;
//            //if (NetworkServer.active)
//            //{
//            //    base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
//            //}
//            //this.duration = this.baseDuration / this.attackSpeedStat;
//            //this.earlyExitTime = this.baseEarlyExitTime / this.attackSpeedStat;
//            //this.hasFired = false;
//            //this.animator = base.GetModelAnimator();
//            //base.StartAimMode(0.5f + this.duration, false);
//            //base.characterBody.outOfCombatStopwatch = 0f;
//            //this.animator.SetBool("attacking", true);

//            //HitBoxGroup hitBoxGroup = null;
//            //Transform modelTransform = base.GetModelTransform();

//            //if (modelTransform)
//            //{
//            //    hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
//            //}

//            //this.PlayCrossfade(this.enterAnimationLayerName, this.enterAnimationStateName, this.enterAnimationCrossfadeDuration);

//            //this.attack = new OverlapAttack();
//            //this.attack.damageType = this.damageType;
//            //this.attack.attacker = base.gameObject;
//            //this.attack.inflictor = base.gameObject;
//            //this.attack.teamIndex = base.GetTeam();
//            //this.attack.damage = this.damageCoefficient * this.damageStat;
//            //this.attack.procCoefficient = this.procCoefficient;
//            //this.attack.hitEffectPrefab = this.hitEffectPrefab;
//            //this.attack.forceVector = this.bonusForce;
//            //this.attack.pushAwayForce = this.pushForce;
//            //this.attack.hitBoxGroup = hitBoxGroup;
//            //this.attack.isCrit = base.RollCrit();
//            //this.attack.impactSound = this.impactSound;
//        }

//        public override void OnExit()
//        {
//            if (NetworkServer.active)
//            {
//                base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
//            }
//            base.characterMotor.velocity *= this.speedCoefficientOnExit;
//            base.SmallHop(base.characterMotor, this.exitSmallHop);
//            Util.PlaySound(this.endSoundString, base.gameObject);
//            base.PlayAnimation(this.exitAnimationLayerName, this.exitAnimationStateName);
//            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
//            base.characterMotor.Motor.RebuildCollidableLayers();
//            base.OnExit();
//        }


//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();
//            if (!base.inHitPause)
//            {
//                base.characterMotor.rootMotion += this.dashVelocity * Time.fixedDeltaTime;
//                base.characterDirection.forward = this.dashVelocity;
//                base.characterDirection.moveVector = this.dashVelocity;
//                base.characterBody.isSprinting = true;

//            }
//        }

//    }
//}
