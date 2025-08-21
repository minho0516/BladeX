using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Complete")]
    public class CompleteStep : TutorialStepHandler
    {
        public static CompleteStep Instance;
        private ShinTutorialSystem shinTutorialSystem;
        public override void Enter(ShinTutorialSystem system)
        {
            shinTutorialSystem = system;
            if (Instance == null) Instance = this;
        }

        public override void Update()
        {
            
        }
        public override bool IsCompleted()
        {
            return false;
        }

        public void AllKillEnemyCallback()
        {
            shinTutorialSystem.TestCallback(0);
        }
    }
}
