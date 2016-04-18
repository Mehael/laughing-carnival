using UnityEngine;
using System.Collections;

public class nextLevel : MonoBehaviour {
    public bool isLastLevel = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            if (isLastLevel)
                LevelManager.LoadSpecialClip("Save Clip");
            else
                LevelManager.NextLevel();

    }
}
