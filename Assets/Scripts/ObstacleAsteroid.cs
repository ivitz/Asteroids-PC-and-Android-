/// <summary>
/// Asteroid that will destroy the ship on collision. This class is derived from ItemBase class.
/// For now it only overrides one method from ItemBase that will detect the collision with player's ship.
/// It also uses IObstacle interface. This is absolutely not necessary in case of this demo project, but it will help to add stuff in the future
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAsteroid : ItemBase, IObstacle {

	public string Name { get { return Name; } set { Name = "Asteroid"; } } 

	protected override void OnCollisionEnter (Collision coll)
	{
        
        if (coll.collider.tag == "Player")
        {            
            //Find the game manager and call GameOver method
            StaticVariables.gameManager.GameOver();
        }
	}
}
