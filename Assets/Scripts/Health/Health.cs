using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int curHealth = 100;

    public ScaledCurve curve;



    public HealthBar healthbar;
    private Transform _transform;



    // Use this for initialization
    void Start ()
    {
       _transform = transform;
       switch(_transform.tag)
       {
        case "Player":
            healthbar = new PlayerHealthBar(healthbar,this, _transform);
            break;
        default:
            healthbar = new EnemyHealthBar(healthbar,this, _transform);
            break;
       }

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
