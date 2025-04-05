using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamPikerView : ScreenUI
{
    [SerializeField] 
    private string nickName;
    
    [SerializeField]
    private TextMeshProUGUI teamA;

    [SerializeField]
    private TextMeshProUGUI teamB;

    [SerializeField] 
    private TextMeshProUGUI waitTeam;
    
    [SerializeField]
    private Button joinButtonTeamA;

    [SerializeField]
    private Button joinButtonTeamB;

    [SerializeField]
    private Button joinButtonWaitTeam;
    
    [SerializeField]
    Button startButton;

    [SerializeField]
    Toggle readyToggle;

    public event Action<TeamGroup> OnTeamButtonClick;
    public event Action<bool> OnReadyToggleClick;
    public event Action OnStartButtonClick;
    private void Start()
    {
        teamA.richText = true;
        teamB.richText = true;
        waitTeam.richText = true;
       
        
        joinButtonTeamA.onClick.AddListener(OnTeamAButtonClick);
        joinButtonTeamB.onClick.AddListener(OnTeamBButtonClick);
        joinButtonWaitTeam.onClick.AddListener(OnWaitTeamButtonClick);
        
        readyToggle.onValueChanged.AddListener(OnReadyToggleInteract);
        startButton.onClick.AddListener(OnStartButtonInteract);
    }

    private void OnStartButtonInteract()
    {
        OnStartButtonClick?.Invoke();
    }

    private void OnTeamAButtonClick()
    {
        OnTeamButtonClick?.Invoke(TeamGroup.TeamA);
    }
    private void OnTeamBButtonClick()
    {
        OnTeamButtonClick?.Invoke(TeamGroup.TeamB);
    }
    private void OnWaitTeamButtonClick()
    {
        OnTeamButtonClick?.Invoke(TeamGroup.WaitTeam);
    }
    private void OnReadyToggleInteract(bool value)
    {
        OnReadyToggleClick?.Invoke(value);
    }
    public void TeamUpdate(string nickNamesString,TeamGroup team)
    {
        TMP_Text teamText = null;
        switch (team)
        {
            case TeamGroup.TeamA:
                teamText = teamA;
                break;
            case TeamGroup.TeamB:
                teamText = teamB;
                break;
            case TeamGroup.WaitTeam:
                teamText = waitTeam;
                break;
            default:
                break;
        }
        
        String[] nickNamesArray = nickNamesString.Split('/');

        string text = "";

        foreach (string playerNickName in nickNamesArray)
        {

            if (playerNickName == nickName)
            {

                text = text + "<color=green>" + playerNickName + "  </color>" + "\n";
            }
            else
            {
                text = text + "<color=black>" + playerNickName + "  </color>" + "\n";
            }
        }

        teamText.text = text;
    }
    public void TurnOnServerView()
    {
        startButton.gameObject.SetActive(true);
        readyToggle.gameObject.SetActive(false);
    }
    public void ReadyState(bool value)
    {
        startButton.interactable = value;
    }
}