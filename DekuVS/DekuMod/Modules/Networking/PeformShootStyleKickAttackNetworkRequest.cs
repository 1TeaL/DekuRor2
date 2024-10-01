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
        NetworkInstanceId dekunetID;
        Vector3 direction;
        private float force;
        private float damage;

        //Don't network these.
        GameObject bodyObj;
        GameObject dekubodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;
        private GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");

        public PeformShootStyleKickAttackNetworkRequest()
        {

        }

        public PeformShootStyleKickAttackNetworkRequest(NetworkInstanceId netID, Vector3 direction, float force, float damage, NetworkInstanceId dekunetID)
        {
            this.netID = netID;
            this.direction = direction;
            this.force = force;
            this.damage = damage;
            this.dekunetID = dekunetID;
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

                GameObject dekumasterobject = Util.FindNetworkObject(dekunetID);
                CharacterMaster dekucharMaster = dekumasterobject.GetComponent<CharacterMaster>();
                CharacterBody dekucharBody = dekucharMaster.GetBody();
                dekubodyObj = dekucharBody.gameObject;

                //Damage target and stun
                DamageTargets(charBody, dekucharBody);
            }
        }

        private void DamageTargets(CharacterBody charBody, CharacterBody dekucharBody)
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
                    attacker = dekubodyObj,
                    inflictor = dekubodyObj,
                    damage = damage,
                    position = charBody.transform.position,
                    procCoefficient = 0.1f,
                    damageType = DamageType.Generic,
                    crit = dekucharBody.RollCrit(),

                };

                charBody.healthComponent.TakeDamageForce(direction * force * (Weight), true, true);
                charBody.healthComponent.TakeDamage(damageInfo);
                GlobalEventManager.instance.OnHitEnemy(damageInfo, charBody.healthComponent.gameObject);

                

                EffectManager.SpawnEffect(Modules.DekuAssets.dekuHitImpactEffect, new EffectData
                {
                    origin = charBody.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(direction),

                }, true);
            }
        }      

    }
}