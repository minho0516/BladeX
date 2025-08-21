using DG.Tweening;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Swift_Blade
{
    public abstract class BaseOrb : MonoBehaviour
    {
        //public abstract void SetRandom();// ColorType color);
        public abstract IPlayerEquipable GetEquipable { get; }
    }
    public abstract class BaseOrb<T> : BaseOrb, IInteractable
        where T : class
    {
        [SerializeField] protected float startFadeScale;
        [SerializeField] protected float collectFadeEndDuration;

        //[SerializeField] protected bool isPrePlaced;//random select
        [SerializeField] protected T defaultItem;
        [SerializeField] protected Material[] colors;
        [SerializeField] private GameObject meshObject;

        private Tween interactTween;
        protected MeshRenderer itemRenderer;

        private bool isCollected;
        [SerializeField] private Transform interactionTransform;

        protected virtual bool CanInteract => !isCollected;

        protected abstract IReadOnlyList<T> GetReadonlyList { get; }
        protected virtual void Awake()
        {

            itemRenderer = GetComponentInChildren<MeshRenderer>();
            const float START_FADE_DURATION = 0.75f;
            transform.DOScale(startFadeScale, START_FADE_DURATION)
                .SetDelay(0.1f)
                .SetEase(Ease.OutElastic)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            //Debug.Assert(!isPrePlaced || defaultItem == null, "preplaced but defaultItem is not null");

            if (defaultItem == null)
            {
                OnDefaultItemNull();
            }
        }
        protected virtual void Start()
        {
            Initialize();
        }
        protected virtual void OnDefaultItemNull()
        {
            Debug.Assert(defaultItem == null, "default item isn't null");
            IReadOnlyList<T> readonlyList = GetReadonlyList;
            //crazy
            if (readonlyList == null) return;
            //Debug.Assert(readonlyList != null, "list is null");

            T randomItem = SelectRandomItem(readonlyList);
            defaultItem = randomItem;
            Debug.Assert(defaultItem != null, "default item is still null");
        }
        protected abstract void Initialize();
        protected static T SelectRandomItem(IReadOnlyList<T> readonlyList)
        {
            int randomIndex = Random.Range(0, readonlyList.Count);
            T result = readonlyList[randomIndex];
            return result;
        }
        //protected abstract void SelectRandomItem();// ColorType color);
        //public sealed override void SetRandom()// ColorType color)
        //{
        //    //if (color.ContainsNonRGBColor())
        //    //{
        //    //    Debug.LogError($"{color} is not RGB");
        //    //    return;
        //    //}

        //    //if(itemRenderer != null)
        //    //{
        //    //    itemRenderer.material = colors[(int)color];
        //    //}
        //    SelectRandomItem();// color);
        //}
        protected virtual void Interact()
        {
            //print(InventoryManager.Instance.IsAllSlotsFull());

            isCollected = true;

            if (interactTween != null)
            {
                interactTween.Kill();
            }

            interactTween = InteractTween();

            TweenCallback onComplete = CollectTweenCallback();
            bool isCallbackNull = onComplete == null;
            if (!isCallbackNull)
            {
                /*static bool IsSubjectParticle(char lastChar)//jyd code
                {
                    bool result = (lastChar - 0xAC00) % 28 != 0;
                    return result;
                }*/
                
                bool isEquipment = GetEquipable == null;
                string itemName = isEquipment ? "potion" : GetEquipable.DisplayName;
                //string jungYungDo = IsSubjectParticle(itemName[^1]) ? "À»" : "¸¦";

                string textColor = GetTextColor();
                textColor ??= "#1abc9c";
                
                string itemString = $"<color={textColor}>{itemName}</color>";
                string collectMessage = LocalizationManager.GetString(LocalizationManager.Prefix.Popup, "collectedInfo", new object[] { itemString });

                void CollectMessageCallback()
                {
                    PopupManager.Instance.LogInfoBox(collectMessage);
                }
            
                interactTween.OnComplete(onComplete + CollectMessageCallback);
            }
        }

        private string GetTextColor()
        {
            switch (GetEquipable.GetColor)
            {
                case ColorType.RED:
                    return "#ff5252";
                case ColorType.BLUE: 
                    return "#3498db";
                case ColorType.GREEN:
                    return "#44bd32";
            }
            
            return null;
        }

        void IInteractable.Interact()
        {
            if (!CanInteract)
            {
                return;
            }

            Interact();
        }
        Transform IInteractable.InteractionIconTrasnform()
        {
            return interactionTransform;
        }
        protected virtual Tween InteractTween()
        {
            return transform.DOScale(0.01f, collectFadeEndDuration)
                .SetDelay(0.1f)
                .SetEase(Ease.OutExpo)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        protected virtual TweenCallback CollectTweenCallback() => null;

    }
}