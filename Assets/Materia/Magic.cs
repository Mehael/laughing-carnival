using UnityEngine;
using System.Collections;

public class Magic : MonoBehaviour {
    public int StartRadius = 15;
    int radius = 0;
    float WorldRadius = 0f;
    float maxXorY = 0f;

    void Start(){
        maxXorY = Board.instance.nodeSize * Board.instance.BoardSize;
        UpdateRadius(StartRadius);
    }

    public void UpdateRadius(int newRadius){
        radius = newRadius;
        WorldRadius = Board.instance.nodeSize * newRadius;
        var scale = 2 * WorldRadius;

        var daddy = transform.parent;
        transform.SetParent(null);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.SetParent(daddy);
    }

    void Update()
    {
        Focus();

        if (Board.instance.isInDragMode)
            if (!Input.GetMouseButton(0))
                Board.instance.Unlock();
        
        if (Input.GetMouseButtonDown(0))
            Board.instance.Lock();

        if (Input.GetMouseButtonDown(1))
            Board.instance.Slice();

    }

    void Focus()
    {
        var m = mousePosition();
        m.z = transform.position.z;
        if (Vector3.Distance(m, transform.position) > WorldRadius)
        {
            if (!Board.instance.isInDragMode)
                Board.instance.Unlock();
            return;
        }

        if (m.x > 0 && m.y > 0 && m.x < maxXorY && m.y < maxXorY)
            Board.instance.FocuseNode(m);
        else
            Board.instance.Unlock();
            
    }

    public static Vector3 mousePosition(float z = 10){
        var v2mouse = Input.mousePosition;
        v2mouse.z = z;

        return  Camera.main.ScreenToWorldPoint(v2mouse);
    }
}
