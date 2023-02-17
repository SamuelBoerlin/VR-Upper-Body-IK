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

        private void FixedUpdate()
        {
            var pose = skeleton.CalibratedWorldPose;

            solver ??= new(positioners);
            solver.Solve(pose);

            ApplyPoseToSkeleton(pose);
        }

        protected void ApplyPoseToSkeleton(Pose pose)
        {
            skeleton.leftShoulder.position = pose.leftArm.shoulderPosition;
            skeleton.leftShoulder.rotation = pose.leftArm.shoulderRotation;

            skeleton.leftElbow.position = pose.leftArm.elbowPosition;
            skeleton.leftElbow.rotation = pose.leftArm.elbowRotation;

            skeleton.rightShoulder.position = pose.rightArm.shoulderPosition;
            skeleton.rightShoulder.rotation = pose.rightArm.shoulderRotation;

            skeleton.rightElbow.position = pose.rightArm.elbowPosition;
            skeleton.rightElbow.rotation = pose.rightArm.elbowRotation;

            if (skeleton.neck != null)
            {
                skeleton.neck.position = pose.neckPosition;
                skeleton.neck.rotation = pose.neckRotation;
            }
        }
    }
}