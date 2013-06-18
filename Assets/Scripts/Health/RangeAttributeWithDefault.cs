using UnityEngine;
public class RangeAttributeWithDefault : PropertyAttribute {

    public float min;
    public float max;
    public float defaultValue;
    public RangeAttributeWithDefault (float min, float max, float defaultValue) {
        this.defaultValue = defaultValue;
        this.min = min;
        this.max = max;

    }
}
