using UnityEngine;

namespace VRUpperBodyIK.Skeleton
{
    [System.Serializable]
    public class Arm
    {
        public Vector3 shoulderPosition = Vector3.zero;
        public Quaternion shoulderRotation = Quaternion.identity;

        public Vector3 elbowPosition = Vector3.zero;
        public Quaternion elbowRotation = Quaternion.identity;

        public Vector3 handPosition = Vector3.zero;
        public Quaternion handRotation = Quaternion.identity;
    }
}