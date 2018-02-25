using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour {
	private float zCoord;

	// Use this for initialization
	void Start () {
		zCoord = transform.position.z;
		print (zCoord);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 mousePos = Input.mousePosition;
		//mousePos.y = Camera.main.pixelHeight - Input.mousePosition.y;

		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 
			Mathf.Abs(Camera.main.transform.position.z - this.zCoord)));

		mouseWorldPoint.z = this.zCoord;
		this.transform.position = mouseWorldPoint;
	}
}
