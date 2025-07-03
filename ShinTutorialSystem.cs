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
        [SerializeField] private CanvasGroup currentQuestCanvasGroup; // Fade 용
        [SerializeField] private RectTransform nextQuestPanel;
        [SerializeField] private CanvasGroup nextQuestCanvasGroup;
        [SerializeField] private CanvasGroup fadePanelCanvasGroup;

        private Vector2 baseQuestPosition;

        public static Action stepDoorActiveEvent;
        public static Action stepQuestClearEvent;

        private bool hasStepEntered = false;
 
        //Clear이벤트 작동시 Clear이벤트 깨준 스텝의 액션 끊는 함수 작동
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


            //퀘스트 클리어하면 tutorialStemMaps[currentStep].도어.함수호출 후 문올라옴.


            // 맵 이동

            //Vector3 spawnPos;

            //fadePanelCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            //{
            //    Debug.Log("ㅁ.뭐야 . 오. 외안뒈");
            //    
            //
            //    fadePanelCanvasGroup.DOFade(0, 0.5f);
            //});
            //player.gameObject.SetActive(false);
        }
        private void CallbackLevelClearEvent()
        {
            Debug.Log("안녕하세요 콜백레벨클리어이벤트입니다");

            Transform doorTrm = tutorialStepMaps[currentStep].transform.Find("DeathDoor");

            Debug.Log(doorTrm); 

            float playerY = Player.Instance.GetPlayerMovement.transform.position.y;

            doorTrm.GetComponent<DeathDoor>().transform.DOMoveY(playerY, 1f);
            QuestClearAnimation();
            //currentQuestPanel.DOScale(new Vector3(0, 0, 0), 1f);
            //다시 키워줘야겠지? 줄였으면.
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
                go.transform.position = new Vector3(0, 0, idx * 100); // 각 맵을 Z축으로 100씩 떨어뜨려 배치
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
            Debug.Log("레벨 클리어");
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

            //다시키우기
            currentQuestPanel.DOScale(new Vector3(1, 1, 1), 1f);

            // 위로 슉 사라지기
            currentQuestPanel.DOAnchorPosY(baseQuestPosition.y + moveDistance, 0.4f)
                .SetEase(Ease.InQuad);
            currentQuestCanvasGroup.DOFade(0f, 0.3f);

            // 다음 퀘스트 패널 위치 초기화 (아래)
            nextQuestPanel.anchoredPosition = new Vector2(baseQuestPosition.x, baseQuestPosition.y - moveDistance);
            nextQuestCanvasGroup.alpha = 0f;

            // 텍스트 갱신
            nextQuestPanel.GetComponentInChildren<TMP_Text>().text = nextQuestText;

            // 아래에서 위로 올라오기
            DOVirtual.DelayedCall(0.35f, () =>
            {
                nextQuestPanel.DOAnchorPosY(baseQuestPosition.y, 0.4f)
                    .SetEase(Ease.OutQuad);
                nextQuestCanvasGroup.DOFade(1f, 0.4f);

                // 패널 교체
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
