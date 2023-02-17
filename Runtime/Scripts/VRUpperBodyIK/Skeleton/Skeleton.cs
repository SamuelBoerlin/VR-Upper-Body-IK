using UnityEngine;

namespace VRUpperBodyIK.Skeleton
{
    public class Skeleton : MonoBehaviour
    {
        public Transform head;

        public Transform neck;

        public Transform leftShoulder;
        public Transform leftElbow;
        public Transform leftHand;

        public Transform rightShoulder;
        public Transform rightElbow;
        public Transform rightHand;

        private Transform[] _transforms;
        public Transform[] Transforms
        {
            get
            {
                if (_transforms == null)
                {
                    _transforms = new Transform[] { head, neck, leftShoulder, leftElbow, leftHand, rightShoulder, rightElbow, rightHand };
                }
                return _transforms;
            }
        }

        public SkeletonCalibrationData calibrationData;

        public bool drawUncalibratedSkeletonGizmos;
        public bool drawCalibratedSkeletonGizmos;

        public Pose CalibratedLocalPose
        {
            get
            {
                if (calibrationData != null)
                {
                    var offset = calibrationData.Offset;
                    var pose = new Pose()
                    {
                        headPosition = head.localPosition + head.localRotation * offset.headRotation * offset.headPosition,
                        headRotation = head.localRotation * offset.headRotation
                    };
                    pose.leftArm.shoulderPosition = leftShoulder.localPosition + leftShoulder.localRotation * offset.leftArm.shoulderRotation * offset.leftArm.shoulderPosition;
                    pose.leftArm.shoulderRotation = leftShoulder.localRotation * offset.leftArm.shoulderRotation;
                    pose.leftArm.elbowPosition = leftElbow.localPosition + leftElbow.localRotation * offset.leftArm.elbowRotation * offset.leftArm.elbowPosition;
                    pose.leftArm.elbowRotation = leftElbow.localRotation * offset.leftArm.elbowRotation;
                    pose.leftArm.handPosition = leftHand.localPosition + leftHand.localRotation * offset.leftArm.handRotation * offset.leftArm.handPosition;
                    pose.leftArm.handRotation = leftHand.localRotation * offset.leftArm.handRotation;
                    pose.rightArm.shoulderPosition = rightShoulder.localPosition + rightShoulder.localRotation * offset.rightArm.shoulderRotation * offset.rightArm.shoulderPosition;
                    pose.rightArm.shoulderRotation = rightShoulder.localRotation * offset.rightArm.shoulderRotation;
                    pose.rightArm.elbowPosition = rightElbow.localPosition + rightElbow.localRotation * offset.rightArm.elbowRotation * offset.rightArm.elbowPosition;
                    pose.rightArm.elbowRotation = rightElbow.localRotation * offset.rightArm.elbowRotation;
                    pose.rightArm.handPosition = rightHand.localPosition + rightHand.localRotation * offset.rightArm.handRotation * offset.rightArm.handPosition;
                    pose.rightArm.handRotation = rightHand.localRotation * offset.rightArm.handRotation;
                    return pose;
                }
                return UncalibratedLocalPose;
            }
        }

        public Pose CalibratedWorldPose
        {
            get
            {
                if (calibrationData != null)
                {
                    var offset = calibrationData.Offset;
                    var pose = new Pose()
                    {
                        headPosition = head.position + head.rotation * offset.headRotation * offset.headPosition,
                        headRotation = head.rotation * offset.headRotation,
                    };
                    pose.leftArm.shoulderPosition = leftShoulder.position + leftShoulder.rotation * offset.leftArm.shoulderRotation * offset.leftArm.shoulderPosition;
                    pose.leftArm.shoulderRotation = leftShoulder.rotation * offset.leftArm.shoulderRotation;
                    pose.leftArm.elbowPosition = leftElbow.position + leftElbow.rotation * offset.leftArm.elbowRotation * offset.leftArm.elbowPosition;
                    pose.leftArm.elbowRotation = leftElbow.rotation * offset.leftArm.elbowRotation;
                    pose.leftArm.handPosition = leftHand.position + leftHand.rotation * offset.leftArm.handRotation * offset.leftArm.handPosition;
                    pose.leftArm.handRotation = leftHand.rotation * offset.leftArm.handRotation;
                    pose.rightArm.shoulderPosition = rightShoulder.position + rightShoulder.rotation * offset.rightArm.shoulderRotation * offset.rightArm.shoulderPosition;
                    pose.rightArm.shoulderRotation = rightShoulder.rotation * offset.rightArm.shoulderRotation;
                    pose.rightArm.elbowPosition = rightElbow.position + rightElbow.rotation * offset.rightArm.elbowRotation * offset.rightArm.elbowPosition;
                    pose.rightArm.elbowRotation = rightElbow.rotation * offset.rightArm.elbowRotation;
                    pose.rightArm.handPosition = rightHand.position + rightHand.rotation * offset.rightArm.handRotation * offset.rightArm.handPosition;
                    pose.rightArm.handRotation = rightHand.rotation * offset.rightArm.handRotation;
                    return pose;
                }
                return UncalibratedWorldPose;
            }
        }

