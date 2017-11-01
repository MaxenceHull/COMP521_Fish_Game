using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour, Shape {
	public LineRenderer line;
	private List<BoundingBox> boundingBoxes = new List<BoundingBox> ();

	// Use this for initialization
	void Start () {
		PhysicsManager.subscribe(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<BoundingBox> getBoundingBoxes(){
		return boundingBoxes;
	}

	public string getTag(){
		return "water";
	}
	public void setCollisionVector(Vector2 u, string tag){
		//Do nothing: water does not move
	}

	public void generateWater(int nb_points, int wavelength, float amplitude, int nb_octave){
		line = new GameObject("Water").AddComponent<LineRenderer>();
		List<float> points = makePerlinNoise(nb_points, wavelength, amplitude);
		for (int i = 2; i <= (2 ^ (nb_octave-1)); i *= 2) {
			List<float> new_points= makePerlinNoise(nb_points, (int)wavelength/i, amplitude/i);
			for (int j = 0; j < nb_points; j++) {
				points [j] = points [j] + new_points [j];
			}
		}

		line.positionCount = points.Count;
		line.startWidth = 0.05f;
		line.endWidth = 0.05f;
		line.useWorldSpace = true;

		float step = 11.3f / nb_points;
		float x = 3.3f;
		for(int i=0; i < points.Count; i++) {
			line.SetPosition (i, new Vector2(x, points[i] + 3.2f));
			x += step;
		}

		List<Vector2> bb = new List<Vector2> ();
		bb.Add (new Vector2(3.5f, 3.1f));
		bb.Add (new Vector2(14.4f, 3.1f));
		bb.Add (new Vector2(14.4f, 2.5f));
		bb.Add (new Vector2(3.5f, 2.5f));
		BoundingBox boundingBox = new GameObject("BoundingBox-water").AddComponent<BoundingBox>();
		boundingBox.points = bb;
		boundingBoxes.Add (boundingBox);
	}


	//cosin interpolation
	private float interpolate(float a, float b, float x){
		float f = (1 - Mathf.Cos (x * Mathf.PI)) * 0.5f;
		return a * (1 -f) + b * f;
	}


	private List<float> makePerlinNoise(int nb_points, int wavelength, float amplitude){
		List<float> result = new List<float> ();
		float a = Random.value;
		float b = Random.value;

		for (int i = 0; i <= nb_points; i += 1) {
			if (i % wavelength == 0) {
				a = b;
				b = Random.value;
				result.Add (a * amplitude);
			} else {
				result.Add (interpolate (a, b, (i % wavelength) / wavelength) * amplitude);
			}
		}
		return result;
	}
}
