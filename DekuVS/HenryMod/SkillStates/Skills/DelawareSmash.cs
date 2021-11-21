using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{
    public class DelawareSmash : BaseSkillState
    {
        public static float damageCoefficient = 6f;
        public float baseDuration = 0.5f;
        private float duration;
        public static event Action<int> Compacted;
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerSmokeChase");
        public static GameObject explosionPrefab = Resources.Load<GameObject>("prefabs/effects/MageLightningBombExplosion");

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration;
            Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
            //Util.PlaySound(DiggerPlugin.Sounds.Backblast, base.gameObject);
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

                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = 15f;
                blastAttack.procCoefficient = 2f;
                blastAttack.position = theSpot;
                blastAttack.attacker = base.gameObject;
                //blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                blastAttack.crit = base.RollCrit();
                blastAttack.baseDamage = this.damageStat * DelawareSmash.damageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = 600f;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.damageType = DamageType.Stun1s;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
                BlastAttack.Result result = blastAttack.Fire();

                //EffectData effectData = new EffectData();
                //effectData.origin = theSpot;
                //effectData.scale = 15;
                EffectData effectData = new EffectData();
                {
                effectData.scale = 15;
                effectData.origin = theSpot;
                };

                EffectManager.SpawnEffect(explosionPrefab, effectData, true);

                base.characterMotor.velocity = -100 * aimRay.direction;

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

            base.characterMotor.velocity *= 0.2f;

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
            return InterruptPriority.Skill;
        }
    }
}