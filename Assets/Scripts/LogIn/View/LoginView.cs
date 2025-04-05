using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoginView : ScreenUI
{
    [SerializeField] [Tooltip("Button to connect to the server")]
    private Button connectButton;

    [SerializeField] [Tooltip("Text input field for entering a nickname")]
    private TMP_InputField  textNickName;

    
    public event Action OnConnectButtonClicked;
    private void Start()
    {
        connectButton.interactable = true;
        connectButton.onClick.AddListener(() =>
        {
            OnConnectButtonClicked?.Invoke();
        });
        textNickName.onValueChanged.AddListener(OnNickNameChanged);
        
    }

    private void OnNickNameChanged(string text)
    {
        connectButton.interactable = NickNameValidator(text);
    }

    private bool NickNameValidator(string text)
    { 
        //TODO:Implements the complete validation logic
        return true;
        if ((!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text)))
        {
            return true;EnableConnectButton();
        }
        else
        {
            return false;DisableConnectButton();
        }

    }

    public void EnableConnectButton()
    {
        connectButton.interactable = true;
    }
    public void DisableConnectButton()
    {
        connectButton.interactable = true;
    }
    
    public string GetNickName()
    {
        return textNickName.text;
    }
    
}