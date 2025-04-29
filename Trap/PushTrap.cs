using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class PushTrap : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform trapPusher;
        [SerializeField, Range(0f, 2f)] private float delay = 1;

        [Header("Active/Inactive")]
        [SerializeField] private AnimationCurve easeCurbeActive;
        [SerializeField] private AnimationCurve easeCurveInactive;

        private bool isActive;
        private float timer;
        private void Awake()
        {
            trapPusher.localPosition = new Vector3(0, 0, GetZValue());
            timer = 0;
        }
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                timer = 0;
                isActive = !isActive;

                AnimationCurve curve = isActive ? easeCurbeActive : easeCurveInactive;

                const float duration = 0.5f;
                float targetZ = GetZValue();

                trapPusher.DOLocalMoveZ(targetZ, duration).SetEase(curve);
            }
        }

        private float GetZValue()
        {
            const float zActive = 0f;
            const float zInactive = 5f;
            return isActive ? zActive : zInactive;
        }
    }
}
