using UnityEngine;
using System.Collections;
using System;

public class Board : MonoBehaviour {
    public float nodeSize = .1f;
    public int BoardSize = 100;
    public static Board instance;

    public float maxXorY { get { return BoardSize * nodeSize; } }

    Node[,] Nodes;

	void Start () {
        instance = this;
        initMatrix();	
	}

    void initMatrix()
    {
        //all nodes are in +x,+y space
        Nodes = new Node[BoardSize, BoardSize];
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).transform;
            if (child.childCount > 0)
                for (var j = 0; j < child.childCount; j++)
                    initNode(child.GetChild(j).transform);
            else
                initNode(child);
        }
    }

    private void initNode(Transform child)
    {
        var nodeCoord = toCoords(child.position);

        var node = child.gameObject.AddComponent<Node>();
        node.Init(nodeCoord);
        Nodes[nodeCoord.x, nodeCoord.y] = node;
    }


    public void FocuseNode(Vector3 v3position)
    {
        var coords = toCoords(v3position);
        Debug.Log(coords.x +  ";" + coords.y);
        
        if (Nodes[coords.x, coords.y] != null)
            Nodes[coords.x, coords.y].Focus();
    }

    Coords toCoords(Vector3 worldCoords)
    {
        return new Coords(
            worldCoords.x / nodeSize,
            worldCoords.y / nodeSize);
    }
}

public class Coords {
    public int x;
    public int y;

    public Coords(float x,float y){
        this.x = Mathf.RoundToInt(x);
        this.y = Mathf.RoundToInt(y);
    }
}