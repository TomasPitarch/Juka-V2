using System;
using UnityEngine;
using UnityEngine.UI;

public class ConnectView:ScreenUI
{
    [SerializeField]
    private Button connectButton;
    
    [SerializeField]
    private Image loadingImage;
    
    
    public event Action OnConnectButtonClicked;

    private void Start()
    {
        connectButton.onClick.AddListener(()=>OnConnectButtonClicked?.Invoke());
    }

    public void LoadingStateUI()
    {
        connectButton.gameObject.SetActive(false);
        //TODO:Add loading animation
        loadingImage.gameObject.SetActive(true);
    }
    public void AvailableStateUI()
    {
        connectButton.gameObject.SetActive(true);
        loadingImage.gameObject.SetActive(false);
    }
}
