using UnityEngine;

namespace Swift_Blade
{
    public class IngamePopupText : MonoBehaviour
    {
        public float floatStrength = 0.5f;  // ���Ʒ� �����̴� ����
        public float floatSpeed = 1.0f;     // �����̴� �ӵ�

        private Vector3 initialPosition;

        void Start()
        {
            initialPosition = transform.position;
        }

        void Update()
        {
            float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatStrength;
            transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
        }
    }
}
