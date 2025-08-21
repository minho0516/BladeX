using System;
using UnityEngine;

namespace Swift_Blade
{
    public class ComboStepCollisionDetected : MonoBehaviour
    {
        public Action DetectedAction;

        [SerializeField] private ComboStepCollisionDetected childTrm;
        [SerializeField] private bool haveChild = false;
        public bool IsDetected = false;

        private MeshRenderer mesh;
        private BoxCollider boxCollider;

        private void Awake()
        {
            mesh = GetComponent<MeshRenderer>();
            mesh.enabled = false;
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
        public void TurnOffWall()
        {
            mesh.enabled = false;
            boxCollider.isTrigger = true;

            transform.position += new Vector3(0, 0, 2);
            
            if(haveChild && childTrm != null)
            {
                childTrm.TurnOffWall();
            }
        }
        public void TurnOnWall()
        {
            mesh.enabled = true;
            boxCollider.isTrigger = false;

            transform.position += new Vector3(0, 0, -2);

            if(haveChild && childTrm != null)
            {
                childTrm.TurnOnWall();
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (IsDetected) return;

            IsDetected = true;
            DetectedAction?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            //IsDetected = false;
        }
    }
}
