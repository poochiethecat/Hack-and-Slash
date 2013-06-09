using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
    public Texture crosshair;
    
    public int crosshairSize = 80;
   
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        

	}
    
    void OnGUI(){
        GUI.DrawTexture(new Rect(Screen.width /2-crosshairSize/2, Screen.height / 2-crosshairSize/2, crosshairSize, crosshairSize), crosshair);
 
    }
}
