using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/ComboAttack")]
    public class ComboAttackStep : TutorialStepHandler
    {
        public static ComboAttackStep Instance;
        private bool firstStepClear;
        ShinTutorialSystem shinTutorialSystem;
        private int currentStep = 0;
        //이프문 여러개 만들어서 업데이트에서 여러개 퀘스트 돌리기러기하하하

        [SerializeField] private DialogueDataSO dg;

        private bool isCalledKillEnemyDg = false;
        public override void Enter(ShinTutorialSystem system)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("뭐야 이거 왜 인스턴스가 널이 아니지??");
            }
            shinTutorialSystem = system;

            ComboAttackMap.Instance.SpawnWeapon();

            isCalledKillEnemyDg = false;
        }
        public override void Update()
        {

        }

        public void FirstComboClear()
        {
            //이제 이것들을 퀘스트 깨면 호출을 하자고 음 그래그래
            shinTutorialSystem.TestCallback(0, false);
        }

        public void SecondComboClear()
        {
            shinTutorialSystem.TestCallback(1, false);
        }

        public void ThirdComboClear()
        {
            shinTutorialSystem.TestCallback(2, true);
        }
        public override bool IsCompleted()
        {
            return true;
        }

        public void DescriptionForExp()
        {
            if (isCalledKillEnemyDg) return;
            isCalledKillEnemyDg = true;
            DialogueManager.Instance.StartDialogue(dg);
        }
    }
}
