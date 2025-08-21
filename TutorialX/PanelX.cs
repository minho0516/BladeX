using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class PanelX : MonoBehaviour
    {
        private void OnEnable()
        {
            CloseTab();
        }
        public void OpenTab()
        {
            transform.DOScale(new Vector3(1, 1, 1), 1f);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void CloseTab()
        {
            transform.DOScale(new Vector3(0, 0, 0), 1f);
            Cursor.visible = false;
        }
    }
}
