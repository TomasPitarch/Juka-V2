using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    //TODO:Refactor this implementation try to avoid mono behaviour inheritance and implement factory pattern
    [SerializeField]
    private string initialScreenId;
    
    [SerializeField]
    private List<ScreenUI> screens;
    
    private Dictionary<string,ScreenUI> _screenDictionary;
    
    private ScreenUI _currentScreen;
   
    void Start()
    {
        _screenDictionary = new Dictionary<string, ScreenUI>();
        foreach (ScreenUI screen in screens)
        {
            _screenDictionary.Add(screen.ScreenId,screen);
            screen.Hide();
        }
        OpenScreen(initialScreenId);
    }
     public void OpenScreen(string screenId)
    {
        _currentScreen?.Hide();
        _currentScreen = _screenDictionary[screenId];
        _currentScreen.Show();
    }
}
