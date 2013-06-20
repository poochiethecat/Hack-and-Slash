using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth = 100;




    public PlayerHealthBar playerHealthBar;
    public EnemyHealthBar enemyHealthBar;
    private HealthBar healthbar;
    private Transform _transform;



    // Use this for initialization
    void Start ()
    {
       _transform = transform;
       switch(_transform.tag)
       {
        case "Player":
            healthbar = playerHealthBar;
            break;
        default:
            healthbar = enemyHealthBar;
            break;
       }

       healthbar.setHealthAndTransform(this,_transform);
       healthbar.configure(transform: transform, currentHealth: ()=>100, maxHealth: () =>  100);

    }

    void OnGUI()
    {
        healthbar.OnGUI();
    }




    // Update is called once per frame
    void Update ()
    {
        AdjustCurrentHealth (0);
    }


    public void AdjustCurrentHealth (int adj)
    {
        curHealth += adj;
        if (curHealth > maxHealth)
            curHealth = maxHealth;
        if (curHealth < 1){
            Targeting targeting = GameObject.FindGameObjectWithTag("Player").GetComponent<Targeting>();
            targeting.RemoveTarget(_transform);
            Destroy(gameObject);
        }
    }
}
