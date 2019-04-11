using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [HideInInspector]
	public float highestScore;
    [HideInInspector]
	public float currentScore;
    [HideInInspector]
    public bool gameOver;

	[Header("Starting UI")]
	public GameObject pausePanel;
	public Text pauseText;

    [Header("In game UI")]
    public Text playerStatsUI;
    [HideInInspector]
    public int asteroidsPassed;
    private float timePlayed;
    private bool newHighScore;

	[Header("GAME OVER UI")]
	public GameObject gameOverPanel;
	public Text gameOverText;

    [Header("Ship")]
    public GameObject playerShip;
    public GameObject shipDestroyParticles;



	void Awake () 
	{		
        //Get the one instance of the Game Manager here
        if (StaticVariables.gameManager == null) 
        {
            StaticVariables.gameManager = this;
        } 
        else 
        {
            Destroy (this);
        }

		//Set timescale to 0 (to imitate pause)
		Time.timeScale = 0;
	}

	void Start () 
	{
		StaticVariables.gamePaused = true;

		//load the game to get the highscore
		LoadGame ();
        playerStatsUI.text = "";
        pauseText.text = string.Format("Your highest score is: {0} \nPress any key to start the game! \nA D to move. SPACE to boost", Convert.ToInt32(highestScore));		
	}
	
	
	void Update () 
	{
        if (!gameOver)
        {
            if (StaticVariables.gamePaused)
            {
                if (Input.anyKeyDown)
                {
                    StartGame();
                }
            }

            if (!StaticVariables.gamePaused)
            {        
                CalculateStats();
                playerStatsUI.text = string.Format("Play Time: {0}s \nAsteroids Passed: {1} \nScore: {2} \nHighest Score: {3}", Convert.ToInt32(timePlayed), asteroidsPassed, Convert.ToInt32(currentScore), Convert.ToInt32(highestScore));
                if (newHighScore)
                {
                    highestScore = currentScore;
                    playerStatsUI.text += "\nNew Highscore!";
                }
            }
        }      

	}

	//player presses any key and the game begins! Time scale to 1, remove UI.
	private void StartGame()
	{
		StaticVariables.gamePaused = false;		
		Time.timeScale = 1;
		pausePanel.SetActive (false);
	}

    //Calculate the score for flying and time of play 
    //+1 for each second you fly and +2 fore each second you use booster
    private void CalculateStats()
    {
        if (StaticVariables.usingBooster)
        {
            currentScore += Time.deltaTime * 2;
        }
        else 
            currentScore += Time.deltaTime;

        if (currentScore >= highestScore)
        {
            newHighScore = true;
        }
     
        timePlayed += Time.deltaTime;   
       
    }

	//Reload the current scene to start again after GAME OVER
	public void PlayAgain()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	/// <summary>
	/// Will stop the game, show GAME OVER message and the score. If new high score reached - update the high score and save the game
	/// </summary>
	public void GameOver()
	{       
        gameOver = true;
        StaticVariables.gamePaused = true;     
        bool newHighScore = false;

		if (currentScore >= highestScore) 
		{           
			SaveGame ();
            newHighScore = true;
		}

        //Do something to player here
        shipDestroyParticles.GetComponent<ParticleSystem>().Play();
        playerShip.GetComponentInChildren<Animator>().SetBool("tiltRight", false);
        playerShip.GetComponentInChildren<Animator>().SetBool("tiltRight", false);
        playerShip.GetComponent<ShipControls>().enabled = false;
        Rigidbody rb = playerShip.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(new Vector3(0, 0, playerShip.GetComponent<ShipControls>().shipSpeed), ForceMode.VelocityChange);

        //Show GAMEOVER UI here
        //playerStatsUI.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        if (newHighScore)
        {
            gameOverText.text = string.Format("GAME OVER! \nCongratulations, you broke your high score! \nNew High Score: {0}", Convert.ToInt32(highestScore));
        }
        else
        {
            gameOverText.text = string.Format("GAME OVER! \nYour score: {1} \nHigh Score: {0}", Convert.ToInt32(currentScore), Convert.ToInt32(highestScore));
        }
        timePlayed = 0;
	}

	/// <summary>
	/// Saves the game. Check if the file is already exists. Create one. Serialize data. Close.
	/// </summary>
	private void SaveGame ()
	{
		SaveData saveData = new SaveData ();

		FileStream stream = new FileStream(Application.dataPath + "/SaveData.dat", FileMode.OpenOrCreate);
		
		BinaryFormatter formatter = new BinaryFormatter();
		try 
		{
            //No need to check the high score here. 
            //SaveGame will only be called if the current high score > saved high score
            saveData.highScore = currentScore;           
            formatter.Serialize(stream, saveData);
		}
		catch (Exception e) 
		{
			Debug.Log ("Failed to save data: " + e.Message);
			throw;
		}
		finally 
		{
			stream.Close();
		}
	}

	/// <summary>
	/// Check if there is a save file already. Do nothing if not. Load Highest score if there is. Show it on the screen.
	/// </summary>
	private void LoadGame ()
	{
		SaveData saveData = null;

		if (File.Exists(Application.dataPath + "/SaveData.dat"))
		{
			FileStream stream = new FileStream(Application.dataPath + "/SaveData.dat", FileMode.OpenOrCreate);		

			try
			{
				BinaryFormatter formatter = new BinaryFormatter();

				saveData = (SaveData)formatter.Deserialize(stream);
				highestScore = saveData.highScore;             
			}
			catch (Exception e)
			{
				Debug.Log("Failed to save data: " + e.Message);
				throw;
			}
			finally
			{
				stream.Close();
			}
		}
	}
}
