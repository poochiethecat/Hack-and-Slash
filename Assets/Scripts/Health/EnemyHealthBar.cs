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

    private Transform _player;

    override public void OnGUI()
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
                healthBox = new Rect(
                    left: screenPos.x-realWidth/2,
                    top: (float)(Screen.height - screenPos.y - ratio*baseYoffset-minYoffset),
                    width: realWidth,
                    height: height
                );
                this.backgroundBox = new Rect(left: this.healthBox.x, top: this.healthBox.y,width:  this.healthBox.width,height: this.healthBox.height);
                this.borderBox = new Rect(
                    left: this.healthBox.x-this.barBorder.left,
                    top: this.healthBox.y-this.barBorder.top,
                    width: this.healthBox.width+this.barBorder.right+this.barBorder.left,
                    height: this.healthBox.height+this.barBorder.bottom+this.barBorder.top
                );
                switch (mystate.TargetState)
                {
                case StateName.Targetted:
                    this.draw(
                        text: this._transform.name+": ",
                        drawDescription: realWidth > this.noDescriptionLimit,
                        noText: realWidth < this.noTextLimit
                    );
                    break;
                default:
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

