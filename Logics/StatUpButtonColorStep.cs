using UnityEngine;

namespace Swift_Blade
{
    public class StatUpButtonColorStep : MonoBehaviour
    {
        public void CallColorUpgrade()
        {
            ColorStep.Instance.StatUpCallback();
        }

    }
}
