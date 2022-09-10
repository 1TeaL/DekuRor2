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

        //Don't network these.
        GameObject bodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;
        private GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");

        public PeformShootStyleKickAttackNetworkRequest()
        {

        }

        public PeformShootStyleKickAttackNetworkRequest(NetworkInstanceId netID, Vector3 direction, float force)
        {
            this.netID = netID;
            this.direction = direction;
            this.force = force;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            direction = reader.ReadVector3();
            force = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(direction);
            writer.Write(force);
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
                PullTargets(charBody);
            }
        }

        //Don't do anything until we have a target.
        //Pull target: let's use a blast attack pointed towards the player with a slight upwards force.
        //directional vector: terminal point - initial point.
        //get mass, apply force. object mass * forcemultiplier for enemies.
        //Make direction point towards player.
        private void PullTargets(CharacterBody charBody)
        {

            if (charBody.healthComponent && charBody.healthComponent.body)
            {
                float Weight = 1f;

                if (charBody.healthComponent.body.characterMotor)
                {
                    Weight = charBody.healthComponent.body.characterMotor.mass;
                }
                else if (charBody.healthComponent.body.rigidbody)
                {
                    Weight = charBody.healthComponent.body.rigidbody.mass;
                }

                

                DamageInfo damageInfo = new DamageInfo
                {
                    attacker = bodyObj,
                    damage = charBody.damage * Modules.StaticValues.shootkick100DamageCoefficient,
                    position = charBody.transform.position,
                    procCoefficient = 1f,
                    damageType = DamageType.Stun1s,
                    crit = charBody.RollCrit(),

                };

                charBody.healthComponent.TakeDamageForce(direction * force * (Weight), true, true);
                charBody.healthComponent.TakeDamage(damageInfo);
                GlobalEventManager.instance.OnHitEnemy(damageInfo, charBody.healthComponent.gameObject);


                EffectManager.SpawnEffect(Modules.Assets.detroitEffect, new EffectData
                {
                    origin = charBody.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(direction),

                }, true);
            }
        }      

    }
}