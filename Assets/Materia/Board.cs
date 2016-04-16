using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public float nodeSize = .1f;
    public int BoardSize = 100;
    public static Board instance;

    public float maxXorY { get { return BoardSize * nodeSize; } }

    Node focusedNode;
    public Node[,] Nodes;
    bool isInDragMode = false;
    Coords lastFocuseCoords;

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
        //Nodes[nodeCoord.x, nodeCoord.y] = node;
    }


    public void FocuseNode(Vector3 v3position)
    {
        if (v3position.x > 0 && v3position.y > 0
            && v3position.x < maxXorY && v3position.y < maxXorY)
        {
            var coords = toCoords(v3position);
            if (coords.IsEqual(lastFocuseCoords))
                return;

            lastFocuseCoords = coords;
            Debug.Log("underMouse " + coords.x + ";" + coords.y);

            if (isInDragMode)
                DragMode(coords);     
            else
                SelectMode(coords);
        }
    }

    private void SelectMode(Coords coords)
    {
        if (Nodes[coords.x, coords.y] != null)
        {
            if (focusedNode != null)
                focusedNode.Unfocus();
            if (Nodes[coords.x, coords.y].isFixed)
                return;
            
            focusedNode = Nodes[coords.x, coords.y];
            Nodes[coords.x, coords.y].Focus();

            //Debug.Log("focused " + coords.x + ';' + coords.y);
        }
    }

    private void DragMode(Coords coords)
    {
        if (Nodes[coords.x, coords.y] != null)
            Slice();
        else
            Grow(coords);
    }

    private void Grow(Coords growCoords)
    {
        var nodeToMove = focusedNode.coords;
        if (GetNeighbours(nodeToMove).Where(c => c.IsEqual(growCoords)).Count() == 0)
            return;

        Dictionary<Node, Coords> moved = new Dictionary<Node, Coords>();
        MoveNodes(growCoords, nodeToMove, ref moved);

    }

    private void MoveNodes(Coords growCoords, Coords nodeToMove, ref Dictionary<Node, Coords> moved, int RecLvl = 0, Coords beforeChain = null)
    {
        if (moved.ContainsKey(Nodes[nodeToMove.x, nodeToMove.y]) && (moved[Nodes[nodeToMove.x, nodeToMove.y]] == growCoords))
            return;

        moved[Nodes[nodeToMove.x, nodeToMove.y]] = growCoords;

        Debug.Log("place " + nodeToMove.x + ';' + nodeToMove.y + " to " + growCoords.x + ';' + growCoords.y);
        Nodes[nodeToMove.x, nodeToMove.y].placeOnCoords(growCoords);

        if (Nodes[growCoords.x, growCoords.y].memory != null
            && Math.Abs(Nodes[growCoords.x, growCoords.y].memory.coords.x - growCoords.x) > 1
            && Math.Abs(Nodes[growCoords.x, growCoords.y].memory.coords.y - growCoords.y) > 1 )
        {
            MoveNodes(Nodes[growCoords.x, growCoords.y].lastCoords, 
                Nodes[growCoords.x, growCoords.y].memory.coords, ref moved, RecLvl++, growCoords);
            return;
        }

        if ((Nodes[growCoords.x, growCoords.y].memory == null)
            && nodeIsConnected(growCoords, nodeToMove, beforeChain))
            return;


        var possibleNodesToMove = GetNeighbours(nodeToMove)
            .Where(c => !c.IsEqual(growCoords)
                && Nodes[c.x, c.y] != null
                && Nodes[c.x, c.y].isFixed == false);
        if (possibleNodesToMove.Count() == 0)
            return;


        //var shift = new Coords(growCoords.x - nodeToMove.x, growCoords.y - nodeToMove.y);

   
        //if (shift.x == 0)
         //   possibleNodesToMove = possibleNodesToMove.OrderBy(c => c.x);

        //if (RecLvl > 5)
        //    Unlock();

        var newtoMove = possibleNodesToMove.First();
        Nodes[growCoords.x, growCoords.y].memory = Nodes[newtoMove.x, newtoMove.y];

        MoveNodes(nodeToMove, newtoMove, ref moved, RecLvl++, growCoords);
    }

    private void Slice()
    {

        //throw new NotImplementedException();
    }

    bool nodeIsConnected(Coords node, Coords without, Coords without2)
    {
        var bh = GetNeighbours(node)
            .Where(c => !c.IsEqual(without) && !c.IsEqual(without2)  && Nodes[c.x, c.y] != null)
            .ToList();

        return (bh.Count() > 0);
    }

    Coords toCoords(Vector3 worldCoords)
    {
        return new Coords(
            worldCoords.x / nodeSize,
            worldCoords.y / nodeSize);
    }

    public void Lock()
    {
        if (focusedNode == null)
            return;

        isInDragMode = true;
    }

    public void Unlock() {
        isInDragMode = false;
        Debug.Log("unlock");
        detectFalls();

    }

    void detectFalls()
    {
        //detectFigure
        List<Coords> figureNodes = getFigure();
       // if (figureNodes.Where(c => Nodes[c.x,c.y].isFixed == true).Count() == 0)

        foreach (var c in figureNodes)
            Nodes[c.x,c.y].Focus();
    }

    private List<Coords> getFigure()
    {
        List<Coords> figureNodes = new List<Coords>();
        figureNodes.Add(focusedNode.coords);
        int i = 0;
        while (i < figureNodes.Count)
        {
            foreach (var node in GetNeighbours(figureNodes[i]).Where(c => Nodes[c.x, c.y] != null))
                if (figureNodes.Where(n => n.IsEqual(node)).Count() == 0)
                    figureNodes.Add(node);

            i++;
        }
        return figureNodes.OrderBy(c=>c.y).ToList();
    }

    bool IsInBounds(Coords coords)
    {
        return coords.x >= 0
               && coords.x < BoardSize
               && coords.y >= 0
               && coords.y < BoardSize;
    }

    public IEnumerable<Coords> GetNeighbours(Coords coords)
    {
        int[] d = { -1, 1 };
      /*  return d
            .SelectMany(x => d
                .Select(y => new Coords(x + coords.x, y + coords.y))
            )
            .Where(elem => IsInBounds(elem) && !elem.IsEqual(coords));
    
       */
        return d.Select(x => new Coords(x + coords.x, coords.y))
            .Union(d.Select(y => new Coords(coords.x, y+ coords.y)))
            .Where(elem => IsInBounds(elem));
    }
}

public class Coords {
    public int x;
    public int y;

    public Coords(float x,float y){
        this.x = Mathf.RoundToInt(x);
        this.y = Mathf.RoundToInt(y);
    }

    public bool IsEqual(Coords other)
    {
        if (other == null)
            return false;
        return (other.x == x && other.y == y);
    }
}