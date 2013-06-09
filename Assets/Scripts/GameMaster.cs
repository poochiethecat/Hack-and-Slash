using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
 
    public GameObject playerCharacter;
    public GameObject gameSettings;
    public Camera mainCamera;
    
    public float zOffset;
    public float yOffset;
    public float xRotOffset;
    
    private GameObject _pc;
    private PlayerCharacter _pcScript;
    
    // Use this for initialization
    void Start () {
        _pc = Instantiate(playerCharacter, new Vector3(0, 0, 6), Quaternion.identity) as GameObject;
        _pc.name = "PC";
        
        _pcScript = _pc.GetComponent<PlayerCharacter>();
        
        zOffset = -2.5f;
        yOffset = 2.5f;
        xRotOffset = 22.5f;
        
        mainCamera.transform.position = new Vector3(_pc.transform.position.x, _pc.transform.position.y + yOffset, _pc.transform.position.z + zOffset);
        mainCamera.transform.Rotate(xRotOffset, 0, 0);
        
        LoadCharacter();
        
    }
    
    // Update is called once per frame
    void Update () {
    
    }
    
    public void LoadCharacter(){
        GameObject gs = GameObject.Find("__GameSettings");
        if(gs == null){
            gs = Instantiate(gameSettings, Vector3.zero, Quaternion.identity) as GameObject;
            gs.name = "__GameSettings";
        }
        GameSettings gScript = GameObject.Find("__GameSettings").GetComponent<GameSettings>();
        
        gScript.LoadCharacterData();
    }
    
}
