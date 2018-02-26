using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPaddle : NetworkBehaviour {
	private float zCoord;
	private float zOffset = 10;

	private float spawnTimer = 2;
	private float spawnTimerMax = 2;

	public GameObject ballPrefab;
	private GameObject ball;

	private Camera playerCam;

	// Use this for initialization
	void Start () {
		playerCam = Camera.main;
		if (!NetworkServer.active) {
			print ("network server not active");
			playerCam.transform.position = new Vector3 (0, 0, 20);
			playerCam.transform.eulerAngles = new Vector3 (0, 180, 0);
			zCoord = zOffset;
		} else {
			print ("network server active");
			playerCam.transform.position = new Vector3 (0, 0, -20);
			playerCam.transform.eulerAngles = new Vector3 (0, 0, 0);
			zCoord = -zOffset;
		}

		print (zCoord);
	}

	[Command]
	void CmdSpawnBall(){
		if (spawnTimer <= spawnTimerMax) {
			return;
		}
		spawnTimer = 0f;

		print ("should be spawning");
		if (ball != null) {
			print ("ball not null");
			Destroy (ball);
		}

		ball = (GameObject)Instantiate (
			ballPrefab,
			new Vector3 (0, 0, 0),
			new Quaternion ());

		ball.GetComponent<Rigidbody> ().velocity = new Vector3 (3, 5, -10);

		NetworkServer.Spawn (ball);
		print ("ball pos: " + ball.transform.position);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!isLocalPlayer) {
			return;
		}

		spawnTimer += Time.deltaTime;
		if (Input.GetKeyDown ("space")) {
			print ("space");
			CmdSpawnBall ();
		}

		Vector2 mousePos = Input.mousePosition + new Vector3(0, 75, 0);
		//mousePos.y = Camera.main.pixelHeight - Input.mousePosition.y;

		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 
			Mathf.Abs(Camera.main.transform.position.z - this.zCoord)));

		mouseWorldPoint.z = this.zCoord;
		this.transform.position = mouseWorldPoint;
	}
}
