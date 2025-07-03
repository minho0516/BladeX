using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/FrontEnemy")]
    public class FrontEnemyStep : TutorialStepHandler
    {
        public override void Enter(ShinTutorialSystem system)
        {
            
        }
        public override void Update()
        {
            
        }
        public override bool IsCompleted()
        {
            return TutorialStatusSystem.IsDetectedEnemyFront;
            //닿았을때 이벤트 재사용 ㅠㅠ
        }
    }
}
