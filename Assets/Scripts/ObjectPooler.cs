using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour 
{

    public GameObject objectToPull;

    public int amount;

    public List<GameObject> objectList = new List<GameObject>();

	// Use this for initialization
	void Start () 
    {
        //populate the pool with objects
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPull);
            obj.SetActive(false);
            objectList.Add(obj);
        }		
	}

    /// <summary>
    /// Gets the object from the pool. Return none if all objects are currently active
    /// </summary>
    /// <returns>The object.</returns>
    public GameObject GetObject()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            if (!objectList[i].activeInHierarchy)
            {
                return objectList[i];
            }
        } 

        //If couldn't find any inactive object, add more objects to the pool. NEED MORE OBJECTS!
        GameObject obj = (GameObject)Instantiate(objectToPull);
        objectList.Add(obj);
        return obj;

    }
}
