using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour {

	private static List<Shape> shapes = new List<Shape> ();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < shapes.Count - 1; i++) {
			for (int j = i + 1; j < shapes.Count; j++) {
				for (int k = 0; k < shapes [i].getBoundingBoxes ().Count; k++) {
					for (int l = 0; l < shapes [j].getBoundingBoxes ().Count; l++) {
						Vector2 speedVector = checkForCollision(shapes[i].getBoundingBoxes()[k], shapes[j].getBoundingBoxes()[l]);
						if (speedVector != new Vector2 (0, 0)) {
							shapes [i].setCollisionVector (speedVector, shapes[j].getTag());
							shapes [j].setCollisionVector (speedVector, shapes[i].getTag());
						}
					}
				}
			}
		}
	}

	public static void subscribe(Shape box){
		shapes.Add (box);
	}

	public static void unsubscribe(Shape box){
		shapes.Remove (box);
	}

	Vector2 checkForCollision(BoundingBox shape_1, BoundingBox shape_2){
		List<Vector2> axis = new List<Vector2> ();
		float minOverlap = Mathf.Infinity;
		Vector2 minAxis = new Vector2 (0, 0);

		axis.AddRange (shape_1.getAxis ());
		axis.AddRange (shape_2.getAxis ());
		for (int i = 0; i < axis.Count; i++) {
			Vector2 projection_1 = project (shape_1.points, axis[i]);
			Vector2 projection_2 = project (shape_2.points, axis[i]);
			if (projection_1.y < projection_2.x || projection_2.y < projection_1.x) {
				return new Vector2 (0, 0);
			} else {
				float overlap = getOverlap (projection_1, projection_2);

				if (contains(projection_1, projection_2) || contains(projection_2, projection_1)) {
					float mins = Mathf.Abs(projection_1.y - projection_2.y);
					float maxs = Mathf.Abs(projection_1.x - projection_2.x);
					if (mins < maxs) {
						overlap += mins;
					} else {
						overlap += maxs;
					}
				}

				if (overlap <= minOverlap) {
					minOverlap = overlap;
					minAxis = axis [i];
				}
			}
		}
		return minAxis;
	}

	private Vector2 project(List<Vector2> shape, Vector2 axis){
		float min = Mathf.Infinity;
		float max = Mathf.NegativeInfinity;
		for (int i = 0; i < shape.Count; i++) {
			//float p = Vector2.Dot (shape[i], axis);
			float p = Vector2.Dot (axis, shape[i]);
			if (p > max) {
				max = p;
			} else if (p < min) {
				min = p;
			}
		}
		return new Vector2 (min, max);
	}

	private bool contains(Vector2 p1, Vector2 p2){
		return p1.y < p2.y && p1.x > p2.x;
	}

	private float getOverlap(Vector2 u, Vector2 v){
		if (u.x <= v.x && u.y >= v.y) {
			return u.y - u.x;
		} else if (v.x < u.x && v.y > u.y) {
			return v.y - v.x;
		} else if (u.x <= v.x && u.y <= v.y) {
			return u.y - v.x;
		} else if (v.x < u.x && v.y < u.y) {
			return v.y - u.x;
		} else {
			Debug.Log ("Oups");
			return 0f;
		}
	}
}
