using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Swift_Blade
{
    public class TutorialNPC : NPC
    {
        [SerializeField] private NavMeshAgent Agent;

        //[SerializeField] private List<Transform> targetPositions;
        //[SerializeField] private int currentTargetPos = 0;
        //안쓰는변수워링떠서지움

        private readonly int talk1AnimationHash = Animator.StringToHash("Talk1");
        private readonly int talk2AnimationHash = Animator.StringToHash("Talk2");

        private Animator animator;

        public int currentStepInt = 0;
        public DialogueDataSO startDialogue;

        public override void Interact()
        {
            Debug.Log("인터랙트 윗 튜토리얼 엔피씨");
            TalkWithNPC();

            if (animator != null)
                PlayRandomAnimation();
        }

        private IEnumerator CheckArrival()
        {
            while (Agent.pathPending || Agent.remainingDistance > Agent.stoppingDistance)
            {
                yield return null;
            }

            // 목적지에 도달했을 때 실행
            Debug.Log("도착함!");
            TutorialStatusSystem.FinishedComeTrapFront = true;
        }

        public void CallInteractive(DialogueDataSO dialogue, Action dialogueEndEvent = null)
        {
            DialogueManager.Instance.StartDialogue(dialogue).Subscribe(() =>
            {
                if (animator != null)
                    ClearAnimation();

                dialogueEndEvent?.Invoke();
                OnDialogueEndEvent?.Invoke();
            });
        }

        protected override void TalkWithNPC(Action dialogueEndEvent = null)
        {
            DialogueManager.Instance.StartDialogue(dialogueData).Subscribe(() =>
            {
                if (animator != null)
                    ClearAnimation();

                dialogueEndEvent?.Invoke();
                OnDialogueEndEvent?.Invoke();
            });
        }

        private void PlayRandomAnimation()
        {
            // 진행 중인 Animation 초기화
            ClearAnimation();

            int randAnimation = UnityEngine.Random.Range(0, 2);
            int hash = randAnimation == 0 ? talk1AnimationHash : talk2AnimationHash;

            animator.SetBool(hash, true);
        }
        private void ClearAnimation()
        {
            animator.SetBool(talk1AnimationHash, false);
            animator.SetBool(talk2AnimationHash, false);
        }

        public void MoveToDestination()
        {
            ////if (TutorialStatusSystem.FinishedComeTrapFront) return;
            //Agent.SetDestination(targetPositions[currentStepInt].position);
            //StartCoroutine(nameof(CheckArrival));
            //
            //Debug.Log(targetPositions.Count);
        }

        public void TeleportToDestination(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}
