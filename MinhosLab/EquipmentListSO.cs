using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentListSO", menuName = "SO/EquipmentListSO")]
    public class EquipmentListSO : ScriptableObject
    {
        public SerializableDictionary<string, string> equipmentList;
    }
}
