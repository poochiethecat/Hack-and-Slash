using UnityEngine;
using System;
public abstract class HealthBar : MonoBehaviour
{
    public int maxWidth = 300;
    public int maxVisibleDistance = 100;
    public int minWidth = 50;
    public Rect bar;
    public RectOffset barBorder;

    
    // Style of the bar-part of the healthbar
    public GUIStyle healthBarStyle;
    // style for the text
    public GUIStyle textStyleNormal;
    public GUIStyle textStyleTargetted;
    // Style for the border/outer box
    protected GUIStyle healthBarBoxStyle_outer = null;
    // Style for the background/inner box
    protected GUIStyle healthBarBoxStyle_inner = null;

    protected Transform myTransform;

    // Rectangle of the healthbar, gets smaller with change in health

    // Rectangle of the max healthbar
    protected Rect healthBarBox_inner;
    // Rectangle for the border, is healthBarBorder pixels bigger in all directions than the inner box
    protected Rect healthBarBox_outer;


    // Background Texture, visible if health < 100%
    protected Texture2D backgroundTexture ;
    // Border Texture, this color takes the border
    protected Texture2D borderTexture;
    // transparent texture for the background of the text
    protected Texture2D clearTexture ;
    
    private bool firstrun;
    
    protected Health myHealth;
    
    public double cutoffPercentage = 0.2; // Below this ratio of health, the bar will be red
    
    public float healthPercentage;
    
    public Color background = Color.cyan;
    public Color border = Color.black;
    
    public Color fullHealth = Color.green;
    public Color criticalHealth = Color.red;
    
    private LABColor _fullHealth;
    private LABColor _critHealth;
    
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
        backgroundTexture = ColoredTexture.generatePixel(background);
        borderTexture = ColoredTexture.generatePixel(border);
        clearTexture = ColoredTexture.generatePixel(Color.clear);
    }
 
    // Returns a Texture with a color based on the current Health of the entity.
    Texture2D healthTexture(int curHealth, int maxHealth)
    {
        // Use LAB-Colors to find a linear path between any two colors in the human colorspace.
        _fullHealth = new LABColor(fullHealth);
        _critHealth = new LABColor(criticalHealth);
        
//        float healthPercentage = ((float)curHealth)/((float)maxHealth);
        // Reach 0 a bit faster than standard
        if (healthPercentage < cutoffPercentage)
            healthPercentage = 0;
        double newLightness = _critHealth.l - healthPercentage*(_critHealth.l - _fullHealth.l);
        double newA = _critHealth.a -healthPercentage*(_critHealth.a - _fullHealth.a);
        double newB = _critHealth.b - healthPercentage*(_critHealth.b - _fullHealth.b);
        LABColor targetColor = new LABColor((float)newLightness, (float)newA, (float)newB);
        return ColoredTexture.generatePixel(targetColor.ToColor());
    }
    
    private void updateTextures()
    {
        backgroundTexture = ColoredTexture.generatePixel(background);
        borderTexture = ColoredTexture.generatePixel(border);
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

//            healthBarStyle = new GUIStyle(GUI.skin.box);
            healthBarStyle.fixedHeight=0;
            healthBarStyle.fixedWidth = 0;

            healthBarStyle.normal.textColor = Color.black;

//            textStyleNormal = new GUIStyle(healthBarStyle);
            textStyleNormal.stretchWidth = false;
            textStyleNormal.normal.background = clearTexture;
            textStyleNormal.fontStyle = FontStyle.Normal;
        
        
//            textStyleTargetted = new GUIStyle(healthBarStyle);
            textStyleTargetted.stretchWidth = false;
            textStyleTargetted.normal.background = clearTexture;
            textStyleTargetted.fontStyle = FontStyle.BoldAndItalic;

    }
    
    private void updateStyles(int curHealth, int maxHealth)
    {
        healthBarStyle.normal.background = healthTexture(curHealth, maxHealth);
        healthBarBoxStyle_outer.normal.background = borderTexture;
        healthBarBoxStyle_inner.normal.background = backgroundTexture;
    }
    
    

 
    
    
    public void draw(string text, int curHealth, int maxHealth, bool drawDescription = true, bool noText = false)
    {
        healthPercentage = ((float)curHealth)/((float)maxHealth);
        
        
        if (firstrun)
        {
            createStyles();
            firstrun = false;
        }
        updateTextures();
        updateStyles(curHealth, maxHealth);

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
        if (State.getState (myTransform).TargetState == StateName.Targetted)
        {
            GUI.Box(healthBarBox_inner,text,textStyleTargetted);
        } else 
        {
            GUI.Box(healthBarBox_inner,text,textStyleNormal);
        }
    }
}

