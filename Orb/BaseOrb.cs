using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class BaseOrb : MonoBehaviour
    {
        //public abstract void SetRandom();// ColorType color);
    }
    public abstract class BaseOrb<T> : BaseOrb, IInteractable
        where T : class
    {
        [SerializeField] protected float startFadeScale;
        [SerializeField] protected float collectFadeEndDuration;

        //[SerializeField] protected bool isPrePlaced;//random select
        [SerializeField] protected T defaultItem;
        [SerializeField] protected Material[] colors;

        private Tween interactTween;
        protected MeshRenderer itemRenderer;

        private bool isCollected;
        protected virtual bool CanInteract => !isCollected;
        protected abstract IReadOnlyList<T> GetReadonlyList { get; }
        protected virtual void Awake()
        {
            itemRenderer = GetComponent<MeshRenderer>();
            const float START_FADE_DURATION = 0.75f;
            transform.DOScale(startFadeScale, START_FADE_DURATION)
                .SetDelay(0.1f)
                .SetEase(Ease.OutElastic)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            //Debug.Assert(!isPrePlaced || defaultItem == null, "preplaced but defaultItem is not null");

            if (defaultItem == null)
            {
                IReadOnlyList<T> readonlyList = GetReadonlyList;
                //crazy
                if (readonlyList == null) return;
                //Debug.Assert(readonlyList != null, "list is null");

                T randomItem = SelectRandomItem(readonlyList);
                Debug.Assert(randomItem != null, "item is null");
                defaultItem = randomItem;
            }
        }
        protected virtual void Start()
        {
            Initialize();
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
                interactTween.OnComplete(onComplete);
            }
        }
        void IInteractable.Interact()
        {
            if (!CanInteract)
            {
                return;
            }

            Interact();
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