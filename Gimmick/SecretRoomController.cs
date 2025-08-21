using UnityEngine;
using DG.Tweening;
using Swift_Blade.Feeling;
using Swift_Blade.Level;
using System.Collections.Generic;

namespace Swift_Blade
{
    public class SecretRoomController : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform secretRoomTrm;
        [SerializeField] private Transform secretRoomBridgeTrm;

        [SerializeField] private Transform offBoundery;

        [Header("SecretFloor")]
        [SerializeField] private bool IsFloor = false;
        [SerializeField] private Transform floorTrm;
        [SerializeField] private ParticleSystem dustPar;
        [SerializeField] private Transform dustParTrm;
        [SerializeField] private Chest ChestPrefab;

        private bool isActive = false;
        [SerializeField] private GameObject meshOutlineObject;
        private IEnumerable<GameObject> ienumerableCache;
        [SerializeField] private Transform interactionIconTransform;

        IEnumerable<GameObject> IInteractable.GetMeshGameObject()
        {
            if (ienumerableCache == null)
            {
                ienumerableCache = CollectionReturn();
            }
            return ienumerableCache;

            IEnumerable<GameObject> CollectionReturn()
            {
                yield return meshOutlineObject;
            }
        }

        private void DestroyFloor()
        {
            isActive = true;

            if (floorTrm != null)
            {
                floorTrm.DOScale(new Vector3(0, 0, 0), 0.5f);
            }
            if (dustPar != null)
            {
                Instantiate(dustPar, dustParTrm.position, Quaternion.identity).gameObject.SetActive(true);
            }
            if (ChestPrefab != null)
            {
                Instantiate(ChestPrefab, dustParTrm.position, Quaternion.identity);
            }
        }
        public void Interact()
        {
            if (isActive) return;

            if (IsFloor)
            {
                DestroyFloor();
            }
            //else
            //{
            //    return;
            //}

            isActive = true;
            /// 비밀의 방으로 이동
            /// 이동할때 카메라를 비밀의 방으로 이동
            /// 카메라 셰이크
            /// 비밀의 방, 또는 비밀의 섬이 안개 밑에서 올라옴

            if (offBoundery != null)
            {
                offBoundery.gameObject.SetActive(false);
            }

            Sequence seq = DOTween.Sequence();

            if (secretRoomTrm != null)
            {
                Vector3 pos = secretRoomTrm.position + new Vector3(0, Mathf.Abs(secretRoomTrm.localPosition.y), 0);
                CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);
                seq.Append(secretRoomTrm.DOMove(pos, 1f)); //비밀의 방 올라옴
            }

            if (secretRoomBridgeTrm != null)
            {
                Vector3 bridgePos = secretRoomBridgeTrm.position + new Vector3(0, Mathf.Abs(secretRoomBridgeTrm.localPosition.y), 0);
                CameraShakeManager.Instance.DoShake(CameraShakeType.Middle);
                seq.Append(secretRoomBridgeTrm.DOMove(bridgePos, 1f)); //연결다리 올라옴
            }
        }

        Transform IInteractable.InteractionIconTrasnform()
        {
            return interactionIconTransform;
        }
    }
}
