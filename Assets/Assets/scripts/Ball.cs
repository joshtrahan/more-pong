using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour {
	private Rigidbody rb;

	public Text playerText;
	public Text opponentText;
	public GameObject scoreDisplayPrefab;

	private int playerScore = 0;
	private int opponentScore = 0;

	private float acc;
	private float maxSpeed;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		makeScoreDisplays ();

		acc = 1.05f;
		maxSpeed = 60f;
	}

	void makeScoreDisplays () {
		GameObject playerScore = (GameObject)Instantiate (
			scoreDisplayPrefab,
			new Vector3 (-400, -30, 0),
			new Quaternion ());

		playerScore.transform.SetParent (GameObject.FindGameObjectWithTag ("UI Canvas").transform);

		playerText = playerScore.GetComponent<Text> ();
		playerText.text = "Player: 0";

		GameObject opponentScore = (GameObject)Instantiate (
			scoreDisplayPrefab,
			new Vector3 (400, -30, 0),
			new Quaternion ());

		opponentScore.transform.SetParent (GameObject.FindGameObjectWithTag ("UI Canvas").transform);

		opponentText = opponentScore.GetComponent<Text> ();
		opponentText.text = "Opponent: 0";
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Paddle")) {
			//switchDirection (other);
			print("collided");
			CmdSwitchDirection (other.transform.position.z);
		}
		if (other.gameObject.CompareTag ("Goal")){
			resetBall ();

			if (transform.position.z >= 0) {
				playerScore += 1;
				playerText.text = "Player: " + playerScore.ToString ();
			} else {
				opponentScore += 1;
				opponentText.text = "Opponent: " + opponentScore.ToString ();
			}
		}
	}

	[Command]
	void CmdSwitchDirection (float otpz){
		print ("cmd");
		SwitchDirection (otpz);
	}

	void SwitchDirection (float otpz){
		print ("switching");
		SphereCollider sc = transform.GetComponent<SphereCollider> ();

		if (Mathf.Abs(rb.velocity.z) < maxSpeed) {
			rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * -acc);
		} else {
			rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * -1);
		}

		if (transform.position.z < 0) {
			transform.position = new Vector3(transform.position.x, transform.position.y, 
				otpz + sc.radius);
		} else {
			transform.position = new Vector3(transform.position.x, transform.position.y, 
				otpz - sc.radius);
		}

		print(rb.velocity.z);
	}

	void resetBall (){
		Destroy (gameObject);
	}
}
