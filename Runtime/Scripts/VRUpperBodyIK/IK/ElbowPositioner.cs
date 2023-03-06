using UnityEngine;
using VRUpperBodyIK.Skeleton;

namespace VRUpperBodyIK.IK
{
    public class ElbowPositioner : Positioner
    {
        private float elbowRollAngle;
        private Vector3 elbowCenter;
        private Quaternion elbowRotation;
        private float elbowRadius;

        private readonly bool isLeftArm;
        private readonly bool isRightArm;

        public ElbowPositioner(bool isLeftArm)
        {
            this.isLeftArm = isLeftArm;
            this.isRightArm = !isLeftArm;
        }

        public void Update(Skeleton.Pose pose, BodySettings bodySettings)
        {
            PositionElbows(isLeftArm ? pose.leftArm : pose.rightArm, bodySettings);
        }

        private void PositionElbows(Arm arm, BodySettings bodySettings)
        {
            PositionElbowsByInnerAngle(arm, bodySettings);

            (elbowRollAngle, elbowCenter, elbowRotation, elbowRadius) = CalculateElbowRollAngleAndCenterFromModel(arm, bodySettings);

            var elbowRollRotation = Quaternion.AngleAxis(elbowRollAngle, Vector3.forward);

            arm.elbowPosition = elbowCenter + elbowRotation * elbowRollRotation * Vector3.up * elbowRadius;

            ConstrainElbowSingularitiesToFixedPosition(arm);

            ConstrainElbowsToHand(arm);

            elbowRollRotation = Quaternion.AngleAxis(elbowRollAngle, Vector3.forward);
            arm.elbowRotation = Quaternion.LookRotation((arm.handPosition - arm.elbowPosition).normalized, elbowRotation * elbowRollRotation * Vector3.left * (isLeftArm ? -1 : 1));

            Vector3 lhte = arm.elbowPosition - arm.handPosition;
            if (lhte.magnitude > bodySettings.HandElbowDistance)
            {
                arm.elbowPosition = arm.handPosition + lhte.normalized * bodySettings.HandElbowDistance;
            }
        }

        private void PositionElbowsByInnerAngle(Arm arm, BodySettings bodySettings)
        {
            // TODO Try using trigonometry instead

            Vector3 initialElbowOffset = Vector3.Cross(Vector3.Cross(arm.shoulderRotation * Vector3.forward, (arm.handPosition - arm.shoulderPosition).normalized), (arm.handPosition - arm.shoulderPosition).normalized).normalized;
            arm.elbowPosition = (arm.handPosition + arm.shoulderPosition) * 0.5f - initialElbowOffset * 0.1f;

            for (int i = 0; i < 10; ++i)
            {
                Vector3 hte = arm.elbowPosition - arm.handPosition;
                arm.elbowPosition = arm.handPosition + hte.normalized * bodySettings.HandElbowDistance;

                Vector3 ste = arm.elbowPosition - arm.shoulderPosition;
                arm.elbowPosition = arm.shoulderPosition + ste.normalized * bodySettings.ElbowShoulderDistance;
            }
        }

        private (float Angle, Vector3 Position, Quaternion Rotation, float Radius) CalculateElbowRollAngleAndCenterFromPosition(Arm arm)
        {
            Vector3 sth = arm.handPosition - arm.shoulderPosition;
            Vector3 ste = arm.elbowPosition - arm.shoulderPosition;
            Vector3 center = arm.shoulderPosition + sth.normalized * Vector3.Dot(sth.normalized, ste);
            float radius = (arm.elbowPosition - center).magnitude;

            Vector3 forward = sth.normalized;
            Vector3 up = Vector3.Cross(Vector3.Cross(sth.normalized, Vector3.up).normalized, sth.normalized);

            Vector3 dx = up;
            Vector3 dy = Vector3.Cross(up, forward);

            Vector3 rotatedDir = (arm.elbowPosition - center).normalized;

            float x = Vector3.Dot(dx, rotatedDir);
            float y = Vector3.Dot(dy, rotatedDir);

            float angle = -Mathf.Atan2(y, x) * Mathf.Rad2Deg;

            var rotation = Quaternion.LookRotation(forward, up);

            return (angle, center, rotation, radius);
        }

        private (float Angle, Vector3 Center, Quaternion Rotation, float Radius) CalculateElbowRollAngleAndCenterFromModel(Arm arm, BodySettings bodySettings)
        {
            Vector3 center;
            Quaternion rotation;
            float radius;
            (_, center, rotation, radius) = CalculateElbowRollAngleAndCenterFromPosition(arm);

            float angle = 15.0f;

            Vector3 localHandPos = Quaternion.Inverse(arm.shoulderRotation) * (arm.handPosition - arm.shoulderPosition) / bodySettings.ArmLength;

            float b1 = 30.0f;
            float b2 = 120.0f;
            float b3 = 65.0f;

            float w1 = -50.0f;
            float w2 = -60.0f;
            float w3 = 260.0f;

            angle += Mathf.Max(0.0f, localHandPos.x * (isLeftArm ? 1.0f : -1.0f) * w1 + b1);
            angle += Mathf.Max(0.0f, localHandPos.y * w2 + b2);
            angle += Mathf.Max(0.0f, localHandPos.z * w3 + b3);

            angle = Mathf.Clamp(angle, 13.0f, 175.0f);

            angle *= -1.0f;

            if (isLeftArm)
            {
                angle *= -1.0f;
            }

            return (angle, center, rotation, radius);
        }

