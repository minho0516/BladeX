using UnityEngine;

namespace Swift_Blade
{
    public class Tutorial_Color : MonoBehaviour
    {
        [SerializeField] private float expValue = 10f;
        private void OnEnable()
        {
            
        }

        public void AddPlayerExp(float expValue)
        {
            //Player.level.AddExp(expValue);
        }
    }
}
