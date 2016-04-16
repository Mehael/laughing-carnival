using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
    public Coords coords;
    SpriteRenderer renderer;
    public Node memory;
    public Coords lastCoords;
    public bool isFixed = false;

    public void Init(Coords coords)
    {
        placeOnCoords(coords);
        renderer = transform.GetComponent<SpriteRenderer>();
    }

    public void placeOnCoords(Coords newCoords){
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
        renderer.color = new Color(1f, 1f, 1f);
    }
}
