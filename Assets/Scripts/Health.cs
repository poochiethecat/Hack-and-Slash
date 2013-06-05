using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    
    public int maxHealth = 100;
    public int curHealth = 100;
    
    public bool showHealthBar = false;
    
    public FontStyle fontStyle;
    
    private Transform myTransform;
    
    private Rect healthBar;
    
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
        
        healthBar = new Rect(10, 0, Screen.width / 2, 20);
        
        switch (tag) {
        case "Player":
            healthBar.y = 10;
            showHealthBar = true;
            break;
        case "Enemy":
            healthBar.y = 40;
//            Debug.Log("Enemy Health Bar set for " + myTransform.name);
//            Targeting targeting = (Targeting)GameObject.FindGameObjectWithTag("Player").GetComponent("Targeting");
//            if(targeting != null)
//                healthBar.y = 40 + (30 * targeting.targets.IndexOf(myTransform));
            break;
        }
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
        
        if(showHealthBar) GUI.Box(healthBar, myTransform.name + " = " + curHealth + "/" + maxHealth);

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
