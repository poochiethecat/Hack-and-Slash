using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {
    
    private StateName _state;
    
    public StateName state {
        get {
            return this._state;
        }
        set {
            _state = value;
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
    
    public void target()
    {
        state = StateName.targetted;
    }
    public void untarget()
    {
        state = StateName.normal;
    }
    
    

}

public enum StateName {
    normal,
    targetted,
    visible,
}
