using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Node : MonoBehaviour {
    Coords coords;
    public SpriteRenderer renderer;

    public void Init(Coords coords)
    {
        placeOnCoords(coords);
        renderer = transform.GetComponent<SpriteRenderer>();
    }

    public void placeOnCoords(Coords newCoords){
        coords = newCoords;
        transform.position = new Vector3(
            newCoords.x * Board.instance.nodeSize,
            newCoords.y * Board.instance.nodeSize);
    }

    public void Focus()
    {
        renderer.color = new Color(1f,0f,0f);
        //HOTween.To(renderer, 1.0f, new TweenParms().Prop("color", colorTo));
    }
}
