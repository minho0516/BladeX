using UnityEngine;

namespace Swift_Blade
{
    public class GrinderTrap : MonoBehaviour
    {
        private Transform grinder;

        private void Awake()
        {
            grinder = transform.GetChild(0);    
        }

        private void Update()
        {
            float grinderSpeed = 50.0f;
            Quaternion angle = grinder.rotation;
            //grinder.rotation = angle * grinderSpeed;
        }
    }
}
