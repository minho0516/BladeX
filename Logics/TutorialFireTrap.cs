using Swift_Blade.Combat.Health;
using Swift_Blade.Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class TutorialFireTrap : MonoBehaviour
    {
        private const float FIRE_DAMAGE = 3f;
        private const float FIRE_DAMAGE_DURATION = 4f;
        private const float DURATION = 0.5f;

        private Dictionary<GameObject, float> fireTimers = new();

        private void OnTriggerStay(Collider other)
        {
            GameObject obj = other.gameObject;

            if (!fireTimers.ContainsKey(obj))
            {
                fireTimers[obj] = 0f;
            }

            if (Time.time > fireTimers[obj])
            {
                if (other.TryGetComponent(out BaseEnemy enemy))
                {
                    fireTimers[obj] = Time.time + DURATION;
                    enemy.GetEffectController().SetFire(FIRE_DAMAGE, FIRE_DAMAGE_DURATION);
                }
                else if (other.TryGetComponent(out PlayerHealth playerHealth))
                {
                    fireTimers[obj] = Time.time + DURATION;
                    ActionData actionData = new ActionData
                    {
                        damageAmount = 0,
                        stun = false
                    };

                    playerHealth.TakeDamage(actionData);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            fireTimers.Remove(other.gameObject);
        }
    }
}
