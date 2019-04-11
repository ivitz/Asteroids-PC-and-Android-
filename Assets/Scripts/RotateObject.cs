//Attach this to gameobject to make it randomly rotate

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

	public float minSpeed;
	public float maxSpeed;

	private float rndX;
	private float rndY;

	void Start () 
	{
		rndX = Random.Range (minSpeed, maxSpeed);
		rndY = Random.Range (minSpeed, maxSpeed);
	}	

	void Update () 
	{
		float x = rndX * Time.timeScale * Time.deltaTime;
		float y = rndY * Time.timeScale * Time.deltaTime;
		transform.Rotate (x, y, 0);
	}
}
