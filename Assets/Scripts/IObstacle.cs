using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle
{
	//This interface is used to make adding more different obstacles or pickups more easy
	string Name { get; set; }	

}
