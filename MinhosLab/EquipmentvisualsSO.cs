using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentvisualsSO", menuName = "SO/EquipmentvisualsSO")]
    public class EquipmentvisualsSO : ScriptableObject
    {
        public SerializableDictionary<string, GameObject> equipVisuals;
    }
}
