using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl: MonoBehaviour, Shape {

	private Vector2 bowlCenter = new Vector2(11, 5);
	private int bowlRadius = 4;
	private float bowlLength = 4.0f; 
	private LineRenderer bowl;
	private List<BoundingBox> boundingBoxes = new List<BoundingBox>();

	void Start () {
		//boundingBox = new GameObject("BoundingBox").AddComponent<BoundingBox>();
		generateBowl ();
		//boundingBox.points = createBoundingBox ();
		PhysicsManager.subscribe (this);
	}

	public List<BoundingBox> getBoundingBoxes(){
		return boundingBoxes;
	}

	public string getTag(){
		return "bowl";
	}

	public void setCollisionVector(Vector2 u, string tag){
		//Do nothing, the bowl does not move
	}

	private void generateBowl(){
		List<Vector2> points = generateBowlPoints (bowlCenter, bowlRadius, bowlLength);
		//Debug.Log (points.Count);
		createBoundingBoxes (points);
		bowl = new GameObject("Bowl").AddComponent<LineRenderer>();
		bowl.positionCount = points.Count;
		bowl.startWidth = 0.1f;
		bowl.endWidth = 0.1f;
		bowl.useWorldSpace = true; 
		for (int i = 0; i < points.Count; i++) {
			bowl.SetPosition (i, points[i]);
		}
	}

	private List<Vector2> generateBowlPoints(Vector2 center, int radius, float lenght){
		List<Vector2> result = new List<Vector2> ();
		result.Add (new Vector2 (Mathf.Cos (Mathf.PI/4) * radius + center.x + 0.5f, Mathf.Sin (Mathf.PI/4) * radius +center.y - 0.05f));
		for (float theta = Mathf.PI/4; theta > -Mathf.PI/2; theta -= 0.25f) {
			result.Add (new Vector2 (Mathf.Cos (theta) * radius +center.x, Mathf.Sin (theta) * radius +center.y));
		}
			
		result.Add (new Vector2 (result[result.Count-1].x -lenght, result[result.Count-1].y));
		for (float theta = 3*Mathf.PI/2; theta > 3*Mathf.PI/4; theta -= 0.25f) {
			result.Add (new Vector2 (Mathf.Cos (theta) * radius +center.x -lenght, Mathf.Sin (theta) * radius +center.y));
		}
		result.Add (new Vector2 (result[result.Count-1].x - 0.5f, result[result.Count-1].y - 0.05f));
		return result;
	}

	private void createBoundingBoxes(List<Vector2> points){
		createBoundingBox (points [0], points [1], new Vector2(0f, -0.1f));
		for (int i = 1; i < 7; i++) {
			createBoundingBox (points [i], points [i + 1], new Vector2(-0.1f, 0f));
		}
		for (int i = points.Count - 7; i < points.Count - 1; i++) {
			createBoundingBox (points [i], points [i + 1], new Vector2(0.1f, 0f));
		}
		createBoundingBox (points [points.Count - 2], points [points.Count - 1], new Vector2(0f, -0.1f));
	}

	private void createBoundingBox(Vector2 i, Vector2 j, Vector2 interval){
		List<Vector2> bb = new List<Vector2> ();
		bb.Add (i);
		bb.Add (j);
		bb.Add (new Vector2(j.x + interval.x, j.y + interval.y));
		bb.Add (new Vector2(i.x + interval.x, i.y + interval.y));
		BoundingBox boundingBox = new GameObject("BoundingBox-"+i.x).AddComponent<BoundingBox>();
		boundingBox.points = bb;
		boundingBoxes.Add (boundingBox);
	}
}
