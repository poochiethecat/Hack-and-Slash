using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth = 100;
    
    public float barPositionx = -1;
    public float barPositiony = -1;
    
    public float width = -1;
    public float height = -1;

    
    public HealthBar healthbar;
    private Transform myTransform;
 
   

    // Use this for initialization
    void Start ()
    {
       myTransform = transform;
        

        
        healthbar = new HealthBar(myTransform);
    }

    public float Width {
        get {
            if (width < 0)
                return Screen.width/2;
            else
                return this.width;
        }
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
