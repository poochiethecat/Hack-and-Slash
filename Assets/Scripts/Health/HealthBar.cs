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

    public Rect healthBox;
    private Rect drawing_bar;
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
    public Color criticalHealth = Color.red;



    // Style for the border/outer box
    protected GUIStyle borderStyle;
    // Style for the background/inner box
    protected GUIStyle backgroundStyle;



    protected Transform _transform;

    // Rectangle of the healthbar, gets smaller with change in health

    // Rectangle of the max healthbar
    protected Rect backgroundBox;
    // Rectangle for the border, is healthBarBorder pixels bigger in all directions than the inner box
    protected Rect borderBox;


    // Background Texture, visible if health < 100%
    protected Texture2D backgroundTexture ;
    // Border Texture, this color takes the border
    protected Texture2D borderTexture;
    // transparent texture for the background of the text
    protected Texture2D clearTexture ;


    private Texture2D _lastHealthTexture;



    private float _lastHealth;

    private bool firstrun;


    private LABColor _fullHealth;
    private LABColor _critHealth;


    private float _healthPercentage;


    private Func<float> maxHealth;
    private Func<float> currentHealth;


    private float CurrentHealth {
        get {
            float health = currentHealth();
            if (this._lastHealth != health)
            {
                this._lastHealth = health;
                this._lastHealthTexture = this.healthTexture();
            }
            return _lastHealth;
        }
    }
    private float MaximumHealth {
        get {
            return maxHealth();
        }
    }

    private bool Configured {
        get {
            return this.isConfigured();
        }
    }

    private float HealthPercentage {
        get {
            this._healthPercentage = this.CurrentHealth/this.MaximumHealth;
            return this._healthPercentage;
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
        this.firstrun = true;
    }

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

        float healthPercentage = this.HealthPercentage;

        // Reach 0 a bit faster than standard
        if (healthPercentage < cutoffPercentage)
            healthPercentage = 0;
        double newLightness = this._critHealth.l - healthPercentage*(this._critHealth.l - this._fullHealth.l);
        double newA = this._critHealth.a -healthPercentage*(this._critHealth.a - this._fullHealth.a);
        double newB = this._critHealth.b - healthPercentage*(this._critHealth.b - this._fullHealth.b);
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
        this.backgroundStyle = new GUIStyle();
        this.backgroundStyle.normal.background = this.backgroundTexture;
        this.backgroundStyle.stretchWidth = false;
        this.backgroundStyle.normal.textColor = Color.gray;

        this.borderStyle = new GUIStyle();
        this.borderStyle.normal.background = this.borderTexture;
        this.borderStyle.stretchWidth = false;

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
        this.healthBarStyle.normal.background = this._lastHealthTexture;
        this.borderStyle.normal.background = this.borderTexture;
        this.backgroundStyle.normal.background = this.backgroundTexture;
    }



    protected void initGUI()
    {
        if (this.firstrun)
        {
            this.initTextures();
            this.createStyles();
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
        if (this.Configured)
        {
            this.initGUI();
            this.healthBox.width  = this.backgroundBox.width*this.HealthPercentage;
            this.updateStyles();



            GUI.Box(this.borderBox,"",this.borderStyle);
            GUI.Box(this.backgroundBox,"",this.backgroundStyle);
            GUI.Box(this.healthBox,"", this.healthBarStyle);
            if (drawDescription)
            {
                text = text + this.CurrentHealth + "/" + this.MaximumHealth;
            } else if (noText)
            {
                text = "";
            } else
            {
                text = this.currentHealth() + "/" + this.maxHealth();
            }
            if (State.getState (this._transform).TargetState == StateName.Targetted)
            {
                GUI.Box(this.backgroundBox,text,this.textStyleTargetted);
            } else
            {
                GUI.Box(this.backgroundBox,text,this.textStyleNormal);
            }
        }

    }
}

