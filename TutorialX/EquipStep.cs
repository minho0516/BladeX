using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Equeip")]
    public class EquipStep : TutorialStepHandler
    {
        public override void Enter(ShinTutorialSystem system)
        {
            
        }

        public override void Update()
        {
            
        }
        public override bool IsCompleted()
        {
            
            return (Input.GetMouseButtonDown(1) && PopupManager.Instance.IsRemainPopup);
        }
    }
}
