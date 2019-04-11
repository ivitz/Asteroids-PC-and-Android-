/// <summary>
/// An abstract class that each item on the road in game will use (asteroids, coins, enemies, small asteroids etc)
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemBase : MonoBehaviour 
{
    //add some properties and methods later
    protected abstract void OnCollisionEnter(Collision coll); 
}
