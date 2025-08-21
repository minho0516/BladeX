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
            Debug.Log("Ʃ�丮���κ��丮���ܸ� ���̳ʺ��");
            Instance = this;
        }

        private void OnDisable()
        {
            Debug.Log("Ʃ�丮���κ��丮���ܸ� �µ��̳ʺ��");
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
