using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (RangeAttributeWithDefault))]
public class RangeAttributeWithDefaultDrawer: PropertyDrawer {


    override public void OnGUI (Rect position, SerializedProperty  property, GUIContent  label) {
        RangeAttributeWithDefault att = attribute as RangeAttributeWithDefault;
        if (property.intValue < att.min || property.intValue > att.max)
            property.intValue = (int)att.defaultValue;
        if (att.min <= 0 && att.max >= 0)
        {
            EditorGUI.LabelField (position, label.text, "Do not include 0 in Range");
        } else
        {
        if (property.propertyType == SerializedPropertyType.Float)
            EditorGUI.IntSlider(position,property, (int)att.min,(int) att.max, label);
        else if (property.propertyType == SerializedPropertyType.Integer)
            EditorGUI.IntSlider(position,property, (int)att.min, (int)att.max, label);
        else
            EditorGUI.LabelField (position, label.text, "Use Range with float or int.");

        }

    }


    override public float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        return base.GetPropertyHeight(property, label);
    }




}
