using UnityEngine;

namespace Swift_Blade
{
    public class CheckOnDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            WeaponStep.IsBreakabled = true;
        }
    }
}
