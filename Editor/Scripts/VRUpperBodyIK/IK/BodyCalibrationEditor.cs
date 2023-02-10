using UnityEditor;
using UnityEngine;

namespace VRUpperBodyIK.IK
{
    [CustomEditor(typeof(ManualBodyCalibration))]
    public class BodyCalibrationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (ManualBodyCalibration)target;

            if (GUILayout.Button("Calibrate"))
            {
                script.Calibrate();
            }
        }
    }
}