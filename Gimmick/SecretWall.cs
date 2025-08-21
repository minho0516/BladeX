using DG.Tweening;
using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade
{
    public class SecretWall : MonoBehaviour, IInteractable
    {
        private Vector3 offsetPos, offsetPos2;
        [SerializeField] private Transform interactionIconTransform;

        private void Awake()
        {
            if(transform.eulerAngles.y == 0)
            {
                offsetPos = transform.position + new Vector3(0, 0, 0.3f);
                offsetPos2 = transform.position + new Vector3(3.0f, 0, 0.3f);
            }

            if(transform.eulerAngles.y == 90)
            {
                offsetPos = transform.position + new Vector3(0.3f, 0, 0f);
                offsetPos2 = transform.position + new Vector3(0.3f, 0, 3.0f);
            }

            if (transform.eulerAngles.y == -90)
            {
                offsetPos = transform.position + new Vector3(-0.3f, 0, 0f);
                offsetPos2 = transform.position + new Vector3(-0.3f, 0, -3.0f);
            }

            if (transform.eulerAngles.y == 180)
            {
                offsetPos = transform.position + new Vector3(0, 0, -0.3f);
                offsetPos2 = transform.position + new Vector3(-3.0f, 0, -0.3f);
            }

            
        }
        public void Activate()
        {
            //CamShake
            CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);

            float duration = 1.5f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(offsetPos, duration));
            sequence.Append(transform.DOMove(offsetPos2, duration));
        }

        public void Interact()
        {
            Activate();
        }

        Transform IInteractable.InteractionIconTrasnform()
        {
            return interactionIconTransform;
        }
    }
}
