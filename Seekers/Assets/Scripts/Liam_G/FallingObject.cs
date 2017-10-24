﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{

    //prefabs and spawning locations
    private Rigidbody RB;
    public GameObject Block;
    //public GameObject FallingObjectSpawner;


    public Vector3 BlockSpawn = new Vector3(0, 0, 0);
    public Transform FallSpawn;
    public Transform FallSpawn1;
    public Transform FallSpawn2;
    public Transform FallSpawn3;

    public float Spawnlimite = 5;
    public float spawnTimer = 1;
    // Use this for initialization
    void Start ()
    {
        RB = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    //trigger to start spwning the rocks to fall
    //void OnTriggerEnter(Collider other)
    //{
    //    //make sure to only let it trigger for players
    //    if (GameObject.FindGameObjectWithTag("Car"))
    //    {
    //        Debug.Log("Block should be falling");
    //        //spawing location
    //        GameObject FallingObject = Instantiate(Block, FallSpawn.position, Quaternion.identity);
    //        GameObject FallingObject1 = Instantiate(Block, FallSpawn1.position, Quaternion.identity);
    //        GameObject FallingObject2 = Instantiate(Block, FallSpawn2.position, Quaternion.identity);
    //        GameObject FallingObject3 = Instantiate(Block, FallSpawn3.position, Quaternion.identity);
    //    }
    //}

    //Then just do Instantiate(gameObject, Vector3(Random.Range(minY,maxY), 
    //                         Random.Range(minZ,maxZ), Random.Range(minX,maxX)), 
    //                         Quaternion.identity)

    void OnTriggerEnter(Collider other)
    {
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            if(Time.time > spawnTimer)
            {
                spawnTimer = Time.time + Spawnlimite;

                Vector3 rndPosWithin;
                rndPosWithin = new Vector3(Random.Range(FallSpawn.position.x, FallSpawn1.position.x), Random.Range(50, 50), Random.Range(FallSpawn2.position.z, FallSpawn3.position.z));
                //rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
                GameObject FallingObjectSpawner = Instantiate(Block, rndPosWithin, Quaternion.identity);
            }
        }
    }
}