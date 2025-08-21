using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Breakable")]
    public class WeaponStep : TutorialStepHandler
    {
        public static WeaponStep Instance = null;

        public DialogueDataSO redDescription;
        public DialogueDataSO greenDescription;
        public DialogueDataSO blueDescription;

        public static bool IsBreakabled = false;
        private ShinTutorialSystem systemMing;

        private bool isRedActive = false, isGreenActive = false, isBlueActive = false;
        public override void Enter(ShinTutorialSystem system)
        {
            systemMing = system;

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("WeaponStep instance already exists, not spawning weapons again.");
            }

            isRedActive = false;
            isGreenActive = false;
            isBlueActive = false;

            if (WeaponStepMap.Instance != null) WeaponStepMap.Instance.SpawnWeapon();
        }

        public override void Update()
        {
            
        }
        public override bool IsCompleted()
        {
            //Debug.Log(TutorialStatusSystem.FinishedBreakable);

            return false; // (Input.GetMouseButtonDown(0) && TutorialStatusSystem.FinishedBreakable);
        }

        public void UseRedWeaponAbil()
        {
            if (isRedActive) return;
            isRedActive = true;

            //

            DialogueManager.Instance.StartDialogue(redDescription);
            systemMing.TestCallback(0, false);
        }
        public void UseGreenWeaponAbil()
        {
            if (isRedActive == false) return;

            //

            if (isGreenActive) return;
            isGreenActive = true;

            //

            DialogueManager.Instance.StartDialogue(greenDescription);
            systemMing.TestCallback(1, false);
        }

        public void UseBlueWeaponAbil()
        {
            if (isRedActive == false) return;
            if (isGreenActive == false) return;

            //

            if (isBlueActive) return;
            isBlueActive = true;

            //

            DialogueManager.Instance.StartDialogue(blueDescription);
            systemMing.TestCallback(2, true);
        }
    }
}
