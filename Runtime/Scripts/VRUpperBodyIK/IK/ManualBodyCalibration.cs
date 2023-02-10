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

        public void Calibrate()
        {
            var pose = skeleton.CalibratedWorldPose;

            var handToHandDistance = (pose.leftArm.handPosition - pose.rightArm.handPosition).magnitude;

            bodyPositioner.elbowShoulderDistance = bodyPositioner.handElbowDistance = (handToHandDistance - 2.0f * bodyPositioner.neckShoulderDistance) / 4.0f;
        }
    }
}
