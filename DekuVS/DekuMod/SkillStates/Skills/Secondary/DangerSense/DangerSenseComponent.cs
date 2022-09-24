using RoR2;
using UnityEngine;


namespace DekuMod.SkillStates
{
    public class DangerSenseComponent : MonoBehaviour, IOnIncomingDamageServerReceiver
    {
        public CharacterBody body;

        
        public void OnIncomingDamageServer(DamageInfo damageInfo)
        {
            body = gameObject.GetComponent<CharacterBody>();
            if (body.HasBuff(Modules.Buffs.counterBuff))
            {
                body.RemoveBuff(Modules.Buffs.counterBuff);
                damageInfo.damage = 0f;

                var damageInfo2 = new DamageInfo();

                damageInfo2.damage = body.damage * Modules.StaticValues.counterDamageCoefficient;
                damageInfo2.position = damageInfo.attacker.transform.position;
                damageInfo2.force = Vector3.zero;
                damageInfo2.damageColorIndex = DamageColorIndex.Default;
                damageInfo2.crit = Util.CheckRoll(body.crit, body.master);
                damageInfo2.attacker = body.gameObject;
                damageInfo2.inflictor = null;
                damageInfo2.damageType = DamageType.BypassArmor | DamageType.Stun1s;
                damageInfo2.procCoefficient = 2f;
                damageInfo2.procChainMask = default(ProcChainMask);

                if (damageInfo.attacker.gameObject.GetComponent<CharacterBody>().baseNameToken
                    != DekuPlugin.developerPrefix + "_DEKU_BODY_NAME" && damageInfo.attacker != null)
                {
                    damageInfo.attacker.GetComponent<CharacterBody>().healthComponent.TakeDamage(damageInfo2);
                }

                Vector3 enemyPos = damageInfo.attacker.transform.position;
                EffectManager.SpawnEffect(Modules.Projectiles.airforceTracer, new EffectData
                {
                    origin = body.transform.position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(enemyPos - body.transform.position)

                }, true);


                EntityStateMachine[] stateMachines = body.gameObject.GetComponents<EntityStateMachine>();
                foreach (EntityStateMachine stateMachine in stateMachines)
                {
                    if (stateMachine.customName == "Body")
                    {

                        body.gameObject.GetComponent<EntityStateMachine>().SetInterruptState(new DangerSenseCounter
                        {
                            
                            enemyPosition = enemyPos
                        }, EntityStates.InterruptPriority.Frozen);


                    }
                }

                //Destroy(dangercon);
            }
        }

    }
}