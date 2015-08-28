﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float speed;
	public Vector3 vector;
	public Vulcan weapon;
	public Shotgun specialWeapon;
	public GameObject owner;
	
	private GameObject affinity;
	private Transform body;
	private SpriteRenderer bodySprite;
	
	void Start(){
		if(owner == null) {
			if(weapon){
				owner = weapon.player.gameObject;
			}else if(specialWeapon){
				owner = specialWeapon.player.gameObject;
			}
		}
		affinity = owner.GetComponent<Entity>().affinity;
		body = transform.Find("Body");
		bodySprite = body.GetComponent<SpriteRenderer>();
		bodySprite.color = affinity.GetComponent<Fleet>().teamColor;
		transform.parent = owner.transform.root.Find ("Bullets");
	}
	
	void Update () {
		transform.Translate (vector * Time.deltaTime * speed);
	}
	
	void OnTriggerEnter2D (Collider2D collision) {
		Entity hitEntity = collision.gameObject.GetComponent<Entity>();
		if(hitEntity){
			if(hitEntity.affinity != affinity){
				IHarmable harmedObject = collision.gameObject.GetComponent(typeof(IHarmable)) as IHarmable;
				if(harmedObject != null){
					harmedObject.ReceiveHit(1);
				}
			}
			if(collision.gameObject != owner && collision.gameObject.GetComponent<Bullet>() == null){
				DestroyMe ();
			}
		}
	}
	
	public void DestroyMe(){
		if(weapon){
			weapon.UnregisterBullet();
		}
		Destroy(gameObject);
	}
}