using UnityEngine;

namespace VRUpperBodyIK.Skeleton
{
    public class ArmScaler : MonoBehaviour
    {
        public enum Limb
        {
            LeftLowerArm,
            LeftUpperArm,
            RightLowerArm,
            RightUpperArm
        }

        [Tooltip("What kind of arm part this is.")]
        public Limb limb;

        [Tooltip("Skeleton to scale the arm to.")]
        public Skeleton skeleton;

        public bool offsetZ = false;

        public bool useCalibratedPose = true;

        private void LateUpdate()
        {
            var pose = useCalibratedPose ? skeleton.CalibratedWorldPose : skeleton.UncalibratedWorldPose;

            var joints = GetJointPositions(GetArm(pose));
            var limbLength = (joints.Lower - joints.Upper).magnitude;

            var scale = transform.localScale;
            scale.z = limbLength;
            transform.localScale = scale;

            if (offsetZ)
            {
                var pos = transform.localPosition;
                pos.z = limbLength * 0.5f;
                transform.localPosition = pos;
            }
        }

        private Arm GetArm(Pose pose)
        {
            return limb switch
            {
                Limb.RightLowerArm or Limb.RightUpperArm => pose.rightArm,
                _ => pose.leftArm,
            };
        }

        private (Vector3 Lower, Vector3 Upper) GetJointPositions(Arm arm)
        {
            return limb switch
            {
                Limb.LeftUpperArm or Limb.RightUpperArm => (arm.shoulderPosition, arm.elbowPosition),
                _ => (arm.elbowPosition, arm.handPosition),
            };
        }
    }
}
