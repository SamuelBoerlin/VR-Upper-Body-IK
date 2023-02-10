﻿using UnityEngine;

namespace VRUpperBodyIK.Skeleton
{
    public class SkeletonCalibrationData : MonoBehaviour
    {
        public Pose Offset { get; set; } = new();

        [Tooltip("Skeleton to which this calibration should be applied to. Used for visualisation purposes only.")]
        public Skeleton targetSkeleton;

        [Tooltip("Whether editing mode should be enabled.")]
        public bool edit;

        [Tooltip("Whether the calibrated positions should be rendered as gizmos.")]
        public bool drawPositionGizmos;

        [Tooltip("Whether the calibration data should be loaded from the specified file.")]
        public bool loadFromFile;

        [Tooltip("File to load calibration data from if 'Load From File' is true.")]
        public string calibrationFile;

        private void Start()
        {
            if (loadFromFile && calibrationFile != null)
            {
                LoadCalibrationData(calibrationFile);
            }
        }

        public void SaveCalibrationData(string path)
        {
            var json = JsonUtility.ToJson(Offset);
            System.IO.File.WriteAllText(path, json);
        }

        public bool LoadCalibrationData(string path)
        {
            var json = System.IO.File.ReadAllText(path);
            var offset = JsonUtility.FromJson<Pose>(json);
            if (offset != null)
            {
                Offset = offset;
                return true;
            }
            return false;
        }

        public void OnDrawGizmos()
        {
            if (drawPositionGizmos && targetSkeleton != null)
            {
                for (int i = 0; i < 7; ++i)
                {
                    var transform = targetSkeleton.Transforms[i];

                    var rot = transform.rotation * Offset.Rotation(i);
                    var pos = transform.position + rot * Offset.Position(i);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(pos, 0.015f);

                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(pos, rot * new Vector3(0.1f, 0, 0));

                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(pos, rot * new Vector3(0, 0.1f, 0));

                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(pos, rot * new Vector3(0, 0, 0.1f));
                }
            }
        }
    }
}