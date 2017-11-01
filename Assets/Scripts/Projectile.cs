using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, Shape {

	public Vector2 center;
	public float radius;
	public float angle;
	public float speed;

	public BoundingBox boundingBox;

	private LineRenderer projectile;
	private Vector2 g = new Vector2(0, -0.1f);
	private Vector2 speedVector, collisionVector;
	private string collisionTag;

	// Use this for initialization
	void Start () {
		createProjectile (radius);
		speedVector = new Vector2 ( speed * Mathf.Cos(angle), speed * Mathf.Sin(angle));
		collisionVector = new Vector2 (0, 0);
		boundingBox = new GameObject("BoundingBox-Projectile").AddComponent<BoundingBox>();
		boundingBox.points = createBoundingBox ();
		PhysicsManager.subscribe (this);
	}

	// Update is called once per frame
	void Update () {
		destroyIfOut (center);

		if (collisionVector != new Vector2 (0, 0)) {
			if (collisionTag.Equals ("water") || collisionTag.Equals ("bug")) {
				destroy ();
			} else if (collisionTag.Equals ("projectile")) {
				collisionVector = new Vector2 (0, 0);
				computeNewSpeedVector ();
			} else {
				if (Vector2.Dot (speedVector, collisionVector) > 0) {
					collisionVector = new Vector2 (-collisionVector.x, -collisionVector.y);
				}
				speedVector = Vector2.Reflect (speedVector, collisionVector) * 1.1f;
				collisionVector = new Vector2 (0, 0);
			}

		} else {
			computeNewSpeedVector ();
		}

		center = center + speedVector;
		projectile.transform.position = center;
		boundingBox.move (speedVector);
	}

	private void computeNewSpeedVector(){
		//If above the bowl
		if (center.y > 7.6f) {
			speedVector = speedVector + ( (g + GameManager.windVector) * Time.deltaTime * 1.3f) ;
		} else {
			speedVector = speedVector + (g * Time.deltaTime);
		}

	}
		

	public List<BoundingBox> getBoundingBoxes(){
		List<BoundingBox> list = new List<BoundingBox> ();
		list.Add (boundingBox);
		return list;
	}

	public string getTag(){
		return "projectile";
	}
					

	public void setCollisionVector(Vector2 v, string tag){
		collisionVector = v;
		collisionTag = tag;
	}

	private void createProjectile(float radius){
		List<Vector2> points = new List<Vector2> ();
		for (float theta = 0.0f; theta < 2*Mathf.PI; theta += 0.1f) {
			points.Add (new Vector2 (Mathf.Cos (theta) * radius , Mathf.Sin (theta) * radius));
		}
		projectile = new GameObject("Projectile").AddComponent<LineRenderer>();
		projectile.positionCount = points.Count;
		projectile.startWidth = 0.05f;
		projectile.useWorldSpace = false; 
		for (int i = 0; i < points.Count; i++) {
			projectile.SetPosition (i, new Vector2(points[i].x, points[i].y));
		}
		projectile.transform.position = center;
	}

	private List<Vector2> createBoundingBox(){
		//find xmax, xmin, ymax and ymin
		List<Vector2> points = new List<Vector2>();
		float xMin = Mathf.Infinity;
		float xMax = Mathf.NegativeInfinity;
		float yMin = Mathf.Infinity;
		float yMax = Mathf.NegativeInfinity;

		for (int i = 0; i < projectile.positionCount; i++) {
			Vector2 point = projectile.GetPosition (i);
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

	void destroy(){
		PhysicsManager.unsubscribe (this);
		Destroy (boundingBox.gameObject);
		Destroy (projectile.gameObject);
		Destroy (this.gameObject);
	}

	void destroyIfOut(Vector2 center){
		if (center.x < -0.5f || center.x > 17 || center.y < -3f) {
			destroy ();
		}
	}

}
