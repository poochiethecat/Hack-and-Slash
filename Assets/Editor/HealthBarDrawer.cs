using UnityEditor;
using UnityEngine;
using System;
// [CustomPropertyDrawer(typeof(HealthBar))]
//

public class HealthBarDrawer : PropertyDrawer
{
    bool show = true;

    const float min = 50;
    const float max = 1000;
    public override void OnGUI (Rect pos,SerializedProperty prop,GUIContent label) {
        SerializedProperty maxWidth = prop.FindPropertyRelative("minWidth");
     EditorGUI.BeginProperty (pos, label, prop);
        Debug.Log(maxWidth.floatValue);

//        EditorGUI.BeginProperty(new Rect(pos.x, pos.y, pos.width, pos.height*10), new GUIContent("Healthbar"), prop);
        // UnityEngine.Object[] objs = { Selection.activeTransform };
         if ( show = EditorGUI.Foldout(pos, show, new GUIContent("Healthbar"), false))
         {
//             Rect pos2 = EditorGUI.PrefixLabel(new Rect(pos.x, pos.y+20, pos.width, pos.height), GUIUtility.GetControlID (FocusType.Passive), new GUIContent("max Width"));

        EditorGUI.Slider (
            new Rect (pos.x, pos.y+20, pos.width, pos.height),
            maxWidth, min, max, new GUIContent("Max Width"));


         }

         EditorGUI.EndProperty ();
//

    }

    // Make folding change height!
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label=null)
    {

         // if (children)
         //    return base.GetPropertyHeight (property, label, false)*10;
         // else
             return base.GetPropertyHeight (property, label);
    }



}
