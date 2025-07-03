using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Inventory")]
    public class InventoryStep : TutorialStepHandler
    {
        private bool isOpenedInven;
        private bool isEquiepItem;
        public override void Enter(ShinTutorialSystem system)
        {
            
        }
        public override void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                isOpenedInven = true;
                if(Input.GetMouseButtonDown(1))
                {
                    if(isOpenedInven)
                    {
                        isEquiepItem = true;
                    }
                }
            }
        }
        public override bool IsCompleted()
        {
            return Input.GetKeyDown(KeyCode.I);
        }
    }
}
