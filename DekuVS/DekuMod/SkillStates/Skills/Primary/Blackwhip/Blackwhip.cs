using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Networking;
using R2API.Networking.Interfaces;
using R2API.Networking;
using System;

namespace DekuMod.SkillStates
{
    public class Blackwhip : BaseQuirk
    {
        private float duration = 0.5f;
        public float fireTime= 0.25f;
        public bool hasFired;

        public static float blastRadius = Modules.StaticValues.blackwhipRange;
        public Vector3 theSpot;
        public float whipage;


        public override void OnEnter()
        {
            base.OnEnter();
            hasFired = false;
            duration /= attackSpeedStat;
            theSpot = base.transform.position;
        }

        protected override void DoSkill()
        {
            base.DoSkill();

            Ray aimRay = base.GetAimRay();
            base.StartAimMode(duration, true);

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);

            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("blackwhipvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("blackwhipsfx", this.gameObject);



            EffectManager.SpawnEffect(Modules.Assets.blackwhip, new EffectData
            {
                origin = FindModelChild("RHand").transform.position,
                scale = 1f,
                rotation = Quaternion.LookRotation(aimRay.direction),

            }, true);
            if (NetworkServer.active)
            {
                characterBody.AddTimedBuffAuthority(Modules.Buffs.blackwhipBuff.buffIndex, Modules.StaticValues.blackwhipDebuffDuration);
            }

            if (!dekucon.attachment)
            {
                dekucon.attachment = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/BodyAttachments/SiphonNearbyBodyAttachment")).GetComponent<NetworkedBodyAttachment>();
                dekucon.attachment.AttachToGameObjectAndSpawn(base.gameObject, null);
            }
            if (!dekucon.siphonNearbyController)
            {
                dekucon.siphonNearbyController = dekucon.attachment.GetComponent<SiphonNearbyController>();
                dekucon.siphonNearbyController.damagePerSecondCoefficient = Modules.StaticValues.blackwhipDamageCoefficient;
                dekucon.siphonNearbyController.Networkradius = Modules.StaticValues.blackwhipRange;
                dekucon.siphonNearbyController.NetworkmaxTargets =  Modules.StaticValues.blackwhipTargets;
            }
            ApplyDebuff();

        }
        public void ApplyDebuff()
        {
            Ray aimRay = base.GetAimRay();
            theSpot = base.transform.position;
            BullseyeSearch search = new BullseyeSearch
            {

                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = theSpot,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = Modules.StaticValues.blackwhipRange,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);


            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget)
                {
                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        singularTarget.healthComponent.body.ApplyBuff(Modules.Buffs.blackwhipDebuff.buffIndex, 1, 6);
                    }
                }
            }
        }

        protected override void DontDoSkill()
        {
            base.DontDoSkill();
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
                base.PlayCrossfade("RightArm, Override", "Blackwhip", "Attack.playbackRate", fireTime, 0.01f);
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
