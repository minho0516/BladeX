using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    enum ComboAttackType
    {
        None,
        Left3Combo,
        Right2Combo,
        SpaceRightCombo,
        End
    }
    public class ComboAttackMap : MonoBehaviour
    {
        [SerializeField] private Transform followImagesParent;

        [Header("mouse Images")]
        [SerializeField] private Sprite leftMouseImage;
        [SerializeField] private Sprite rightMouseImage;
        [SerializeField] private Sprite spacebarImage;

        [SerializeField] private Sprite plusImage;

        [Header("기준포지션FollowActorOriginPosition")]
        [SerializeField] private Transform originPos;

        private ComboAttackType currentComboState;

        //SpaceRightCombo
        private Action<bool> spaceRightComboAction;
        private Action<bool> rightrightComboAction;
        private Action<bool> left3ComboAction;

        private bool isComboCoroutineActive = false;

        private Transform currentImages = null;

        private Image spaceRightComboSpaceImage, spaceRightComboRightImage, spaceRightComboPlusImage;
        private Image right2ComboRightImage1, right2ComboRightImage2, right2ComboPlusImage;
        private Image left3ComboLeftImage1, left3ComboLeftImage2, left3ComboLeftImage3, left3ComboPlusImage1, left3ComboPlusImage2;

        [SerializeField] private Sprite clearSprite;

        [Header("TutorialX Enemy Spawner")]
        [field: SerializeField] public TutorialXSpawner TutorialXSpawner { get; private set; }

        private bool left3StepClear = false; //이거 불값 있어야 이미 클리어했는데 또 눌러서 이미지 다시 띄우게 하는거 막고,
        //클리어 해야 적에게 대미지 들어가게 할듯 크크루삥뽕
        private bool right2StepClear = false;
        private bool spaceRightStepClear = false;

        public static bool ComboStepClear = false;

        [SerializeField] private ComboAttackType firstStep, secondStep, thirdStep;

        [SerializeField] private ComboStepCollisionDetected firstColl, secondColl, thirdColl;

        [SerializeField] private ImageBillboardMing imageBillboardMing;

        public static bool IsCanKillEnemy = false;
        public static bool isComboStepActive = false; //콤보 스텝이 활성화되면 true, 아니면 false

        public static ComboAttackMap Instance;

        [SerializeField] private WeaponOrb weaponOrbDagger;
        [SerializeField] private WeaponOrb weaponOrbGreat;
        [SerializeField] private WeaponOrb weaponOrbSword;

        [SerializeField] private Transform daggerPos, greatPos, swordPos;

        public void SpawnWeapon()
        {
            //Instantiate(weaponOrbDagger, daggerPos.position, Quaternion.identity);
            //Instantiate(weaponOrbGreat, greatPos.position, Quaternion.identity);
            //Instantiate(weaponOrbSword, swordPos.position, Quaternion.identity);

            //웨폰 전용 스텝이 되었기떄문에 (상자, 수정 부수는스텝이) 이거는 지우기
        }
        private void Awake()
        {
            if (Instance == null) Instance = this;
            spaceRightComboAction += SpaceRightComboActionCallback;
            rightrightComboAction += RightRightComboActonCallback;
            left3ComboAction += Left3ComboActionCallback;
            //SpaceRightCombo();
            //Right2Combo();
            //Left3Combo();

            firstColl.DetectedAction += FirstDetectedCallback;
            secondColl.DetectedAction += SecondDetectedCallback;
            thirdColl.DetectedAction += ThirdDetectedCallback;
        }
        private void OnEnable()
        {
            isComboStepActive = true;
        }

        private void OnDisable()
        {
            isComboStepActive = false;
        }
        private void OnDestroy()
        {
            spaceRightComboAction -= SpaceRightComboActionCallback;
            rightrightComboAction -= RightRightComboActonCallback;
            left3ComboAction -= Left3ComboActionCallback;

            firstColl.DetectedAction -= FirstDetectedCallback;
            secondColl.DetectedAction -= SecondDetectedCallback;
            thirdColl.DetectedAction -= ThirdDetectedCallback;
        }

        private void Update()
        {
            switch (currentComboState)
            {
                case ComboAttackType.Left3Combo:
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            //Debug.Log("LeftClick is pressed");
                            if (isComboCoroutineActive) return;
                            if (left3StepClear) return;

                            FadeClearStepImage(left3ComboLeftImage1);

                            StartCoroutine(Left3ClickCoroutine());
                        }
                    }
                    break;
                case ComboAttackType.Right2Combo:
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            //Debug.Log("RightClick is pressed");
                            if (isComboCoroutineActive) return;
                            if (right2StepClear) return;

                            FadeClearStepImage(right2ComboRightImage1);
                            StartCoroutine(Right2ClickCoroutine(right2ComboRightImage2));
                        }
                    }
                    break;
                case ComboAttackType.SpaceRightCombo:
                    {
                        if(Input.GetKeyDown(KeyCode.Space))
                        {
                            //Debug.Log("Spacebar is pressed");
                            if (isComboCoroutineActive) return;
                            if (spaceRightStepClear) return;

                            FadeClearStepImage(spaceRightComboSpaceImage);
                            StartCoroutine(SpaceRightClickCoroutine(spaceRightComboRightImage));
                        }
                    }
                    break;
                case ComboAttackType.End:
                    // Do nothing
                    break;
            }

            //따라다니기

            if(currentImages != null) currentImages.transform.position = new Vector3(originPos.position.x, originPos.position.y, Player.Instance.GetPlayerMovement.transform.position.z);
        }

        // 클리어한 상태에서 또 누르면 다시 되는거 막기

        public void DetectedWallTurnOn()
        {
            firstColl.TurnOnWall();
            secondColl.TurnOnWall();
            thirdColl.TurnOnWall();
        }
        public void DetectedWallTurnOff()
        {
            firstColl.TurnOffWall();
            secondColl.TurnOffWall();
            thirdColl.TurnOffWall();
        }
        private void FirstDetectedCallback()
        {
            Debug.Log("FIRST DETECTED CALLBACK");

            //currentImages = null; //뭐 함 해보든가
            DetectedWallTurnOn();
            ChangeComboStep(firstStep);
            TutorialXSpawner.Ming(0); //한번 소환해보자 ㅇㅇ
        }
        private void SecondDetectedCallback()
        {
            Debug.Log("SECOND DETECTED CALLBACK");

            //currentImages = null; //뭐 함 해보든가
            DetectedWallTurnOn();
            ChangeComboStep(secondStep);
            TutorialXSpawner.Ming(1); //한번 소환해보자 ㅇㅇ
        }

        private void ThirdDetectedCallback()
        {
            Debug.Log("THIRD DETECTED CALLBACK");

            //currentImages = null; //뭐 함 해보든가
            DetectedWallTurnOn();
            ChangeComboStep(thirdStep);
            TutorialXSpawner.Ming(2); //한번 소환해보자 ㅇㅇ
        }
        
        private void ChangeComboStep(ComboAttackType indexType)
        {
            //적 죽일수 있는거 false
            IsCanKillEnemy = false;
            currentComboState = indexType;
            switch (indexType)
            {
                case ComboAttackType.Left3Combo:
                    if (left3StepClear == false)
                    {
                        Left3Combo();
                    }
                    break;
                case ComboAttackType.Right2Combo:
                    if (right2StepClear == false)
                    {
                        Right2Combo();
                    }
                    break;
                case ComboAttackType.SpaceRightCombo:
                    if (spaceRightStepClear == false)
                    {
                        SpaceRightCombo();
                    }
                    break;
                case ComboAttackType.End:
                    // Do nothing
                    break;
            }

            imageBillboardMing.RefreshAwake(); //이미지 빌보드 새로고침
        }

        private void Left3Combo()
        {
            //이거는 한번만
            currentComboState = ComboAttackType.Left3Combo;

            GameObject parentGO = new GameObject($"Right2Combo_ImageParent");
            parentGO.transform.SetParent(followImagesParent);

            for (int i = 0; i < 5; i++)
            {
                GameObject newImageObject = new GameObject($"FollowImage_{i}");
                newImageObject.transform.SetParent(parentGO.transform);
                Image img = newImageObject.AddComponent<Image>();

                img.rectTransform.sizeDelta = new Vector2(2, 2);

                if (i == 0)
                {
                    img.sprite = leftMouseImage; img.rectTransform.position = new Vector3(0, 0, -4);
                    left3ComboLeftImage1 = img; // Left Mouse 이미지 저장
                }
                else if (i == 1)
                {
                    img.sprite = plusImage; img.rectTransform.position = new Vector3(0, 0, -2);
                    left3ComboPlusImage1 = img;
                }
                else if (i == 2)
                {
                    img.sprite = leftMouseImage; img.rectTransform.position = new Vector3(0, 0, 0);
                    left3ComboLeftImage2 = img; // Left Mouse 이미지 저장
                }
                else if(i == 3)
                {
                    img.sprite = plusImage; img.rectTransform.position = new Vector3(0, 0, 2);
                    left3ComboPlusImage2 = img;
                }
                else if(i == 4)
                {
                    img.sprite = leftMouseImage; img.rectTransform.position = new Vector3(0, 0, 4);
                    left3ComboLeftImage3 = img; // Left Mouse 이미지 저장
                }
            }

            currentImages = parentGO.transform;
        }
        private void Right2Combo()
        {
            //이거는 한번만
            currentComboState = ComboAttackType.Right2Combo;

            GameObject parentGO = new GameObject($"Right2Combo_ImageParent");
            parentGO.transform.SetParent(followImagesParent);

            for (int i = 0; i < 3; i++)
            {
                GameObject newImageObject = new GameObject($"FollowImage_{i}");
                newImageObject.transform.SetParent(parentGO.transform);
                Image img = newImageObject.AddComponent<Image>();

                img.rectTransform.sizeDelta = new Vector2(2, 2);

                if (i == 0)
                {
                    img.sprite = rightMouseImage; img.rectTransform.position = new Vector3(0, 0, -2);
                    right2ComboRightImage1 = img; // Right Mouse 이미지 저장
                }
                else if (i == 1)
                {
                    img.sprite = plusImage; img.rectTransform.position = new Vector3(0, 0, 0);
                    right2ComboPlusImage = img;
                }
                else if (i == 2)
                {
                    img.sprite = rightMouseImage; img.rectTransform.position = new Vector3(0, 0, 2);
                    right2ComboRightImage2 = img; // Right Mouse 이미지 저장
                }
            }

            currentImages = parentGO.transform;
        }

        private void SpaceRightCombo()
        {
            //이거는 한번만
            currentComboState = ComboAttackType.SpaceRightCombo;

            GameObject parentGO = new GameObject($"SpaceRightCombo_ImageParent");
            parentGO.transform.SetParent(followImagesParent);

            for (int i = 0; i < 3; i++)
            {
                GameObject newImageObject = new GameObject($"FollowImage_{i}");
                newImageObject.transform.SetParent(parentGO.transform);
                Image img = newImageObject.AddComponent<Image>();

                img.rectTransform.sizeDelta = new Vector2(2, 2);

                if (i == 0)
                {
                    img.sprite = spacebarImage; img.rectTransform.position = new Vector3(0, 0, -2);
                    spaceRightComboSpaceImage = img; // Spacebar 이미지 저장
                }
                else if (i == 1)
                {
                    img.sprite = plusImage; img.rectTransform.position = new Vector3(0, 0, 0);
                    spaceRightComboPlusImage = img;
                }
                else if (i == 2)
                {
                    img.sprite = rightMouseImage; img.rectTransform.position = new Vector3(0, 0, 2);
                    spaceRightComboRightImage = img; // Right Mouse 이미지 저장
                }
            }

            currentImages = parentGO.transform;
        }

        private IEnumerator SpaceRightClickCoroutine(Image rightClickImage)
        {
            isComboCoroutineActive = true;

            //space바가 눌렸을때 작동
            float timer = 0f;
            float waitTime = 0.3f;

            while (timer < waitTime)
            {
                if (Input.GetMouseButtonDown(1)) // 우클릭
                {
                    Debug.Log("코루틴중 우클릭");
                    FadeClearStepImage(rightClickImage);

                    isComboCoroutineActive = false;
                    spaceRightComboAction?.Invoke(true);

                    yield break;
                }

                if (Input.GetMouseButtonDown(0)) //우클릭 안됩니다
                {
                    isComboCoroutineActive = false;
                    rightrightComboAction?.Invoke(false);

                    yield break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            spaceRightComboAction?.Invoke(false);

            isComboCoroutineActive = false;
        }

        private IEnumerator Right2ClickCoroutine(Image rightClickImage)
        {
            isComboCoroutineActive = true;

            //우클릭이 눌렸을때 작동
            float timer = 0f;
            float waitTime = 0.7f;
            float canSucessTime = 0.2f;

            while (timer < waitTime)
            {
                timer += Time.deltaTime;
                yield return null;

                if (timer > canSucessTime) //바로누르면 안되게 (실제로 콤보가 좀 쉬었다가 써줘야됨)
                {
                    if (Input.GetMouseButtonDown(1)) // 우클릭
                    {
                        Debug.Log("코루틴중 우클릭");
                        FadeClearStepImage(rightClickImage);

                        isComboCoroutineActive = false;
                        rightrightComboAction?.Invoke(true);
                        yield break;
                    }

                    if(Input.GetMouseButtonDown(0)) // 좌클 안됩니다
                    {
                        isComboCoroutineActive = false;
                        rightrightComboAction?.Invoke(false);

                        yield break;
                    }

                    if(Input.GetKeyDown(KeyCode.Space)) //스페이스 안됩니다
                    {
                        isComboCoroutineActive = false;
                        rightrightComboAction?.Invoke(false);

                        yield break;
                    }
                }
            }

            isComboCoroutineActive = false;
            rightrightComboAction?.Invoke(false);
        }

        private IEnumerator Left3ClickCoroutine()
        {
            isComboCoroutineActive = true;

            float timer = 0f;
            float waitTime = 0.5f;

            while (timer < waitTime)
            {
                timer += Time.deltaTime;
                yield return null;

                if (Input.GetMouseButtonDown(0)) // 좌클릭
                {
                    Debug.Log("코루틴중 좌클릭");
                    FadeClearStepImage(left3ComboLeftImage2);

                    isComboCoroutineActive = false;
                    StartCoroutine(Left3ClickCoroutine2());
                    yield break;
                }

                if (Input.GetMouseButtonDown(1)) //우클릭 안됩니다
                {
                    isComboCoroutineActive = false;
                    rightrightComboAction?.Invoke(false);

                    yield break;
                }

                if (Input.GetKeyDown(KeyCode.Space)) //스페이스 안됩니다
                {
                    isComboCoroutineActive = false;
                    rightrightComboAction?.Invoke(false);

                    yield break;
                }
            }

            isComboCoroutineActive = false;
            left3ComboAction?.Invoke(false);
        }

        private IEnumerator Left3ClickCoroutine2()
        {
            isComboCoroutineActive = true;

            float timer = 0f;
            float waitTime = 0.5f;

            while (timer < waitTime)
            {
                timer += Time.deltaTime;
                yield return null;

                if (Input.GetMouseButtonDown(0)) // 우클릭
                {
                    Debug.Log("코루틴중 좌클릭 22");
                    FadeClearStepImage(left3ComboLeftImage3);

                    isComboCoroutineActive = false;
                    left3ComboAction?.Invoke(true);
                    yield break;
                }
            }

            isComboCoroutineActive = false;
            left3ComboAction?.Invoke(false);
        }

        private void SpaceRightComboActionCallback(bool isSuccess)
        {
            Debug.Log(isSuccess);
            if(isSuccess == false)
            {
                ClearFadeEffect();

                //싯파이스루
            }
            else if(isSuccess == true)
            {
                ComboStepClear = true;

                spaceRightComboPlusImage.sprite = clearSprite;

                spaceRightStepClear = true;

                IsCanKillEnemy = true;

                Debug.Log(currentComboState);
                Debug.Log("커런트콤보스테이트입니다");

                if (currentComboState == ComboAttackType.SpaceRightCombo)
                {
                    if (currentComboState == firstStep)
                    {
                        ComboAttackStep.Instance.FirstComboClear();
                        Debug.Log("퍼스트콤보 클리어 함수 호출");
                        //아이거 생각해보니까 리스트안에 텍스트 그거도 이거에 맞게 고쳐야되는데 좀 구조 망한거같은데 
                    }
                    else if (currentComboState == secondStep)
                    {
                        ComboAttackStep.Instance.SecondComboClear();
                        Debug.Log("세컨드콤보 클리어 함수 호출");
                    }
                    else if (currentComboState == thirdStep)
                    {
                        ComboAttackStep.Instance.ThirdComboClear();
                        Debug.Log("떠드콤보 클리어 함수 호출");
                    }
                }
            }
        }
        private void RightRightComboActonCallback(bool isSuccess)
        {
            if(isSuccess == false)
            {
                ClearFadeEffect();
            }
            else if(isSuccess == true)
            {
                ComboStepClear = true;

                right2ComboPlusImage.sprite = clearSprite;

                right2StepClear = true;

                IsCanKillEnemy = true;

                Debug.Log(currentComboState);
                Debug.Log("커런트콤보스테이트입니다");

                if (currentComboState == ComboAttackType.Right2Combo)
                {
                    if (currentComboState == firstStep)
                    {
                        ComboAttackStep.Instance.FirstComboClear();
                        Debug.Log("퍼스트콤보 클리어 함수 호출");
                        //아이거 생각해보니까 리스트안에 텍스트 그거도 이거에 맞게 고쳐야되는데 좀 구조 망한거같은데 
                    }
                    else if (currentComboState == secondStep)
                    {
                        ComboAttackStep.Instance.SecondComboClear();
                        Debug.Log("세컨드콤보 클리어 함수 호출");
                    }
                    else if (currentComboState == thirdStep)
                    {
                        ComboAttackStep.Instance.ThirdComboClear();
                        Debug.Log("떠드콤보 클리어 함수 호출");
                    }
                }
            }
        }

        private void Left3ComboActionCallback(bool isSuccess)
        {
            if(isSuccess == false)
            {
                ClearFadeEffect();
            }
            else if(isSuccess == true)
            {
                ComboStepClear = true;

                left3ComboPlusImage1.sprite = clearSprite;
                left3ComboPlusImage2.sprite = clearSprite;

                left3StepClear = true;

                IsCanKillEnemy = true;

                Debug.Log(currentComboState);
                Debug.Log("커런트콤보스테이트입니다");
                if(currentComboState == ComboAttackType.Left3Combo)
                {
                    if (currentComboState == firstStep)
                    {
                        ComboAttackStep.Instance.FirstComboClear();
                        Debug.Log("퍼스트콤보 클리어 함수 호출");
                        //아이거 생각해보니까 리스트안에 텍스트 그거도 이거에 맞게 고쳐야되는데 좀 구조 망한거같은데 
                    }
                    else if (currentComboState == secondStep)
                    {
                        ComboAttackStep.Instance.SecondComboClear();
                        Debug.Log("세컨드콤보 클리어 함수 호출");
                    }
                    else if (currentComboState == thirdStep)
                    {
                        ComboAttackStep.Instance.ThirdComboClear();
                        Debug.Log("떠드콤보 클리어 함수 호출");
                    }
                }
            }
        }

        private void FadeClearStepImage(Image img)
        {
            if (img != null)
            {
                img.CrossFadeAlpha(0f, 1f, false);
                img.gameObject.SetActive(false);
            }
        }

        private void ClearFadeEffect()
        {
            switch(currentComboState)
            {
                case ComboAttackType.Left3Combo:
                    {
                        left3ComboLeftImage1.CrossFadeAlpha(1f, 0.1f, false);
                        left3ComboLeftImage1.gameObject.SetActive(true);

                        left3ComboLeftImage2.CrossFadeAlpha(1f, 0.1f, false);
                        left3ComboLeftImage2.gameObject.SetActive(true);

                        left3ComboLeftImage3.CrossFadeAlpha(1f, 0.1f, false);
                        left3ComboLeftImage3.gameObject.SetActive(true);
                    }
                    break;
                case ComboAttackType.Right2Combo:
                    {
                        right2ComboRightImage1.CrossFadeAlpha(1f, 0.1f, false);
                        right2ComboRightImage1.gameObject.SetActive(true);

                        right2ComboRightImage2.CrossFadeAlpha(1f, 0.1f, false);
                        right2ComboRightImage2.gameObject.SetActive(true);
                    }
                    break;
                case ComboAttackType.SpaceRightCombo:
                    {
                        spaceRightComboSpaceImage.CrossFadeAlpha(1f, 0.1f, false);
                        spaceRightComboSpaceImage.gameObject.SetActive(true);

                        spaceRightComboRightImage.CrossFadeAlpha(1f, 0.1f, false);
                        spaceRightComboRightImage.gameObject.SetActive(true);
                    }
                    break;
                case ComboAttackType.End:
                    // Do nothing
                    break;
            }
        }
    }
}
