using Swift_Blade.Enemy.Boss.Sword;
using UnityEngine;

namespace Swift_Blade
{
    public class WeaponStepMap : MonoBehaviour
    {
        public static WeaponStepMap Instance = null;

        [SerializeField] private WeaponOrb weaponOrbDagger;
        [SerializeField] private WeaponOrb weaponOrbGreat;
        [SerializeField] private WeaponOrb weaponOrbSword;

        [SerializeField] private Transform daggerPos, greatPos, swordPos;

        private void OnEnable()
        {
            Debug.Log("WeaponStepMap enabled, spawning weapons.");
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("WeaponStepMap instance already exists, not spawning weapons again.");
            }
        }

        private void OnDisable()
        {
            Instance = null;
        }
        public void SpawnWeapon()
        {
            GameObject orb1 = Instantiate(weaponOrbDagger, daggerPos.position, Quaternion.identity).gameObject;
            orb1.transform.parent = daggerPos;

            GameObject orb2 = Instantiate(weaponOrbGreat, greatPos.position, Quaternion.identity).gameObject;
            orb2.transform.parent = greatPos;

            GameObject orb3 = Instantiate(weaponOrbSword, swordPos.position, Quaternion.identity).gameObject;
            orb3.transform.parent = swordPos;
        }
    }
}
