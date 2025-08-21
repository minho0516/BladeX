using Swift_Blade.UI;
using System;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Inventory")]
    public class InventoryStep : TutorialStepHandler
    {
        public static InventoryStep Instance = null;

        public bool CanUsePotion = false;

        //[SerializeField] private DialogueDataSO openInventoryDialogue;
        [SerializeField] private DialogueDataSO getItemDialogue;
        [SerializeField] private DialogueDataSO equipItemDialogue;

        [SerializeField] private ItemDataSO largePotion;

        private ShinTutorialSystem shinTutorialSystem;

        private bool isGetItemCalled = false;

        public bool isClosePopup = false;

        private bool isCallRagullarPotion = false;

        private bool pressedI = false;
        private bool pressedTab = false;

        private bool isOpendGreatPotionChest = false;

        private bool firstStepClear = false;
        private bool secondStepClear = false;
        private bool thirdStepClear = false;
        private bool fourthStepClear = false;
        private bool fifthStepClear = false;
        private bool sixthStepClear = false;
        public override void Enter(ShinTutorialSystem system)
        {
            if(Instance == null) Instance = this;

            CanUsePotion = false;

            isGetItemCalled = false;
            isClosePopup = false;

            isCallRagullarPotion = false;

            pressedI = false;
            pressedTab = false;

            isOpendGreatPotionChest = false;

            firstStepClear = false;
            secondStepClear = false;
            thirdStepClear = false;
            fourthStepClear = false;
            sixthStepClear = false;

            fifthStepClear = false;
            shinTutorialSystem = system;
        }

        private void OnDestroy()
        {
            Debug.Log("온디스트로이 인벤토리스텝");
            Instance = null;
        }
        public override void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                if (pressedI) return;
                pressedI = true;

                InputIKey();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (isCallRagullarPotion == false) return;

                if (pressedTab) return;
                pressedTab = true;

                InputTabKey();
            }
        }
        public override bool IsCompleted()
        {
            return Input.GetKeyDown(KeyCode.I);
        }

        public void GetItemCallback()
        {
            if (isGetItemCalled) return;
            isGetItemCalled = true;
            DialogueManager.Instance.StartDialogue(getItemDialogue);
        }
        
        public void OpenChestCallback()
        {
            shinTutorialSystem.TestCallback(0, false);

            firstStepClear = true;
        }

        private void InputIKey()
        {
            if (firstStepClear == false) return;
            shinTutorialSystem.TestCallback(1, false);

            secondStepClear = true;
        }
        public void OnEquipParts()
        {
            if (firstStepClear == false) return;
            if (secondStepClear == false) return;

            if (shinTutorialSystem != null)
            {
                shinTutorialSystem.TestCallback(2, false);
            }

            PopupManager.Instance.PopDown(PopupType.Inventory);
            DialogueManager.Instance.StartDialogue(equipItemDialogue);

            thirdStepClear = true;
            CanUsePotion = true;
        }

        public void CallUseRagullarPotion()
        {
            if (firstStepClear == false) return;
            if (secondStepClear == false) return;
            if (thirdStepClear == false) return;

            if (isCallRagullarPotion == true) return; //한번 실행됐으면 실행 안되게

            Debug.Log("콜유즈레귤러포션");
            shinTutorialSystem.TestCallback(3, false);

            SpawnChest();

            isCallRagullarPotion = true;

            fourthStepClear = true;
            CanUsePotion = false;
        }

        public void IsFindGreatPotion()
        {
            if (firstStepClear == false) return;
            if (secondStepClear == false) return;
            if (thirdStepClear == false) return;
            if (fourthStepClear == false) return;

            shinTutorialSystem.TestCallback(4, false);

            fifthStepClear = true;
        }

        private void InputTabKey()
        {
            if (firstStepClear == false) return;
            if (secondStepClear == false) return;
            if (thirdStepClear == false) return;
            if (fourthStepClear == false) return;
            if (fifthStepClear == false) return;

            //위에꺼 깨야 호출할수있게
            if (isOpendGreatPotionChest == false) return;

            //물약이 교환되야 작동되야되는데 지금4벽새시여서 심신미약상태 그래서 물약이 담겨져있는 상자가 열리면
            //탭키를 누를수 있는거로 걍 뭐 완전 야매는아닌데 음 

            Debug.Log("탭키 누름");
            shinTutorialSystem.TestCallback(5, false);

            sixthStepClear = true;
            CanUsePotion = true;
        }

        
        public void CallUseLargePotion()
        {
            if (firstStepClear == false) return;
            if (secondStepClear == false) return;
            if (thirdStepClear == false) return;
            if (fourthStepClear == false) return;
            if (fifthStepClear == false) return;
            if (sixthStepClear == false) return;

            shinTutorialSystem.TestCallback(6, true);
        }

        public void GetGreatPotion()
        {
            isOpendGreatPotionChest = true;
            IsFindGreatPotion();
        }
        private void SpawnChest()
        {
            Debug.Log("스폰체스트 호출됨");
            TutorialInventoryStepMap.Instance.SpawnChest();
        }
    }
}
