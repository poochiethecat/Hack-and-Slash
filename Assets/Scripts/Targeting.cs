using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeting : MonoBehaviour {
    public List<Transform> targets;
    public Transform selectedTarget;
    
    public Color targetingColor = Color.red;
    
    public int targetingDistance = 30;
    

   
    
    private Transform myTransform;

    // Use this for initialization
    void Start () {
        targets = new List<Transform>();
        
        selectedTarget = null;
        
        myTransform = transform;
        
        AddAllEnemies();
        sortTargetsByDistance();
        Debug.Log("Enemies added to targeting system");
        
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
        SelectTarget(targets[index]);
    }
    
    private void SelectTarget(Transform t){
        selectedTarget = t;
        State.getState(selectedTarget).target(targetingColor);
    }
    
    private void DeselectTarget(){
        if (selectedTarget != null)
        {
            State.getState(selectedTarget).untarget();
            selectedTarget = null;
        }
    }
    
    public void RemoveTarget(Transform t){
        DeselectTarget();
        targets.Remove(t);
    }
    
    // Update is called once per frame
    void Update () {
        RaycastHit  hit;
        // Raycast to find out what object the crosshair hits.
        if (Physics.Raycast(myTransform.position,myTransform.forward, out hit)) 
        {
//            Debug.Log("Targeting: " + hit.transform.name);
            if (hit.transform != selectedTarget)
            {
                DeselectTarget();
                SelectTarget(hit.transform);
            }
        } else {
            DeselectTarget();
        }
        
//        if(Input.GetKeyDown(KeyCode.Tab)) TargetEnemy();
//        if (selectedTarget != null && Input.GetKeyDown(KeyCode.Escape)) DeselectTarget();
    
    }
}
