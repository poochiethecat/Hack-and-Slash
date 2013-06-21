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
            if (this.textStyle == null)
                this.textStyle = () => playerTextStyle;
            backgroundRect = new Rect(
                this.maximumSize.x,
                this.maximumSize.y,
                this.maximumSize.width,
                this.maximumSize.height
            );
            healthRect = new Rect(backgroundRect);

            this.healthRect.width  = this.backgroundRect.width*this.HealthPercentage;
            base.draw(this._transform.name+": ");
        }
    }
}

