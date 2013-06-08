using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {
    
    private StateName _state;
    private Color _color;
    
    public StateName state {
        get {
            return this._state;
        }
        set {
            _state = value;
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
        switch (tag) {
            case "Player":
                state = StateName.visible;
                break;
            case "Enemy":
                state = StateName.normal;
                break;
        }
          
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void target(Color color)
    {
        state = StateName.targetted;
        this.color = color;
        
    }
    public void untarget()
    {
        state = StateName.normal;
    }
    
    public static State getState(Transform entity)
    {
        return (State)entity.GetComponent("State");
    }
    

}

public enum StateName {
    normal,
    targetted,
    visible,
}
