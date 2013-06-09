using UnityEngine;
using System.Collections;

public class PlayerHealthBar : HealthBar
{
    

    // Use this for initialization
    new public void Start ()
    {
        base.Start();
        
        
        
    
    }
    
    new private void updateRectangles()
    {   
        
        RectOffset border = this.barBorder;
        healthBarBox_inner = new Rect(bar.x, bar.y, bar.width, bar.height);
        healthBarBox_outer = new Rect(bar.x-border.left, bar.y-border.top, bar.width+border.right+border.left, bar.height+border.bottom+border.top);
        
    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
    
    void OnGUI()
    {
        base.draw(myTransform.name+": ", myHealth.curHealth, myHealth.maxHealth,drawDescription: true); 
    }
}

