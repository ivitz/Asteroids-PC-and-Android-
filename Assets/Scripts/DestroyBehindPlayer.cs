using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBehindPlayer : MonoBehaviour {

    //This script will destroy everything that is behind the view

    private Transform player;

    private float timeToDestroy = 0;
    private bool startDestroying;

    [Tooltip("Set to true if you want the player to receive score when this object is passed")]
    public bool giveScore;
    public float scoreAmount;

    [Tooltip("How much time to wait until destroying the object")]
    public float timeToWait;

	void Start () 
    {
        player = GameObject.FindWithTag("Player").transform;
	}
	
    void OnEnable()
    {
        //Disable destroying when the object is enabled from the pool again
        startDestroying = false;
    }

    void Update ()
    {
        //Check if the game is paused
        if (!StaticVariables.gamePaused)
        {           
            if (!startDestroying)
            {
                //Check if player position Z is further than object position Z
                if (player.position.z > transform.position.z)
                {
                    //Calculate when to destroy this object
                    timeToDestroy = Time.time + timeToWait;

                    //Give score to player for successfully passing the object
                    if (giveScore)
                    {
                        StaticVariables.gameManager.currentScore += scoreAmount;
                        StaticVariables.gameManager.asteroidsPassed += 1;
                    }

                    //Start the destroy sequence
                    startDestroying = true;
                }
            }

            //If the destroy started - wait some time to disable this object.
            else
            {
                if (timeToDestroy < Time.time)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
