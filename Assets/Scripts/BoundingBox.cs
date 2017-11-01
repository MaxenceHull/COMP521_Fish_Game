using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour {

	public List<Vector2> points = new List<Vector2>();
	public static bool draw = false;

	private LineRenderer line;

	// Use this for initialization
	void Start () {
		if (draw) {
			//Create line bounding box
			line = new GameObject("BoundingBox-line").AddComponent<LineRenderer>();
			line.positionCount = points.Count;
			line.startWidth = 0.05f;
			line.loop = true;
			line.useWorldSpace = true;
			drawBoundingBox ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (draw) {
			drawBoundingBox ();
		}
	}

	public void move(Vector2 speedVector){
		for (int i = 0; i < points.Count; i++) {
			points [i] += speedVector;
		}
	}

	public List<Vector2> getAxis(){
		List<Vector2> axis = new List<Vector2> ();
		for (int i = 0; i < points.Count; i++) {
			Vector2 edge = points [i] - points [i == points.Count-1 ? 0 : i + 1];
			axis.Add (getPerpandicular(edge).normalized);
		}
		return axis;
	}

	private Vector2 getPerpandicular(Vector2 vector){
		return new Vector2 (vector.y, -vector.x);
	}

	private void drawBoundingBox(){
		for (int i = 0; i < points.Count; i++) {
			line.SetPosition (i, new Vector2(points[i].x, points[i].y));
		}
	}

	void OnDestroy() {
		Destroy (line);
		//Destroy (this.gameObject);
	}
}
