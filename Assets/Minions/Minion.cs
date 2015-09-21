﻿
using UnityEngine;
using System.Collections;

public class Minion : Agent, IAttacker, IShreddable {
	public float speed;
	public float fireDelay;
	public GameObject formation;
	public bool reversePosition;
	
	protected float timeSinceLastFire;
	protected DamageBehavior damageBehavior;
	protected float timeSinceStart = 0;
	
	
	public virtual void Start() {
		damageBehavior = GetComponent<DamageBehavior>();
		GenerateExhaust();
	}
	
	public virtual void Update () {
		timeSinceLastFire += Time.deltaTime;
		timeSinceStart += Time.deltaTime;
	}
	
	public override void ReceiveHit(float damage, GameObject attackerObject, GameObject attack) {
		IAttacker attacker = ResolveAttacker(attackerObject);
		if(attackerObject && attackerObject.GetComponent<Minion>()) damage /= 2;
		if(attacker != null) attacker.RegisterSuccessfulAttack(0);
		damageBehavior.ReceiveDamage(damage);
		if(damageBehavior.CurrentHealthRatio() <= 0){
			if(attacker != null) attacker.RegisterSuccessfulDestroy(5);
			DestroyMe ();
		}
	}
	
	public void RegisterSuccessfulAttack(float value){
	}
	
	public void RegisterSuccessfulDestroy(float value){
	}
	
	private void GenerateExhaust(){
		GameObject body = transform.Find ("Body").gameObject;
		foreach(Transform child in body.transform){
			Exhaust exhaust = child.GetComponent<Exhaust>();
			if(exhaust){
				exhaust.SetColor(GetComponent<Entity>().affinity.GetComponent<Fleet>().teamColor);
			}
		}
	}
	
	public void DestroyMe(){
		Destroy(transform.parent.gameObject);
		GameObject explosion = Instantiate ( Resources.Load ("Explosion"), transform.position, Quaternion.identity) as GameObject;
		explosion.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
	}
}