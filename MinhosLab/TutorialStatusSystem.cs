using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public enum TutorialStep
    {
        None,
        Move,
        Roll,
        OpenInventory,
        EquipItem,
        BreakObject,
        UpgradeStat,
        FrontEnemy,
        Complete
    }

    [System.Serializable]
    public class IntList
    {
        public List<Transform> values = new List<Transform>();
    }

    public class TutorialStatusSystem : MonoBehaviour
    {
        public TutorialStep currentStep = TutorialStep.None;
        public Canvas TutorialCanvas;
        public List<TutorialStatusSO> statusSOs;
        public TMP_Text tutorialText;
        public List<CollisionEvent> collisionEvents;
        public List<GameObject> KeyPanels;

        //[SerializeField] private float requiredMoveDuration = 3f;
        //private float moveTimer = 0f;
        //private float doTime = 0.7f;
        //private bool detectedFireTrap = false;

        public static bool IsTutorial = false;

        public GameObject fadedPanel;

        public TutorialNPC npc;

        //[SerializeField]
        //private List<IntList> npcMovingPositions = new List<IntList>();

        //public TutorialStepHandler stepLogic;

        //
        public static bool IsOnTrap = false;

        public static bool FinishedComeTrapFront = false;
        public static bool FinishedBreakable = false;
        public static bool IsDetectedEnemyFront = false;

        [SerializeField] private DialogueDataSO startDialogue;

        [SerializeField] private RectTransform currentQuestPanel;
        [SerializeField] private CanvasGroup currentQuestCanvasGroup; // Fade ��
        [SerializeField] private RectTransform nextQuestPanel;
        [SerializeField] private CanvasGroup nextQuestCanvasGroup;

        private Vector2 baseQuestPosition;

        [SerializeField] private Transform[] stepColliders;
        private void Awake()
        {
            Debug.Log($"{statusSOs.Count} tutorial steps loaded.");
            // Key �г� �̸� ����
            foreach (var so in statusSOs)
            {
                if (so.KeyGuideObj == null) continue;
                GameObject go = Instantiate(so.KeyGuideObj, TutorialCanvas.transform);
                KeyPanels.Add(go);
                //700 300
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 300);
            }
        }

        private void Start()
        {
            baseQuestPosition = currentQuestPanel.anchoredPosition;

            // �̺�Ʈ ���
            foreach (var e in collisionEvents)
            {
                e.detectedEvent += CheckThrewFiretrap;
            }

            SetKeyGuidePanel(0);

            Image img = fadedPanel.GetComponent<Image>();
            img.color = new Color(0, 0, 0, 1);

            img.DOColor(new Color(0, 0, 0, 0f), 4).OnComplete(() =>
            {
                fadedPanel.SetActive(false);
            });

            AnimateQuestTransition(statusSOs[(int)currentStep].Description);
            //npc.CallInteractive(startDialogue);
        }

        private void OnDisable()
        {
            foreach (var e in collisionEvents)
            {
                e.detectedEvent -= CheckThrewFiretrap;
            }
        }

        private void Update()
        {
            if (statusSOs[(int)currentStep] == null) return;

            statusSOs[(int)currentStep].stepLogic.Update();
            if (statusSOs[(int)currentStep].stepLogic.IsCompleted())
            {
                Debug.Log("Sucessed");
                AdvanceStep();
            }
            // Ʃ�丮�� �ܰ躰 ������Ʈ
        }

        void TryAdvanceStepIf(bool condition)
        {
            if (condition)
                AdvanceStep();
        }

        void AdvanceStep()
        {
            if((int)currentStep + 1 > statusSOs.Count)
            {
                Debug.Log("All tutorial steps completed.");
                //IsTutorial = false;
                //TutorialCanvas.gameObject.SetActive(false);
                return;
            }
            currentStep++;
            npc.currentStepInt = (int)currentStep;

            Debug.Log($"{((int)currentStep)} currentStepInt");
            if(stepColliders[(int)currentStep] != null)
            {
                stepColliders[(int)currentStep].gameObject.SetActive(false);
            }

            if (statusSOs[(int)currentStep].dialogue != null)
            {
                npc.CallInteractive(statusSOs[(int)currentStep].dialogue);
            }
            if (statusSOs[(int)currentStep].itemReward != null)
            {
                InventoryManager.Instance.AddItemToEmptySlot(statusSOs[(int)currentStep].itemReward);
            }
            SetKeyGuidePanel((int)currentStep);
            AnimateQuestTransition(statusSOs[(int)currentStep].Description);

            Debug.Log("NPCMOVE");
            //npc.MoveToDestination();

        }

        void CheckThrewFiretrap()
        {
            Debug.Log("CheckThrewFiretrap called!");
            IsOnTrap = true;
            StartCoroutine(WaitTimer(0.5f, () =>
            {
                IsOnTrap = false;
            }));

            //

            Debug.Log("Invoke!");
            if (!Player.Instance.GetPlayerMovement.CanRoll) // ������ �� ���̶� ���� ��
            {
                StartCoroutine(WaitTimer(1f, () =>
                {
                    //���� ��� �Ⱦ��º��� ���׶� ��°��?
                    //detectedFireTrap = true;
                }));
            }
        }

        IEnumerator WaitTimer(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

        void SetKeyGuidePanel(int idx)
        {
            if (idx < 0 || idx >= KeyPanels.Count)
            {
                Debug.LogError("Index out of range for KeyPanels.");
                return;
            }

            for (int i = 0; i < KeyPanels.Count; i++)
            {
                KeyPanels[i].SetActive(i == idx);
                if(i == idx)
                {
                    tutorialText.text = statusSOs[i].Description;
                }
            }
        }

        public void AnimateQuestTransition(string nextQuestText)
        {
            float moveDistance = 100f;

            // ���� �� �������
            currentQuestPanel.DOAnchorPosY(baseQuestPosition.y + moveDistance, 0.4f)
                .SetEase(Ease.InQuad);
            currentQuestCanvasGroup.DOFade(0f, 0.3f);

            // ���� ����Ʈ �г� ��ġ �ʱ�ȭ (�Ʒ�)
            nextQuestPanel.anchoredPosition = new Vector2(baseQuestPosition.x, baseQuestPosition.y - moveDistance);
            nextQuestCanvasGroup.alpha = 0f;

            // �ؽ�Ʈ ����
            nextQuestPanel.GetComponentInChildren<TMP_Text>().text = nextQuestText;

            // �Ʒ����� ���� �ö����
            DOVirtual.DelayedCall(0.35f, () =>
            {
                nextQuestPanel.DOAnchorPosY(baseQuestPosition.y, 0.4f)
                    .SetEase(Ease.OutQuad);
                nextQuestCanvasGroup.DOFade(1f, 0.4f);

                // �г� ��ü
                SwapQuestPanels();
            });
        }

        private void SwapQuestPanels()
        {
            var tempPanel = currentQuestPanel;
            currentQuestPanel = nextQuestPanel;
            nextQuestPanel = tempPanel;

            var tempGroup = currentQuestCanvasGroup;
            currentQuestCanvasGroup = nextQuestCanvasGroup;
            nextQuestCanvasGroup = tempGroup;
        }
    }
}