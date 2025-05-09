﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public int MaxPlayersTeamA;

    public int MaxPlayersTeamB;


    public int TotalPlayersToStartGame()
    {
        return MaxPlayersTeamA + MaxPlayersTeamB -1;
    }
}
