using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public float nodeSize = .1f;
    public int BoardSize = 100;
    public static Board instance;
    public Transform ExplosionPrefab;

    public float maxXorY { get { return BoardSize * nodeSize; } }

    public Node focusedNode;
    public Node[,] Nodes;
    public Transform NodePieces;
    public List<Node> allNodes = new List<Node>();
    public bool isInDragMode = false;
    Coords lastFocuseCoords;
    Dictionary<Coords, IEnumerable<Coords>> neighCash = new Dictionary<Coords, IEnumerable<Coords>>();

	void Awake () {
        instance = this;
        initMatrix();
        detectFalls();
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
        Debug.Log(child.name + nodeCoord.x + ' ' + nodeCoord.y);
        node.Init(nodeCoord);
        allNodes.Add(node);
        //Nodes[nodeCoord.x, nodeCoord.y] = node;
    }

    public void FocuseNode(Vector3 v3position)
    {
        var coords = toCoords(v3position);
        if (coords.IsEqual(lastFocuseCoords) && isInDragMode == false)
            return;

        if (isInDragMode == true && focusedNode != null && coords.IsEqual(focusedNode.coords))
            return;

        lastFocuseCoords = coords;
        Debug.Log("underMouse " + coords.x + ";" + coords.y);

        if (isInDragMode)
            DragMode(coords);
        else
            SelectMode(coords);

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
        else {
            if (focusedNode != null)
            {
                focusedNode.Unfocus();
                focusedNode = null;
            }
        }
    }

    private void DragMode(Coords coords)
    {
        if (Nodes[coords.x, coords.y] == null)
            Grow(coords);
    }

    private void Grow(Coords growCoords)
    {
        var nodeToMove = focusedNode.coords;
        var coordShift = new Coords(nodeToMove.x, nodeToMove.y);

        if (growCoords.x > focusedNode.coords.x)
            coordShift.x++;
        else if (growCoords.y > focusedNode.coords.y)
            coordShift.y++;
        else if (growCoords.x < focusedNode.coords.x)
            coordShift.x--;
        else if (growCoords.y < focusedNode.coords.y)
            coordShift.y--;

        if (coordShift.IsEqual(focusedNode.coords))
            return;
        if (Nodes[coordShift.x, coordShift.y] != null)
            return;

        List<History> moved = new List<History>();
        MoveNodes(coordShift, nodeToMove, ref moved);

    }

    private void MoveNodes(Coords growCoords, Coords nodeToMove, ref List<History> moved, int RecLvl = 0, Coords beforeChain = null)
    {
        RecLvl++;
        if (RecLvl > 25)
            return;
        
        moved.Add(new History(Nodes[nodeToMove.x, nodeToMove.y], growCoords));

        Debug.Log("place " + nodeToMove.x + ';' + nodeToMove.y + " to " + growCoords.x + ';' + growCoords.y);
        Nodes[nodeToMove.x, nodeToMove.y].placeOnCoords(growCoords);

        if (Nodes[growCoords.x, growCoords.y].memory != null)
        {
            var xD = Math.Abs(Nodes[growCoords.x, growCoords.y].memory.coords.x - growCoords.x); 
            var yD = Math.Abs(Nodes[growCoords.x, growCoords.y].memory.coords.y - growCoords.y);

            if ((xD > 1 || yD > 1) && (xD < 3 && yD < 3))
            {
                MoveNodes(Nodes[growCoords.x, growCoords.y].lastCoords,
                    Nodes[growCoords.x, growCoords.y].memory.coords, ref moved, RecLvl, growCoords);
                return;
            }
        }

        if ((Nodes[growCoords.x, growCoords.y].memory == null)
            && nodeIsConnected(growCoords, nodeToMove, beforeChain))
            return;


        var possibleNodesToMove = GetNeighbours(nodeToMove)
            .Where(c => !c.IsEqual(growCoords)
                && Nodes[c.x, c.y] != null
                && Nodes[c.x, c.y].isFixed == false);

        Coords newtoMove = null;
        foreach (var poss in possibleNodesToMove){
            if (moved.Contains(new History(Nodes[nodeToMove.x, nodeToMove.y], growCoords)))
                continue;

            newtoMove = poss;
            break;
        }

        if (newtoMove == null)
            return;

        Nodes[growCoords.x, growCoords.y].memory = Nodes[newtoMove.x, newtoMove.y];
        MoveNodes(nodeToMove, newtoMove, ref moved, RecLvl, growCoords);
    }

    public void Slice()
    {

        if (focusedNode == null)
            return;

        var pos = new Vector3(focusedNode.coords.x * Board.instance.nodeSize, focusedNode.coords.y * Board.instance.nodeSize);
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
            Instantiate(NodePieces, pos, Quaternion.identity);
        Board.instance.Nodes[focusedNode.coords.x, focusedNode.coords.y].selfDestroy();
        detectFalls();
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
        if (isInDragMode)
        {
            detectFalls();
        }

        isInDragMode = false;
        if (focusedNode != null)
            focusedNode.Unfocus();
        focusedNode = null;
        Debug.Log("unlock");
        

    }

    public void detectFalls()
    {
        foreach (var n in allNodes)
            if (n != null)
                n.isChecked = false;

        foreach (var n in allNodes)
        {
            if (n != null && n.isChecked)
                continue;

            List<Coords> figureNodes = getFigure(n);

            List<Coords> surfaceNodes = new List<Coords>();
            foreach (var figN in figureNodes)
                foreach (var posSurfN in GetNeighbours(figN).Where(f => Nodes[f.x, f.y] == null))
                    if (surfaceNodes.Where(v => v.IsEqual(posSurfN)).Count() == 0)
                        surfaceNodes.Add(posSurfN);

            foreach (var surfN in surfaceNodes)
            {
                var neighNulls = GetNeighbours(surfN);

                if (neighNulls.Where(p => Nodes[p.x, p.y] != null).Count() == 4)
                    Explosion(surfN, 1);

                if (neighNulls.Where(p => Nodes[p.x, p.y] != null).Count() == 3)
                {
                    var secondFrend = neighNulls.Where(p => Nodes[p.x, p.y] == null).First();
                    if (GetNeighbours(secondFrend).Where(p => Nodes[p.x, p.y] != null).Count() == 3)
                        Explosion(surfN, .25f);
                }
            }

            if (figureNodes.Where(c => Nodes[c.x, c.y]!= null && Nodes[c.x, c.y].isFixed == true).Count() > 0)
                continue;

            StartCoroutine(Falling(figureNodes));
        }
    }

    void Explosion(Coords coords, float power)
    {
        var Explode = 
            ((Transform)Instantiate(ExplosionPrefab, new Vector3(coords.x * nodeSize, coords.y * nodeSize, transform.position.z), Quaternion.identity))
            .GetComponent<ExplodeAfterPause>();

        Explode.nearNodes = GetNeighbours(coords)
            .Where(p => Nodes[p.x, p.y] != null && Nodes[p.x, p.y].isFixed == false)
            .ToList();
    }

    IEnumerator Falling(List<Coords> figureNodes)
    {
        bool dropIsPossible = true;
        foreach (var c in figureNodes)
        {
            if (c.y - 1 < 0)
            {
                dropIsPossible = false;
                break;
            }

            if (Nodes[c.x, c.y - 1] != null
                && (figureNodes.Where(t => t.IsEqual(new Coords(c.x, c.y - 1))).Count() == 0))
            {
                dropIsPossible = false;
                break;
            }
        }

        if (!dropIsPossible)
            yield break;

        foreach (var c in figureNodes)
        {
            if (Nodes[c.x, c.y] != null)
            {
                Nodes[c.x, c.y].placeOnCoords(new Coords(c.x, c.y - 1));
                c.y -= 1;
            }
        }
        yield return new WaitForSeconds(0.05f);
        yield return StartCoroutine(Falling(figureNodes));
    }

    private List<Coords> getFigure(Node startNode)
    {
        List<Coords> figureNodes = new List<Coords>();
        figureNodes.Add(startNode.coords);
        startNode.isChecked = true;
        int i = 0;
        while (i < figureNodes.Count)
        {
            foreach (var node in GetNeighbours(figureNodes[i]).Where(c => Nodes[c.x, c.y] != null))
                if (figureNodes.Where(n => n.IsEqual(node)).Count() == 0)
                {
                    figureNodes.Add(node);
                    Nodes[node.x,node.y].isChecked = true;
                }

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
        if (neighCash.ContainsKey(coords))
            return neighCash[coords];

        int[] d = { -1, 1 };
        /*return d
             .SelectMany(x => d
                 .Select(y => new Coords(x + coords.x, y + coords.y))
             )
             .Where(elem => IsInBounds(elem) && !elem.IsEqual(coords));*/
        neighCash[coords] = d.Select(x => new Coords(x + coords.x, coords.y))
             .Union(d.Select(y => new Coords(coords.x, y+ coords.y)))
             .Where(elem => IsInBounds(elem));

        return neighCash[coords];
    }
}

public struct History
{
    public Coords coords;
    public Node node;

    public History(Node node, Coords coords)
    {
        this.coords = coords;
        this.node = node;
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