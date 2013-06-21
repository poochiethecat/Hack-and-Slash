using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class EnemyHealthBar : HealthBar
{
    public GUIStyle textStyleNormal;
    public GUIStyle textStyleTargetted;
    public double distanceRatio = 0.8;
    public float minYoffset = 20;
    public float baseYoffset = 50;
    public float powersOfRatio = 3;
    public float barHeight = 20;
    public float minHeight = 20;
    public float noTextLimit = 80;
    public float noDescriptionLimit = 140;

    private Transform _player;


    //TODO: Refactor this monster xD

    override public void OnGUI()
    {
        if (this.Configured)
        {
            State mystate = State.getState(_transform);

            if (mystate.ScreenState == StateName.OnScreen)
            {
                if (_player == null)
                    _player = GameObject.FindGameObjectWithTag("Player").
                    GetComponent<Transform>();

                Vector3 screenPos = GameObject.FindGameObjectWithTag("MainCamera")
                    .GetComponent<Camera>()
                    .WorldToScreenPoint(this._transform.position);
                float distanceFromPlayer = (this._player.position - this._transform.position).magnitude;


                if (distanceFromPlayer < this.maxVisibleDistance) // Don't draw if we are too far away
                {


                    double ratio_distance = distanceRatio*this.maxVisibleDistance;

                    double ratio = Math.Pow(1-distanceFromPlayer/ratio_distance,1);
                    if (ratio < 0 ) ratio = 0;
                    double currentWidth = Math.Pow( 1-distanceFromPlayer/ratio_distance, this.powersOfRatio )*maxWidth;
                    float realWidth = (float)( currentWidth < this.minWidth ? this.minWidth : currentWidth );
                    float height = (float) barHeight < this.minHeight ? this.minHeight   : this.barHeight;
                    healthRect = new Rect(
                        left: screenPos.x-realWidth/2,
                        top: (float)(Screen.height - screenPos.y - ratio*baseYoffset-minYoffset),
                        width: realWidth,
                        height: height
                    );
                    this.backgroundRect = new Rect(left: this.healthRect.x, top: this.healthRect.y,width:  this.healthRect.width,height: this.healthRect.height);
                    this.borderRect = new Rect(
                        left: this.healthRect.x-this.barBorder.left,
                        top: this.healthRect.y-this.barBorder.top,
                        width: this.healthRect.width+this.barBorder.right+this.barBorder.left,
                        height: this.healthRect.height+this.barBorder.bottom+this.barBorder.top
                    );
                    this.healthRect.width  = this.backgroundRect.width*this.HealthPercentage;
                    switch (mystate.TargetState)
                    {
                    case StateName.Targetted:
                        base.textStyle = () => this.textStyleTargetted;
                        this.draw(
                            text: this._transform.name+": ",
                            drawDescription: realWidth > this.noDescriptionLimit,
                            noText: realWidth < this.noTextLimit
                        );
                        break;
                    default:
                        base.textStyle = () => this.textStyleNormal;
                        this.draw(
                            text: this._transform.name+": ",
                            drawDescription: realWidth > this.noDescriptionLimit,
                            noText: realWidth < this.noTextLimit
                        );
                        break;
                    }

                }
            }
        }
    }

}

