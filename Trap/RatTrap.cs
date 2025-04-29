using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class RatTrap : MonoBehaviour
    {
        [SerializeField] private Transform leftDoor, rightDoor;
        [Header("General")]
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
        private void ActiveTrap()
        {
            isActive = !isActive;

            AnimationCurve curve = isActive ? easeCurbeActive : easeCurveInactive;
            Vector3 targetXRot = GetXRotValue();

            const float duration = 0.5f;

            leftDoor.DOLocalRotate(targetXRot, duration).SetEase(curve);
            rightDoor.DOLocalRotate(-targetXRot, duration).SetEase(curve);
        }

        private Vector3 GetXRotValue()
        {
            const float zActive = -90f;
            const float zInactive = 0f;
            return isActive ? new Vector3(0, 0, zActive) : new Vector3(0, 0, zInactive);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isActive) return;
            if (other.CompareTag("Player") == false) return;
            StartCoroutine(nameof(ActiveTrapCoroutine));
        }

        private IEnumerator ActiveTrapCoroutine()
        {
            ActiveTrap();
            yield return new WaitForSeconds(delay);
            ActiveTrap();
        }
    }
}
