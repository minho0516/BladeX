using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStepHandlerSO/Move")]
    public class MoveStep : TutorialStepHandler
    {
        private float timer = 0f;
        private bool finishedMove = false;
        private bool stopMing = false;

        private ShinTutorialSystem systemMing;

        public override void Enter(ShinTutorialSystem system)
        {
            
            timer = 0f;
            Debug.Log("START");

            finishedMove = false;
            stopMing = false;

            //systemMing = system;

            // ���� �� ����
        }

        public override void Update()
        {
            //Debug.Log("�ȳ��ϼ��� �����Ʈ�����Դϴ�");
            //Debug.Log(timer);
            //Debug.Log(systemMing);
            var velocity = Player.Instance.GetPlayerMovement.Controller.linearVelocity.magnitude;
            if (velocity > 0.01f)
                timer += Time.deltaTime;

            if(timer >= 3f && stopMing == false)
            {
                finishedMove = true;
            }

            //finishedMove = timer >= 3f; �̷��ϸ� false���ǳ�

            if (finishedMove)
            {
                ShinTutorialSystem.stepQuestClearEvent?.Invoke();
                finishedMove = false;
                stopMing = true;
            }
        }

        public override bool IsCompleted()
        {
            //ShinTutorialSystem.stepClearEvent?.Invoke();
            if(finishedMove)
            {
                ShinTutorialSystem.stepQuestClearEvent?.Invoke();
                finishedMove = false;
            }

            return (finishedMove);// && TutorialStatusSystem.FinishedComeTrapFront);
        }

        public void Exit()
        {

        }
    }
}
