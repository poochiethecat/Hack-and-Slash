using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth = 100;

    
    public HealthBar healthbar;
    private Transform myTransform;
 
    
    private string state = "unselected";
   

    // Use this for initialization
    void Start ()
    {
       myTransform = transform;
        

        int[] position = {10,0};
        switch (tag) {
        case "Player":
            position[1] = 10;
            this.state = "visible";
            break;
        case "Enemy":
            position[1] = 60;
            break;
        }
        healthbar = new HealthBar(position[0], position[1], Screen.width/2);
    }
    
    
    public void select()
    {
        this.state = "selected";
        
    }
    
    public void deselect()
    {
        this.state = "unselected";
        
    }
 
    
   

    // Update is called once per frame
    void Update ()
    {
        AdjustCurrentHealth (0);
    }

    void OnGUI ()
    {
        switch (this.state)
        {
        case "selected":
            healthbar.draw(myTransform.name, curHealth, maxHealth,FontStyle.BoldAndItalic);
            break;
        case "visible":
            healthbar.draw(myTransform.name, curHealth, maxHealth,FontStyle.Normal);
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
