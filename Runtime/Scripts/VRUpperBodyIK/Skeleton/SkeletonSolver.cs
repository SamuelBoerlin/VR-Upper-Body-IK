using System;
using System.Linq;
using UnityEngine;
using VRUpperBodyIK.IK;

namespace VRUpperBodyIK.Skeleton
{
    public class SkeletonSolver : MonoBehaviour
    {
        [Tooltip("Skeleton to be solved with IK. Head and hand should already be positioned appropriately.")]
        public Skeleton sourceSkeleton;

        [Tooltip("Skeleton to apply the IK solution to. May be the same as Source Skeleton.")]
        public Skeleton targetSkeleton;

        [Tooltip("Array of positioners that position the individual skeleton parts: Body Positioner Provider, Elbow Positioner Provider, Shoulder Positioner Provider.")]
        public PositionerProviderBehaviour[] positioners;

        public bool useCalibratedPose = true;

        public Joint[] applyJoints = (Joint[])Enum.GetValues(typeof(Joint));

        private Solver solver;

        private void Update()
        {
            SolveAndApply();
        }

        public void SolveAndApply()
        {
            var pose = useCalibratedPose ? sourceSkeleton.CalibratedWorldPose : sourceSkeleton.UncalibratedWorldPose;

            solver ??= new(positioners);
            solver.Solve(pose);

            ApplyPoseToSkeleton(pose);
        }

        protected void ApplyPoseToSkeleton(Pose pose)
        {
            if (targetSkeleton.head != null && applyJoints.Contains(Joint.Head))
            {
                targetSkeleton.head.position = pose.headPosition;
                targetSkeleton.head.rotation = pose.headRotation;
            }

            if (targetSkeleton.neck != null && applyJoints.Contains(Joint.Neck))
            {
                targetSkeleton.neck.position = pose.neckPosition;
                targetSkeleton.neck.rotation = pose.neckRotation;
            }

            if (targetSkeleton.leftHand != null && applyJoints.Contains(Joint.LeftHand))
            {
                targetSkeleton.leftHand.position = pose.leftArm.handPosition;
                targetSkeleton.leftHand.rotation = pose.leftArm.handRotation;
            }

            if (targetSkeleton.leftElbow != null && applyJoints.Contains(Joint.LeftElbow))
            {
                targetSkeleton.leftElbow.position = pose.leftArm.elbowPosition;
                targetSkeleton.leftElbow.rotation = pose.leftArm.elbowRotation;
            }

            if (targetSkeleton.leftShoulder != null && applyJoints.Contains(Joint.LeftShoulder))
            {
                targetSkeleton.leftShoulder.position = pose.leftArm.shoulderPosition;
                targetSkeleton.leftShoulder.rotation = pose.leftArm.shoulderRotation;
            }

            if (targetSkeleton.rightHand != null && applyJoints.Contains(Joint.RightHand))
            {
                targetSkeleton.rightHand.position = pose.rightArm.handPosition;
                targetSkeleton.rightHand.rotation = pose.rightArm.handRotation;
            }

            if (targetSkeleton.rightElbow != null && applyJoints.Contains(Joint.RightElbow))
            {
                targetSkeleton.rightElbow.position = pose.rightArm.elbowPosition;
                targetSkeleton.rightElbow.rotation = pose.rightArm.elbowRotation;
            }

            if (targetSkeleton.rightShoulder != null && applyJoints.Contains(Joint.RightShoulder))
            {
                targetSkeleton.rightShoulder.position = pose.rightArm.shoulderPosition;
                targetSkeleton.rightShoulder.rotation = pose.rightArm.shoulderRotation;
            }
        }
    }
}