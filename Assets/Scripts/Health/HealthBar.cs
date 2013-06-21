using UnityEngine;
using System;
[System.Serializable]
public abstract class HealthBar {


    /*
    * Attributes configurable through Unity
    */

    [RangeAttributeWithDefault(50, 1000,300)]
    public int maxWidth = 300;
    [RangeAttributeWithDefault(5, 200, 100)]
    public int maxVisibleDistance = 100;
    [RangeAttributeWithDefault(1,100,50)]
    public int minWidth = 50;
 
    
    //TODO: Really set the maximum size with this, then we do not need maxWidth up there.
    
    ///<summary>
    /// The Total size of the Healthbar
    ///</summary>
    public Rect healthRect;

    ///<summary>
    /// The border around the healthbar
    ///</summary>
    public RectOffset barBorder;

    public double cutoffPercentage = 0.2; // Below this ratio of health, the bar will be red


    ///<summary>
    /// The color chosen in the unity GUI, used for the background
    ///</summary>
    public Color backgroundColor = Color.cyan;

    ///<summary>
    /// The color chosen in the unity GUI, used for the broder
    ///</summary>
    public Color borderColor = Color.black;

    ///<summary>
    /// The color chosen in the unity GUI, used for the full Health
    ///</summary>
    public Color fullHealthColor = Color.green;

    ///<summary>
    /// The color chosen in the unity GUI, used for the critical Health
    ///</summary>
    public Color criticalHealthColor = Color.red;





    /*
    * Protected Variables, things used in subclasses like Textures, styles, the transform, the computed rectangles.
    */

    protected Transform _transform;

    /// <summary>
    /// The current text style.
    /// </summary>
    protected Func<GUIStyle> textStyle;

    /// <summary>
    /// Style for the border rect
    /// </summary>
    protected GUIStyle borderStyle;
    /// <summary>
    /// Style for the background rect
    /// </summary>
    protected GUIStyle backgroundStyle;
    /// <summary>
    /// Style of the bar-part of the healthbar
    /// </summary>
    protected GUIStyle healthBarStyle;

    /// <summary>
    /// The background rect
    /// </summary>
    protected Rect backgroundRect;
    /// <summary>
    /// The border rect, has the maximum size of the healthbar.
    /// </summary>
    protected Rect borderRect;


    /// <summary>
    /// Background Texture, visible if health < 100%
    /// </summary>
    protected Texture2D backgroundTexture ;
    
    /// <summary>
    /// Border Texture, this color takes the border
    /// </summary>
    protected Texture2D borderTexture;
    
    /// <summary>
    ///  transparent texture for the background of the text
    /// </summary>
    protected Texture2D clearTexture ;



    /*
    * Private Variables, used for saving the state of colors.
    */

    private Color _lastBackgroundColor = Color.cyan;
    private Color _lastBorderColor = Color.black;
    private Color _lastFullHealthColor = Color.green;
    private Color _lastCriticalHealthColor = Color.red;
    private Texture2D _lastHealthTexture;
    private float _lastHealth;
    private bool firstrun;
    private float _healthPercentage;
    private Func<float> maxHealth;
    private Func<float> currentHealth;


    /*
    * Getters for computed variables
    */

    protected float CurrentHealth
    {
        get {
            float health = currentHealth();
            if (this._lastHealth != health)
            {
                this._lastHealth = health;
            }
            return _lastHealth;
        }
    }
    protected float MaximumHealth
    {
        get {
            return maxHealth();
        }
    }

    protected float HealthPercentage
    {
        get {
            this._healthPercentage = this.CurrentHealth/this.MaximumHealth;
            return this._healthPercentage;
        }
    }

    protected bool Configured
    {
        get {
            return this.isConfigured();
        }
    }

    protected Color FullHealth
    {
        get {
            return getGUIColor("fullHealth");

        }

    }
    protected Color CriticalHealth
    {
        get {
            return getGUIColor("criticalHealth");
        }

    }
    protected Texture2D BorderTexture
    {
        get {
            if (this.borderTexture == null || !this.colorsEqual(this.borderColor, this._lastBackgroundColor))
            {
                this._lastBorderColor = this.borderColor;
                this.borderTexture = ColoredTexture.generatePixel(this._lastBorderColor);
            }
            return this.borderTexture;
        }
    }

