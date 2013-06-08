using UnityEngine;
using System.Collections;

public class RenderHelper : MonoBehaviour {
    
    
    private Color defaultColor;
    
    
    private Transform myTransform;

	// Use this for initialization
	void Start () {
	    myTransform = transform;
        
        defaultColor = transform.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
        State mystate = State.getState(myTransform);
         if (mystate.TargetState == StateName.Targetted)
        {
            myTransform.renderer.material.color = mystate.color;
        }
        else {
            myTransform.renderer.material.color = defaultColor;
        }
	
	}
    
    void OnBecameVisible()
    {
        
        Debug.Log("I just became visible: "+myTransform.name);
        State.getState(myTransform).ScreenState = StateName.OnScreen;
        
    }
    
    void OnBecameInvisible()
    {
        Debug.Log("I just became invisible: "+myTransform.name); 
        State.getState(myTransform).ScreenState = StateName.OffScreen;
    }
}
