using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class TutorialKeyPanel : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.localScale = Vector3.one;

            transform
                .DOScale(1.2f, 1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }


    }
}
