using UnityEngine;
using System.Collections;

public class Magic : MonoBehaviour {

    void Update()
    {
        Focus();
        if (Input.GetMouseButtonDown(0))
            Board.instance.Lock();
        if (Input.GetMouseButtonUp(0))
            Board.instance.Unlock();
    }

    private static void Focus()
    {
        var v2mouse = Input.mousePosition;
        v2mouse.z = 10;

        var mouseposition = Camera.main.ScreenToWorldPoint(v2mouse);
        Board.instance.FocuseNode(mouseposition);
    }


}
