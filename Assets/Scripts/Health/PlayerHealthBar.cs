using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class PlayerHealthBar : HealthBar
{
    public GUIStyle playerTextStyle;
    override public void OnGUI()
    {
        if (this.Configured)
        {
            backgroundRect = new Rect(
                this.healthRect.x,
                this.healthRect.y,
                this.healthRect.width,
                this.healthRect.height
            );
            borderRect = new Rect(
                this.healthRect.x-this.barBorder.left,
                this.healthRect.y-this.barBorder.top,
                this.healthRect.width+this.barBorder.right+this.barBorder.left,
                this.healthRect.height+this.barBorder.bottom+this.barBorder.top
            );
            this.healthRect.width  = this.backgroundRect.width*this.HealthPercentage;
            base.draw(this._transform.name+": ");
        }
    }

    public void configure(Transform transform, Func<float> maxHealth, Func<float> currentHealth)
    {
        base.configure(transform, maxHealth, currentHealth);
        this.textStyle = () => playerTextStyle;
    }
}

