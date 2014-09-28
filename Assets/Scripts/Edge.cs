using UnityEngine;
using System.Collections;

public class Edge : MonoBehaviour {

	public Node FirstParent = null;
	public Node SecondParent = null;

	// Use this for initialization
	void Start () {
		//tag = "Edge";
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = (FirstParent.transform.position + SecondParent.transform.position) / 2.0f;
		transform.up = (FirstParent.transform.position - SecondParent.transform.position).normalized;
		transform.localScale = new Vector3(1.0f, (FirstParent.transform.position - SecondParent.transform.position).magnitude / 2.0f ,1.0f);
	}

	void Remove () {
		FirstParent.Edges.Remove(this);
		SecondParent.Edges.Remove(this);
		Destroy (this.gameObject);
	}

	public static Edge BuildEdge(Node one, Node two) {
		//GameObject go = GameObject.FindGameObjectWithTag("Edge");
		Edge [] edges = Resources.FindObjectsOfTypeAll<Edge>();
		Edge e = (Edge)Instantiate(edges[0]);
		e.FirstParent = one;
		e.SecondParent = two;	

		e.FirstParent.Edges.Add (e);
		e.SecondParent.Edges.Add(e);
		return e;
	}
}
