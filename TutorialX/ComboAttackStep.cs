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
        //������ ������ ���� ������Ʈ���� ������ ����Ʈ �����ⷯ��������

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
                Debug.LogError("���� �̰� �� �ν��Ͻ��� ���� �ƴ���??");
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
            //���� �̰͵��� ����Ʈ ���� ȣ���� ���ڰ� �� �׷��׷�
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