        public Pose UncalibratedLocalPose
        {
            get
            {
                var pose = new Pose()
                {
                    headPosition = head.localPosition,
                    headRotation = head.localRotation,
                };
                pose.leftArm.shoulderPosition = leftShoulder.localPosition;
                pose.leftArm.shoulderRotation = leftShoulder.localRotation;
                pose.leftArm.elbowPosition = leftElbow.localPosition;
                pose.leftArm.elbowRotation = leftElbow.localRotation;
                pose.leftArm.handPosition = leftHand.localPosition;
                pose.leftArm.handRotation = leftHand.localRotation;
                pose.rightArm.shoulderPosition = rightShoulder.localPosition;
                pose.rightArm.shoulderRotation = rightShoulder.localRotation;
                pose.rightArm.elbowPosition = rightElbow.localPosition;
                pose.rightArm.elbowRotation = rightElbow.localRotation;
                pose.rightArm.handPosition = rightHand.localPosition;
                pose.rightArm.handRotation = rightHand.localRotation;
                return pose;
            }
            set
            {
                head.localPosition = value.headPosition;
                head.localRotation = value.headRotation;
                leftShoulder.localPosition = value.leftArm.shoulderPosition;
                leftShoulder.localRotation = value.leftArm.shoulderRotation;
                leftElbow.localPosition = value.leftArm.elbowPosition;
                leftElbow.localRotation = value.leftArm.elbowRotation;
                leftHand.localPosition = value.leftArm.handPosition;
                leftHand.localRotation = value.leftArm.handRotation;
                rightShoulder.localPosition = value.rightArm.shoulderPosition;
                rightShoulder.localRotation = value.rightArm.shoulderRotation;
                rightElbow.localPosition = value.rightArm.elbowPosition;
                rightElbow.localRotation = value.rightArm.elbowRotation;
                rightHand.localPosition = value.rightArm.handPosition;
                rightHand.localRotation = value.rightArm.handRotation;
            }
        }

        public Pose UncalibratedWorldPose
        {
            get
            {
                var pose = new Pose()
                {
                    headPosition = head.position,
                    headRotation = head.rotation,
                };
                pose.leftArm.shoulderPosition = leftShoulder.position;
                pose.leftArm.shoulderRotation = leftShoulder.rotation;
                pose.leftArm.elbowPosition = leftElbow.position;
                pose.leftArm.elbowRotation = leftElbow.rotation;
                pose.leftArm.handPosition = leftHand.position;
                pose.leftArm.handRotation = leftHand.rotation;
                pose.rightArm.shoulderPosition = rightShoulder.position;
                pose.rightArm.shoulderRotation = rightShoulder.rotation;
                pose.rightArm.elbowPosition = rightElbow.position;
                pose.rightArm.elbowRotation = rightElbow.rotation;
                pose.rightArm.handPosition = rightHand.position;
                pose.rightArm.handRotation = rightHand.rotation;
                return pose;
            }
            set
            {
                head.position = value.headPosition;
                head.rotation = value.headRotation;
                leftShoulder.position = value.leftArm.shoulderPosition;
                leftShoulder.rotation = value.leftArm.shoulderRotation;
                leftElbow.position = value.leftArm.elbowPosition;
                leftElbow.rotation = value.leftArm.elbowRotation;
                leftHand.position = value.leftArm.handPosition;
                leftHand.rotation = value.leftArm.handRotation;
                rightShoulder.position = value.rightArm.shoulderPosition;
                rightShoulder.rotation = value.rightArm.shoulderRotation;
                rightElbow.position = value.rightArm.elbowPosition;
                rightElbow.rotation = value.rightArm.elbowRotation;
                rightHand.position = value.rightArm.handPosition;
                rightHand.rotation = value.rightArm.handRotation;
            }
        }

        private void OnDrawGizmos()
        {
            if (drawUncalibratedSkeletonGizmos)
            {
                DrawSkeleton(UncalibratedWorldPose);
            }

            if (drawCalibratedSkeletonGizmos)
            {
                DrawSkeleton(CalibratedWorldPose);
            }
        }

        private void DrawSkeleton(Pose pose)
        {
            Gizmos.DrawLine(pose.leftArm.shoulderPosition, pose.leftArm.elbowPosition);
            Gizmos.DrawLine(pose.leftArm.elbowPosition, pose.leftArm.handPosition);

            Gizmos.DrawLine(pose.rightArm.shoulderPosition, pose.rightArm.elbowPosition);
            Gizmos.DrawLine(pose.rightArm.elbowPosition, pose.rightArm.handPosition);
        }
    }
}