    protected Texture2D BackgroundTexture
    {
        get {
        if (this.backgroundTexture == null || !this.colorsEqual(this.backgroundColor, this._lastBackgroundColor))
        {
            this._lastBackgroundColor = this.backgroundColor;
            this.backgroundTexture = ColoredTexture.generatePixel(this._lastBackgroundColor);
        }
            return this.backgroundTexture;
        }
    }



    /*
    * Public Functions
    */

    public abstract void OnGUI();

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
            if (this.firstrun)
            {
                // GUI-Functions can only be called during GUI calls, so we call it in here.
                this.initGUI();
                this.firstrun = false;
            }

            this.update();

            GUI.Box(this.borderRect,"",this.borderStyle);
            GUI.Box(this.backgroundRect,"",this.backgroundStyle);
            GUI.Box(this.healthRect,"", this.healthBarStyle);
            if (drawDescription)
            {
                text = text + this.CurrentHealth + "/" + this.MaximumHealth;
            } else if (noText)
            {
                text = "";
            } else
            {
                text = this.CurrentHealth + "/" + this.MaximumHealth;
            }
            GUI.Box(this.backgroundRect,text,this.textStyle());
        }
    }
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


    /*
    * Initialization
    */

    protected void initGUI()
    {
        this.initTextures();
        this.createStyles();
    }

    private void initTextures()
    {
        this.backgroundTexture = ColoredTexture.generatePixel(this.backgroundColor);
        this.borderTexture = ColoredTexture.generatePixel(this.borderColor);
        this.clearTexture = ColoredTexture.generatePixel(Color.clear);
    }

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


    }


    /*
    * Helper Functions
    */
    private bool isConfigured()
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

    private bool healthChanged()
    {
        return this._lastHealth != currentHealth();
    }

    private bool healthColorChanged()
    {
        return !this.colorsEqual(this.criticalHealthColor,this._lastCriticalHealthColor) || !this.colorsEqual(this.fullHealthColor,this._lastFullHealthColor);
    }


    private Color getGUIColor(String which)
    {
        Color returnvalue = Color.red;
        switch (which)
        {
            case "fullHealth":
                if (!this.colorsEqual(this.fullHealthColor,this._lastFullHealthColor))
                    this._lastFullHealthColor = this.fullHealthColor;
                returnvalue =  this._lastFullHealthColor;
                break;
            case "criticalHealth":
                if (!this.colorsEqual(this.criticalHealthColor,this._lastCriticalHealthColor))
                    this._lastCriticalHealthColor = this.criticalHealthColor;
                returnvalue = this._lastCriticalHealthColor;
                break;
        }
        return returnvalue;
    }

    protected Texture2D HealthTexture
    {
        get {
            if (this.healthChanged() || this.healthColorChanged())
                this._lastHealthTexture = this.generateHealthTexture();
            return this._lastHealthTexture;
        }
    }

    private bool colorsEqual(Color a, Color b, bool debug = false)
    {
        return Mathf.Approximately(a.r, b.r) && Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.g) && Mathf.Approximately(a.a,b.a);
    }

    /// <summary>
    /// Generates the health texture with a color based on the current HealthPercentage, it chooses linearly a color between FullHealth and CrititcalHealth
    /// </summary>
    /// <returns>
    /// A 1x1 Texture with the computed color.
    /// </returns>
    Texture2D generateHealthTexture()
    {

        // Use LAB-Colors to find a linear path between any two colors in the human colorspace.
        LABColor _fullHealth = new LABColor(this.FullHealth);
        LABColor _critHealth = new LABColor(this.CriticalHealth);

        float healthPercentage = this.HealthPercentage;


        // Reach 0 a bit faster than standard
        if (healthPercentage < cutoffPercentage)
            healthPercentage = 0;
        double newLightness = _critHealth.l - healthPercentage*(_critHealth.l - _fullHealth.l);
        double newA = _critHealth.a -healthPercentage*(_critHealth.a - _fullHealth.a);
        double newB = _critHealth.b - healthPercentage*(_critHealth.b - _fullHealth.b);
        LABColor targetColor = new LABColor((float)newLightness, (float)newA, (float)newB);
        return ColoredTexture.generatePixel(targetColor.ToColor());
    }

    /*
    * Update Functions
    */

    private void update()
    {
        this.updateStyles();
    }

    private void updateStyles()
    {
        this.healthBarStyle.normal.background = this.HealthTexture;
        this.borderStyle.normal.background = this.BorderTexture;
        this.backgroundStyle.normal.background = this.BackgroundTexture;
    }












}

