using UnityEngine;
using VRUpperBodyIK.IK;

namespace VRUpperBodyIK.Skeleton
{
    public class SkeletonSolver : MonoBehaviour
    {
        [Tooltip("Skeleton to be solved with IK. Head and hand should already be positioned appropriately.")]
        public Skeleton skeleton;

        [Tooltip("Array of positioners that position the individual skeleton parts: Body Positioner Provider, Elbow Positioner Provider, Shoulder Positioner Provider.")]
        public PositionerProviderBehaviour[] positioners;

        private Solver solver;

        private void Awake()
        {
            solver = new Solver(positioners);
        }

        private void FixedUpdate()
        {
            var pose = skeleton.CalibratedWorldPose;

            solver.Solve(pose);

            ApplyPoseToSkeleton(pose);
        }

        protected void ApplyPoseToSkeleton(Pose pose)
        {
            skeleton.leftShoulder.position = pose.leftArm.shoulderPosition;
            skeleton.leftElbow.position = pose.leftArm.elbowPosition;

            skeleton.rightShoulder.position = pose.rightArm.shoulderPosition;
            skeleton.rightElbow.position = pose.rightArm.elbowPosition;
        }
    }
}