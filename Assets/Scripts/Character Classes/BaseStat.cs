using UnityEngine;
using System.Collections;

public class BaseStat
{

	private int _baseValue;        //The base value of the Stat
	private int _buffValue;        //amount buffs adds to this stat
	private int _expToLevel;       //experience points needed for next level of this stat
	private float _levelModifier;  //the modifier applied to the exp needed to raise the stat
	
	public BaseStat ()
	{
		_baseValue = 0;
		_buffValue = 0;
		_levelModifier = 1.1f;
		_expToLevel = 100;
	}
	
	
#region Basic getters and setters
	//Basic getters/setters
	public int BaseValue {
		get{ return _baseValue; }
		set{ _baseValue = value; }
	}

	public int BuffValue {
		get{ return _buffValue; }
		set{ _buffValue = value; }
	}

	public int ExpToLevel {
		get{ return _expToLevel; }
		set{ _expToLevel = value; }
	}

	public float LevelModifier {
		get{ return _levelModifier; }
		set{ _levelModifier = value; }
	}
#endregion
	
	private int CalculateExpToLevel ()
	{
		return (int)(_expToLevel * _levelModifier);
	}
	
	public void LevelUp ()
	{
		_expToLevel = CalculateExpToLevel ();
		_baseValue++;
	}
	
	public int AdjustedBaseValue{
		get { return _baseValue + _buffValue; }
	}
}
