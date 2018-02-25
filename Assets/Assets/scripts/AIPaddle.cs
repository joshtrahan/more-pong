using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour {
	float zCoord;

	// Use this for initialization
	void Start () {
		zCoord = this.transform.position.z;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameObject ball = GameObject.FindGameObjectWithTag ("Ball");
		this.transform.position = new Vector3(ball.transform.position.x, 
			ball.transform.position.y, this.zCoord);
	}
}
