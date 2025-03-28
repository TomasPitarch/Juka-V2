using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectView:ScreenUI
{
    [SerializeField]
    private Button connectButton;
    
    [SerializeField]
    private Image loadingImage;

    [SerializeField] private float loadingRotationSpeed; 
    
    Coroutine _rotateLoadingImageCoroutine;
    
    public event Action OnConnectButtonClicked;

    private void Start()
    {
        connectButton.onClick.AddListener(()=>OnConnectButtonClicked?.Invoke());
    }

    public void LoadingStateUI()
    {
        connectButton.gameObject.SetActive(false);
        loadingImage.gameObject.SetActive(true);
        _rotateLoadingImageCoroutine = StartCoroutine(RotateLoadingImageCoroutine());
    }
    public void AvailableStateUI()
    {
        if (_rotateLoadingImageCoroutine is not null)
        {
            StopCoroutine(_rotateLoadingImageCoroutine); 
        }
        
        connectButton.gameObject.SetActive(true);
        loadingImage.gameObject.SetActive(false);
    }
    private IEnumerator RotateLoadingImageCoroutine()
    {
        while (true)
        {
            loadingImage.transform.Rotate(0,0,loadingRotationSpeed*Time.deltaTime);
            yield return null;
        }
    }
}
