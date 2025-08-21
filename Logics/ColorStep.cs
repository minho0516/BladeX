using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Color")]
    public class ColorStep : TutorialStepHandler
    {
        public static ColorStep Instance;

        private bool isActive1 = false;
        private bool isActive2 = false;

        private int statUpCount = 0;
        private int mustUseCount = 4; // The number of stat up actions required to complete this step.

        private ShinTutorialSystem shinTutorialSystem = null;
        public override void Enter(ShinTutorialSystem system)
        {
            statUpCount = 0;
            Player.level.AddExp(10);
            Debug.Log("Color Step Enter");

            isActive1 = false;
            isActive2 = false;

            shinTutorialSystem = system;

            if (Instance == null) Instance = this;
        }

        public override bool IsCompleted()
        {
            return (statUpCount >= mustUseCount);
        }

        public override void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                FirstComboClear();
            }
        }

        public void StatUpCallback()
        {
            Debug.Log("스탯업콜백 on ColorStep");
            statUpCount += 1;

            SecondComboClear();

            //if (statUpCount >= mustUseCount)
            //{
            //    SecondComboClear();
            //}
            // This method can be used to check if the stat up action is completed.
            // For now, it does nothing but can be expanded later.
        }

        public void FirstComboClear()
        {
            if (isActive1) return;
            isActive1 = true;

            //이제 이것들을 퀘스트 깨면 호출을 하자고 음 그래그래
            shinTutorialSystem.TestCallback(0, false);
        }

        public void SecondComboClear()
        {
            if (isActive2) return;
            if (isActive1 == false) return;
            isActive2 = true;

            shinTutorialSystem.TestCallback(1, true);
        }
    }
}
