﻿using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    void Start()
    {
        //StartButton();
    }

    public void StartButton()
    {
        //LevelManager.RestartLevel();
        LevelManager.NewGame();
    }
}
