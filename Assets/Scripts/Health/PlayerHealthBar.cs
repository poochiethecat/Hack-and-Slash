using UnityEngine;
using System.Collections;
[System.Serializable]
public class PlayerHealthBar : HealthBar
{
    //TODO: Create a constructor from an existing healthbar

    // public PlayerHealthBar(Transform transform,, Func<float> maxHealth, Func<float> currentHealth) : base(transform: transform, currentHealth: ()=>100, maxHealth: () =>  100)
    // {

    // }
    public PlayerHealthBar(Health health, Transform entity) : base(health,entity)
    {
    }

    public PlayerHealthBar(HealthBar bar, Health health, Transform entity) : base(bar,health, entity)
    {

    }

    private void updateRectangles()
    {
        healthBarBox_inner = new Rect(bar.x, bar.y, bar.width, bar.height);
        healthBarBox_outer = new Rect(bar.x-barBorder.left, bar.y-barBorder.top, bar.width+barBorder.right+barBorder.left, bar.height+barBorder.bottom+barBorder.top);

    }


    override public void OnGUI()
    {

        base.initGUI();
        updateRectangles();
        base.draw(_transform.name+": ", _health.curHealth, _health.maxHealth,drawDescription: true);
    }
}

