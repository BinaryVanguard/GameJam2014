using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour {

	private RelationMatrix RelationMatrix = null;
	private List<Node> Nodes = null;
	private List<Node> OldNodes = null;
	public int NodeCount = 50;
	public GameObject GameNode;
	public GameObject GameEdge;

	public Rect Extents = new Rect (0f, 0f, 0f, 0f);
	public float Aspect = 16f / 9f ; 
	public float Scale  = 100; // <-- Camera Size
	public float Adjust = 2.0f;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < NodeCount; ++i){
			Vector2 limit = new Vector2( (Random.value - 0.5f) * Scale * Aspect * Adjust, (Random.value - 0.5f) * Scale * Adjust);
			Instantiate(GameNode, limit, Quaternion.identity );
		}

		RelationMatrix = new RelationMatrix(GameObject.FindObjectsOfType<Node>());
		Nodes = new List<Node>(GameObject.FindObjectsOfType<Node>());
		OldNodes = new List<Node>(GameObject.FindObjectsOfType<Node>());
		Nodes[0].friction = 0;
		OldNodes[0].friction = 0;
		for(int i = 1; i < Nodes.Count; ++i){ 
			Nodes[i].friction = OldNodes[i].friction = 0.05f * NodeCount;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 1; i < Nodes.Count; ++i) {
			Node node = Nodes[i];
			for(int j = 0; j < OldNodes.Count;	 ++j) {
				node.velocity += node.RepulsiveForce(OldNodes[j]);	
				if(RelationMatrix[i,j])
					node.velocity += node.AttractiveForce(OldNodes[j]);
			}

			Vector3 position = node.gameObject.transform.position;

			// Update environment extents - minimum and maximum
			if (position.x < Extents.xMin) Extents.xMin = position.x;
			if (position.y < Extents.yMin) Extents.yMin = position.y;
			if (position.x > Extents.xMax) Extents.xMax = position.x;
			if (position.y > Extents.yMax) Extents.yMax = position.y;
		};

		//OldNodes.Clear();
		for(int i = 0; i < Nodes.Count; ++i)
			OldNodes[i] = Nodes[i];  // <-- can we move this to the end of the above loop?
	}

	public void AddRelation(Node one, Node two) {
		RelationMatrix.AddRelation(one, two);
	}
}

