using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;                    //added to access the enum class

public class BaseCharacter : MonoBehaviour {
	
	private string _name;
	private int _level;
	private uint _freeExp;
	
	private Attribute[] _primaryAttributes;
	private Vital[] _vitals;
	private Skill[] _skills;
	
	public void Awake(){
		_name = string.Empty;
		_level = 0;
		_freeExp = 0;
		
		_primaryAttributes = new Attribute[Enum.GetValues(typeof(AttributeName)).Length];
		_vitals = new Vital[Enum.GetValues(typeof(VitalName)).Length];
		_skills = new Skill[Enum.GetValues(typeof(SkillName)).Length];
		
		SetupPrimaryAttributes();
		SetupVitals();
		SetupSkills();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private void SetupPrimaryAttributes(){
		for(int i = 0; i < _primaryAttributes.Length; i++){
			_primaryAttributes[i] = new Attribute();
		}
	}
	private void SetupVitals(){
		for(int i = 0; i < _vitals.Length; i++){
			_vitals[i] = new Vital();
		}
	}
	private void SetupSkills(){
		for(int i = 0; i < _skills.Length; i++){
			_skills[i] = new Skill();
		}
	}
	
	public Attribute GetPrimaryAttribute(int index){
		return _primaryAttributes[index];
	}
	public Vital GetVital(int index){
		return _vitals[index];
	}
	public Skill GetSkill(int index){
		return _skills[index];
	}
	
	private void SetupVitalModifiers() {
		//health
		AddVitalModifier(VitalName.Health, AttributeName.Constitution, .5f);
		//energy
		AddVitalModifier(VitalName.Energy, AttributeName.Constitution, 1);
		//mana
		AddVitalModifier(VitalName.Mana, AttributeName.Willpower, 1);
	}
	
	private void AddVitalModifier(VitalName vital, AttributeName mod, float ratio){
		GetSkill((int)vital).AddModifier(new ModifyingAttribute{attribute = GetPrimaryAttribute((int)mod), ratio = ratio});
	}
	
	private void SetupSkillModifiers() {
		//melee offence
		AddSkillModifier(SkillName.Melee_Offence, AttributeName.Might, .33f);
		AddSkillModifier(SkillName.Melee_Offence, AttributeName.Nimbleness, .33f);
		//melee defence
		AddSkillModifier(SkillName.Melee_Defence, AttributeName.Speed, .33f);
		AddSkillModifier(SkillName.Melee_Defence, AttributeName.Constitution, .33f);
		//magic offence
		AddSkillModifier(SkillName.Magic_Offence, AttributeName.Concentration, .33f);
		AddSkillModifier(SkillName.Magic_Offence, AttributeName.Willpower, .33f);
		//magic defence
		AddSkillModifier(SkillName.Magic_Defence, AttributeName.Concentration, .33f);
		AddSkillModifier(SkillName.Magic_Defence, AttributeName.Willpower, .33f);
		//ranged offence
		AddSkillModifier(SkillName.Ranged_Offence, AttributeName.Concentration, .33f);
		AddSkillModifier(SkillName.Ranged_Offence, AttributeName.Speed, .33f);
		//ranged defence
		AddSkillModifier(SkillName.Ranged_Defence, AttributeName.Speed, .33f);
		AddSkillModifier(SkillName.Ranged_Defence, AttributeName.Nimbleness, .33f);
	}
	
	private void AddSkillModifier(SkillName skill, AttributeName mod, float ratio){
		GetSkill((int)skill).AddModifier(new ModifyingAttribute{attribute = GetPrimaryAttribute((int)mod), ratio = ratio});
	}
	
	public void StatUpdate(){
		foreach(Vital vital in _vitals) vital.Update();
		foreach(Skill skill in _skills) skill.Update();
	}
	
	public void AddExp(uint exp){
		_freeExp += exp;
		
		CalculateLevel();
	}
	
	//take average of all of the players skills and assign that as the player level
	public void CalculateLevel(){
		
	}
	
	#region getters and setters
	public uint FreeExp {
		get { return this._freeExp; }
		set { _freeExp = value; }
	}

	public int Level {
		get { return this._level; }
		set { _level = value; }
	}

	public string Name {
		get { return this._name; }
		set { _name = value; }
	}
	#endregion
}
