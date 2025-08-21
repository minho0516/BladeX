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
            float grinderSpeed = 150.0f;
            grinder.Rotate(Vector3.left * grinderSpeed * Time.deltaTime);
        }
    }
}