        private void ConstrainElbowSingularitiesToFixedPosition(Arm arm)
        {
            Vector3 fixedPoseLeft = new Vector3(0.133f, -0.443f, -0.886f);
            Vector3 fixedPoseRight = new Vector3(fixedPoseLeft.x, fixedPoseLeft.y, -fixedPoseLeft.z);

            arm.elbowPosition = ConstrainElbowSingularityToFixedPosition(arm, isLeftArm ? fixedPoseLeft : fixedPoseRight);

            (elbowRollAngle, elbowCenter, elbowRotation, elbowRadius) = CalculateElbowRollAngleAndCenterFromPosition(arm);
        }

        private Vector3 ConstrainElbowSingularityToFixedPosition(Arm arm, Vector3 fixedPosition)
        {
            fixedPosition = arm.shoulderPosition + arm.shoulderRotation * Quaternion.AngleAxis(90, Vector3.up) * fixedPosition * 0.25f;
            return arm.elbowPosition + (fixedPosition - arm.elbowPosition) * CalculateElbowSingularityBlend(arm);
        }

        private float CalculateElbowSingularityBlend(Arm arm)
        {
            float threshold = 0.5f;
            float horizontalDst = Mathf.Sqrt((arm.handPosition.x - arm.shoulderPosition.x) * (arm.handPosition.x - arm.shoulderPosition.x) + (arm.handPosition.z - arm.shoulderPosition.z) * (arm.handPosition.z - arm.shoulderPosition.z));
            return horizontalDst < threshold ? 1.0f - horizontalDst / threshold : 0.0f;
        }

        private void ConstrainElbowsToHand(Arm arm)
        {
            arm.elbowPosition = ConstrainElbowToHand(arm, elbowRollAngle, elbowCenter, elbowRotation, elbowRadius);

            (elbowRollAngle, elbowCenter, elbowRotation, elbowRadius) = CalculateElbowRollAngleAndCenterFromPosition(arm);
        }

        private Vector3 ConstrainElbowToHand(Arm arm, float elbowRoll, Vector3 elbowCenter, Quaternion elbowRotation, float elbowRadius)
        {
            var elbowPosition = arm.elbowPosition;

            float m = !isLeftArm ? -1 : 1;

            Vector3 elbowZ = (arm.handPosition - elbowPosition).normalized;
            Vector3 elbowY = m * Vector3.Cross((elbowPosition - elbowCenter).normalized, elbowZ).normalized;
            Vector3 elbowX = Vector3.Cross(elbowY, elbowZ).normalized;

            if (elbowZ.magnitude < 0.1f || elbowY.magnitude < 0.1f || elbowX.magnitude < 0.1f)
            {
                return elbowPosition;
            }

            float elbowRelativeHandYaw = (isLeftArm ? 1 : -1) * Mathf.DeltaAngle(elbowRoll - (isLeftArm ? 1 : -1) * 90, arm.handRotation.eulerAngles.z);
            float elbowRelativeHandPitch = Mathf.DeltaAngle(0.0f, arm.handRotation.eulerAngles.x);

            static float saturate(float value, float pow) => (1.0f - 1.0f / Mathf.Pow(Mathf.Abs(value) + 1, pow)) * Mathf.Sign(value);

            {
                float wl = 120.0f;
                float wu = 60.0f;

                float pow = 0.2f;

                float al = 0.0f;
                float cl = -1 / 600.0f;
                float au = 90.0f;
                float cu = 1 / 300.0f;

                if (elbowRelativeHandYaw < al) elbowRoll += wl * m * saturate(cl * Mathf.Pow(elbowRelativeHandYaw - al, 2.0f), pow);
                if (elbowRelativeHandYaw > au) elbowRoll += wu * m * saturate(cu * Mathf.Pow(elbowRelativeHandYaw - au, 2.0f), pow);
            }

            {
                float wl = 120.0f;
                float wu = 60.0f;

                float pow = 0.2f;

                float al = -45.0f;
                float cl = -1 / 135.0f;
                float au = 45.0f;
                float cu = 1 / 135.0f;

                if (elbowRelativeHandPitch < al) elbowRoll -= wl * m * saturate(cl * Mathf.Pow(elbowRelativeHandPitch - al, 2.0f), pow);
                if (elbowRelativeHandPitch > au) elbowRoll -= wu * m * saturate(cu * Mathf.Pow(elbowRelativeHandPitch - au, 2.0f), pow);
            }

            elbowPosition = elbowCenter + elbowRotation * Quaternion.AngleAxis(elbowRoll, Vector3.forward) * Vector3.up * elbowRadius;

            return elbowPosition;
        }
    }
}