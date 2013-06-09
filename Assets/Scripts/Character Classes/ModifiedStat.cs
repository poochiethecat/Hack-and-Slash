using System.Collections.Generic;

public class ModifiedStat : BaseStat {
    private List<ModifyingAttribute> _mods; //A list of Attributes that modify this stat
    private int _modValue;                  //The amount added to the baseValue from the modifiers
    
    public ModifiedStat(){
        _mods = new List<ModifyingAttribute>();
        _modValue = 0;
    }
    
    public void AddModifier(ModifyingAttribute mod){
        _mods.Add(mod);
    }
    
    private void CalculateModValue(){
        _modValue = 0;
        if(_mods.Count > 0){
            foreach(ModifyingAttribute att in _mods){
                _modValue += (int)(att.attribute.AdjustedBaseValue * att.ratio);
            }
        }
    }
    new public int AdjustedBaseValue{
        get { return BaseValue + BuffValue + _modValue; }
    }
    
    public void Update(){
        CalculateModValue();
    }
    public string GetModifyingAttributesString(){
        string temp = "";
        foreach(ModifyingAttribute mod in _mods){
            temp += mod.attribute.StatName + ":" + mod.ratio + ";";
        }
        UnityEngine.Debug.Log(temp);
        return temp;
    }
}

public struct ModifyingAttribute {
    public Attribute attribute;
    public float ratio;
    
    public ModifyingAttribute(Attribute att, float rat){
        attribute = att;
        ratio = rat;
    }
}