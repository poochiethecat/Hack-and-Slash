using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class EnemyHealthBar : HealthBar
{

    public double distanceRatio = 0.8;
    public float minYoffset = 20;
    public float baseYoffset = 50;
    public float powersOfRatio = 3;
    public float barHeight = 20;
    public float minHeight = 20;
    public float noTextLimit = 80;
    public float noDescriptionLimit = 140;


    // public EnemyHealthBar(Health health, Transform entity) : base(health,entity) { }

    // public EnemyHealthBar(HealthBar bar, Health health, Transform entity) : base(bar,health, entity){}



    override public void OnGUI()
    {
        base.initGUI();
        State mystate = State.getState(_transform);

        if (mystate.ScreenState == StateName.OnScreen)
        {
            Vector3 screenPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(_transform.position);
            float DistanceFromPlayer = (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position - _transform.position).magnitude;


            if (DistanceFromPlayer < maxVisibleDistance) // Don't draw if we are too far away
            {


                double ratio_distance = distanceRatio*maxVisibleDistance;

                double ratio = Math.Pow(1-DistanceFromPlayer/ratio_distance,1);
                if (ratio < 0 ) ratio = 0;
                double currentWidth = Math.Pow(1-DistanceFromPlayer/ratio_distance,powersOfRatio)*maxWidth;
                float realWidth = (float)(currentWidth<minWidth ? minWidth : currentWidth);
                float height = (float) barHeight < minHeight ? minHeight   : barHeight;
                bar = new Rect(left: screenPos.x-realWidth/2, top: (float)(Screen.height - screenPos.y - ratio*baseYoffset-minYoffset), width: realWidth, height:height);
                healthBarBox_inner = new Rect(bar.x, bar.y, bar.width, bar.height);
                healthBarBox_outer = new Rect(bar.x-barBorder.left, bar.y-barBorder.top, bar.width+barBorder.right+barBorder.left, bar.height+barBorder.bottom+barBorder.top);
                switch (mystate.TargetState)
                {
                case StateName.Targetted:
                    this.draw(_transform.name+": ", _health.curHealth, _health.maxHealth,drawDescription: realWidth > noDescriptionLimit,noText: realWidth < noTextLimit);
                    break;
                default:
                    this.draw(_transform.name+": ", _health.curHealth, _health.maxHealth,drawDescription: realWidth > noDescriptionLimit,noText: realWidth < noTextLimit);
                    break;
                }

            }
        }
    }

}

