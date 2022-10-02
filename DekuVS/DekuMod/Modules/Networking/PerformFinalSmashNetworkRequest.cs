using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace DekuMod.Modules.Networking
{
    internal class PerformFinalSmashNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        Vector3 origin;
        Vector3 direction;
        private float damage;

        //Don't network these.
        GameObject bodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;

        public PerformFinalSmashNetworkRequest()
        {

        }

        public PerformFinalSmashNetworkRequest(NetworkInstanceId netID, Vector3 origin, Vector3 direction, float damage)
        {
            this.netID = netID;
            this.origin = origin;
            this.direction = direction;
            this.damage = damage;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            origin = reader.ReadVector3();
            direction = reader.ReadVector3();
            damage = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(origin);
            writer.Write(direction);
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

                //Check targets in range
                SearchForTarget(charBody);
                //Pull targets and stun
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
            if (trackingTargets.Count > 0)
            {
                foreach (HurtBox singularTarget in trackingTargets)
                {
                    singularTarget.healthComponent.body.characterMotor.Motor.SetPositionAndRotation(charBody.transform.position + charBody.characterDirection.forward * 5f,
                        Util.QuaternionSafeLookRotation(charBody.characterDirection.forward), true);

                    DamageInfo damageInfo = new DamageInfo
                    {
                        attacker = bodyObj,
                        damage = damage,
                        position = singularTarget.transform.position,
                        procCoefficient = 1f,
                        damageType = DamageType.Stun1s,
                        crit = charBody.RollCrit(),

                    };

                    singularTarget.healthComponent.TakeDamage(damageInfo);
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, singularTarget.healthComponent.gameObject);


                    EffectManager.SpawnEffect(Modules.Assets.windringEffect, new EffectData
                    {
                        origin = singularTarget.transform.position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(singularTarget.transform.position - origin),

                    }, true);
                    
                }
            }
        }

        private void SearchForTarget(CharacterBody charBody)
        {
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(charBody.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.searchOrigin = origin;
            this.search.searchDirection = direction;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = StaticValues.finalsmashRange;
            this.search.maxAngleFilter = 360f;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(charBody.gameObject);
            this.trackingTargets = this.search.GetResults().ToList<HurtBox>();
        }
    }
}
