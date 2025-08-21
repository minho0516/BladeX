using UnityEngine;

namespace Swift_Blade
{
    public class PlayerVisualController : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private EquipmentListSO equipListSO;

        [SerializeField] private Transform HelmetVisual, HelmetVisual2, BodiesVisual, HipVisual;
        [SerializeField] private Transform LeftShoesVisuals, RightShoesVisuals;

        [Header("Default vis")]
        [SerializeField] private Transform DefaultHelemt;
        [SerializeField] private Transform DefaultTorso;
        [SerializeField] private Transform DefaultHip;
        [SerializeField] private Transform DefaultShoesLeft;
        [SerializeField] private Transform DefaultShoesRight;

        [Header("Visuals")]
        [SerializeField] private Transform[] Visuals;

        private bool objIsHelmet = false;
        private bool objIsTorso = false;
        private bool objIsHip = false;
        private bool objIsShoes = false;
        
        public void EntityComponentAwake(Entity entity)
        {
            
        }

        public void OnParts(string equipment)
        {
            //Debug.Log(equipment);
            GameObject go = null;
            //foreach(string name in equipListSO.equipmentList.Keys)
            //{
            //    //Debug.Log($"name : {name}");
            //}
            if (equipListSO.equipmentList.TryGetValue(equipment, out var partsName))
            {
                //Debug.Log(partsName);
                go = GetVisualObj(partsName);
                if (go is null)
                {
                    string offsetR = $"Chr_LegRight_Male_{partsName}";
                    string offsetL = $"Chr_LegLeft_Male_{partsName}";
                    
                    //신발은 파츠네임에 숫자만기입
                    //Debug.Log($"{partsName}");

                    go = RightShoesVisuals.Find(offsetR).gameObject;
                    go.SetActive(true);
                    go = LeftShoesVisuals.Find(offsetL).gameObject;
                    go.SetActive(true);
                }
                else
                {
                    go.SetActive(true);
                }

                if (objIsHelmet) DefaultHelemt.gameObject.SetActive(false);
                if (objIsTorso) DefaultTorso.gameObject.SetActive(false);
                if (objIsHip) DefaultHip.gameObject.SetActive(false);
                if(objIsShoes)
                {
                    DefaultShoesLeft.gameObject.SetActive(false);
                    DefaultShoesRight.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError($"fail to get target visual {equipment}");
            }
        }

        public void OffParts(string equipment)
        {

            GameObject go = null;
            if (equipListSO.equipmentList.TryGetValue(equipment, out var partsName))
            {
                go = GetVisualObj(partsName);
                if (go is null)
                {
                    string offsetR = $"Chr_LegRight_Male_{partsName}";
                    string offsetL = $"Chr_LegLeft_Male_{partsName}";
                    //신발은 파츠네임에 숫자만기입
                    //Debug.Log($"{offsetR} and {offsetL}");

                    go = RightShoesVisuals.Find(offsetR).gameObject;
                    go.SetActive(false);
                    go = LeftShoesVisuals.Find(offsetL).gameObject;
                    go.SetActive(false);
                }
                else
                {
                    go.SetActive(false);
                }

                if (objIsHelmet) DefaultHelemt.gameObject.SetActive(true);
                if (objIsTorso) DefaultTorso.gameObject.SetActive(true);
                if (objIsHip) DefaultHip.gameObject.SetActive(true);
                if (objIsShoes)
                {
                    DefaultShoesLeft.gameObject.SetActive(true);
                    DefaultShoesRight.gameObject.SetActive(true);
                }
            }
        }

        private GameObject GetVisualObj(string partsName)
        {
            objIsHelmet = false;
            objIsTorso = false;
            objIsHip = false;
            objIsShoes = false;

            GameObject go = null;

            foreach(Transform parentVis in Visuals)
            {
                objIsHelmet = (parentVis.name == "Male_Head_No_Elements"); // || parentVis.name == "Helmet" 머리를 가리는게 이쏘 안가리는게 잇음
                objIsTorso = (parentVis.name == "Male_03_Torso");
                objIsHip = (parentVis.name == "Male_10_Hips");
                objIsShoes = (parentVis.name == "Male_11_Leg_Right" || parentVis.name == "Male_12_Leg_Left");

                go = GetFindObj(parentVis, partsName);
                if(go != null)
                {
                    //Debug.Log($"exist on {parentVis}");
                    break;
                }
            }

            return go;

        
            //if(trm == null)

            //Debug.Log("슈에 있습니다");
            //return null;

            //if error by shoe part? fixing like this style at FixingErrorTime
        }

        private GameObject GetFindObj(Transform visParent, string partsName)
        {
            Transform trm = visParent.Find(partsName);
            if (trm is not null)
            {
                //Debug.Log($"target on {visParent}");
                SetOffVisuals(visParent);

                return trm.gameObject;
            }

            return null;
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
