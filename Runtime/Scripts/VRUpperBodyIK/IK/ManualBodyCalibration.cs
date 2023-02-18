using UnityEngine;
using UnityEngine.InputSystem;

namespace VRUpperBodyIK.IK
{
    public class ManualBodyCalibration : MonoBehaviour
    {
        [Tooltip("Skeleton that is used as input for the calibration.")]
        public Skeleton.Skeleton skeleton;

        [Tooltip("Body positioner to be calibrated.")]
        public BodyPositionerProvider bodyPositioner;

        [Tooltip("Input action that triggers calibration.")]
        public InputAction calibrateAction;

        [Tooltip("Triggers a calibration after the specified number of seconds. Handy for playback of tracked data.")]
        public float calibrateAfterSeconds = 0;

        private void Start()
        {
            if (calibrateAction != null)
            {
                calibrateAction.performed += OnCalibrateInput;
            }
        }

        private void OnDestroy()
        {
            if (calibrateAction != null)
            {
                calibrateAction.performed -= OnCalibrateInput;
            }
        }

        public void OnCalibrateInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Calibrate();
            }
        }

        private void FixedUpdate()
        {
            if (calibrateAfterSeconds > Mathf.Epsilon && Time.time > calibrateAfterSeconds)
            {
                Calibrate();
                calibrateAfterSeconds = 0;
            }
        }

        public void Calibrate()
        {
            var pose = skeleton.CalibratedWorldPose;

            var handToHandDistance = (pose.leftArm.handPosition - pose.rightArm.handPosition).magnitude;

            bodyPositioner.ElbowShoulderDistance = bodyPositioner.HandElbowDistance = (handToHandDistance - 2.0f * bodyPositioner.NeckShoulderDistance) / 4.0f;

            bodyPositioner.Height = skeleton.head.position.y;
        }
    }
}
