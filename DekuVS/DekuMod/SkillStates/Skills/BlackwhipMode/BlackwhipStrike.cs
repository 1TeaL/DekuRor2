using DekuMod.Modules;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using EntityStates;
using R2API.Networking;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.Networking;

namespace DekuMod.SkillStates.BlackWhip
{
    public class BlackwhipStrike : BaseDekuSkillState
    {
        public Vector3 moveDirection;
        public float duration;
        public float fireTime;
        public BullseyeSearch search;
        public bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();

            duration = 1f;
            fireTime = duration / 4f;
            switch (level)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge > fireTime && !hasFired)
            {
                if(characterBody.inputBank.moveVector != Vector3.zero)
                {
                    moveDirection = characterBody.inputBank.moveVector;
                    ApplyComponent(moveDirection.normalized);
                    hasFired = true;
                }
            }
            if(base.fixedAge > duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public void ApplyComponent(Vector3 moveDirection)
        {
            search = new BullseyeSearch();
            search.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
            search.filterByLoS = false;
            search.searchOrigin = base.GetAimRay().origin;
            search.searchDirection = base.GetAimRay().direction;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = StaticValues.blackwhipStrikeRange;
            search.maxAngleFilter = 90f;


            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);



            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget.healthComponent.body && singularTarget.healthComponent)
                {
                    //int buffcount = singularTarget.healthComponent.body.GetBuffCount(Modules.Buffs.delayAttackDebuff.buffIndex);
                    //if (NetworkServer.active)
                    //{
                    //    singularTarget.healthComponent.body.ApplyBuff(Modules.Buffs.delayAttackDebuff.buffIndex, numberOfHits + buffcount);
                    //}
                    BlackwhipComponent blackwhipComponent = singularTarget.healthComponent.body.gameObject.GetComponent<BlackwhipComponent>();

                    if (blackwhipComponent)
                    {

                    }
                    if (!blackwhipComponent)
                    {
                        blackwhipComponent = singularTarget.healthComponent.body.gameObject.AddComponent<BlackwhipComponent>();
                        blackwhipComponent.totalDuration = duration;
                        blackwhipComponent.moveDirection = moveDirection;
                        blackwhipComponent.pushDamage = true;
                        blackwhipComponent.dekucharbody = characterBody;

                    }



                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}