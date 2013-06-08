using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth = 100;
    
    
    public Rect bar;
    public RectOffset barBorder;

    
    private HealthBar healthbar;
    private Transform myTransform;
 
   

    // Use this for initialization
    void Start ()
    {
       myTransform = transform;
        this.healthbar = new HealthBar(myTransform);
        

        
//        healthbar = new HealthBar(myTransform);
    }

    public float Width {
        get {
            if (bar.width <= 0)
                return Screen.width/2;
            else
                return this.bar.width;
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
