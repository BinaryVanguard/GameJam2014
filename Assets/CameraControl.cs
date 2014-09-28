using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public enum Mode { global, focusEdge }
	public Mode mode = Mode.focusEdge;
	private Mode lastMode = Mode.focusEdge;

	public NodeManager gameManager = null;
	public Camera camera = null;
	public Vector3 center = new Vector3 (0f, 0f);

	public Edge focusEdge = null;

	public float restSize = 100f;
	public Vector3 restPosition = new Vector3(0f, 0f, -10f);
	public Quaternion restRotation = new Quaternion ();
	public float damper = 0.0f;
	public float focusScale = 1f;

	const float MathPiDiv2 = Mathf.PI / 2f;

	private 
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


		switch (mode) {
		case Mode.global: UpdateGlobal(); break;
		case Mode.focusEdge: UpdateEdge(); break;
		}

		if (Input.GetButtonDown("Fire1")) {


			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 10f;
			
			Vector2 v = camera.ScreenToWorldPoint(mousePosition);
			
			Collider2D[] col = Physics2D.OverlapPointAll(v);
			
			if(col.Length > 0){
				//foreach(Collider2D c in col)
				//{
				Collider2D c = col[0];  // select only the top hit
					Debug.Log("Collided with: " + c.collider2D.gameObject.name);
					Edge e = c.collider2D.gameObject.GetComponent(typeof(Edge)) as Edge;
					if (e != null) focusEdge = e;
				//}
			}


			//RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			//Debug.Log("pressed");
			//if(hit.collider != null)
			//{
			//	Edge e = hit.collider.gameObject.GetComponent(typeof(Edge)) as Edge;
			//	if (e != null) focusEdge = e;
			//	Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
			//}
		}
	}

	void UpdateGlobal () {
		if (mode != lastMode) { 
			camera.orthographicSize = restSize;
			camera.transform.position = restPosition;
			camera.transform.rotation = restRotation;
			lastMode = mode;
		}
	}

	void UpdateEdge () { 
		if (mode != lastMode) {
			restSize = camera.orthographicSize;
			restPosition = camera.transform.position;
			restRotation = camera.transform.rotation;
			lastMode = mode;
		}

		if (focusEdge != null) {
			// rotation and center and size 
			// camera.transform.rotation = focusEdge.transform.rotation;
			Quaternion camRotation = camera.transform.rotation;
			// camPosition.eulerAngles = Vector3.Lerp(camPosition.eulerAngles, new Vector3(camPosition.eulerAngles.x, camPosition.eulerAngles.y, -focusEdge.transform.rotation.eulerAngles.z + MathPiDiv2), Time.deltaTime * damper);
			camRotation.eulerAngles = new Vector3(camRotation.eulerAngles.x, camRotation.eulerAngles.y, focusEdge.transform.rotation.eulerAngles.z -90f );
			camera.transform.rotation = camRotation;

			Vector3 position = focusEdge.transform.position;
			position.z = camera.transform.position.z;
			camera.transform.position = position;

			camera.orthographicSize = focusEdge.transform.localScale.y * focusScale;
		}
	}

	void SwitchToEdgeMode() { 

	}
}
