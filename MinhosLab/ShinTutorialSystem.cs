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
 
        //Clear�̺�Ʈ �۵��� Clear�̺�Ʈ ���� ������ �׼� ���� �Լ� �۵�
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
                //�̹��� ������ֱ�

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
            Debug.Log($"���� ����: {currentStep}, ���� �̸�: {statusSOs[currentStep].stepLogic.name}");
        }
        private void SetImageForStepQuests(int idx)
        {
            //����Ʈ �̶� �����
            prefabList.Clear();
            Debug.Log($"{idx} ���� ����Ʈ ��Ʈ�� �߰�");

            if (idx <= 0) return;

            foreach(Transform trm in ParentCanvas.transform)
            {
                Debug.Log($"{trm.name} ������Ʈ ����");
                Destroy(trm.gameObject);
                //����������µ� �κ�����ĳ��Ʈ�ͼ���
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
                
                //����Ʈ ó���� �� ����� �̷��� �߰��Ҷ� �ϳ��� �߰�
            }
        }
        public void TestCallback(int idx, bool isLastQuest = true)
        {
            Debug.Log("Ŭ���� �Լ� ȣ�� �׽�Ʈ");
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
            //Debug.Log("�ȳ��ϼ��� �ݹ鷹��Ŭ�����̺�Ʈ�Դϴ�");

            Transform doorTrm = tutorialStepMaps[currentStep].transform.Find("DeathDoor");

            Debug.Log(doorTrm); 

            float playerY = Player.Instance.GetPlayerMovement.transform.position.y;

            doorTrm.GetComponent<DeathDoor>().transform.DOMoveY(playerY, 1f);
            //QuestClearAnimation();
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
            //���⼭ ���������°Ű�����
            currentStep++;
            hasStepEntered = false;

            TeleportToStartPosition();

            //npc.CallInteractive();
            DialogueManager.Instance.StartDialogue(statusSOs[currentStep].dialogue);
            //statusSOs[currentStep].stepLogic
        }

        private void CallbackDoorActiveEvent()
        {
            //Debug.Log("���� Ŭ����");
            //ActiveDoorAdvanceStep();
            DeathDoorActiveFadePanelCallback();
            //AnimateQuestTransition(statusSOs[currentStep].description);
        }
    }
}
