using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);

        Vector3 vecRight = fow.DirFormAngle(fow.viewAngle * 0.5f, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + vecRight * fow.viewRadius);

        Vector3 vecLeft = fow.DirFormAngle(-fow.viewAngle * 0.5f, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + vecLeft * fow.viewRadius);
    }
}
