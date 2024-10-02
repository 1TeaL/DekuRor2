using DekuMod.Modules;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using EntityStates;
using R2API.Networking;
using RoR2;
using UnityEngine;

namespace DekuMod.SkillStates.Might
{
    public class Fajin : BaseDekuSkillState
    {

        public override void OnEnter()
        {
            dekucon = base.GetComponent<DekuController>();

            PlayCrossfade("UpperBody, Override", "FaJin", "Attack.playbackRate", 0.8f, 0.01f);
            base.OnEnter();
        }

        public override void Level1()
        {
            base.Level1();
            //add buff amounts if you have any previously
            characterBody.ApplyBuff(Buffs.fajinBuff.buffIndex, characterBody.GetBuffCount(Buffs.fajinBuff) + StaticValues.fajinHitAmount);
        }
        public override void Level2()
        {
            base.Level2();
            characterBody.ApplyBuff(Buffs.fajinBuff.buffIndex, characterBody.GetBuffCount(Buffs.fajinBuff) + StaticValues.fajin2HitAmount);
        }

        public override void Level3()
        {
            base.Level3();
            characterBody.ApplyBuff(Buffs.fajinMaxBuff.buffIndex, characterBody.GetBuffCount(Buffs.fajinMaxBuff) + StaticValues.fajin2HitAmount);
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