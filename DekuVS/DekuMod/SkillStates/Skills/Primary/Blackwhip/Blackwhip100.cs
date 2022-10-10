using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Networking;
using R2API.Networking.Interfaces;
using R2API.Networking;

namespace DekuMod.SkillStates
{
    public class Blackwhip100 : BaseQuirk100
    {
        private BlastAttack blastAttack;

        public float baseDuration = 0.5f;
        private float duration;
        public float fireTime;
        public float timer;
        public bool hasFired;

        public static float blastRadius = Modules.StaticValues.blackwhip100Range;
        public static float succForce = 5f;

        public Vector3 theSpot;
        public float whipage;


        public override void OnEnter()
        {
            base.OnEnter();
            

        }

        protected override void DoSkill()
        {
            base.DoSkill();

            Ray aimRay = base.GetAimRay();
            base.StartAimMode(duration, true);
            this.duration = this.baseDuration / attackSpeedStat;
            fireTime = duration / 2f;
            timer = 0f;
            hasFired = false;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);

            theSpot = aimRay.origin + 0.5f * attackSpeedStat * blastRadius * aimRay.direction;
            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("blackwhipvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("blackwhipsfx", this.gameObject);



            //EffectManager.SpawnEffect(Modules.Assets.blackwhipforward, new EffectData
            //{
            //    origin = aimRay.origin,
            //    scale = 1f,
            //    rotation = Quaternion.LookRotation(aimRay.direction),

            //}, true);
            if (NetworkServer.active)
            {
                characterBody.AddTimedBuffAuthority(Modules.Buffs.blackwhipBuff.buffIndex, Modules.StaticValues.blackwhip100DebuffDuration);
            }

            if (!dekucon.attachment)
            {
                dekucon.attachment = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/BodyAttachments/SiphonNearbyBodyAttachment")).GetComponent<NetworkedBodyAttachment>();
                dekucon.attachment.AttachToGameObjectAndSpawn(base.gameObject, null);
            }
            if (!dekucon.siphonNearbyController)
            {
                dekucon.siphonNearbyController = dekucon.attachment.GetComponent<SiphonNearbyController>();
                dekucon.siphonNearbyController.damagePerSecondCoefficient = Modules.StaticValues.blackwhip100DamageCoefficient;
                dekucon.siphonNearbyController.Networkradius = Modules.StaticValues.blackwhip100Range;
                dekucon.siphonNearbyController.NetworkmaxTargets =  Modules.StaticValues.blackwhip100Targets;
            }


            if (base.isAuthority)
            {
                new SpendHealthNetworkRequest(characterBody.masterObjectId, Modules.StaticValues.blackwhip100HealthCostFraction * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
            }

        }


        public override void OnExit()
        {

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= fireTime && base.isAuthority && hasFired)
            {
                base.PlayCrossfade("FullBody, Override", "Blackwhip", "Attack.playbackRate", fireTime, 0.05f);
                hasFired = true;
            }

            
            if ((base.fixedAge >= this.duration && base.isAuthority))
            {
                this.outer.SetNextStateToMain();
                return;
            }


        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
