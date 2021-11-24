using UnityEditor;

[CustomEditor(typeof(Client))]
public class ClientEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var client = target as Client;
        
    }
}