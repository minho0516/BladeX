using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class OnTriggerTextPopup : MonoBehaviour
    {
        [SerializeField] private DialogueDataSO dialogueSO;
        [SerializeField] private TMP_Text popupText;
        [SerializeField] private float fadeTime = 0.5f;

        private bool isOpen = false;

        private void OnEnable()
        {
            if(popupText == null) popupText = FindAnyObjectByType<TMP_Text>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(DialogueManager.Instance.IsDialogueOpen) return;

            isOpen = !isOpen;
            if(isOpen)
            {
                OnPopup();
            }
            else
            {
                OffPopup();
            }

            DialogueManager.Instance.StartDialogue(dialogueSO);
        }

        private void OnPopup()
        {
            if (popupText.transform != null)
            {
                popupText.transform.DOScaleX(1, fadeTime)
                    .SetEase(Ease.OutCirc);
            }
        }

        private void OffPopup()
        {
            if (popupText.transform != null)
            {
                popupText.transform.DOScaleX(0, fadeTime)
                    .SetEase(Ease.OutCirc);
            }
        }
    }
}
