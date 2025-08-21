using UnityEngine;

namespace Swift_Blade
{
    public class MinimapSpotlight : MonoSingleton<MinimapSpotlight>
    {
        [SerializeField] private GameObject spotlight1, spotlight2, spotlight3, spotlight4;
        [SerializeField] private int nowIdx = 0;

        public void SetSpotlight()
        {
            if(nowIdx == 0)
            {
                spotlight1.SetActive(true);
                spotlight2.SetActive(false);
                spotlight3.SetActive(false);
                spotlight4.SetActive(false);
            }
            if(nowIdx == 1)
            {
                spotlight1.SetActive(false);
                spotlight2.SetActive(true);
                spotlight3.SetActive(false);
                spotlight4.SetActive(false);
            }
            if (nowIdx == 2)
            {
                spotlight1.SetActive(false);
                spotlight2.SetActive(false);
                spotlight3.SetActive(true);
                spotlight4.SetActive(false);
            }
            if (nowIdx == 3)
            {
                spotlight1.SetActive(false);
                spotlight2.SetActive(false);
                spotlight3.SetActive(false);
                spotlight4.SetActive(true);
            }

            if(nowIdx == 3)
            {
                nowIdx = 0;
            }
            else
            {
                nowIdx++;
            }
        }
    }
}
