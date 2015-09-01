﻿using UnityEngine;
using System.Collections;

public class Vulcan : Weapon {
	private float defaultSpeed = 8;
	private float defaultFireDelay = 0.3f;
	private int defaultMaxBulletsInPlay = 8;
	
	private GameObject bulletPrefab;
	private float speed;
	
	
	public Vulcan(){
		maxBulletsInPlay = defaultMaxBulletsInPlay;
		fireDelay = defaultFireDelay;
		speed = defaultSpeed;
		bulletPrefab = Resources.Load ("bullet") as GameObject;
	}
 
	public void Fire (bool exAttempt) {
		if(CanFire ()){
			bool ex = exAttempt && player.SpendEx(1);
			
			BulletProjectile bullet = newProjectile();
			
			if(ex){
				maxBulletsInPlay = defaultMaxBulletsInPlay * 2;
				fireDelay = defaultFireDelay / 1.5f;
				speed = defaultSpeed * 1.5f;
			}else{
				speed = defaultSpeed;
				fireDelay = defaultFireDelay;
				maxBulletsInPlay = defaultMaxBulletsInPlay;
			}
			
			bullet.speed = speed;
			bullet.weapon = this;
			bullet.GetComponent<Entity>().affinity = GetComponent<Entity>().affinity;
			bullet.vector = Vector3.up;
			RegisterBullet ();
			OrientProjectile(bullet);
			timeSinceLastFire = 0f;
		}
	}
	
	private BulletProjectile newProjectile(){
		GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		BulletProjectile bullet = bulletObject.GetComponent<BulletProjectile>();
		return(bullet);
	}
}
