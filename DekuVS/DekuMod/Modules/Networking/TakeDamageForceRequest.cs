using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace DekuMod.Modules.Networking
{
    internal class TakeDamageForceRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId enemynetID;
        NetworkInstanceId charnetID;
        Vector3 direction;
        private float force;
        private float damage;

        //Don't network these.
        GameObject bodyObj;
        GameObject charbodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;
        private GameObject blastEffectPrefab = Asset.loaderOmniImpactLightningEffect;

        public TakeDamageForceRequest()
        {

        }

        public TakeDamageForceRequest(NetworkInstanceId enemynetID, Vector3 direction, float force, float damage, NetworkInstanceId charnetID)
        {
            this.enemynetID = enemynetID;
            this.direction = direction;
            this.force = force;
            this.damage = damage;
            this.charnetID = charnetID;
        }

        public void Deserialize(NetworkReader reader)
        {
            enemynetID = reader.ReadNetworkId();
            direction = reader.ReadVector3();
            force = reader.ReadSingle();
            damage = reader.ReadSingle();
            charnetID = reader.ReadNetworkId();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(enemynetID);
            writer.Write(direction);
            writer.Write(force);
            writer.Write(damage);
            writer.Write(charnetID);
        }

        public void OnReceived()
        {

            if (NetworkServer.active)
            {
                search = new BullseyeSearch();
                //Spawn the effect around this object.
                GameObject enemymasterobject = Util.FindNetworkObject(enemynetID);
                CharacterMaster enemycharMaster = enemymasterobject.GetComponent<CharacterMaster>();
                CharacterBody enemycharBody = enemycharMaster.GetBody();
                bodyObj = enemycharBody.gameObject;

                GameObject charmasterobject = Util.FindNetworkObject(charnetID);
                CharacterMaster charcharMaster = charmasterobject.GetComponent<CharacterMaster>();
                CharacterBody charcharBody = charcharMaster.GetBody();
                charbodyObj = charcharBody.gameObject;

                //Damage target and stun
                DamageTargets(enemycharBody, charcharBody);
            }
        }

        private void DamageTargets(CharacterBody enemycharBody, CharacterBody charcharBody)
        {

            if (enemycharBody.healthComponent && enemycharBody.healthComponent.body)
            {
                float Weight = 1f;

                if (enemycharBody.characterMotor)
                {
                    Weight = enemycharBody.characterMotor.mass;
                }
                else if (enemycharBody.rigidbody)
                {
                    Weight = enemycharBody.rigidbody.mass;
                }


                DamageInfo damageInfo = new DamageInfo
                {
                    attacker = charbodyObj,
                    inflictor = charbodyObj,
                    damage = damage,
                    position = enemycharBody.transform.position,
                    procCoefficient = StaticValues.blackwhipProc,
                    damageType = DamageType.Generic,
                    crit = charcharBody.RollCrit(),

                };

                enemycharBody.healthComponent.TakeDamageForce(direction * force * (Weight), true, true);
                enemycharBody.healthComponent.TakeDamage(damageInfo);
                GlobalEventManager.instance.OnHitEnemy(damageInfo, enemycharBody.healthComponent.gameObject);

                

                EffectManager.SpawnEffect(blastEffectPrefab, new EffectData
                {
                    origin = enemycharBody.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(direction),

                }, true);
            }
        }      

    }
}