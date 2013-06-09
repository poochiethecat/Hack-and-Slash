using UnityEngine;
using System;
public class HealthBar : MonoBehaviour
{
    public int healthBarBaseWidth = 300;
    public int healthBarMaxVisibleDistance = 100;
    public int healthBarMinWidth = 50;
    public Rect bar;
    public RectOffset barBorder;

    
    // Style of the bar-part of the healthbar
    public GUIStyle healthBarStyle;
    // style for the text
    public GUIStyle healthBarStyleText;
    // Style for the border/outer box
    protected GUIStyle healthBarBoxStyle_outer = null;
    // Style for the background/inner box
    protected GUIStyle healthBarBoxStyle_inner = null;

    protected Transform myTransform;

    // Rectangle of the healthbar, gets smaller with change in health
//    private Rect healthBar;
    // Rectangle of the max healthbar
    protected Rect healthBarBox_inner;
    // Rectangle for the border, is healthBarBorder pixels bigger in all directions than the inner box
    protected Rect healthBarBox_outer;


    // Background Texture, visible if health < 100%
    protected Texture2D backgroundTexture;
    // Border Texture, this color takes the border
    protected Texture2D borderTexture;
    // transparent texture for the background of the text
    protected Texture2D clearTexture;
    
    private bool firstrun;
    
    protected Health myHealth;
    
    
    public void Start()
    {
        this.myTransform = transform;
        initTextures();
        
        myHealth = myTransform.GetComponent<Health>();


        firstrun = true;
    }
    public float Width {
        get {
            if (bar.width <= 0)
                return Screen.width/2;
            else
                return this.bar.width;
        }
    }   
 
    
    
  
    // Initializes border and background textures.
    private void initTextures()
    {
        backgroundTexture = ColoredTexture.generatePixel(Color.cyan);
        borderTexture = ColoredTexture.generatePixel(Color.black);
        clearTexture = ColoredTexture.generatePixel(Color.clear);
    }
 
    // Returns a Texture with a color based on the current Health of the entity.
    Texture2D healthTexture(int curHealth, int maxHealth)
    {
        
        float healthPercentage = ((float)curHealth)/((float)maxHealth);
        // Reach 0 a bit faster than standard
        if (healthPercentage < 0.2)
            healthPercentage -= 0.1f;
        if (healthPercentage < 0)
            healthPercentage = 0;
        return ColoredTexture.generatePixel( r: 1-healthPercentage, g: healthPercentage ,b: 0);
    }
 
    // Creates the styles we need
    private void createStyles()
    {
            healthBarBoxStyle_inner = new GUIStyle();
            healthBarBoxStyle_outer = new GUIStyle();
            healthBarBoxStyle_outer.normal.background = borderTexture;
            healthBarBoxStyle_inner.normal.background = backgroundTexture;
            healthBarBoxStyle_outer.stretchWidth = false;
            healthBarBoxStyle_inner.stretchWidth = false;
            healthBarBoxStyle_inner.normal.textColor = Color.gray;

            healthBarStyle = new GUIStyle(GUI.skin.box);
            healthBarStyle.fixedHeight=0;
            healthBarStyle.fixedWidth = 0;

            healthBarStyle.normal.textColor = Color.black;

            healthBarStyleText = new GUIStyle(healthBarStyle);
            healthBarStyleText.stretchWidth = false;
            healthBarStyleText.normal.background = clearTexture;

    }
    
    private void updateStyles(int curHealth, int maxHealth)
    {
        healthBarStyle.normal.background = healthTexture(curHealth, maxHealth);
        switch(State.getState(myTransform).TargetState)
        {
            case StateName.Targetted:
                healthBarStyleText.fontStyle = FontStyle.BoldAndItalic;
                break;
            default:
                healthBarStyleText.fontStyle = FontStyle.Normal;
                break;
        }
    }
    
    
    protected void updateRectangles()
    {   
        
        RectOffset border = this.barBorder;
        healthBarBox_inner = new Rect(bar.x, bar.y, bar.width, bar.height);
        healthBarBox_outer = new Rect(bar.x-border.left, bar.y-border.top, bar.width+border.right+border.left, bar.height+border.bottom+border.top);
        
    }
    
//    void OnGui()
//    {
////        State mystate = State.getState(myTransform);
//////        if (this.healthbar == null)
//////        {
//////            Debug.Log ("Healthbar was null, why?");
//////            this.healthbar = new HealthBar(myTransform);
//////        }
////        // If this entity is on scree, render the healthbar
////        
////        if (mystate.ScreenState == StateName.OnScreen)
////        {
////            Vector3 screenPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(myTransform.position);
////            float DistanceFromPlayer = (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position - myTransform.position).magnitude;
////            
////            
////            if (DistanceFromPlayer < healthBarMaxVisibleDistance) // Don't draw if we are too far away
////            { 
////                
////                
////                double ratio_distance = 0.8*healthBarMaxVisibleDistance;
////                
////                double ratio = Math.Pow(1-DistanceFromPlayer/ratio_distance,1);
////                if (ratio < 0 ) ratio = 0;
////                double currentWidth = Math.Pow(1-DistanceFromPlayer/ratio_distance,3)*healthBarBaseWidth;
////                float realWidth = (float)currentWidth+healthBarMinWidth;
////                bar = new Rect(left: screenPos.x-realWidth/2, top: (float)(Screen.height - screenPos.y - ratio*50-20), width: realWidth, height: 20);
////                
////                switch (mystate.TargetState) 
////                {
////                case StateName.Targetted:
////                    this.draw(myTransform.name+": ", myHealth.curHealth, myHealth.maxHealth,drawDescription: realWidth > 138,noText: realWidth < 80);
////                    break;
////                default:
////                    this.draw(myTransform.name+": ", myHealth.curHealth, myHealth.maxHealth,drawDescription: realWidth > 138,noText: realWidth < 80);
////                    break;
////                }
////                
////            }
////        }
////        if (myTransform.tag == "Player")
////        {
//////            bar = new Rect(0,0,0,0);
////            this.draw(myTransform.name+": ", myHealth.curHealth, myHealth.maxHealth,drawDescription: true); 
////        }
//    }

 
    
    
    public void draw(string text, int curHealth, int maxHealth, bool drawDescription = true, bool noText = false)
    {
        
        
        if (firstrun)
        {
            createStyles();
            firstrun = false;
        }
        updateStyles(curHealth, maxHealth);
        updateRectangles();


        bar.width = this.Width * (curHealth / (float)maxHealth);
        
        GUI.Box(healthBarBox_outer,"",healthBarBoxStyle_outer);
        GUI.Box(healthBarBox_inner,"",healthBarBoxStyle_inner);
        GUI.Box(bar,"", healthBarStyle);
        if (drawDescription)
        {
            text = text + curHealth + "/" + maxHealth;
        } else if (noText)
        {
            text = "";
        } else
        {
            text = curHealth + "/" + maxHealth;
        }
        GUI.Box(healthBarBox_inner,text,healthBarStyleText);
    }
}

