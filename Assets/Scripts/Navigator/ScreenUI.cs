using UnityEngine;
using System;

    public abstract class ScreenUI:MonoBehaviour
    {
        [SerializeField]
        private string screenId;

        public event Action OnShow;
        public event Action OnHide;
        public string ScreenId => screenId;
        public void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke();
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }
      
    }


