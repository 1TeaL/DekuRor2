using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using EntityStates;
using HG;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using EntityStates.Bandit2;

namespace DekuMod.SkillStates
{
    public class Smokescreenold : BaseQuirk45
    {

        public static float duration = 0.5f;
        public static float radius = 15f;

        public override void OnEnter()
        {
            base.OnEnter();
            duration /= attackSpeedStat;


        }

        protected override void DoSkill()
        {
            base.DoSkill();

            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreenDuration);
                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreenDuration);
            }

            Util.PlaySound(StealthMode.enterStealthSound, base.gameObject);

            EffectManager.SpawnEffect(Modules.Assets.smokeEffect, new EffectData
            {
                origin = base.transform.position,
                scale = radius,
                rotation = Quaternion.LookRotation(Vector3.up)
            }, true);

            Util.PlaySound(StealthMode.exitStealthSound, base.gameObject);

        }
        protected override void DontDoSkill()
        {
            base.DontDoSkill();
            skillLocator.utility.AddOneStock();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= duration && base.isAuthority)
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