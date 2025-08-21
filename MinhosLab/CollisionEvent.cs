using System;
using UnityEngine;

namespace Swift_Blade
{
    public class CollisionEvent : MonoBehaviour
    {
        public Action detectedEvent;

        [SerializeField] private bool IsSecond = false;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(IsSecond)
                {
                    TutorialStatusSystem.IsDetectedEnemyFront = true;
                }
                detectedEvent?.Invoke();
                Debug.Log("Collision detected with Player");
            }
        }
    }
}
