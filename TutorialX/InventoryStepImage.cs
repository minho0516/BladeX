using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class InventoryStepImage : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public static InventoryStepImage Instance;

        [SerializeField] private TMP_Text followTxt;
        [SerializeField] private string followDescription;

        private bool isChestOpened = false; //체스트가 열렸는지 여부
        private bool isClieckStep = false;
        private bool isMouseOver = false;

        private Image thisImage;

        private void Awake()
        {
            if (Instance == null) Instance = this;

            isChestOpened = false;

            thisImage = GetComponent<Image>();
            thisImage.enabled = false;

            followTxt.gameObject.SetActive(false);

            followTxt.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            followTxt.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            followTxt.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private void Update()
        {
            if (isMouseOver)
            {
                followTxt.rectTransform.position = Input.mousePosition;
            }
        }

        public void ChestIsOpened()
        {
            isChestOpened = true;
            thisImage.enabled = true;
        }

        public void IsClickStepTrue()
        {
            isClieckStep = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isClieckStep)
            {
                thisImage.enabled = false;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOver = true;
            followTxt.text = followDescription;
            followTxt.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOver = false;
            followTxt.gameObject.SetActive(false);
        }
    }
}
