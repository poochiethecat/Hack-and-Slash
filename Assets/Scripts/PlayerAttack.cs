using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	
	public GameObject target;
	
	public float attackTimer;
	public float coolDown;
	
	private Targeting targeting;

	// Use this for initialization
	void Start () {
		attackTimer = 0;
		coolDown = 0.0f;
		targeting = (Targeting)GetComponent("Targeting");
	}
	
	// Update is called once per frame
	void Update () {
		if(targeting.selectedTarget != null) target = targeting.selectedTarget.gameObject;
		if(attackTimer > 0){
			attackTimer -= Time.deltaTime;
		}
		if(attackTimer < 0) attackTimer = 0;
		if(Input.GetKeyUp(KeyCode.F) && attackTimer == 0 && target != null){
			Attack();
			attackTimer = coolDown;
		}
	}
	
	private void Attack(){
		float distance = Vector3.Distance(target.transform.position, transform.position);
		
		Vector3 dir = (target.transform.position - transform.position).normalized;
		
		float direction = Vector3.Dot(dir, transform.forward);
		
		Debug.Log(direction);
		
		if(distance <= 2.5f && direction > 0){
			Health targetHealth = (Health)target.GetComponent("Health");
			targetHealth.AdjustCurrentHealth(-10);
		}
	}
}
