using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour {
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

		//RelationMatrix = new RelationMatrix(GameObject.FindObjectsOfType<Node>());
		SetupRelations(GameObject.FindObjectsOfType<Node>());
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
//				if(node.Edges.FindIndex(e => e.FirstParent == OldNodes[j] || e.SecondParent == OldNodes[j]) >= 0)
//					node.velocity += node.AttractiveForce(OldNodes[j]);
//				}
			}
			foreach ( Edge e in node.Edges)
				node.velocity += node.AttractiveForce( e.FirstParent == node ? e.SecondParent : e.FirstParent);

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

	void SetupRelations(Node [] _nodes) {
		int nGroups = (int)(UnityEngine.Random.value * 10) + 1;
		int nGroupSize = 4;
		int maxGroupSize = 6;

//		Relations = new List<List<Edge>>();
		//for(int i = nodes.Length-1; i >= 0; --i){
			//			System.Linq.Enumerable.Repeat(false,i);
			//Relations.Add(new List<Edge>(new Edge[i]));

		//}
		//size = Nodes.Length;
		
		List<int> indicies = new List<int>();
		for(int i = 1; i < _nodes.Length - 1; ++i)
			indicies.Add(i);
		
		List<int> groupLeaders = new List<int>();
		for(int i = 0; i < nGroups; ++i){
			if(indicies.Count < 2)
				break;
			nGroupSize = (int)System.Math.Min(UnityEngine.Random.value * maxGroupSize + 1, indicies.Count);
			List<int> groupIndexs = new List<int>();
			for(int j = 0; j < nGroupSize; ++j) {
				int index = (int)(UnityEngine.Random.value * (indicies.Count - 1));
				groupIndexs.Add(indicies[index]);
				if(groupIndexs.Count == 1)
					groupLeaders.Add(indicies[index]);
				indicies.RemoveAt(index);
			}
			for(int j = 0; j < groupIndexs.Count; ++j)
			for(int k = 0; k < groupIndexs.Count; ++k){
				Edge.BuildEdge(_nodes[j],_nodes[k]);//true;
			}
		}
		
		//		for(int i = 0; i < n-1; ++i)
		//			Relations[0][i] = true;
		foreach (int i in groupLeaders)
			Edge.BuildEdge(_nodes[0], _nodes[i]);//true;
		
	}
}

