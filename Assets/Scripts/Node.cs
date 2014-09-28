using UnityEngine;
using System.Collections;
using System;

public class Node : MonoBehaviour {

	public double charge = 0.0;
	public float friction = 0.01f; 
	public Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += velocity * Time.deltaTime;
		velocity -= friction * velocity * Time.deltaTime;
	}

	//static const double e_0 = 1.0 / ( 4 * Mathf.PI * Mathf.Pow(10.0, -7) * Mathf.Pow(299792458, 2));
	//static const double K_e = Math.Pow(10.0, -7) * Math.Pow(299792458, 2);	//Coulomb's Constant
	const double K_e = 0.0000001 * 299792458 * 299792458;	//Coulomb's Constant
	public Vector3 RepulsiveForce (Node q2) {
		Vector3 position = gameObject.transform.position;
		Vector3 otherPos = q2.gameObject.transform.position;

		Vector3 toNode = position - otherPos;
		double r = toNode.sqrMagnitude;

		if(r != 0.0)
			return toNode.normalized * (float)(K_e * Math.Abs(charge * q2.charge)  / r);
		else
			return Vector3.zero;
	}

	const float K = .01f;	//Spring constant
	const float length = 3.0f;
	public Vector3 AttractiveForce (Node q2) {
		Vector3 position = gameObject.transform.position;
		Vector3 otherPos = q2.gameObject.transform.position;

		Vector3 toNode = otherPos - position;
		return K * (toNode - toNode.normalized * length) / 2f;
	}

	void OnTriggerEnter (Collider Other) {
		Node other = Other.gameObject.GetComponent<Node>();
		//RelationMatrix rmat = GameObject.FindObjectOfType<RelationMatrix>();
		//if(other != null && rmat != null) {
		//	RelationMatrix[i,j] = true;
		//}
		GameObject.FindObjectOfType<NodeManager>().AddRelation(this, other);
	}
}
