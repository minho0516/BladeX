using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Complete")]
    public class CompleteStep : TutorialStepHandler
    {
        public override void Enter(ShinTutorialSystem system)
        {
            
        }

        public override void Update()
        {
            
        }
        public override bool IsCompleted()
        {
            return false;
        }
    }
}
