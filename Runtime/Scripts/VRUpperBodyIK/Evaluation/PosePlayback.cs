using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace VRUpperBodyIK.Evaluation
{
    public class PosePlayback : MonoBehaviour
    {
        public Skeleton.Skeleton skeleton;

        public string recordingFileName = "pose_recording";

        public bool isPlaying;
        private float playbackStartTime;

        private Recording recording;
        private int recordingIndex = 0;

        public bool disableTrackers = true;

        private bool[] reenableTrackers = new bool[8];

        private void Start()
        {
            string json = File.ReadAllText(recordingFileName);
            recording = JsonUtility.FromJson<Recording>(json);
        }

        private void OnEnable()
        {
            if (disableTrackers)
            {
                int i = 0;
                foreach (var transform in skeleton.Transforms)
                {
                    if(transform != null)
                    {
                        var driver = transform.gameObject.GetComponent<TrackedPoseDriver>();
                        if (driver != null)
                        {
                            reenableTrackers[i++] = driver.enabled;
                            driver.enabled = false;
                        }
                    }
                }
            }
        }

        private void OnDisable()
        {
            int i = 0;
            foreach (var transform in skeleton.Transforms)
            {
                if(transform != null)
                {
                    var driver = transform.gameObject.GetComponent<TrackedPoseDriver>();
                    if (driver != null && reenableTrackers[i++])
                    {
                        driver.enabled = true;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (isPlaying)
            {
                if (playbackStartTime <= 0)
                {
                    playbackStartTime = Time.time;
                    recordingIndex = 0;
                }

                if (recording != null)
                {
                    if (recordingIndex >= 0 && recordingIndex < recording.poses.Count)
                    {
                        skeleton.UncalibratedLocalPose = recording.poses[recordingIndex];

                        ++recordingIndex;
                    }
                }
            }
            else
            {
                playbackStartTime = -1;
            }
        }
    }
}