using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour, Shape {
	private bool isAlive = true;
	private LineRenderer line;
	private Vector2 center = new Vector2(0, 4f);

	private Vector2 g = new Vector2(0, -0.3f);
	private Vector2 speedVector = new Vector2 (0.035f, 0);
	private Vector2 collisionVector;
	private List<BoundingBox> boundingBoxes = new List<BoundingBox>();
	private string collisionTag;

	// Use this for initialization
	void Start () {
		generateBug ();
		BoundingBox boundingBox = new GameObject("BoundingBox-Bug").AddComponent<BoundingBox>();
		boundingBox.points = createBoundingBox ();
		boundingBoxes.Add (boundingBox);
		PhysicsManager.subscribe (this);
	}

	// Update is called once per frame
	void Update () {
		destroyIfOut (center);

		if (isAlive) {
			if (collisionVector != new Vector2 (0, 0) && collisionTag.Equals ("projectile")) {
				isAlive = false;
				speedVector = g * Time.deltaTime;
				collisionVector = new Vector2 (0, 0);
			}else {
				//speedVector = speedVector + (GameManager.windVector * Time.deltaTime);
				center = center + speedVector + GameManager.windVector;
				line.transform.position = center;
				boundingBoxes [0].move (speedVector + GameManager.windVector);
				return;
			}
		} else {
			if (collisionVector != new Vector2 (0, 0) && collisionTag.Equals ("water")) {
				destroy ();
			}else if(collisionVector != new Vector2 (0, 0) && collisionTag.Equals ("bowl")){
				if (Vector2.Dot (speedVector, collisionVector) > 0) {
					collisionVector = new Vector2 (-collisionVector.x, -collisionVector.y);
				}

				if (center.x < 5) {
					speedVector = Vector2.Reflect (speedVector, collisionVector) + new Vector2(-0.05f, 0);
				} else {
					speedVector = Vector2.Reflect (speedVector, collisionVector) + new Vector2(0.05f, 0);
				}

				collisionVector = new Vector2 (0, 0);
			}else {
				speedVector = speedVector + g * Time.deltaTime;
			}
		}

		center = center + speedVector;
		line.transform.position = center;
		boundingBoxes [0].move (speedVector);
	}

	public string getTag(){
		return "bug";
	}

	public List<BoundingBox> getBoundingBoxes(){
		return boundingBoxes;
	}

	public void setCollisionVector(Vector2 u, string tag){
		collisionVector = u;
		collisionTag = tag;
	}

	private List<Vector2> createBoundingBox(){
		//find xmax, xmin, ymax and ymin
		List<Vector2> points = new List<Vector2>();
		float xMin = Mathf.Infinity;
		float xMax = Mathf.NegativeInfinity;
		float yMin = Mathf.Infinity;
		float yMax = Mathf.NegativeInfinity;

		for (int i = 0; i < line.positionCount; i++) {
			Vector2 point = line.GetPosition (i);
			if (point.x < xMin) {
				xMin = point.x;
			} else if (point.x > xMax) {
				xMax = point.x;
			}

			if (point.y < yMin) {
				yMin = point.y;
			}else if(point.y > yMax){
				yMax = point.y;
			}
		}

		points.Add (new Vector2 (xMax + center.x, yMax + center.y));
		points.Add (new Vector2 (xMin + center.x, yMax + center.y));
		points.Add (new Vector2 (xMin + center.x, yMin + center.y));
		points.Add (new Vector2 (xMax + center.x, yMin + center.y));
		return points;
	}

	void createBug(){
		line = new GameObject("Bug").AddComponent<LineRenderer>();
		line.positionCount = 8;
		line.startWidth = 0.05f;
		line.useWorldSpace = false;
		line.SetPosition (0, new Vector2(0f, 0.5f));
		line.SetPosition (1, new Vector2(0.1f, 0.6f));
		line.SetPosition (2, new Vector2(0.2f, 0.7f));
		line.SetPosition (3, new Vector2(0.3f, 0.6f));
		line.SetPosition (4, new Vector2(0.4f, 0.5f));
		line.SetPosition (5, new Vector2(0.3f, 0.4f));
		line.SetPosition (6, new Vector2(0.2f, 0.3f));
		line.SetPosition (7, new Vector2(0.1f, 0.4f));
		line.loop = true;
		line.transform.position = center;
	}

	private void generateBug(){
		List<Vector2> points = generateBugPoints (center, 0.1f, 0.3f);
		line = new GameObject("Bug").AddComponent<LineRenderer>();
		line.positionCount = points.Count;
		line.startWidth = 0.05f;
		line.endWidth = 0.05f;
		line.loop = true;
		line.useWorldSpace = false; 
		for (int i = 0; i < points.Count; i++) {
			line.SetPosition (i, points[i]);
		}
		line.transform.position = center;
	}

	private List<Vector2> generateBugPoints(Vector2 center, float radius, float lenght){
		List<Vector2> result = new List<Vector2> ();
		for (float theta = Mathf.PI/2; theta > -Mathf.PI/2; theta -= 0.15f) {
			result.Add (new Vector2 (Mathf.Cos (theta) * radius +center.x, Mathf.Sin (theta) * radius +center.y));
		}

		result.Add (new Vector2 (result[result.Count-1].x -lenght, result[result.Count-1].y));
		for (float theta = 3*Mathf.PI/2; theta > Mathf.PI/2; theta -= 0.15f) {
			result.Add (new Vector2 (Mathf.Cos (theta) * radius +center.x -lenght, Mathf.Sin (theta) * radius +center.y));
		}
		result.Add (new Vector2 (result[result.Count-1].x + (lenght/3), result[result.Count-1].y));
		result.Add (new Vector2 (result[result.Count-1].x, result[result.Count-1].y + 0.01f));

		for (float theta = Mathf.PI; theta > -Mathf.PI/2; theta -= 0.15f) {
			result.Add (new Vector2 (Mathf.Cos (theta) * 0.01f +result[result.Count-1].x, Mathf.Sin (theta) * 0.01f +result[result.Count-1].y));
		}

		for (float theta = (2*Mathf.PI)/3; theta > -Mathf.PI; theta -= 0.15f) {
			result.Add (new Vector2 (Mathf.Cos (theta) * 0.01f +result[result.Count-1].x, Mathf.Sin (theta) * 0.01f +result[result.Count-1].y));
		}
		return result;
	}

	void destroy ()
	{
		PhysicsManager.unsubscribe (this);
		Destroy (line.gameObject);
		Destroy (boundingBoxes [0].gameObject);
		Destroy (this.gameObject);
	}

	void destroyIfOut(Vector2 center){
		if (center.x > 20 || center.y < -10f || center.x < -1) {
			destroy ();
		}
	}
}
