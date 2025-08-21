using UnityEngine;

namespace Swift_Blade
{
    public class TutorialInventoryStepMap : MonoBehaviour
    {
        public static TutorialInventoryStepMap Instance;

        [SerializeField] private Transform chsetSpawnPosition1, chsetSpawnPosition2, chsetSpawnPosition3;
        [SerializeField] private GameObject chestPrefab, potionChestPrefab;

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }

        public void SpawnChest()
        {
            Instantiate(potionChestPrefab, chsetSpawnPosition1.position, Quaternion.identity);
            Instantiate(chestPrefab, chsetSpawnPosition2.position, Quaternion.identity);
            //Instantiate(chestPrefab, chsetSpawnPosition2.position, Quaternion.identity);
        }
    }
}
