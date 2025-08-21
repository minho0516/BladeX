using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade
{
    public class TimelineCamShake : MonoBehaviour
    {
        public void CallCamShake()
        {
            CameraShakeManager.Instance.DoShake(CameraShakeType.LeftRight);
        }
    }
}
