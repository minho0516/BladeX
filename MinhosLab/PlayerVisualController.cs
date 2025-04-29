using UnityEngine;

namespace Swift_Blade
{
    public class PlayerVisualController : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private EquipmentListSO equipListSO;

        [SerializeField] private Transform HelmetVisual, HelmetVisual2, BodiesVisual, HipVisual;

        public void EntityComponentAwake(Entity entity)
        {
            
        }

        public void OnParts(string equipment)
        {
            if(equipListSO.equipmentList.TryGetValue(equipment, out var partsName))
            {
                GameObject go = null;
                go = GetVisualObj(partsName);

                go.SetActive(true);
            }
        }

        public void OffParts(string equipment)
        {
            if (equipListSO.equipmentList.TryGetValue(equipment, out var partsName))
            {
                GameObject go = null;
                go = GetVisualObj(partsName);

                go.SetActive(false);
            }
        }

        private GameObject GetVisualObj(string partsName)
        {
            if (HelmetVisual.Find(partsName) is not null)
            {
                Debug.Log("«Ô∏‰ø° ¿÷Ω¿¥œ¥Ÿ");
                return HelmetVisual.Find(partsName).gameObject;
            }
            else if (HelmetVisual2.Find(partsName) is not null)
            {
                Debug.Log("«Ô∏‰2ø° ¿÷Ω¿¥œ¥Ÿ");
                return HelmetVisual2.Find(partsName).gameObject;
            }
            else if(BodiesVisual.Find(partsName) is not null)
            {
                Debug.Log("∞©ø ø° ¿÷Ω¿¥œ¥Ÿ");
                return BodiesVisual.Find(partsName).gameObject;
            }
            else
            {
                Debug.Log("»¸ø° ¿÷Ω¿¥œ¥Ÿ");
                return HipVisual.Find(partsName).gameObject;
            }
        }
        private void SetOffVisuals(Transform t)
        {
            foreach(Transform child in t)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
