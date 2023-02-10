using UnityEngine;

namespace VRUpperBodyIK.Skeleton
{
    [System.Serializable]
    public class Pose
    {
        public Vector3 headPosition = Vector3.zero;
        public Quaternion headRotation = Quaternion.identity;

        public Arm leftArm = new Arm();

        public Arm rightArm = new Arm();

        public Vector3 Position(int idx)
        {
            return idx switch
            {
                1 => leftArm.shoulderPosition,
                2 => leftArm.elbowPosition,
                3 => leftArm.handPosition,
                4 => rightArm.shoulderPosition,
                5 => rightArm.elbowPosition,
                6 => rightArm.handPosition,
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
                    leftArm.shoulderPosition = position;
                    break;
                case 2:
                    leftArm.elbowPosition = position;
                    break;
                case 3:
                    leftArm.handPosition = position;
                    break;
                case 4:
                    rightArm.shoulderPosition = position;
                    break;
                case 5:
                    rightArm.elbowPosition = position;
                    break;
                case 6:
                    rightArm.handPosition = position;
                    break;
            };
        }

        public Quaternion Rotation(int idx)
        {
            return idx switch
            {
                1 => leftArm.shoulderRotation,
                2 => leftArm.elbowRotation,
                3 => leftArm.handRotation,
                4 => rightArm.shoulderRotation,
                5 => rightArm.elbowRotation,
                6 => rightArm.handRotation,
                _ => headRotation,
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
                    leftArm.shoulderRotation = rotation;
                    break;
                case 2:
                    leftArm.elbowRotation = rotation;
                    break;
                case 3:
                    leftArm.handRotation = rotation;
                    break;
                case 4:
                    rightArm.shoulderRotation = rotation;
                    break;
                case 5:
                    rightArm.elbowRotation = rotation;
                    break;
                case 6:
                    rightArm.handRotation = rotation;
                    break;
            };
        }

        public float time;
    }
}