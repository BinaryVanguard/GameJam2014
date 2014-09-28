using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RelationMatrix {
	List<List<Edge>> Relations;
	int size;
	int nGroups = (int)(UnityEngine.Random.value * 10) + 1;
	int nGroupSize = 4;
	int maxGroupSize = 6;

	public RelationMatrix(Node [] Nodes) {
		Relations = new List<List<Edge>>();
		for(int i = Nodes.Length-1; i >= 0; --i){
//			System.Linq.Enumerable.Repeat(false,i);
			Relations.Add(new List<Edge>(new Edge[i]));
		}
		size = Nodes.Length;

		List<int> indicies = new List<int>();
		for(int i = 1; i < size - 1; ++i)
			indicies.Add(i);

		List<int> groupLeaders = new List<int>();
		for(int i = 0; i < nGroups; ++i){
			if(indicies.Count < 2)
				break;
			nGroupSize = (int)Math.Min(UnityEngine.Random.value * maxGroupSize + 1, indicies.Count);
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
					this[j, k] = Edge.BuildEdge(Nodes[j],Nodes[k]);//true;
				}
		}

//		for(int i = 0; i < n-1; ++i)
//			Relations[0][i] = true;
		foreach (int i in groupLeaders){
			this[0, i] = Edge.BuildEdge(Nodes[0], Nodes[i]);//true;
		}
	}

	public bool this [int i, int j] {
		get { 
			if(i > j) {
				return Relations[j][i-j-1] != null;
			}
			else if (i == j) {
				return true;
			}
			else {
				return Relations[i][j-i-1] != null;
			}
		}
		set { 
			if(i > j) {
				if(value == true) {
					Node [] nodes = GameObject.FindObjectsOfType<Node>();
					Relations[j][i-j-1] = Edge.BuildEdge(nodes[i], nodes[j]);
				}
				else {
					GameObject obj = Relations[j][i-j-1].gameObject;
					if(obj != null)
						GameObject.Destroy(obj);
				}
			}
			else if (i < j) {
				if(value == true) {
					Node [] nodes = GameObject.FindObjectsOfType<Node>();
					Relations[i][j-i-1] = Edge.BuildEdge(nodes[i], nodes[j]);
				}
				else {
					GameObject obj = Relations[i][j - i - 1].gameObject;
					if(obj != null)
						GameObject.Destroy(obj);
				}
			}
		}
	}

	public void AddRelation(Node one, Node two) {
		List<Node> nodes = new List<Node>(GameObject.FindObjectsOfType<Node>());
		int i = nodes.FindIndex(o => o.Equals(one));
		int j = nodes.FindIndex(o => o.Equals(two));
		if(j > i)
			Relations[i][j - i - 1] = Edge.BuildEdge(one, two);
		else
			Relations[j][i - j-1] = Edge.BuildEdge(one, two);
	}
}
