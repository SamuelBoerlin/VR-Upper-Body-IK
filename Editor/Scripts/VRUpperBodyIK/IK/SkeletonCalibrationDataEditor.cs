using UnityEditor;
using UnityEngine;
using VRUpperBodyIK.Skeleton;

namespace VRUpperBodyIK.IK
{
    [CustomEditor(typeof(SkeletonCalibrationData))]
    public class SkeletonCalibrationDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var data = (SkeletonCalibrationData)target;

            if (GUILayout.Button("Save Calibration Data"))
            {
                var path = EditorUtility.SaveFilePanel("Save calibration data to .json", "", "calibration.json", "json");

                if (path != null && path.Length > 0)
                {
                    data.SaveCalibrationData(path);

                    Debug.Log("Saved calibration data to " + path);
                }
            }
        }

        protected virtual void OnSceneGUI()
        {
            var data = (SkeletonCalibrationData)target;
            var offset = data.Offset;
            var sourceChain = data.targetSkeleton;

            if (data.edit && sourceChain != null)
            {
                int i = 0;

                foreach (var transform in sourceChain.Transforms)
                {
                    var positionOffset = offset.Position(i);
                    var rotationOffset = offset.Rotation(i);

                    if (CalibrationPointHandle(data, transform.position, transform.rotation, positionOffset, rotationOffset, out var newPositionOffset, out var newRotationOffset))
                    {
                        offset.SetPosition(i, newPositionOffset);
                        offset.SetRotation(i, newRotationOffset);
                    }

                    ++i;
                }

                data.Offset = offset;
            }
        }

        private bool CalibrationPointHandle(SkeletonCalibrationData data, Vector3 position, Quaternion rotation, Vector3 positionOffset, Quaternion rotationOffset, out Vector3 newPositionOffset, out Quaternion newRotationOffset)
        {
            newPositionOffset = positionOffset;
            newRotationOffset = rotationOffset;

            var calibratedRotation = rotation * rotationOffset;
            var calibratedPosition = position + calibratedRotation * positionOffset;

            Handles.color = Color.yellow;
            Handles.SphereHandleCap(0, calibratedPosition, calibratedRotation, 0.025f, EventType.Repaint);

            var handlesMatrix = Handles.matrix;

            bool changed = false;

            EditorGUI.BeginChangeCheck();
            Handles.matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
            Vector3 newPosition = Handles.PositionHandle(calibratedRotation * positionOffset, calibratedRotation);
            if (EditorGUI.EndChangeCheck())
            {
                //Undo.RecordObject(data, "Change Calibration Position Data");
                newPositionOffset = Quaternion.Inverse(calibratedRotation) * newPosition;
                changed = true;
            }

            EditorGUI.BeginChangeCheck();
            Handles.matrix = Matrix4x4.TRS(calibratedPosition, rotation, Vector3.one * 0.75f);
            Quaternion newRotation = Handles.RotationHandle(rotationOffset, Vector3.zero);
            if (EditorGUI.EndChangeCheck())
            {
                //Undo.RecordObject(data, "Change Calibration Rotation Data");
                newRotationOffset = newRotation;
                changed = true;
            }

            Handles.matrix = handlesMatrix;

            return changed;
        }
    }
}