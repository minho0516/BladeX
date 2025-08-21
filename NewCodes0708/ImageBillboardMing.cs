using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ImageBillboardMing : MonoBehaviour
    {
        [SerializeField] private Transform imagesParent;
        private List<Image> imageList = new List<Image>();

        private void Awake()
        {
            foreach(Image image in imagesParent.GetComponentsInChildren<Image>())
            {
                if (image.gameObject != gameObject) // �ڱ� �ڽ��� ����
                {
                    imageList.Add(image);
                }
            }
        }

        public void RefreshAwake()
        {
            imageList.Clear(); // ���� ����Ʈ �ʱ�ȭ

            foreach (Image image in imagesParent.GetComponentsInChildren<Image>())
            {
                if (image.gameObject != gameObject) // �ڱ� �ڽ��� ����
                {
                    imageList.Add(image);
                }
            }
        }
        private void FixedUpdate()
        {
            foreach(Image image in imageList)
            {
                if (image != null)
                {
                    Transform camTrm = Camera.main.transform;
                    Vector3 direction = camTrm.position - image.transform.position;

                    image.transform.forward = -direction;
                }
            }
        }
    }
}
