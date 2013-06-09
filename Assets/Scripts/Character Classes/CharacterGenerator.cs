using UnityEngine;
using System.Collections;
using System;

public class CharacterGenerator : MonoBehaviour {
    private PlayerCharacter _toon;
    private const int STARTING_POINTS = 350;
    private const int MIN_STARTING_ATTRIBUTE_VALUE = 10;
    private const int STARTING_ATTRIBUTE_VALUE = 50;
    private int _pointsLeft;
    
    //Variables for positioning
    private const int OFFSET = 10;
    private const int LINE_HEIGHT = 20;
    
    private const int STAT_LABEL_WIDTH = 100;
    private const int BASE_VALUE_LABEL_WIDTH = 30;
    private const int BUTTON_WIDTH = LINE_HEIGHT;
    private const int BUTTON_HEIGHT = LINE_HEIGHT;
    private const int BUTTON_OFFSET = OFFSET / 2;
    
    private int _statStartingPos = 40;

    // Use this for initialization
    void Start () {
        
        _toon = new PlayerCharacter();
        _toon.Awake();
        
        _pointsLeft = STARTING_POINTS;
        
        for (int i = 0; i < Enum.GetValues(typeof(AttributeName)).Length; i++) {
            _toon.GetPrimaryAttribute(i).BaseValue = STARTING_ATTRIBUTE_VALUE;
            _pointsLeft -= (STARTING_ATTRIBUTE_VALUE-MIN_STARTING_ATTRIBUTE_VALUE);
        }
        _toon.StatUpdate();
    }
    
    // Update is called once per frame
    void Update () {
    }
    
    void OnGUI(){
        DisplayName();
        DisplayAttributes();
        DisplayVitals();
        DisplaySkills();
        DisplayPointsLeft();
    }
    private void DisplayName(){
        GUI.Label(new Rect(OFFSET,OFFSET,50,LINE_HEIGHT), "Name:");
        _toon.Name = GUI.TextField(new Rect(OFFSET + 50, OFFSET, 100, LINE_HEIGHT), _toon.Name);
    }
    
    private void DisplayAttributes(){
        for(int i = 0; i < Enum.GetValues(typeof(AttributeName)).Length; i++){
            //Attribute Name
            GUI.Label(new Rect( OFFSET,
                                _statStartingPos + (i*LINE_HEIGHT),
                                STAT_LABEL_WIDTH,
                                LINE_HEIGHT
                    ), ((AttributeName)i).ToString());
            //Attribute Value
            GUI.Label(new Rect( STAT_LABEL_WIDTH + OFFSET,
                                _statStartingPos + (i*LINE_HEIGHT),
                                BASE_VALUE_LABEL_WIDTH,
                                LINE_HEIGHT
                             ), _toon.GetPrimaryAttribute(i).AdjustedBaseValue.ToString());
            //Minus-Button
            if(GUI.Button(new Rect( STAT_LABEL_WIDTH + OFFSET + BASE_VALUE_LABEL_WIDTH,
                                    _statStartingPos + (i*BUTTON_HEIGHT),
                                    BUTTON_WIDTH,
                                    BUTTON_HEIGHT
                        ), "-")){
                if(_toon.GetPrimaryAttribute(i).BaseValue > MIN_STARTING_ATTRIBUTE_VALUE){
                    _toon.GetPrimaryAttribute(i).BaseValue--;
                    _pointsLeft++;
                    _toon.StatUpdate();
                }
            }
            if(GUI.Button(new Rect( STAT_LABEL_WIDTH + OFFSET + BASE_VALUE_LABEL_WIDTH + BUTTON_WIDTH + BUTTON_OFFSET,
                                    _statStartingPos + (i*BUTTON_HEIGHT),
                                    BUTTON_WIDTH,
                                    BUTTON_HEIGHT
                                ), "+")){
                if(_pointsLeft > 0){
                    _toon.GetPrimaryAttribute(i).BaseValue++;
                    _pointsLeft--;
                    _toon.StatUpdate();
                }
            }
        }
    }
    private void DisplayVitals(){
        for(int i = 0; i < Enum.GetValues(typeof(VitalName)).Length; i++){
            GUI.Label(new Rect( OFFSET,
                                _statStartingPos + ((i+Enum.GetValues(typeof(AttributeName)).Length)*LINE_HEIGHT),
                                STAT_LABEL_WIDTH,
                                LINE_HEIGHT
                            ), ((VitalName)i).ToString());
            GUI.Label(new Rect( STAT_LABEL_WIDTH + OFFSET,
                                _statStartingPos + ((i+Enum.GetValues(typeof(AttributeName)).Length)*LINE_HEIGHT),
                                BASE_VALUE_LABEL_WIDTH,
                                LINE_HEIGHT
                            ), _toon.GetVital(i).AdjustedBaseValue.ToString());
        }
    }
    private void DisplaySkills(){
        for(int i = 0; i < Enum.GetValues(typeof(SkillName)).Length; i++){
            GUI.Label(new Rect( STAT_LABEL_WIDTH + OFFSET + BASE_VALUE_LABEL_WIDTH + BUTTON_WIDTH * 2 + BUTTON_OFFSET + OFFSET,
                                _statStartingPos + (i*LINE_HEIGHT),
                                STAT_LABEL_WIDTH,
                                LINE_HEIGHT
                            ), ((SkillName)i).ToString().Replace("_", " "));
            GUI.Label(new Rect( STAT_LABEL_WIDTH + OFFSET + BASE_VALUE_LABEL_WIDTH + BUTTON_WIDTH * 2 + BUTTON_OFFSET + OFFSET + STAT_LABEL_WIDTH,
                                _statStartingPos + (i*LINE_HEIGHT),
                                BASE_VALUE_LABEL_WIDTH,
                                LINE_HEIGHT
                            ), _toon.GetSkill(i).AdjustedBaseValue.ToString());
        }
    }
    private void DisplayPointsLeft(){
        GUI.Label(new Rect(250, 10, 100, LINE_HEIGHT), "Points Left: " + _pointsLeft);
    }
}
