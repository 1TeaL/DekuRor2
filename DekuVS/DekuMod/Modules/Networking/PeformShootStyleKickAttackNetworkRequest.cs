using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace DekuMod.Modules.Networking
{
    internal class PeformShootStyleKickAttackNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        Vector3 direction;
        private float force;
        private float damage;

        //Don't network these.
        GameObject bodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;
        private GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");

        public PeformShootStyleKickAttackNetworkRequest()
        {

        }

        public PeformShootStyleKickAttackNetworkRequest(NetworkInstanceId netID, Vector3 direction, float force, float damage)
        {
            this.netID = netID;
            this.direction = direction;
            this.force = force;
            this.damage = damage;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            direction = reader.ReadVector3();
            force = reader.ReadSingle();
            damage = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(direction);
            writer.Write(force);
            writer.Write(damage);
        }

        public void OnReceived()
        {

            if (NetworkServer.active)
            {
                search = new BullseyeSearch();
                //Spawn the effect around this object.
                GameObject masterobject = Util.FindNetworkObject(netID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;

                //Damage target and stun
                DamageTargets(charBody);
            }
        }

        private void DamageTargets(CharacterBody charBody)
        {

            if (charBody.healthComponent && charBody.healthComponent.body)
            {
                float Weight = 1f;

                if (charBody.characterMotor)
                {
                    Weight = charBody.characterMotor.mass;
                }
                else if (charBody.rigidbody)
                {
                    Weight = charBody.rigidbody.mass;
                }

                int buffcount = charBody.GetBuffCount(Modules.Buffs.delayAttackDebuff.buffIndex);

                charBody.ApplyBuff(Modules.Buffs.delayAttackDebuff.buffIndex, buffcount-1);

                DamageInfo damageInfo = new DamageInfo
                {
                    attacker = bodyObj,
                    damage = damage,
                    position = charBody.transform.position,
                    procCoefficient = 1f,
                    damageType = DamageType.Stun1s,
                    crit = charBody.RollCrit(),

                };

                charBody.healthComponent.TakeDamageForce(direction * force * (Weight), true, true);
                charBody.healthComponent.TakeDamage(damageInfo);
                GlobalEventManager.instance.OnHitEnemy(damageInfo, charBody.healthComponent.gameObject);

                

                EffectManager.SpawnEffect(Modules.Assets.dekuHitImpactEffect, new EffectData
                {
                    origin = charBody.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(direction),

                }, true);
            }
        }      

    }
}