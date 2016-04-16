using UnityEngine;
using System.Collections;

public class Magic : MonoBehaviour {
    public int StartRadius = 15;
    int radius = 0;

    void Start(){
        UpdateRadius(StartRadius);
    }

    public void UpdateRadius(int newRadius){
        radius = newRadius;
        var scale = 2 * Board.instance.nodeSize * newRadius;

        var daddy = transform.parent;
        transform.SetParent(null);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.SetParent(daddy);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            Board.instance.Unlock();
        if (Board.instance.isInDragMode && !Input.GetMouseButton(0))
            Board.instance.Unlock();
        Focus();
        if (Input.GetMouseButtonDown(0))
            Board.instance.Lock();

        if (Input.GetMouseButtonDown(1))
            Board.instance.Slice();
 
    }

    private static void Focus()
    {
        Board.instance.FocuseNode(mousePosition());
    }

    public static Vector3 mousePosition(){
        var v2mouse = Input.mousePosition;
        v2mouse.z = 10;

        return  Camera.main.ScreenToWorldPoint(v2mouse);
    }
}
