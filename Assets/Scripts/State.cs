using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {
    
    private StateName _targetState;
    private StateName _screenState;
    private Color _color;
    
    private Transform myTransform;
    
    public StateName TargetState {
        get {
            return this._targetState;
        }
        set {
            _targetState = value;
        }
    }

    public StateName ScreenState {
        get {
            return this._screenState;
        }
        set {
            _screenState = value;
        }
    }
    public Color color {
        get {
            return this._color;
        }
        set {
            _color = value;
        }
    }
	// Use this for initialization
	void Start () {
        State.unpause ();
        switch (tag) {
            case "Player":
                TargetState = StateName.Visible;
                break;
            case "Enemy":
                TargetState = StateName.Normal;
                break;
        }
          
	
	}
	
	// Update is called once per frame
	void Update () {
        if (tag == "Player")
        {
            
//            MouseLook look = GetComponent<MouseLook>();
//            MouseLook camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
            
            if (Input.GetKeyUp(KeyCode.Pause))
            {   
                State.pauseToggle();
                
            }
        }
	
	}
    
    private static void pauseHelper()
    {
        if (Time.timeScale == 0)
            Screen.lockCursor = false;
        else
            Screen.lockCursor = true;
        
        foreach (MouseLook look in GameObject.FindObjectsOfType(typeof(MouseLook)))
            look.enabled = Screen.lockCursor;
    }
    
    public static void pauseToggle()
    {
        Time.timeScale = -Time.timeScale+1;
        State.pauseHelper();
    }
    
    public static void pause()
    {
        Time.timeScale = 0;
        State.pauseHelper();

    }
    public static void unpause()
    {
        Time.timeScale = 1;
        State.pauseHelper();
    }
    
    public void target(Color color)
    {
        TargetState = StateName.Targetted;
        this.color = color;
        
    }
    public void untarget()
    {
        TargetState = StateName.Normal;
    }
    
    public static State getState(Transform entity)
    {
        return entity.GetComponent<State>();
    }
    

}

public enum StateName {
    Normal,
    Targetted,
    Visible,
    OnScreen,
    OffScreen
}
