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

    // Rectangle of the healthbar, gets smaller with change in health
    public Rect healthBox;
    public RectOffset barBorder;


    // Style of the bar-part of the healthbar
    protected    GUIStyle healthBarStyle;
    // style for the text

    // TODO: Create protected delegate for these so that they can be configured only in the subclasses that need them.
    public GUIStyle textStyleNormal;
    public GUIStyle textStyleTargetted;

    public double cutoffPercentage = 0.2; // Below this ratio of health, the bar will be red



    public Color background = Color.cyan;
    public Color border = Color.black;
    ///<summary>
    /// The colors chosen in the unity GUI, do not use these directly.
    ///</summary>
    public Color fullHealth = Color.green;
    public Color criticalHealth = Color.red;





    /*
    * Protected Variables, things used in subclasses like Textures, styles, the transform, the computed rectangles.
    */

    protected Transform _transform;

    // Style for the border/outer box
    protected GUIStyle borderStyle;
    // Style for the background/inner box
    protected GUIStyle backgroundStyle;


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



    /*
    * Private Variables, used for saving the state of colors.
    */

    private Color _lastBackground = Color.cyan;
    private Color _lastBorder = Color.black;
    private Color _lastFullHealth = Color.green;
    private Color _lastCriticalHealth = Color.red;
    private Texture2D _lastHealthTexture;
    private float _lastHealth;
    private bool firstrun;
    private float _healthPercentage;
    private Func<float> maxHealth;
    private Func<float> currentHealth;


    /*
    * Getters for computed variables
    */

    protected float CurrentHealth {
        get {
            float health = currentHealth();
            if (this._lastHealth != health)
            {
                this._lastHealth = health;
                updateTextures(true);
            }
            return _lastHealth;
        }
    }
    protected float MaximumHealth {
        get {
            return maxHealth();
        }
    }

    protected float HealthPercentage {
        get {
            this._healthPercentage = this.CurrentHealth/this.MaximumHealth;
            return this._healthPercentage;
        }
    }

    protected bool Configured {
        get {
            return this.isConfigured();
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
                text = this.CurrentHealth + "/" + this.MaximumHealth;
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
        this.backgroundTexture = ColoredTexture.generatePixel(this.background);
        this.borderTexture = ColoredTexture.generatePixel(this.border);
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

        this.textStyleNormal = new GUIStyle(this.healthBarStyle);
        this.textStyleNormal.stretchWidth = false;
        this.textStyleNormal.normal.background = this.clearTexture;
        this.textStyleNormal.fontStyle = FontStyle.Normal;

        this.textStyleTargetted = new GUIStyle(this.healthBarStyle);
        this.textStyleTargetted.stretchWidth = false;
        this.textStyleTargetted.normal.background = this.clearTexture;
        this.textStyleTargetted.fontStyle = FontStyle.BoldAndItalic;

    }


    /*
    * Helper Functions
    */
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

    private bool colorsEqual(Color a, Color b)
    {
        bool cr = Mathf.Approximately(a.r, b.r);
        bool cg = Mathf.Approximately(a.g, b.g);
        bool cb = Mathf.Approximately(a.b, b.b);
        bool ca = Mathf.Approximately(a.a, b.a);
        bool complete = cr && cg && cb && ca;
        bool direct = Mathf.Approximately(a.r, b.r) && Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.g) && Mathf.Approximately(a.a,b.a);
        return direct;
        //return
    }

    // Returns a 1x1 Texture with a color based on the current Health of the entity.
    Texture2D healthTexture()
    {

        // Use LAB-Colors to find a linear path between any two colors in the human colorspace.
        LABColor _fullHealth = new LABColor(this._lastFullHealth);
        LABColor _critHealth = new LABColor(this._lastCriticalHealth);

        float healthPercentage = this.HealthPercentage;


        // Reach 0 a bit faster than standard
        if (healthPercentage < cutoffPercentage)
            healthPercentage = 0;
        double newLightness = _critHealth.l - healthPercentage*(_critHealth.l - _fullHealth.l);
        double newA = _critHealth.a -healthPercentage*(_critHealth.a - _fullHealth.a);
        double newB = _critHealth.b - healthPercentage*(_critHealth.b - _fullHealth.b);
        LABColor targetColor = new LABColor((float)newLightness, (float)newA, (float)newB);
        // Debug.Log(targetColor.ToColor());
        return ColoredTexture.generatePixel(targetColor.ToColor());
    }

    /*
    * Update Functions
    */

    private void update()
    {
        this.updateTextures();
        this.updateStyles();
    }

    private void updateStyles()
    {
        this.healthBarStyle.normal.background = this._lastHealthTexture;
        this.borderStyle.normal.background = this.borderTexture;
        this.backgroundStyle.normal.background = this.backgroundTexture;
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


    private Color getGUIColor(String which)
    {
        bool criticalHealthEqual = this.colorsEqual(this.criticalHealth,this._lastCriticalHealth);
        bool fullHealthEqual = this.colorsEqual(this.fullHealth,this._lastFullHealth);
        Color returnvalue = Color.red;
        switch (which)
        {
            case "fullHealth":
                if (!fullHealthEqual || this._lastHealthTexture == null)
                    this._lastFullHealth = this.fullHealth;
                returnvalue =  this._lastFullHealth;
                break;
            case "criticalHealth":
                if (!criticalHealthEqual || this._lastHealthTexture == null)
                    this._lastCriticalHealth = this.criticalHealth;
                returnvalue = this._lastCriticalHealth
                break;
        }
        if (forceHealth || !fullHealthEqual || !criticalHealthEqual)
            this._lastHealthTexture = this.healthTexture();
        return Color.red;
    }


    private void updateTextures()//bool forceHealth = false)
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
        // bool criticalHealthEqual = this.colorsEqual(this.criticalHealth,this._lastCriticalHealth);
        // bool fullHealthEqual = this.colorsEqual(this.fullHealth,this._lastFullHealth);
        // if (!fullHealthEqual || this._lastHealthTexture == null)
        //     this._lastFullHealth = this.fullHealth;
        // if (!criticalHealthEqual || this._lastHealthTexture == null)
        //     this._lastCriticalHealth = this.criticalHealth;

    }





}

