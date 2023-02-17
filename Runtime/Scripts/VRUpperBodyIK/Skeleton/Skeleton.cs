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

        public bool calibrateHead = true;

        public bool calibrateNeck = true;

        public bool calibrateLeftShoulder = true;
        public bool calibrateLeftElbow = true;
        public bool calibrateLeftHand = true;

        public bool calibrateRightShoulder = true;
        public bool calibrateRightElbow = true;
        public bool calibrateRightHand = true;

        public bool drawUncalibratedSkeletonGizmos;
        public bool drawCalibratedSkeletonGizmos;

        private static readonly Pose identityPose = new Pose();

        private Pose GetOffsetPose(bool calibrate)
        {
            return calibrate ? calibrationData.Offset : identityPose;
        }

        public Pose CalibratedLocalPose
        {
            get
            {
                if (calibrationData != null)
                {
                    Pose offset;

                    var pose = new Pose();

                    if (head != null)
                    {
                        offset = GetOffsetPose(calibrateHead);
                        pose.headPosition = head.localPosition + head.localRotation * offset.headRotation * offset.headPosition;
                        pose.headRotation = head.localRotation * offset.headRotation;
                    }

                    if (neck != null)
                    {
                        offset = GetOffsetPose(calibrateNeck);
                        pose.neckPosition = neck.localPosition + neck.localRotation * offset.neckRotation * offset.neckPosition;
                        pose.neckRotation = neck.localRotation * offset.neckRotation;
                    }

                    if (leftShoulder != null)
                    {
                        offset = GetOffsetPose(calibrateLeftShoulder);
                        pose.leftArm.shoulderPosition = leftShoulder.localPosition + leftShoulder.localRotation * offset.leftArm.shoulderRotation * offset.leftArm.shoulderPosition;
                        pose.leftArm.shoulderRotation = leftShoulder.localRotation * offset.leftArm.shoulderRotation;
                    }

                    if (leftElbow != null)
                    {
                        offset = GetOffsetPose(calibrateLeftElbow);
                        pose.leftArm.elbowPosition = leftElbow.localPosition + leftElbow.localRotation * offset.leftArm.elbowRotation * offset.leftArm.elbowPosition;
                        pose.leftArm.elbowRotation = leftElbow.localRotation * offset.leftArm.elbowRotation;
                    }

                    if (leftHand != null)
                    {
                        offset = GetOffsetPose(calibrateLeftHand);
                        pose.leftArm.handPosition = leftHand.localPosition + leftHand.localRotation * offset.leftArm.handRotation * offset.leftArm.handPosition;
                        pose.leftArm.handRotation = leftHand.localRotation * offset.leftArm.handRotation;
                    }

                    if (rightShoulder != null)
                    {
                        offset = GetOffsetPose(calibrateRightShoulder);
                        pose.rightArm.shoulderPosition = rightShoulder.localPosition + rightShoulder.localRotation * offset.rightArm.shoulderRotation * offset.rightArm.shoulderPosition;
                        pose.rightArm.shoulderRotation = rightShoulder.localRotation * offset.rightArm.shoulderRotation;
                    }

                    if (rightElbow != null)
                    {
                        offset = GetOffsetPose(calibrateRightElbow);
                        pose.rightArm.elbowPosition = rightElbow.localPosition + rightElbow.localRotation * offset.rightArm.elbowRotation * offset.rightArm.elbowPosition;
                        pose.rightArm.elbowRotation = rightElbow.localRotation * offset.rightArm.elbowRotation;
                    }

                    if (rightHand != null)
                    {
                        offset = GetOffsetPose(calibrateRightHand);
                        pose.rightArm.handPosition = rightHand.localPosition + rightHand.localRotation * offset.rightArm.handRotation * offset.rightArm.handPosition;
                        pose.rightArm.handRotation = rightHand.localRotation * offset.rightArm.handRotation;
                    }

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
                    Pose offset;

                    var pose = new Pose();

                    if (head != null)
                    {
                        offset = GetOffsetPose(calibrateHead);
                        pose.headPosition = head.position + head.rotation * offset.headRotation * offset.headPosition;
                        pose.headRotation = head.rotation * offset.headRotation;
                    }

                    if (neck != null)
                    {
                        offset = GetOffsetPose(calibrateNeck);
                        pose.neckPosition = neck.position + neck.rotation * offset.neckRotation * offset.neckPosition;
                        pose.neckRotation = neck.rotation * offset.neckRotation;
                    }

                    if (leftShoulder != null)
                    {
                        offset = GetOffsetPose(calibrateLeftShoulder);
                        pose.leftArm.shoulderPosition = leftShoulder.position + leftShoulder.rotation * offset.leftArm.shoulderRotation * offset.leftArm.shoulderPosition;
                        pose.leftArm.shoulderRotation = leftShoulder.rotation * offset.leftArm.shoulderRotation;
                    }

                    if (leftElbow != null)
                    {
                        offset = GetOffsetPose(calibrateLeftElbow);
                        pose.leftArm.elbowPosition = leftElbow.position + leftElbow.rotation * offset.leftArm.elbowRotation * offset.leftArm.elbowPosition;
                        pose.leftArm.elbowRotation = leftElbow.rotation * offset.leftArm.elbowRotation;
                    }

                    if (leftHand != null)
                    {
                        offset = GetOffsetPose(calibrateLeftHand);
                        pose.leftArm.handPosition = leftHand.position + leftHand.rotation * offset.leftArm.handRotation * offset.leftArm.handPosition;
                        pose.leftArm.handRotation = leftHand.rotation * offset.leftArm.handRotation;
                    }

                    if (rightShoulder != null)
                    {
                        offset = GetOffsetPose(calibrateRightShoulder);
                        pose.rightArm.shoulderPosition = rightShoulder.position + rightShoulder.rotation * offset.rightArm.shoulderRotation * offset.rightArm.shoulderPosition;
                        pose.rightArm.shoulderRotation = rightShoulder.rotation * offset.rightArm.shoulderRotation;
                    }

                    if (rightElbow != null)
                    {
                        offset = GetOffsetPose(calibrateRightElbow);
                        pose.rightArm.elbowPosition = rightElbow.position + rightElbow.rotation * offset.rightArm.elbowRotation * offset.rightArm.elbowPosition;
                        pose.rightArm.elbowRotation = rightElbow.rotation * offset.rightArm.elbowRotation;
                    }

                    if (rightHand != null)
                    {
                        offset = GetOffsetPose(calibrateRightHand);
                        pose.rightArm.handPosition = rightHand.position + rightHand.rotation * offset.rightArm.handRotation * offset.rightArm.handPosition;
                        pose.rightArm.handRotation = rightHand.rotation * offset.rightArm.handRotation;
                    }

                    return pose;
                }
                return UncalibratedWorldPose;
            }
        }

        public Pose UncalibratedLocalPose
        {
            get
            {
                var pose = new Pose();

                if (head != null)
                {
                    pose.headPosition = head.localPosition;
                    pose.headRotation = head.localRotation;
                }

                if (neck != null)
                {
                    pose.neckPosition = neck.localPosition;
                    pose.neckRotation = neck.localRotation;
                }

                if (leftShoulder != null)
                {
                    pose.leftArm.shoulderPosition = leftShoulder.localPosition;
                    pose.leftArm.shoulderRotation = leftShoulder.localRotation;
                }

                if (leftElbow != null)
                {
                    pose.leftArm.elbowPosition = leftElbow.localPosition;
                    pose.leftArm.elbowRotation = leftElbow.localRotation;
                }

                if (leftHand != null)
                {
                    pose.leftArm.handPosition = leftHand.localPosition;
                    pose.leftArm.handRotation = leftHand.localRotation;
                }

                if (rightShoulder != null)
                {
                    pose.rightArm.shoulderPosition = rightShoulder.localPosition;
                    pose.rightArm.shoulderRotation = rightShoulder.localRotation;
                }

                if (rightElbow != null)
                {
                    pose.rightArm.elbowPosition = rightElbow.localPosition;
                    pose.rightArm.elbowRotation = rightElbow.localRotation;
                }

                if (rightHand != null)
                {
                    pose.rightArm.handPosition = rightHand.localPosition;
                    pose.rightArm.handRotation = rightHand.localRotation;
                }

                return pose;
            }
            set
            {
                if (head != null)
                {
                    head.localPosition = value.headPosition;
                    head.localRotation = value.headRotation;
                }

                if (neck != null)
                {
                    neck.localPosition = value.neckPosition;
                    neck.localRotation = value.neckRotation;
                }

                if (leftShoulder != null)
                {
                    leftShoulder.localPosition = value.leftArm.shoulderPosition;
                    leftShoulder.localRotation = value.leftArm.shoulderRotation;
                }

                if (leftElbow != null)
                {
                    leftElbow.localPosition = value.leftArm.elbowPosition;
                    leftElbow.localRotation = value.leftArm.elbowRotation;
                }

                if (leftHand != null)
                {
                    leftHand.localPosition = value.leftArm.handPosition;
                    leftHand.localRotation = value.leftArm.handRotation;
                }

                if (rightShoulder != null)
                {
                    rightShoulder.localPosition = value.rightArm.shoulderPosition;
                    rightShoulder.localRotation = value.rightArm.shoulderRotation;
                }

                if (rightElbow != null)
                {
                    rightElbow.localPosition = value.rightArm.elbowPosition;
                    rightElbow.localRotation = value.rightArm.elbowRotation;
                }

                if (rightHand != null)
                {
                    rightHand.localPosition = value.rightArm.handPosition;
                    rightHand.localRotation = value.rightArm.handRotation;
                }
            }
        }

        public Pose UncalibratedWorldPose
        {
            get
            {
                var pose = new Pose();

                if (head != null)
                {
                    pose.headPosition = head.position;
                    pose.headRotation = head.rotation;
                }

                if (neck != null)
                {
                    pose.neckPosition = neck.position;
                    pose.neckRotation = neck.rotation;
                }

                if (leftShoulder != null)
                {
                    pose.leftArm.shoulderPosition = leftShoulder.position;
                    pose.leftArm.shoulderRotation = leftShoulder.rotation;
                }

                if (leftElbow != null)
                {
                    pose.leftArm.elbowPosition = leftElbow.position;
                    pose.leftArm.elbowRotation = leftElbow.rotation;
                }

                if (leftHand != null)
                {
                    pose.leftArm.handPosition = leftHand.position;
                    pose.leftArm.handRotation = leftHand.rotation;
                }

                if (rightShoulder != null)
                {
                    pose.rightArm.shoulderPosition = rightShoulder.position;
                    pose.rightArm.shoulderRotation = rightShoulder.rotation;
                }

                if (rightElbow != null)
                {
                    pose.rightArm.elbowPosition = rightElbow.position;
                    pose.rightArm.elbowRotation = rightElbow.rotation;
                }

                if (rightHand != null)
                {
                    pose.rightArm.handPosition = rightHand.position;
                    pose.rightArm.handRotation = rightHand.rotation;
                }

                return pose;
            }
            set
            {
                if (head != null)
                {
                    head.position = value.headPosition;
                    head.rotation = value.headRotation;
                }

                if (neck != null)
                {
                    neck.position = value.neckPosition;
                    neck.rotation = value.neckRotation;
                }

                if (leftShoulder != null)
                {
                    leftShoulder.position = value.leftArm.shoulderPosition;
                    leftShoulder.rotation = value.leftArm.shoulderRotation;
                }

                if (leftElbow != null)
                {
                    leftElbow.position = value.leftArm.elbowPosition;
                    leftElbow.rotation = value.leftArm.elbowRotation;
                }

                if (leftHand != null)
                {
                    leftHand.position = value.leftArm.handPosition;
                    leftHand.rotation = value.leftArm.handRotation;
                }

                if (rightShoulder != null)
                {
                    rightShoulder.position = value.rightArm.shoulderPosition;
                    rightShoulder.rotation = value.rightArm.shoulderRotation;
                }

                if (rightElbow != null)
                {
                    rightElbow.position = value.rightArm.elbowPosition;
                    rightElbow.rotation = value.rightArm.elbowRotation;
                }

                if (rightHand != null)
                {
                    rightHand.position = value.rightArm.handPosition;
                    rightHand.rotation = value.rightArm.handRotation;
                }
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