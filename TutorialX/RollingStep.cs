using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Rolling")]
    public class RollingStep : TutorialStepHandler
    {
        //private bool finishedRoll = false;
        public static bool IsRolled = false;
        private bool eventIsCalled = false;
        public override void Enter(ShinTutorialSystem system)
        {
            
        }

        public override void Update()
        {
            //stepQuestClearEventȣ��
            if(IsRolled && eventIsCalled == false)
            {
                //�̺�Ʈȣ��
                ShinTutorialSystem.stepQuestClearEvent?.Invoke();
                eventIsCalled = true;
            }
        }

        public override bool IsCompleted()
        {
            return (Player.Instance.GetPlayerMovement.CanRoll == false);
        }
    }
}
