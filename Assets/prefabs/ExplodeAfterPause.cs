using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplodeAfterPause : MonoBehaviour {
    public float TimeToExplode = 2f;
    public List<Coords> nearNodes;
    public Transform NodePieces;

	IEnumerator Start () {
        yield return new WaitForSeconds(TimeToExplode);
        foreach (var node in nearNodes)
            if (Board.instance.Nodes[node.x, node.y] == null)
            {
                Destroy(gameObject);
                yield break;
            }
        foreach (var node in nearNodes)
        {
            if (Random.Range(0, 3) == 0)
                continue;

            var pos = new Vector3(node.x * Board.instance.nodeSize, node.y * Board.instance.nodeSize);
            for (int i = 0; i < Random.Range(1, 3); i++)
                Instantiate(NodePieces, pos, Quaternion.identity);
            Board.instance.Nodes[node.x, node.y].selfDestroy();

        }

        GetComponent<PointEffector2D>().enabled = true;
        FadeManager.Expl();

        //if (LevelManager.currentLevel == 7)
            if (Vector3.Distance(transform.position, new Vector3(10f, 6.5f, transform.position.z)) < 1.7f)
                LevelManager.LoadSpecialClip("Kill Clip");

        yield return new WaitForSeconds(0.1f);
        Board.instance.detectFalls();
        Destroy(gameObject);
	}


}
