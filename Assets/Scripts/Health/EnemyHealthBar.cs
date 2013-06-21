using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class EnemyHealthBar : HealthBar
{

    [RangeAttributeWithDefault(5, 200, 100)]
    public int maxVisibleDistance = 100;

    [RangeAttributeWithDefault(1,100,50)]
    public int minWidth = 50;

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
    private Camera _camera;

    private float maxMagnitudeFromPlayer;




    private float distanceFromPlayer()
    {
        if (_player == null)
        _player = GameObject.FindGameObjectWithTag("Player").
            GetComponent<Transform>();

        return (this._player.position - this._transform.position).sqrMagnitude;
    }

    private Vector3 screenPosition()
    {
        if (_camera == null)
            _camera = GameObject.FindGameObjectWithTag("MainCamera")
                .GetComponent<Camera>();

        return _camera.WorldToScreenPoint(this._transform.position);
    }

    //TODO: Refactor this monster xD
    override public void OnGUI()
    {
        if (this.Configured)
        {
            State mystate = State.getState(_transform);

            if (mystate.ScreenState == StateName.OnScreen)
            {
                float distanceFromPlayer =  this.distanceFromPlayer();
                this.maxMagnitudeFromPlayer = Mathf.Pow(this.maxVisibleDistance,2);

                if (distanceFromPlayer <  this.maxMagnitudeFromPlayer) // Don't draw if we are too far away
                {
                    Vector3 screenPos = this.screenPosition();


                    double ratio_distance = Mathf.Pow((float)distanceRatio,2f)*this.maxMagnitudeFromPlayer;

                    double ratio = 1-distanceFromPlayer/ratio_distance;
                    if (ratio < 0 ) ratio = 0;
                    double currentWidth = Math.Pow( 1-distanceFromPlayer/ratio_distance, this.powersOfRatio )*this.maximumSize.width;
                    float realWidth = (float)( currentWidth < this.minWidth ? this.minWidth : currentWidth );
                    float realHeight = (float) this.barHeight < this.minHeight ? this.minHeight   : this.barHeight;



                    healthRect = new Rect(
                        left: screenPos.x-realWidth/2,
                        top: (float)(Screen.height - screenPos.y - ratio*baseYoffset-minYoffset),
                        width: realWidth,
                        height: realHeight
                    );
                    this.backgroundRect = new Rect(left: this.healthRect.x, top: this.healthRect.y,width:  this.healthRect.width,height: this.healthRect.height);
                    this.healthRect.width  = this.healthRect.width*this.HealthPercentage;


                    switch (mystate.TargetState)
                    {
                    case StateName.Targetted:
                        base.textStyle = () => this.textStyleTargetted;
                        break;
                    case StateName.Normal:
                        base.textStyle = () => this.textStyleNormal;
                        break;
                    }
                    this.draw(
                        text: this._transform.name+": ",
                        drawDescription: realWidth > this.noDescriptionLimit,
                        noText: realWidth < this.noTextLimit
                    );

                }
            }
        }
    }

}

