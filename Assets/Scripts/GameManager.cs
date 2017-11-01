using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Fish fishPrefab;
	public Bug bugPrefab;
	public static Vector2 windVector = new Vector2(0, 0); 


	// Use this for initialization
	void Start () {
		//Initiate bowl
		new GameObject("Bowl").AddComponent<Bowl>();
		Water water = new GameObject ("Water").AddComponent<Water> ();
		water.generateWater (2300, 60, 0.2f, 8);
		Fish fish = Instantiate (fishPrefab) as Fish;
		fish.name = "Fish_player";
		fish.trajectory = water.line;

		//Initiate Physics Manager
		new GameObject("PhysicsManager").AddComponent<PhysicsManager>();

		//Instanciate a new bug every few seconds
		InvokeRepeating("instanciateBug", 1f, 2f);

		InvokeRepeating("changeWind", 1f, 1f);

	}

	void instanciateBug(){
		if (Random.Range (0, 3) == 1) {
			Bug bug = Instantiate (bugPrefab) as Bug;
		}
	}

	void changeWind(){
		int value = Random.Range (-3, 3);
		windVector.x = value * 0.01f;
	}

}
