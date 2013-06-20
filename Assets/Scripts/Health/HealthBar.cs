using UnityEngine;
using System;
[System.Serializable]
public abstract class HealthBar {

    [RangeAttributeWithDefault(50, 1000,300)]
    public int maxWidth = 300;
    [RangeAttributeWithDefault(5, 200, 100)]
    public int maxVisibleDistance = 100;
    [RangeAttributeWithDefault(1,100,50)]
    public int minWidth = 50;

    public Rect bar;
    public RectOffset barBorder;


    // Style of the bar-part of the healthbar
    protected    GUIStyle healthBarStyle;
    // style for the text
    public GUIStyle textStyleNormal;
    public GUIStyle textStyleTargetted;

    public double cutoffPercentage = 0.2; // Below this ratio of health, the bar will be red



    public Color background = Color.cyan;
    private Color _lastBackground = Color.cyan;
    public Color border = Color.black;
    private Color _lastBorder = Color.black;

    public Color fullHealth = Color.green;
    private Color _lastFullHealth = Color.green;
    public Color criticalHealth = Color.red;
    private Color _lastFriticalHealth = Color.red;



    // Style for the border/outer box
    protected GUIStyle healthBarBoxStyle_outer = null;
    // Style for the background/inner box
    protected GUIStyle healthBarBoxStyle_inner = null;



    protected Transform _transform;

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


    private LABColor _fullHealth;
    private LABColor _critHealth;


    private float healthPercentage;


    private Func<float> maxHealth;
    private Func<float> currentHealth;


    public float Width {
        get {
            if (bar.width <= 0)
                return Screen.width/2;
            else
                return this.bar.width;
        }
    }


    private Boolean isConfigured()
    {
        bool m =  this.maxHealth == null;
        bool c =  this.currentHealth == null;
        bool t = this._transform == null;

        if (m&&c&&t)
        {
            Debug.LogWarning("This HealthBar has not been configured.");
            return false;
        }

        if (m) Debug.LogWarning("maxHealth has not been configured on this healthbar.");
        if (c) Debug.LogWarning("current has not been configured on this healthbar.");
        if (t) Debug.LogWarning("transform has not been configured on this healthbar.");
        return !m && !c && !t;
    }




    public abstract void OnGUI();

    /// <summary>
    /// This Method MUST be called before any use of a healthbar, it will not do anything.
    /// </summary>
    /// <param name="transform">The transform of the entity this healthbar is for</param>
    /// <param name="maxHealth">Function returning a float/int which is the maximum Health of the entity</param>
    /// <param name="currentHealth">Function returning a float/int which is the current health of the entity</param>
    public void configure(Transform transform, Func<float> maxHealth, Func<float> currentHealth )
    {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this._transform = transform;
    }






    // Initializes border and background textures.
    private void initTextures()
    {
        this.backgroundTexture = ColoredTexture.generatePixel(this.background);
        this.borderTexture = ColoredTexture.generatePixel(this.border);
        this.clearTexture = ColoredTexture.generatePixel(Color.clear);
    }

    // Returns a Texture with a color based on the current Health of the entity.
    Texture2D healthTexture()
    {
        // Use LAB-Colors to find a linear path between any two colors in the human colorspace.
        this._fullHealth = new LABColor(fullHealth);
        this._critHealth = new LABColor(criticalHealth);

        // Reach 0 a bit faster than standard
        if (this.healthPercentage < cutoffPercentage)
            this.healthPercentage = 0;
        double newLightness = _critHealth.l - healthPercentage*(_critHealth.l - _fullHealth.l);
        double newA = _critHealth.a -healthPercentage*(_critHealth.a - _fullHealth.a);
        double newB = _critHealth.b - healthPercentage*(_critHealth.b - _fullHealth.b);
        LABColor targetColor = new LABColor((float)newLightness, (float)newA, (float)newB);
        return ColoredTexture.generatePixel(targetColor.ToColor());
    }

