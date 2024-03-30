using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class FlagManager : MonoBehaviour
{
    public enum Flag
    {
        moveText,
        firstEnemyMeet,
        allEnemyDeathInFirst,
        cc
    }

    public Dictionary<Flag, object> flagDictionary =
    new Dictionary<Flag, object>();


    //インスペクター上でフラグの状態を表示
    [CustomEditor(typeof(FlagManager))]
    public class FlagManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            FlagManager flagManager = (FlagManager)target;

            // 各 Flag の状態を表示
            foreach (var flag in flagManager.flagDictionary.Keys)
            {
                EditorGUILayout.LabelField(flag.ToString(), flagManager.flagDictionary[flag].ToString());
            }
        }
    }

}