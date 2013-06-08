using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth = 100;

    
    public HealthBar healthbar;
    private Transform myTransform;
 
   

    // Use this for initialization
    void Start ()
    {
       myTransform = transform;
        

        int[] bar_position = {10,0};
        switch (tag) {
        case "Player":
            bar_position[1] = 10;
            break;
        case "Enemy":
            bar_position[1] = 60;
            break;
        }
        healthbar = new HealthBar(bar_position[0], bar_position[1], Screen.width/2, myTransform);
    }
   
 
    
   

    // Update is called once per frame
    void Update ()
    {
        AdjustCurrentHealth (0);
    }

    void OnGUI ()
    {
        switch (State.getState(myTransform).state)
        {
        case StateName.targetted:
            healthbar.draw(myTransform.name, curHealth, maxHealth);
            break;
        case StateName.visible:
            healthbar.draw(myTransform.name, curHealth, maxHealth);
            break;
        }


    }

    public void AdjustCurrentHealth (int adj)
    {
        curHealth += adj;
        if (curHealth > maxHealth)
            curHealth = maxHealth;
        if (curHealth < 1){
            Targeting targeting = (Targeting)GameObject.FindGameObjectWithTag("Player").GetComponent("Targeting");
            targeting.RemoveTarget(myTransform);
            Destroy(gameObject);
        }
    }
}
