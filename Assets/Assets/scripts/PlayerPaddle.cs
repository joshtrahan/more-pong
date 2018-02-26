using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPaddle : NetworkBehaviour {
	private float zCoord;
	private float zOffset = 10;
	private float heightOffset = 100;

	[SyncVar]
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
		SpawnBall ();
	}

	[ClientRpc]
	void RpcSetScores(int player1, int player2){

	}

	void SpawnBall(){
		if (ball != null) {
			Destroy (ball);
		}

		ball = (GameObject)Instantiate (
			ballPrefab,
			new Vector3 (0, 0, 0),
			new Quaternion ());

		int[] mults = new int[3];
		for (int i = 0; i < 3; i++) {
			if (Random.Range (0, 2) == 1) {
				mults [i] = 1;
			} else {
				mults [i] = -1;
			}
		}

		ball.GetComponent<Rigidbody> ().velocity = new Vector3 (
			3 * mults[0], 5 * mults[1], 10 * mults[2]);
		//ball.GetComponent<Rigidbody> ().AddForce (3, 5, -10);

		NetworkServer.Spawn (ball);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!isLocalPlayer) {
			return;
		}

		spawnTimer += Time.deltaTime;
		if (Input.GetKeyDown ("space") && spawnTimer >= spawnTimerMax) {
			CmdSpawnBall ();
			spawnTimer = 0f;
		}

		Vector2 mousePos = Input.mousePosition + new Vector3(0, heightOffset, 0);
		//mousePos.y = Camera.main.pixelHeight - Input.mousePosition.y;

		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 
			Mathf.Abs(Camera.main.transform.position.z - this.zCoord)));

		mouseWorldPoint.z = this.zCoord;
		this.transform.position = mouseWorldPoint;
	}
}
