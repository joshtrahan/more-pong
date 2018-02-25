using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
	private Rigidbody rb;
	public Text playerText;
	public Text opponentText;

	private int playerScore = 0;
	private int opponentScore = 0;

	private float acc;
	private float maxSpeed;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		resetBall ();

		acc = 1.05f;
		maxSpeed = 60f;

		Cursor.visible = false;
	}

	void FixedUpdate () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Paddle")) {
			switchDirection (other);
		}
		if (other.gameObject.CompareTag ("Goal")){
			if (transform.position.z >= 0) {
				playerScore += 1;
				playerText.text = "Player: " + playerScore;
			} else {
				opponentScore += 1;
				opponentText.text = "Opponent: " + opponentScore;
			}

			resetBall ();
		}
	}

	void switchDirection (Collider other){
		SphereCollider sc = transform.GetComponent<SphereCollider> ();

		if (Mathf.Abs(rb.velocity.z) < maxSpeed) {
			rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * -acc);
		} else {
			rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * -1);
		}

		if (transform.position.z < 0) {
			transform.position = new Vector3(transform.position.x, transform.position.y, 
				other.transform.position.z + sc.radius);
		} else {
			transform.position = new Vector3(transform.position.x, transform.position.y, 
				other.transform.position.z - sc.radius);
		}

		print(rb.velocity.z);
	}

	void resetBall (){
		transform.position = new Vector3 (0, 0, 0);
		rb.velocity = new Vector3 (0, 0, 0);
		rb.AddForce (10f, 5f, 50f);
	}
}
