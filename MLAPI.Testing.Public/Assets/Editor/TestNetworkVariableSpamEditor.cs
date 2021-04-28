using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestNetworkVariableSpam))]
public class TestNetworkVariableSpamEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var test = target as TestNetworkVariableSpam;
        if(GUILayout.Button("Send Request"))
        {
            test.ClientRequestData();
        }
    }
}