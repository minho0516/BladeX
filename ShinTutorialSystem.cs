using DG.Tweening;
using Swift_Blade.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using Unity.AppUI.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class ShinTutorialSystem : MonoBehaviour
    {
        public List<TutorialStatusSO> statusSOs;

        private List<GameObject> tutorialStepMaps = new List<GameObject>();
        private List<DeathDoor> mapDoors = new List<DeathDoor>();
        private List<Transform> npcMoveSteps = new List<Transform>();

        [SerializeField] private Player player;
        [SerializeField] private TutorialNPC npc;

        private int currentStep = 0;

        [Header("UIS")]
        [SerializeField] private RectTransform currentQuestPanel;
        [SerializeField] private CanvasGroup currentQuestCanvasGroup; // Fade ��
        [SerializeField] private RectTransform nextQuestPanel;
        [SerializeField] private CanvasGroup nextQuestCanvasGroup;
        [SerializeField] private CanvasGroup fadePanelCanvasGroup;

        private Vector2 baseQuestPosition;

        public static Action stepDoorActiveEvent;
        public static Action stepQuestClearEvent;

        private bool hasStepEntered = false;
 
        //Clear�̺�Ʈ �۵��� Clear�̺�Ʈ ���� ������ �׼� ���� �Լ� �۵�
        private void Awake()
        {
            //navMeshSurface = GetComponent<NavMeshSurface>();

            stepDoorActiveEvent += CallbackDoorActiveEvent;
            stepQuestClearEvent += CallbackLevelClearEvent;

            baseQuestPosition = currentQuestPanel.anchoredPosition;

            SetMaps();
        }
        private void OnDestroy()
        {
            stepDoorActiveEvent -= CallbackDoorActiveEvent;
            stepQuestClearEvent -= CallbackLevelClearEvent;
        }

        private void Start()
        {
            Debug.Log(mapDoors.Count);
            Debug.Log(tutorialStepMaps.Count);
            DefaultInit();

            //npc.CallInteractive(npc.startDialogue);
            DialogueManager.Instance.StartDialogue(statusSOs[currentStep].dialogue);

            AnimateQuestTransition(statusSOs[currentStep].description);
            //navMeshSurface.BuildNavMesh();
        }

        private void Update()
        {
            if(hasStepEntered == false)
            {
                statusSOs[currentStep].stepLogic.Enter(this);
                hasStepEntered = true;
            }
            statusSOs[currentStep].stepLogic.Update();
            //statusSOs[currentStep].stepLogic.IsCompleted();
        }
        private void DefaultInit()
        {
            currentStep = 0;

            //for (int i = 0; i < tutorialStepMaps.Count; i++)
            //{
            //    tutorialStepMaps[i].SetActive(i == currentStep);
            //}

            TeleportToStartPosition();
        }
        private void TeleportToStartPosition()
        {
            Vector3 currentStartPos = tutorialStepMaps[currentStep].transform.Find("StartPosition").position;

            Vector3 playerPos = player.GetPlayerMovement.transform.position;

            Vector3 offset = playerPos - currentStartPos;

            if (currentStep != 0)
            {
                fadePanelCanvasGroup.DOFade(1, 0.5f).OnComplete(()=>
                {
                    tutorialStepMaps[currentStep - 1].SetActive(false);
                    fadePanelCanvasGroup.DOFade(0, 0.5f);
                    tutorialStepMaps[currentStep].transform.position += offset;
                });
            }
            else
            {
                tutorialStepMaps[currentStep].transform.position += offset;
            }
            //spawnPos = tutorialStepMaps[currentStep].transform.Find("StartPosition").position;
            //Debug.Log(spawnPos);
            //player.GetPlayerMovement.transform.position = spawnPos;


            //����Ʈ Ŭ�����ϸ� tutorialStemMaps[currentStep].����.�Լ�ȣ�� �� ���ö��.


            // �� �̵�

            //Vector3 spawnPos;

            //fadePanelCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            //{
            //    Debug.Log("��.���� . ��. �ܾȵ�");
            //    
            //
            //    fadePanelCanvasGroup.DOFade(0, 0.5f);
            //});
            //player.gameObject.SetActive(false);
        }
        private void CallbackLevelClearEvent()
        {
            Debug.Log("�ȳ��ϼ��� �ݹ鷹��Ŭ�����̺�Ʈ�Դϴ�");

            Transform doorTrm = tutorialStepMaps[currentStep].transform.Find("DeathDoor");

            Debug.Log(doorTrm); 

            float playerY = Player.Instance.GetPlayerMovement.transform.position.y;

            doorTrm.GetComponent<DeathDoor>().transform.DOMoveY(playerY, 1f);
            QuestClearAnimation();
            //currentQuestPanel.DOScale(new Vector3(0, 0, 0), 1f);
            //�ٽ� Ű����߰���? �ٿ�����.
            //SwapQuestPanels();
        }
        private IEnumerator TimerCoroutine(float timer, Action action)
        {
            yield return new WaitForSeconds(timer);
            action?.Invoke();
        }
        private void SetMaps()
        {
            int idx = 1;
            foreach(var so in statusSOs)
            {
                if (so.tutorialStepMap == null) return;

                GameObject go = Instantiate(so.tutorialStepMap, transform);
                go.transform.position = new Vector3(0, 0, idx * 100); // �� ���� Z������ 100�� ����߷� ��ġ
                tutorialStepMaps.Add(go);
                mapDoors.Add(go.transform.Find("DeathDoor").GetComponent<DeathDoor>());
                npcMoveSteps.Add(go.transform.Find("NPCMovePosition"));

                idx++;
            }

            /*
            //for (int i = 1; i < statusSOs.Count; i++)
            //{
            //    Vector3 endPos = tutorialStepMaps[i - 1].transform.Find("ConnectPoint_End").position;
            //    Vector3 newStartPos = tutorialStepMaps[i].transform.Find("ConnectPoint_Start").position;
            //    Vector3 newMapPos = tutorialStepMaps[i].transform.position;
            //
            //    Vector3 dirVec = newMapPos - newStartPos;
            //    Vector3 calcPos = dirVec + endPos;
            //
            //    tutorialStepMaps[i].transform.position = calcPos;
            //}
            */
        }

        public void ActiveDoorAdvanceStep()
        {
            currentStep++;
            hasStepEntered = false;

            TeleportToStartPosition();

            //npc.CallInteractive();
            DialogueManager.Instance.StartDialogue(statusSOs[currentStep].dialogue);
            //statusSOs[currentStep].stepLogic
        }

        private void CallbackDoorActiveEvent()
        {
            Debug.Log("���� Ŭ����");
            ActiveDoorAdvanceStep();
            AnimateQuestTransition(statusSOs[currentStep].description);
        }

        public void QuestClearAnimation()
        {
            currentQuestPanel.DOScale(new Vector3(0, 0, 0), 1f);
        }

        public void AnimateQuestTransition(string nextQuestText)
        {
            float moveDistance = 100f;

            //�ٽ�Ű���
            currentQuestPanel.DOScale(new Vector3(1, 1, 1), 1f);

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
