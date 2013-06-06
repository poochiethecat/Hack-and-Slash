using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeting : MonoBehaviour {
    public List<Transform> targets;
    public Transform selectedTarget;
    
    public Color defaultColor;
    
    private Transform myTransform;

    // Use this for initialization
    void Start () {
        targets = new List<Transform>();
        
        selectedTarget = null;
        
        myTransform = transform;
        
        AddAllEnemies();
        sortTargetsByDistance();
        Debug.Log("Enemies added to targeting system");
        
        defaultColor = targets[0].renderer.material.color;
    }
    
    public void AddAllEnemies(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
            AddTarget(enemy.transform);
    }
    
    public void AddTarget(Transform t){
        targets.Add(t);
    }
    
    private void sortTargetsByDistance(){
        targets.Sort(delegate(Transform t1, Transform t2){ 
            return Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position));
        });
    }
    
    private void TargetEnemy(){
        
        int index = 0;
        
        if(selectedTarget == null){
            sortTargetsByDistance();
            index = 0;
        }else{
            index = targets.IndexOf(selectedTarget);
            if(index < targets.Count - 1) index++;
            else index = 0;
            DeselectTarget();
        }
        selectedTarget = targets[index];
        SelectTarget();
    }
    
    private void SelectTarget(){
        selectedTarget.renderer.material.color = Color.red;
        Health health = (Health)selectedTarget.GetComponent("Health");
        health.fontStyle = FontStyle.BoldAndItalic;
        health.showHealthBar = true;
    }
    
    private void DeselectTarget(){
        selectedTarget.renderer.material.color = defaultColor;
        Health health = (Health)selectedTarget.GetComponent("Health");
        health.fontStyle = FontStyle.Normal;
        health.showHealthBar = false;
        selectedTarget = null;
    }
    
    public void RemoveTarget(Transform t){
        DeselectTarget();
        targets.Remove(t);
    }
    
    // Update is called once per frame
    void Update () {
        
        if(Input.GetKeyDown(KeyCode.Tab)) TargetEnemy();
        if(Input.GetKeyDown(KeyCode.Escape)) DeselectTarget();
    
    }
}
