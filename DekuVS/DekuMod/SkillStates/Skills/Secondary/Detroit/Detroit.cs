using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using EntityStates;
using EntityStates.Huntress;
using EntityStates.VagrantMonster;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using RoR2.Audio;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class Detroit : BaseSkill
    {
        public bool hasTeleported;
        public bool hasFired;
        public float baseDuration = 1f;
        public float duration;
        public float fireTime;
        private DamageType damageType;

        private float radius = 10f;
        private float damageCoefficient = Modules.StaticValues.detroitDamageCoefficient;
        private float procCoefficient = 1f;
        private float force = 1000f;

        private BlastAttack blastAttack;

        //Indicator
        private readonly BullseyeSearch search = new BullseyeSearch();
        public float maxTrackingDistance = 60f;
        public float maxTrackingAngle = 15f;
        public HurtBox Target;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / base.attackSpeedStat;
            this.fireTime = this.duration / 3f;
            hasFired = false;
            hasTeleported = false;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            //PlayAnimation("FullBody, Override", "Slam", "Attack.playbackRate", fireTime * 2f);

            Ray aimRay = base.GetAimRay();
            this.SearchForTarget(aimRay);



        }

        private void SearchForTarget(Ray aimRay)
        {
            this.search.teamMaskFilter = TeamMask.GetEnemyTeams(TeamIndex.Player);
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = this.maxTrackingDistance;
            this.search.maxAngleFilter = this.maxTrackingAngle;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            this.Target = this.search.GetResults().FirstOrDefault<HurtBox>();
        }

        public override void OnExit()
        {
            base.OnExit();
            this.PlayAnimation("Fullbody, Override", "BufferEmpty");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Target)
            {
                if (!hasTeleported)
                {
                    hasTeleported = true;
                    base.characterMotor.velocity = Vector3.zero;
                    base.characterMotor.Motor.SetPositionAndRotation(Target.healthComponent.body.transform.position + Vector3.up, Target.healthComponent.body.transform.rotation, true);
                    //new PerformDetroitTeleportNetworkRequest(base.characterBody.masterObjectId, Target.gameObject).Send(NetworkDestination.Clients);

                }

                if (base.fixedAge > this.fireTime && !hasFired && base.isAuthority)
                {
                    hasFired = true;
                    new PerformDetroitSmashNetworkRequest(base.characterBody.masterObjectId, Target.healthComponent.body.masterObjectId).Send(NetworkDestination.Clients);

                }

                if ((base.fixedAge >= this.duration && base.isAuthority))
                {
                    this.outer.SetNextStateToMain();
                    return;
                }

            }
            else
            {
                base.skillLocator.secondary.AddOneStock();
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
