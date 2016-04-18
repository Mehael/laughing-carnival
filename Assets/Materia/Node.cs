using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
    public Coords coords;
    SpriteRenderer renderer;
    public Node memory;
    public bool isChecked = false;
    public Coords lastCoords;
    public bool isFixed = false;

    public void Init(Coords coords)
    {
        placeOnCoords(coords);
        renderer = transform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            memory = null;
            lastCoords = null;
        }
    }

    public void selfDestroy()
    {
        if (coords!=null && Board.instance.Nodes[coords.x, coords.y] != null)
            Board.instance.Nodes[coords.x, coords.y] = null;
        Board.instance.allNodes.Remove(this);
        Destroy(gameObject);
    }

    public void placeOnCoords(Coords newCoords){
        if (Board.instance.Nodes[newCoords.x, newCoords.y] != null)
            if (Board.instance.Nodes[newCoords.x, newCoords.y].isFixed == false)
                Board.instance.Nodes[newCoords.x, newCoords.y].selfDestroy();
            else
                selfDestroy();

        Board.instance.Nodes[newCoords.x, newCoords.y] = this; 
        if (coords != null)
            Board.instance.Nodes[coords.x, coords.y] = null;
        lastCoords = coords;

        coords = newCoords;
        transform.position = new Vector3(
            coords.x * Board.instance.nodeSize,
            coords.y * Board.instance.nodeSize);

        if (gameObject.GetComponent<Fixed>() != null)
            isFixed = true;

    }

    public void Focus()
    {
        renderer.color = new Color(1f,0f,0f);
    }

    public void Unfocus()
    {
        renderer.color = new Color(0.55f, 0.67f, 0.65f);
    }
}
