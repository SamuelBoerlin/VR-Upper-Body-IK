using UnityEngine;

namespace VRUpperBodyIK.IK
{
    public class Solver : MonoBehaviour
    {
        [Tooltip("Skeleton to be solved with IK. Head and hand should already be positioned appropriately.")]
        public Skeleton.Skeleton skeleton;

        [Tooltip("Array of positioners that position the individual skeleton parts: Body Positioner Provider, Elbow Positioner Provider, Shoulder Positioner Provider.")]
        public PositionerProviderBehaviour[] positioners;

        private void FixedUpdate()
        {
            var pose = skeleton.CalibratedWorldPose;

            var bodySettings = new BodySettings();

            if(positioners != null)
            {
                foreach (PositionerProvider provider in positioners)
                {
                    provider.Positioner.Update(pose, bodySettings);
                }
            }

            ApplyPoseToChain(pose);
        }

        protected void ApplyPoseToChain(Skeleton.Pose pose)
        {
            skeleton.leftShoulder.position = pose.leftArm.shoulderPosition;
            skeleton.leftElbow.position = pose.leftArm.elbowPosition;

            skeleton.rightShoulder.position = pose.rightArm.shoulderPosition;
            skeleton.rightElbow.position = pose.rightArm.elbowPosition;
        }
    }
}