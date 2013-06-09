using UnityEngine;
using System.Collections;
using System;

public class EnemyHealthBar : HealthBar
{

//    // Use this for initialization
//    void Start ()
//    {
//        base.Start();
//    
//    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
    
    void OnGUI()
    {
                State mystate = State.getState(myTransform);
//        if (this.healthbar == null)
//        {
//            Debug.Log ("Healthbar was null, why?");
//            this.healthbar = new HealthBar(myTransform);
//        }
        // If this entity is on scree, render the healthbar
        
        if (mystate.ScreenState == StateName.OnScreen)
        {
            Vector3 screenPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(myTransform.position);
            float DistanceFromPlayer = (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position - myTransform.position).magnitude;
            
            
            if (DistanceFromPlayer < healthBarMaxVisibleDistance) // Don't draw if we are too far away
            { 
                
                
                double ratio_distance = 0.8*healthBarMaxVisibleDistance;
                
                double ratio = Math.Pow(1-DistanceFromPlayer/ratio_distance,1);
                if (ratio < 0 ) ratio = 0;
                double currentWidth = Math.Pow(1-DistanceFromPlayer/ratio_distance,3)*healthBarBaseWidth;
                float realWidth = (float)currentWidth+healthBarMinWidth;
                bar = new Rect(left: screenPos.x-realWidth/2, top: (float)(Screen.height - screenPos.y - ratio*50-20), width: realWidth, height: 20);
                
                switch (mystate.TargetState) 
                {
                case StateName.Targetted:
                    this.draw(myTransform.name+": ", myHealth.curHealth, myHealth.maxHealth,drawDescription: realWidth > 138,noText: realWidth < 80);
                    break;
                default:
                    this.draw(myTransform.name+": ", myHealth.curHealth, myHealth.maxHealth,drawDescription: realWidth > 138,noText: realWidth < 80);
                    break;
                }
                
            }
        }
    }
    
}

