using UnityEngine;

namespace Swift_Blade
{
    public class DetectedCollision : MonoBehaviour
    {
        [SerializeField] private bool isFireTrapCheck = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player Is Detected");
                if(isFireTrapCheck && Player.Instance.GetPlayerMovement.CanRoll == false)
                {
                    RollingStep.IsRolled = true;
                    Debug.Log("Detected Event Is Called");
                }
            }
        }
    }
}
