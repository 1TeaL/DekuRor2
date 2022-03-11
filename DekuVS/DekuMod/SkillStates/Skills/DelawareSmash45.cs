using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{
    public class DelawareSmash45: BaseSkillState
    {
        public uint Distance = 50;
        public static float damageCoefficient;
        public float baseDuration = 1f;
        private float duration;
        public static event Action<int> Compacted;
        //public static GameObject explosionPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/MageLightningBombExplosion");
        public static GameObject explosionPrefab = Modules.Projectiles.delawareTracer;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration/this.attackSpeedStat;
            AkSoundEngine.PostEvent(1074439307, this.gameObject);
            AkSoundEngine.PostEvent(1356252224, this.gameObject);
            base.StartAimMode(0.6f, true);

            base.characterMotor.disableAirControlUntilCollision = false;


            float angle = Vector3.Angle(new Vector3(0, -1, 0), aimRay.direction);
            if (angle < 60)
            {
                base.PlayAnimation("FullBody, Override", "DelawareSmashUp");
            }
            else if (angle > 120)
            {
                base.PlayAnimation("FullBody, Override", "DelawareSmashDown");
            }
            else
            {
                base.PlayAnimation("FullBody, Override", "DelawareSmash");
            }

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);

            if (base.isAuthority)
            {
                Vector3 theSpot = aimRay.origin + 8 * aimRay.direction;
                Vector3 theSpot2 = aimRay.origin + 2 * aimRay.direction;

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = 15f;
                blastAttack.procCoefficient = 1f;
                blastAttack.position = theSpot;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = this.damageStat * Modules.StaticValues.delaware45DamageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = 600f;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.damageType = DamageType.SlowOnHit;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                BlastAttack.Result result = blastAttack.Fire();

                EffectData effectData = new EffectData();
                {
                effectData.scale = 15;
                effectData.origin = theSpot2;
                effectData.rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z));
                };

                EffectManager.SpawnEffect(explosionPrefab, effectData, true);

                base.characterMotor.velocity = -Distance * aimRay.direction * moveSpeedStat / 7;

                Compacted?.Invoke(result.hitCount);
            }
        }

        public override void OnExit()
        {
            if (NetworkServer.active)
            {
                base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f);
            }

            base.characterMotor.velocity *= 0.7f;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if ((base.fixedAge >= this.duration && base.isAuthority) || (!base.IsKeyDownAuthority()))
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