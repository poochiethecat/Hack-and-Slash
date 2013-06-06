public class Vital : ModifiedStat {
    private int _curVal;
    
    public Vital(){
        _curVal = 0;
        ExpToLevel = 50;
        LevelModifier = 1.1f;
    }
    
    
    public int CurVal {
        get {
            if(_curVal > AdjustedBaseValue) _curVal = AdjustedBaseValue;
            return this._curVal;
        }
        set { _curVal = value; }
    }
}

public enum VitalName{
    Health,
    Energy,
    Mana
}
