using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplodeAfterPause : MonoBehaviour {
    public float TimeToExplode = 2f;
    public List<Node> nearNodes;
    public Transform NodePieces;

	IEnumerator Start () {
        yield return new WaitForSeconds(TimeToExplode);

        foreach (var node in nearNodes)
        {
            if (Random.Range(0, 2) == 0)
                continue;

            for (int i = 0; i < Random.Range(1, 3); i++)
                Instantiate(NodePieces, node.transform.position, Quaternion.identity);
            node.selfDestroy();

        }

        GetComponent<PointEffector2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        Board.instance.detectFalls();
        Destroy(gameObject);
	}
}
