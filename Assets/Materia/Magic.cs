using UnityEngine;
using System.Collections;

public class Magic : MonoBehaviour {
	
	void Update () {
        var v2mouse = Input.mousePosition;
        v2mouse.z = 10;

        var mouseposition = Camera.main.ScreenToWorldPoint(v2mouse);
        var maxDimCoords = Board.instance.maxXorY;

        if (mouseposition.x > 0 && mouseposition.y > 0
            && mouseposition.x < maxDimCoords && mouseposition.y < maxDimCoords)
        {
            Board.instance.FocuseNode(mouseposition);
        }

	}
}
