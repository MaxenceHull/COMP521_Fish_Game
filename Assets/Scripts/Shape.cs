using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Shape {
	List<BoundingBox> getBoundingBoxes ();
	void setCollisionVector(Vector2 collisionVector, string tag);
	string getTag ();
}
