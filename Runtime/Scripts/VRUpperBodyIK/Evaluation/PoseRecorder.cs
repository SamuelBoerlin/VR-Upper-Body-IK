using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VRUpperBodyIK.Evaluation
{
    public class PoseRecorder : MonoBehaviour
    {
        public Skeleton.Skeleton skeleton;

        public InputAction startRecordingAction;

        public string recordingFileName = "pose_recording";

        private Recording recording = new();
        public bool isRecording;
        private float recordingStartTime;

        private void Start()
        {
            if (startRecordingAction != null)
            {
                startRecordingAction.performed += SetRecording;
            }
        }

        private void OnDestroy()
        {
            if (startRecordingAction != null)
            {
                startRecordingAction.performed -= SetRecording;
            }
        }

        public void SetRecording(InputAction.CallbackContext context)
        {
            isRecording = !isRecording;
        }

        private void FixedUpdate()
        {
            if (isRecording)
            {
                if (recordingStartTime <= 0)
                {
                    recordingStartTime = Time.time;
                }

                var pose = skeleton.UncalibratedLocalPose;
                pose.time = Time.time;
                recording.poses.Add(pose);
            }
            else
            {
                recordingStartTime = -1;

                if (recording.poses.Count > 0)
                {
                    var json = JsonUtility.ToJson(recording);

                    var fileName = $"{recordingFileName}_{DateTimeOffset.Now.ToUnixTimeSeconds()}.json";

                    System.IO.File.WriteAllText(fileName, json);

                    Debug.Log("Saved recording " + fileName + " with " + recording.poses.Count + " poses");

                    recording.poses.Clear();
                }
            }
        }
    }
}