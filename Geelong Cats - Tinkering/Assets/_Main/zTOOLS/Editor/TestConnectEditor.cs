using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestConnect))]
public class TestConnectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestConnect myScript = (TestConnect)target;

        GUILayout.Space(5);

        if (GUILayout.Button("Connect as Coach"))
        {
            myScript.ConnectAsCoach();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("ToggleBoardOn"))
        {
            myScript.ToggleBoard(true);
        }

        GUILayout.Space(5);

        if (GUILayout.Button("ToggleOff"))
        {
            myScript.ToggleBoard(false);
        }

        GUILayout.Space(5);

        if(GUILayout.Button("Activate Home AI"))
        {
            myScript.ActivateAI(ETEAM.HOME);
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Activate Away AI"))
        {
            myScript.ActivateAI(ETEAM.AWAY);
        }


        GUILayout.Space(35);

        if (GUILayout.Button("Connect as Player"))
        {
            myScript.ConnectAsPlayer();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Teleport to coach"))
        {
            myScript.TeleportToCoach();
        }

        
        GUILayout.Space(35);

        if(GUILayout.Button("Rescale IK Rig"))
        {
            myScript.RescaleIKRig();
        }
    }
}
