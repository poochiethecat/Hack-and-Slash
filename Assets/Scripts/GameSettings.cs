using UnityEngine;
using System.Collections;
using System;

public class GameSettings : MonoBehaviour {
 
    
    void Awake(){
        DontDestroyOnLoad(this);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
    
    public void SaveCharacterData(){
        GameObject pc = GameObject.Find ("PC");
        
        PlayerCharacter pcClass = pc.GetComponent<PlayerCharacter>();
        Debug.LogWarning("WARNING: PLAYERPREFS ARE RESET WITH DELETALL!"); PlayerPrefs.DeleteAll();
        
        PlayerPrefs.SetString("Player Name", pcClass.Name);
        
        for(int i = 0; i < Enum.GetValues(typeof(AttributeName)).Length; i++){
            PlayerPrefs.SetInt(((AttributeName)i).ToString() + " - Base Stat", pcClass.GetPrimaryAttribute(i).BaseValue);
            PlayerPrefs.SetInt(((AttributeName)i).ToString() + " - ExpToLevel", pcClass.GetPrimaryAttribute(i).ExpToLevel);
            PlayerPrefs.SetFloat(((AttributeName)i).ToString() + " - Level Mod", pcClass.GetPrimaryAttribute(i).LevelModifier);
        }
        
        for(int i = 0; i < Enum.GetValues(typeof(VitalName)).Length; i++){
            PlayerPrefs.SetInt(((VitalName)i).ToString() + " - Base Stat", pcClass.GetVital(i).BaseValue);
            PlayerPrefs.SetInt(((VitalName)i).ToString() + " - ExpToLevel", pcClass.GetVital(i).ExpToLevel);
            PlayerPrefs.SetFloat(((VitalName)i).ToString() + " - Level Mod", pcClass.GetVital(i).LevelModifier);
            PlayerPrefs.SetInt(((VitalName)i).ToString() + " - Cur Value", pcClass.GetVital(i).CurVal);
            PlayerPrefs.SetString(((VitalName)i).ToString() + " - Modifiers", pcClass.GetVital(i).GetModifyingAttributesString());
        }
        
        for(int i = 0; i < Enum.GetValues(typeof(SkillName)).Length; i++){
            PlayerPrefs.SetInt(((SkillName)i).ToString() + " - Base Stat", pcClass.GetSkill(i).BaseValue);
            PlayerPrefs.SetInt(((SkillName)i).ToString() + " - ExpToLevel", pcClass.GetSkill(i).ExpToLevel);
            PlayerPrefs.SetFloat(((SkillName)i).ToString() + " - Level Mod", pcClass.GetSkill(i).LevelModifier);
            PlayerPrefs.SetString(((SkillName)i).ToString() + " - Modifiers", pcClass.GetSkill(i).GetModifyingAttributesString());
        }
    }
    
    public void LoadCharacterData(){
        
    }
}
