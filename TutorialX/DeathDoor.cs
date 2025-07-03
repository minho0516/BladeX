using DG.Tweening;
using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade
{
    public class DeathDoor : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform doorTrm;

        [SerializeField] private GameObject meshObject;

        [SerializeField] private bool IsActive = false;
        [SerializeField] private Transform interactionIconTransform;

        public void Interact()
        {
            if (IsActive) return;
            IsActive = true;

            OpenDoor();
            OpenDoorEffect();
        }

        private void OpenDoor()
        {
            if (doorTrm == null) return;
            doorTrm.DORotate(new Vector3(0, GetOpenDoorYRot(), 0), 2f);
            ShinTutorialSystem.stepDoorActiveEvent?.Invoke();
        }
        private void OpenDoorEffect()
        {
            CameraShakeManager.Instance.DoShake(CameraShakeType.LeftRight);
        }

        private float GetOpenDoorYRot()
        {
            if(transform.eulerAngles.y + 90 >= 360)
            {
                return transform.eulerAngles.y + 90 - 360;
            }
            else
            {
                return transform.eulerAngles.y + 90;
            }
        }

        public GameObject GetMeshGameObject()
        {
            return meshObject;
        }

        Transform IInteractable.InteractionIconTrasnform()
        {
            return interactionIconTransform;
        }
    }
}
