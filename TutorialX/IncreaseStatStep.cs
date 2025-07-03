using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/IncreaseStep")]
    public class IncreaseStatStep : TutorialStepHandler
    {
        public override void Enter(ShinTutorialSystem system)
        {
            
        }
        public override void Update()
        {
            
        }
        public override bool IsCompleted()
        {
            return (Input.GetMouseButtonDown(0) && PopupManager.Instance.IsRemainPopup);
        }
    }
}
