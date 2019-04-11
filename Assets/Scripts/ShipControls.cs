using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControls : MonoBehaviour {

	public Transform shipModel;		//the transform that will be influenced by rotation
	private Animator shipAnimarot;	

	[Header ("Ship stats")]
	public float horizontalSpeed;
	public float shipSpeed;			//The initial speed of the ship
	private float currentShipSpeed;	//The current speed of the ship. Will multiply with booster
	public float rotationSpeed;

	//True if player are using the booster. Will double the speed and score. 
	//Static to make everyone be able to see if the play is using the boost
	public static bool isBooster;

	[Header("---Camera Control---")]
	[Header("No booster")]
	public float cameraHeightNoBooster;
	public float cameraHeightDumpingNoBooster;

	[Header("With booster")]
	public float cameraHeightBooster;
	public float cameraHeightDumpingBooster;

	private SmoothFollow cameraControl;

	// Use this for initialization
	void Start () 
	{
		//initialRotationOfTheShip = shipModel.rotation;
		shipAnimarot = shipModel.GetComponent<Animator>();
		cameraControl = Camera.main.GetComponent<SmoothFollow>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UseBooster();
		FlyTheShip ();
		MoveHorizontal ();
		Rotate ();

	}

	private void UseBooster()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			StaticVariables.usingBooster = true;
		}
		else 
		{
			StaticVariables.usingBooster = false;
		}

		if (StaticVariables.usingBooster)
		{
			currentShipSpeed = shipSpeed * 2;
			cameraControl.height = cameraHeightBooster;
			cameraControl.heightDamping = cameraHeightDumpingBooster;
		}
		else
		{
			currentShipSpeed = shipSpeed;
			cameraControl.height = cameraHeightNoBooster;
			cameraControl.heightDamping = cameraHeightDumpingNoBooster;
		}
	}

	private void FlyTheShip ()
	{
        transform.Translate (0, 0, currentShipSpeed * Time.timeScale * Time.deltaTime);
	}

	private void MoveHorizontal()
	{
		float translation = Input.GetAxis("Horizontal") * horizontalSpeed;

		//if the ship is withing -3 and 3 coordinates (on the road) - player can go anywhere
		if (transform.position.x <= 3 && transform.position.x >= -3)
		{
			translation *= Time.deltaTime * Time.timeScale;
			transform.Translate(translation, 0, 0);
		} 
		//if the ship is on the right border - restrict moving right
		else if (transform.position.x >= 3 )
		{
			if (Input.GetAxis("Horizontal") < 0)
			{
				translation *= Time.deltaTime * Time.timeScale;
				transform.Translate(translation, 0, 0);
			}
		}
		//if the ship is on the left border - restrict moving left
		else if (transform.position.x <= -3)
		{
			if (Input.GetAxis("Horizontal") > 0)
			{
				translation *= Time.deltaTime * Time.timeScale;
				transform.Translate(translation, 0, 0);
			}
		}

	}

	private void Rotate() 
	{
		//Play tilt ship LEFT
		if (Input.GetKey(KeyCode.A))
		{
			shipAnimarot.SetBool("tiltLeft", true);
		}
		else
		{
			shipAnimarot.SetBool("tiltLeft", false);
		}			

		//Play tilt ship RIGHT
		if (Input.GetKey(KeyCode.D))
		{
			shipAnimarot.SetBool("tiltRight", true);
		}
		else
		{
			shipAnimarot.SetBool("tiltRight", false);
		}
	}
}
