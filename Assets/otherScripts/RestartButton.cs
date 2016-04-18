using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {
    public void Restart()
    {
        LevelManager.RestartLevel();
    }
}
