using UnityEngine;

namespace VRUpperBodyIK.Skeleton
{
    [System.Serializable]
    public class Pose
    {
        public Vector3 headPosition = Vector3.zero;
        public Quaternion headRotation = Quaternion.identity;

        public Vector3 neckPosition = Vector3.zero;
        public Quaternion neckRotation = Quaternion.identity;

        public Arm leftArm = new Arm();

        public Arm rightArm = new Arm();

        public Vector3 Position(int idx)
        {
            return idx switch
            {
                1 => neckPosition,
                2 => leftArm.shoulderPosition,
                3 => leftArm.elbowPosition,
                4 => leftArm.handPosition,
                5 => rightArm.shoulderPosition,
                6 => rightArm.elbowPosition,
                7 => rightArm.handPosition,
                _ => headPosition
            };
        }

        public void SetPosition(int idx, Vector3 position)
        {
            switch (idx)
            {
                default:
                    headPosition = position;
                    break;
                case 1:
                    neckPosition = position;
                    break;
                case 2:
                    leftArm.shoulderPosition = position;
                    break;
                case 3:
                    leftArm.elbowPosition = position;
                    break;
                case 4:
                    leftArm.handPosition = position;
                    break;
                case 5:
                    rightArm.shoulderPosition = position;
                    break;
                case 6:
                    rightArm.elbowPosition = position;
                    break;
                case 7:
                    rightArm.handPosition = position;
                    break;
            };
        }

        public Quaternion Rotation(int idx)
        {
            return idx switch
            {
                1 => neckRotation,
                2 => leftArm.shoulderRotation,
                3 => leftArm.elbowRotation,
                4 => leftArm.handRotation,
                5 => rightArm.shoulderRotation,
                6 => rightArm.elbowRotation,
                7 => rightArm.handRotation,
                _ => headRotation
            };
        }

        public void SetRotation(int idx, Quaternion rotation)
        {
            switch (idx)
            {
                default:
                    headRotation = rotation;
                    break;
                case 1:
                    neckRotation = rotation;
                    break;
                case 2:
                    leftArm.shoulderRotation = rotation;
                    break;
                case 3:
                    leftArm.elbowRotation = rotation;
                    break;
                case 4:
                    leftArm.handRotation = rotation;
                    break;
                case 5:
                    rightArm.shoulderRotation = rotation;
                    break;
                case 6:
                    rightArm.elbowRotation = rotation;
                    break;
                case 7:
                    rightArm.handRotation = rotation;
                    break;
            };
        }

        public float time;
    }
}