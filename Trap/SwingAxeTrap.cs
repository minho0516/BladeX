using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Swift_Blade.Combat.Health;

namespace Swift_Blade
{
    public class SwingAxeTrap : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform trapSwingAxe;
        [SerializeField, Range(0f, 2f)] private float delay = 1;

        [Header("Active/Inactive")]
        [SerializeField] private AnimationCurve easeCurbeActive;
        [SerializeField] private AnimationCurve easeCurveInactive;

        private bool isActive;
        private float timer;

        private void Awake()
        {
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
                Vector3 targetXRot = GetXRotValue();

                const float duration = 0.5f;

                trapSwingAxe.DOLocalRotate(targetXRot, duration).SetEase(curve);
            }
        }

        private Vector3 GetXRotValue()
        {
            const float zActive = -70f;
            const float zInactive = 70f;
            return isActive ? new Vector3(0, 0, zActive) : new Vector3(0, 0, zInactive);
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
