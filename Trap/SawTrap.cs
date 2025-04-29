using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Swift_Blade.Combat.Health;

namespace Swift_Blade
{
    public class SawTrap : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform trapSaw;
        [SerializeField, Range(0f, 2f)] private float delay = 1;

        [Header("Active/Inactive")]
        [SerializeField] private AnimationCurve easeCurbeActive;
        [SerializeField] private AnimationCurve easeCurveInactive;

        private bool isActive;
        private float timer;

        private Collider coll;
        private void Awake()
        {
            trapSaw.localPosition = new Vector3(0, 0.3f, GetZValue());
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

                Vector3 targetXRot = GetXRotValue();

                trapSaw.DOLocalMoveZ(targetZ, duration).SetEase(curve);
                trapSaw.DOLocalRotate(targetXRot, duration);
            }
        }

        private float GetZValue()
        {
            const float zActive = -1.1f;
            const float zInactive = 1.1f;
            return isActive ? zActive : zInactive;
        }

        private Vector3 GetXRotValue()
        {
            const float xActive = -180f;
            const float xInactive = 0;
            return isActive ? new Vector3(xActive, 0, 0) : new Vector3(xInactive, 0, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BaseEntityHealth health))
            {
                health.TakeDamage(new ActionData { damageAmount = 1, stun = health is PlayerHealth });
            }
        }
    }
}
