//using DekuMod.Modules.Networking;
//using DekuMod.Modules.Survivors;
//using EntityStates;
//using HG;
//using R2API.Networking.Interfaces;
//using RoR2;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;
//using Random = UnityEngine.Random;
//using EntityStates.Bandit2;

//namespace DekuMod.SkillStates
//{
//    public class Smokescreen100 : BaseQuirk100
//    {

//        public static float duration = 0.5f;
//        public static float radius = 15f;

//        public override void OnEnter()
//        {
//            base.OnEnter();


//        }

//        protected override void DoSkill()
//        {
//            base.DoSkill();

//            bool active = NetworkServer.active;
//            if (active)
//            {
//                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreen100Duration);
//                base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreen100Duration);
//            }


//            Util.PlaySound(StealthMode.enterStealthSound, base.gameObject);

//            EffectManager.SpawnEffect(Modules.Asset.smokebigEffect, new EffectData
//            {
//                origin = base.transform.position,
//                scale = radius,
//                rotation = Quaternion.LookRotation(Vector3.up)
//            }, true);

//            Util.PlaySound(StealthMode.exitStealthSound, base.gameObject);


//            BlastAttack blastAttack = new BlastAttack();
//            blastAttack.radius = radius;
//            blastAttack.procCoefficient = 1f;
//            blastAttack.position = base.characterBody.footPosition;
//            blastAttack.attacker = base.gameObject;
//            blastAttack.crit = base.RollCrit();
//            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.smokescreenDamageCoefficient;
//            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
//            blastAttack.baseForce = 3000f;
//            blastAttack.teamIndex = base.teamComponent.teamIndex;
//            blastAttack.damageType = DamageType.CrippleOnHit;
//            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

//            blastAttack.Fire();

//        }
//        protected override void DontDoSkill()
//        {
//            base.DontDoSkill();
//            skillLocator.utility.AddOneStock();
//        }
//        private void BuffTeam(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
//        {
//            bool flag = !NetworkServer.active;
//            if (!flag)
//            {
//                foreach (TeamComponent teamComponent in recipients)
//                {
//                    bool flag2 = (teamComponent.transform.position - currentPosition).sqrMagnitude <= radiusSqr;
//                    if (flag2)
//                    {
//                        CharacterBody body = teamComponent.body;
//                        bool flag3 = body;
//                        if (flag3)
//                        {
//                            body.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, Modules.StaticValues.smokescreenDuration);
//                            body.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, Modules.StaticValues.smokescreenDuration);

//                        }
//                    }
//                }
//            }
//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();

//            if (base.fixedAge >= duration && base.isAuthority)
//            {
//                this.outer.SetNextStateToMain();
//                return;
//            }
            
//        }

//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.PrioritySkill;
//        }
//    }
//}