using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth = 100;

    public bool showHealthBar = false;

    public FontStyle fontStyle;


    // Style of the bar-part of the healthbar
    private GUIStyle healthBarStyle;
    // style for the text
    private GUIStyle healthBarStyleText;
    // Style for the border/outer box
    private GUIStyle healthBarBoxStyle_outer = null;
    // Style for the background/inner box
    private GUIStyle healthBarBoxStyle_inner = null;

    private Transform myTransform;

    // Rectangle of the healthbar, gets smaller with change in health
    private Rect healthBar;
    // Rectangle of the max healthbar
    private Rect healthBarBox_inner;
    // Rectangle for the border, is healthBarBorder pixels bigger in all directions than the inner box
    private Rect healthBarBox_outer;
    // Border size around the health
    private float healthBarBorder = 1.0f;


    // Background Texture, visible if health < 100%
    private Texture2D backgroundTexture;
    // Border Texture, this color takes the border
    private Texture2D borderTexture;
    // transparent texture for the background of the text
    private Texture2D clearTexture;

    private Font font;
    
    private bool firstrun = true;


    // Initializes border and background textures.
    private void initTextures()
    {
        backgroundTexture = ColoredTexture.generatePixel(g: 255, b: 255);
        borderTexture = ColoredTexture.generatePixel();
        clearTexture = ColoredTexture.generatePixel(a: 0);
    }
 
    // Returns a Texture with a color based on the current Health of the entity.
    Texture2D healthTexture()
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


    // Use this for initialization
    void Start ()
    {
        initTextures();
       myTransform = transform;

        fontStyle = FontStyle.Normal;

        int width = Screen.width/2;
        healthBar = new Rect(10, 0,width, 20);

        switch (tag) {
        case "Player":
            healthBar.y = 10;
            showHealthBar = true;
            break;
        case "Enemy":
            healthBar.y = 60;
//            Debug.Log("Enemy Health Bar set for " + myTransform.name);
//            Targeting targeting = (Targeting)GameObject.FindGameObjectWithTag("Player").GetComponent("Targeting");
//            if(targeting != null)
//                healthBar.y = 40 + (30 * targeting.targets.IndexOf(myTransform));
            break;
        }
        healthBarBox_inner = new Rect(healthBar.x, healthBar.y, healthBar.width, healthBar.height);
        healthBarBox_outer = new Rect(healthBar.x-healthBarBorder, healthBar.y-healthBarBorder, healthBar.width+2*healthBarBorder, healthBar.height+2*healthBarBorder);

    }




    // Update is called once per frame
    void Update ()
    {
        AdjustCurrentHealth (0);
    }

    void updateStyles()
    {
        //healthBarStyle.normal.background = ColoredTextureGenerator.generatePixel( r: 1-healthPercentage, g: healthPercentage ,b: 0);
        healthBarStyle.normal.background = healthTexture();
    }


    void OnGUI ()
    {

        if (firstrun)
        {
            firstrun = false;
            createStyles();
        }
        //GUI.skin.box.fontStyle = fontStyle;

        if(showHealthBar)
        {
             updateStyles();
//            float healthPercentage = ((float)curHealth)/((float)maxHealth);
//           

            GUI.Box(healthBarBox_outer,"",healthBarBoxStyle_outer);
            GUI.Box(healthBarBox_inner,"",healthBarBoxStyle_inner);
            GUI.Box(healthBar,"", healthBarStyle);
            GUI.Box(healthBarBox_inner,myTransform.name + " = " + curHealth + "/" + maxHealth,healthBarStyleText);


            //GUI.Box(healthBarBoxBigger,"",healthBarBoxStyle2);
        }

    }

    public void AdjustCurrentHealth (int adj)
    {
        curHealth += adj;
        if (curHealth > maxHealth)
            curHealth = maxHealth;
        if (curHealth < 1){
            Targeting targeting = (Targeting)GameObject.FindGameObjectWithTag("Player").GetComponent("Targeting");
            targeting.RemoveTarget(myTransform);
            Destroy(gameObject);
        }

//            curHealth = 0;
        healthBar.width = (Screen.width / 2) * (curHealth / (float)maxHealth);
    }
}
