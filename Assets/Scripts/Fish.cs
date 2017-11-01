using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
	private Vector2 moveDirection;
	public int speedRotation = 30;
	public LineRenderer trajectory;
	public int speed = 2;
	public Projectile projectilePrefab;
	private int indexCurrentPosition;


	// Use this for initialization
	void Start () {
		//transform.position = gameManager.water.GetPosition (600);
		transform.position = trajectory.GetPosition (600);
		indexCurrentPosition = 600;
	}
	
	// Update is called once per frame
	void Update () {
		//Make the player move
		float input = Input.GetAxis ("Horizontal");
		if (input > 0 && indexCurrentPosition < trajectory.positionCount - 30) {
			indexCurrentPosition += speed;
			transform.position = trajectory.GetPosition(indexCurrentPosition);
		} else if (input < 0 && indexCurrentPosition > 30) {
			indexCurrentPosition -= speed;
			transform.position = trajectory.GetPosition(indexCurrentPosition);
		}
		input = Input.GetAxis ("Vertical");
		if (input > 0) {
			if ( (transform.eulerAngles.z >= 0 && transform.eulerAngles.z < 50) || (transform.eulerAngles.z >= 308 && transform.eulerAngles.z <= 360)) {
				transform.Rotate(new Vector3(0, 0, speedRotation) * Time.deltaTime);
			}

		} else if (input < 0) {
			if ((transform.eulerAngles.z >= 0 && transform.eulerAngles.z < 51) || (transform.eulerAngles.z >= 310 && transform.eulerAngles.z <= 360)) {
				transform.Rotate (new Vector3 (0, 0, -speedRotation) * Time.deltaTime);
			}
		}

		//Shoot projectiles
		if (Input.GetKeyDown (KeyCode.Space)) {
			float[] sizes = { 0.14f, 0.10f, 0.13f};
			float[] speeds = { 0.14f, 0.11f, 0.13f};
			Vector3[] offsets = {new Vector3(0, 0.3f, 0), new Vector3(0.3f, 0, 0), new Vector3(-0.3f, 0f, 0)};
			for (int i = 0; i < 3; i++) {
				Projectile projectile = Instantiate (projectilePrefab) as Projectile;
				projectile.transform.position = trajectory.GetPosition (indexCurrentPosition) + offsets[i];
				projectile.center = trajectory.GetPosition (indexCurrentPosition) + offsets[i];
				projectile.radius = sizes[i];
				projectile.speed = speeds[i];
				projectile.angle = (transform.rotation.eulerAngles.z * Mathf.Deg2Rad) + (Mathf.PI / 2);
			}
		}
	}
}
