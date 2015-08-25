﻿using UnityEngine;
using System.Collections;

public class Formation : MonoBehaviour {
	public float width = 10f;
	public float height = 10f;
	public GameObject minionPrefab;
	public float maxSpawnSetDelay;
	public float spawnDelay = 0.5f;
	public float currentSpawnSetDelay = 0f;
	
	private GameObject affinity;

	void Start () {
		affinity = GetComponent<Entity>().affinity;
		SpawnUntilFull ();
	}
	
	void OnDrawGizmos () {
		Gizmos.DrawWireCube(
			transform.position,
			new Vector3(width, height)
		);
	}
	
	void Update () {
		currentSpawnSetDelay += Time.deltaTime;
		if(currentSpawnSetDelay >= maxSpawnSetDelay){
			SpawnUntilFull ();
		}
	}
	
	Transform NextFreePosition(){
		foreach(Transform childPosition in transform){
			if (childPosition.childCount <= 0){
				return childPosition;
			}
		}
		return null;
	}
	
	void SpawnUntilFull (){
		Transform freePosition = NextFreePosition();
		
		if(freePosition){
			GameObject minion = Object.Instantiate(minionPrefab, freePosition.position, Quaternion.identity) as GameObject;
			minion.transform.parent = freePosition;
			minion.GetComponent<Entity>().affinity = affinity;
			minion.GetComponent<Entity>().reversePosition = GetComponent<Entity>().reversePosition;
			minion.GetComponent<Minion>().formation = gameObject;
		}
		
		if(NextFreePosition()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
		
	}
	
	public void MinionDestroyed() {
		if(currentSpawnSetDelay >= maxSpawnSetDelay){
			currentSpawnSetDelay = 0f;
		}
	}
}