    private bool colorsEqual(Color a, Color b)
    {
        return a.r == b.r && a.g == b.g && a.b == b.g && a.a == b.a;
    }

    private void updateTextures()
    {
        if (this.backgroundTexture == null || !this.colorsEqual(this.background, this._lastBackground))
        {
            this._lastBackground = this.background;
            this.backgroundTexture = ColoredTexture.generatePixel(this._lastBackground);
        }
        if (this.borderTexture == null || !this.colorsEqual(this.border, this._lastBackground))
        {
            this._lastBorder = this.border;
            this.borderTexture = ColoredTexture.generatePixel(this._lastBorder);
        }
    }


    // Creates the styles we need
    private void createStyles()
    {
            this.healthBarBoxStyle_inner = new GUIStyle();
            this.healthBarBoxStyle_outer = new GUIStyle();
            this.healthBarBoxStyle_outer.normal.background = this.borderTexture;
            this.healthBarBoxStyle_inner.normal.background = this.backgroundTexture;
            this.healthBarBoxStyle_outer.stretchWidth = false;
            this.healthBarBoxStyle_inner.stretchWidth = false;
            this.healthBarBoxStyle_inner.normal.textColor = Color.gray;

            this.healthBarStyle = new GUIStyle(GUI.skin.box);
            this.healthBarStyle.fixedHeight=0;
            this.healthBarStyle.fixedWidth = 0;

            this.healthBarStyle.normal.textColor = Color.black;

            this.textStyleNormal = new GUIStyle(this.healthBarStyle);
            this.textStyleNormal.stretchWidth = false;
            this.textStyleNormal.normal.background = this.clearTexture;
            this.textStyleNormal.fontStyle = FontStyle.Normal;

            this.textStyleTargetted = new GUIStyle(this.healthBarStyle);
            this.textStyleTargetted.stretchWidth = false;
            this.textStyleTargetted.normal.background = this.clearTexture;
            this.textStyleTargetted.fontStyle = FontStyle.BoldAndItalic;

    }

    private void updateStyles()
    {
        this.updateStyles();
        this.healthBarStyle.normal.background = this.healthTexture();
        this.healthBarBoxStyle_outer.normal.background = this.borderTexture;
        this.healthBarBoxStyle_inner.normal.background = this.backgroundTexture;
    }



    protected void initGUI()
    {
        if (this.firstrun)
        {
            createStyles();
            this.firstrun = false;

        }
    }



    /// <summary>
    /// Draws this healthbar on screen. Only draws if this healthbar has been configured.
    /// </summary>
    /// <param name="text">String: The text to draw onto the screen</param>
    /// <param name="drawDescription">Boolean(true)Should the Descriptiontext be drawn</param>
    /// <param name="noText">Boolean(false)Should any text be drawn</param>
    public void draw(string text, bool drawDescription = true, bool noText = false)
    {
        if (this.isConfigured())
        {
            this.healthPercentage = this.currentHealth()/this.maxHealth();


            this.updateStyles();

            this.bar.width = this.Width * this.currentHealth() / this.maxHealth();

            GUI.Box(this.healthBarBox_outer,"",this.healthBarBoxStyle_outer);
            GUI.Box(this.healthBarBox_inner,"",this.healthBarBoxStyle_inner);
            GUI.Box(this.bar,"", this.healthBarStyle);
            if (drawDescription)
            {
                text = text + this.currentHealth() + "/" + this.maxHealth();
            } else if (noText)
            {
                text = "";
            } else
            {
                text = this.currentHealth() + "/" + this.maxHealth();
            }
            if (State.getState (this._transform).TargetState == StateName.Targetted)
            {
                GUI.Box(this.healthBarBox_inner,text,this.textStyleTargetted);
            } else
            {
                GUI.Box(this.healthBarBox_inner,text,this.textStyleNormal);
            }
        }

    }
}

