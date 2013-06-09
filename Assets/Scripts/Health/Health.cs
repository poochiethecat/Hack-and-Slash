using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth = 100;
    

    
    private HealthBar healthbar;
    private Transform myTransform;
 
   

    // Use this for initialization
    void Start ()
    {
       myTransform = transform;
    }


 
    
   

    // Update is called once per frame
    void Update ()
    {
        AdjustCurrentHealth (0);
    }


    public void AdjustCurrentHealth (int adj)
    {
        curHealth += adj;
        if (curHealth > maxHealth)
            curHealth = maxHealth;
        if (curHealth < 1){
            Targeting targeting = GameObject.FindGameObjectWithTag("Player").GetComponent<Targeting>();
            targeting.RemoveTarget(myTransform);
            Destroy(gameObject);
        }
    }
}
