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
    
    private Texture2D healthFull;
    private Texture2D healthDepleted;

    // Use this for initialization
    void Start ()
    {
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
    
    void OnGUI ()
    {
        
        //GUI.skin.box.fontStyle = fontStyle;
        
        if(showHealthBar) GUI.Box(healthBar, myTransform.name + " = " + curHealth + "/" + maxHealth);

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
