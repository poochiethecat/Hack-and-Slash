using UnityEngine;
using System;
[System.Serializable]
public abstract class HealthBar {


    /*
    * Attributes configurable through Unity
    */

    ///<summary>
    /// The Total size of the Healthbar
    ///</summary>
    public Rect maximumSize;

    ///<summary>
    /// The border around the healthbar
    ///</summary>
    public RectOffset padding;

    /// <summary>
    /// Blow this percentage, the critical health color will be displayed instead of a linearly computed color.
    /// </summary>
    [RangeAttribute(0f,1f)]
    public float cutoffPercentage = 0.2f; // Below this ratio of health, the bar will be red


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
    /// The border/padding rect, has the maximum size of the healthbar.
    /// </summary>
    protected Rect borderRect;

    /// <summary>
    /// The health rect.
    /// </summary>
    protected Rect healthRect;


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
            return this.IsConfigured();
        }
    }

    protected Color FullHealth
    {
        get {
            return GetGUIColor("fullHealth");

        }

    }
    protected Color CriticalHealth
    {
        get {
            return GetGUIColor("criticalHealth");
        }

    }
    protected Texture2D BorderTexture
    {
        get {
            if (this.borderTexture == null || !this.ColorsEqual(this.borderColor, this._lastBorderColor))
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
        if (this.backgroundTexture == null || !this.ColorsEqual(this.backgroundColor, this._lastBackgroundColor))
        {
            this._lastBackgroundColor = this.backgroundColor;
            this.backgroundTexture = ColoredTexture.generatePixel(this._lastBackgroundColor);
        }
            return this.backgroundTexture;
        }
    }

    protected Texture2D HealthTexture
    {
        get {
            if (this.HealthChanged() || this.HealthColorChanged())
                this._lastHealthTexture = this.GenerateHealthTexture();
            return this._lastHealthTexture;
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
                this.InitGUI();
                this.firstrun = false;
            }

            this.Update();

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
    public void Configure(Transform transform, Func<float> maxHealth, Func<float> currentHealth )
    {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this._transform = transform;
        this.firstrun = true;
    }


    /*
    * Initialization
    */

    protected void InitGUI()
    {
        this.InitTextures();
        this.CreateStyles();
    }

    private void InitTextures()
    {
        this.backgroundTexture = ColoredTexture.generatePixel(this.backgroundColor);
        this.borderTexture = ColoredTexture.generatePixel(this.borderColor);
        this.clearTexture = ColoredTexture.generatePixel(Color.clear);
    }

    private void CreateStyles()
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
    private bool IsConfigured()
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

    private bool HealthChanged()
    {
        return this._lastHealth != currentHealth();
    }

    private bool HealthColorChanged()
    {
        return !this.ColorsEqual(this.criticalHealthColor,this._lastCriticalHealthColor) || !this.ColorsEqual(this.fullHealthColor,this._lastFullHealthColor);
    }


    private Color GetGUIColor(String which)
    {
        Color returnvalue = Color.red;
        switch (which)
        {
            case "fullHealth":
                if (!this.ColorsEqual(this.fullHealthColor,this._lastFullHealthColor))
                    this._lastFullHealthColor = this.fullHealthColor;
                returnvalue =  this._lastFullHealthColor;
                break;
            case "criticalHealth":
                if (!this.ColorsEqual(this.criticalHealthColor,this._lastCriticalHealthColor))
                    this._lastCriticalHealthColor = this.criticalHealthColor;
                returnvalue = this._lastCriticalHealthColor;
                break;
        }
        return returnvalue;
    }



    private bool ColorsEqual(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) && Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.g) && Mathf.Approximately(a.a,b.a);
    }

    /// <summary>
    /// Generates the health texture with a color based on the current HealthPercentage, it chooses linearly a color between FullHealth and CrititcalHealth
    /// </summary>
    /// <returns>
    /// A 1x1 Texture with the computed color.
    /// </returns>
    Texture2D GenerateHealthTexture()
    {

        // Use LAB-Colors to find a linear path between any two colors in the human colorspace.
        LABColor _fullHealth = new LABColor(this.FullHealth);
        LABColor _critHealth = new LABColor(this.CriticalHealth);

        float healthPercentage = this.HealthPercentage;


        // Reach 0 a bit faster than standard
        if (healthPercentage < cutoffPercentage)
            return ColoredTexture.generatePixel(this.CriticalHealth);

        double newLightness = _critHealth.l - healthPercentage*(_critHealth.l - _fullHealth.l);
        double newA = _critHealth.a -healthPercentage*(_critHealth.a - _fullHealth.a);
        double newB = _critHealth.b - healthPercentage*(_critHealth.b - _fullHealth.b);
        LABColor targetColor = new LABColor((float)newLightness, (float)newA, (float)newB);
        return ColoredTexture.generatePixel(targetColor.ToColor());
    }

    /*
    * Update Functions
    */

    private void Update()
    {
        this.UpdateStyles();
        this.AdjustForPadding();
    }

    private void UpdateStyles()
    {
        this.healthBarStyle.normal.background = this.HealthTexture;
        this.borderStyle.normal.background = this.BorderTexture;
        this.backgroundStyle.normal.background = this.BackgroundTexture;
    }

    private void AdjustForPadding()
    {
        borderRect = new Rect(backgroundRect);
        healthRect = new Rect(
            this.healthRect.x+this.padding.left,
            this.healthRect.y+this.padding.top,
            this.healthRect.width-this.padding.right-this.padding.left,
            this.healthRect.height-this.padding.bottom-this.padding.top
        );
        backgroundRect = new Rect(
            this.backgroundRect.x+this.padding.left,
            this.backgroundRect.y+this.padding.top,
            this.backgroundRect.width-this.padding.right-this.padding.left,
            this.backgroundRect.height-this.padding.bottom-this.padding.top
        );
    }












}

