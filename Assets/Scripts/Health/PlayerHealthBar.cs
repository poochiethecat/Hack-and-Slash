using UnityEngine;
using System.Collections;

public class PlayerHealthBar : HealthBar
{
    

    // Use this for initialization
    new public void Start ()
    {
        base.Start();
        
        
        
    
    }
    
    private void updateRectangles()
    {   
        
        healthBarBox_inner = new Rect(bar.x, bar.y, bar.width, bar.height);
        healthBarBox_outer = new Rect(bar.x-barBorder.left, bar.y-barBorder.top, bar.width+barBorder.right+barBorder.left, bar.height+barBorder.bottom+barBorder.top);
        
    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
    
    void OnGUI()
    {
        updateRectangles();
        base.draw(myTransform.name+": ", myHealth.curHealth, myHealth.maxHealth,drawDescription: true); 
    }
}

