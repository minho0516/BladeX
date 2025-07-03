using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Breakable")]
    public class BreakableStep : TutorialStepHandler
    {
        public override void Enter(ShinTutorialSystem system)
        {

        }

        public override void Update()
        {

        }
        public override bool IsCompleted()
        {
            //Debug.Log(TutorialStatusSystem.FinishedBreakable);

            return (Input.GetMouseButtonDown(0) && TutorialStatusSystem.FinishedBreakable);
        }
    }
}
