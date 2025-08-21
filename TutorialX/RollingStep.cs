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
        private ShinTutorialSystem systemMing = null;
        public override void Enter(ShinTutorialSystem system)
        {
            eventIsCalled = false;
            systemMing = system;
        }

        public override void Update()
        {
            //stepQuestClearEvent호출
            if(IsRolled && eventIsCalled == false)
            {
                //이벤트호출
                Debug.Log("RollingStep Update IsRolled is true, calling event");
                systemMing.TestCallback(0);
                eventIsCalled = true;
            }
        }

        public override bool IsCompleted()
        {
            return (Player.Instance.GetPlayerMovement.CanRoll == false);
        }
    }
}
