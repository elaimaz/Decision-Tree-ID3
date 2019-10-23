using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Draws a red circle around the scene player
[CustomEditor(typeof(PlayerScript))]
public class DrawCircle : Editor
{
    private void OnSceneGUI()
    {
        PlayerScript player = (PlayerScript)target;
        Handles.color = Color.red;
        Handles.DrawWireDisc(player.transform.position, new Vector3(0, 1, 0), player.circleSize);
    }
}
