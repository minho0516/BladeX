using DG.Tweening;
using Swift_Blade.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

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

        private Vector2 baseQuestPosition;

        public static Action stepDoorActiveEvent;
        public static Action stepQuestClearEvent;

        private bool hasStepEntered = false;

        [Header("Tutorial Point Image")]
        [field : SerializeField] public Image pointImage;

        [Header("Tutorial Parent UICanvas")]
        [SerializeField] private UnityEngine.Canvas ParentCanvas;
        [SerializeField] private TutorialXQuestPanel QuestUIPanelPrefabs;

        private List<GameObject> prefabList = new List<GameObject>();

        [SerializeField] private Image fadePanel;
 
        //Clear이벤트 작동시 Clear이벤트 깨준 스텝의 액션 끊는 함수 작동
        private void Awake()
        {
            //navMeshSurface = GetComponent<NavMeshSurface>();

            stepDoorActiveEvent += CallbackDoorActiveEvent;
            stepQuestClearEvent += CallbackLevelClearEvent;

            //baseQuestPosition = currentQuestPanel.anchoredPosition;

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

            //AnimateQuestTransition(statusSOs[currentStep].description);
            //navMeshSurface.BuildNavMesh();
        }

        private void Update()
        {
            if(hasStepEntered == false)
            {
                hasStepEntered = true;
                int count = statusSOs[currentStep].stepLogic.stepDescriptionCollection.Count;
                Debug.Log(count);
                SetImageForStepQuests(count);
                //이미지 만들어주긔

                statusSOs[currentStep].stepLogic.Enter(this);
            }
            statusSOs[currentStep].stepLogic.Update();
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

        public void DebugLogCurrentStep()
        {
            Debug.Log($"현재 스텝: {currentStep}, 스텝 이름: {statusSOs[currentStep].stepLogic.name}");
        }
        private void SetImageForStepQuests(int idx)
        {
            //리스트 이때 지우기
            prefabList.Clear();
            Debug.Log($"{idx} 개의 퀘스트 스트링 발견");

            if (idx <= 0) return;

            foreach(Transform trm in ParentCanvas.transform)
            {
                Debug.Log($"{trm.name} 오브젝트 삭제");
                Destroy(trm.gameObject);
                //여기오류나는디 인벨리드캐스트익셉션
            }

            for(int i = 0; i < idx; i++)
            {
                GameObject prefab = Instantiate(QuestUIPanelPrefabs, ParentCanvas.transform, false).gameObject;
                
                prefabList.Add(prefab);

                prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(-230, -70); // -40f * i);

                TutorialXQuestPanel panelPrefab = prefab.GetComponent<TutorialXQuestPanel>();
                panelPrefab.panelText.text = statusSOs[currentStep].stepLogic.stepDescriptionCollection[i].GetLocalizedString();

                panelPrefab.clearImg.gameObject.SetActive(false);
                if(i > 0)
                {
                    panelPrefab.GetComponent<Image>().color = new Vector4(0, 0, 0, 0.1f);
                    panelPrefab.clearImg.color = new Vector4(0, 0, 0, 0);
                    panelPrefab.clearBackgroundImage.gameObject.SetActive(false);
                    panelPrefab.panelText.gameObject.SetActive(false);
                }
                
                //리스트 처음에 다 지우고 이렇게 추가할때 하나씩 추가
            }
        }
        public void TestCallback(int idx, bool isLastQuest = true)
        {
            Debug.Log("클리어 함수 호출 테스트");
            //TutorialXQuestPanel questPanel = QuestUIPanelPrefabs.GetComponent<TutorialXQuestPanel>();
            //questPanel.clearImg.gameObject.SetActive(true);

            if (idx >= prefabList.Count)
                return;

            Debug.Log(prefabList.Count);
            var a = prefabList[idx].GetComponent<TutorialXQuestPanel>();
            if (a == null)
                return;

            a.clearImg.gameObject.SetActive(true);
            a.panelText.DOColor(Color.green, 1.5f).OnComplete(() =>
            {
                Debug.Log(prefabList.Count);
                if (prefabList[idx] != null)
                {
                    prefabList[idx].GetComponent<TutorialXQuestPanel>().gameObject.SetActive(false);
                }
                
                if(isLastQuest == false)
                {
                    if (idx + 1 >= prefabList.Count)
                        return;

                    if (prefabList[idx + 1] != null)
                    {
                        TutorialXQuestPanel prefab = prefabList[idx + 1].GetComponent<TutorialXQuestPanel>();
                        //prefab.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 40f);
                        prefab.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                        prefab.clearImg.color = new Vector4(1, 1, 1, 1);
                        prefab.clearBackgroundImage.gameObject.SetActive(true);
                        prefab.panelText.gameObject.SetActive(true);
                    }
                }
            });

            if (isLastQuest)
            {
                CallbackLevelClearEvent();
            }
        }

        private void DeathDoorActiveFadePanelCallback()
        {
            fadePanel.DOFade(1, 0.5f).OnComplete(() =>
            {
                ActiveDoorAdvanceStep();
                fadePanel.DOFade(0, 0.5f);
            });
        }
        private void TeleportToStartPosition()
        {
            Vector3 currentStartPos = tutorialStepMaps[currentStep].transform.Find("StartPosition").position;

            Vector3 playerPos = player.GetPlayerMovement.transform.position;

            Vector3 offset = playerPos - currentStartPos;

            if (currentStep != 0)
            {
                //fadePanelCanvasGroup.DOFade(1, 0.5f).OnComplete(()=>
                //{
                //    tutorialStepMaps[currentStep - 1].SetActive(false);
                //    fadePanelCanvasGroup.DOFade(0, 0.5f);
                //    
                //});
                tutorialStepMaps[currentStep - 1].SetActive(false);

                tutorialStepMaps[currentStep].transform.position += offset;
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
            //Debug.Log("안녕하세요 콜백레벨클리어이벤트입니다");

            Transform doorTrm = tutorialStepMaps[currentStep].transform.Find("DeathDoor");

            Debug.Log(doorTrm); 

            float playerY = Player.Instance.GetPlayerMovement.transform.position.y;

            doorTrm.GetComponent<DeathDoor>().transform.DOMoveY(playerY, 1f);
            //QuestClearAnimation();
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
            //여기서 문제터지는거같은데
            currentStep++;
            hasStepEntered = false;

            TeleportToStartPosition();

            //npc.CallInteractive();
            DialogueManager.Instance.StartDialogue(statusSOs[currentStep].dialogue);
            //statusSOs[currentStep].stepLogic
        }

        private void CallbackDoorActiveEvent()
        {
            //Debug.Log("레벨 클리어");
            //ActiveDoorAdvanceStep();
            DeathDoorActiveFadePanelCallback();
            //AnimateQuestTransition(statusSOs[currentStep].description);
        }
    }
}
