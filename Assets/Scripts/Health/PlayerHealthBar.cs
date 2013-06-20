using UnityEngine;
using System.Collections;
[System.Serializable]
public class PlayerHealthBar : HealthBar
{
    override public void OnGUI()
    {
        backgroundBox = new Rect(
            this.healthBox.x,
            this.healthBox.y,
            this.healthBox.width,
            this.healthBox.height
        );
        borderBox = new Rect(
            this.healthBox.x-this.barBorder.left,
            this.healthBox.y-this.barBorder.top,
            this.healthBox.width+this.barBorder.right+this.barBorder.left,
            this.healthBox.height+this.barBorder.bottom+this.barBorder.top
        );
        base.draw(this._transform.name+": ");
    }
}

