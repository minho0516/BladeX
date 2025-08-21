using DG.Tweening;
using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class SecretDoor : MonoBehaviour, IInteractable
    {
        [Header("General")]
        [SerializeField] private Transform door;

        [Header("Curve")]
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Transform interactionIconTransform;

        public void Interact()
        {
            OpenDoor();
        }

        private void OpenDoor()
        {
            float duration = 0.5f;
            Vector3 targetYRot = GetYRotValue();

            door.DOLocalRotate(targetYRot, duration).SetEase(curve);
        }
        private Vector3 GetYRotValue()
        {
            bool playerPosBiggerthan = (Player.Instance.transform.GetChild(0).position.z > transform.position.z);
            Debug.Log(playerPosBiggerthan);

            const float biggerThan = -89f;
            const float smallThan = 90;

            return playerPosBiggerthan ? new Vector3(0, biggerThan, 0) : new Vector3(0, smallThan, 0);
        }

        Transform IInteractable.InteractionIconTrasnform()
        {
            return interactionIconTransform;
        }
    }
}
