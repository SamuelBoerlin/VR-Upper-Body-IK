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

        private bool isLeftArm;

        public ElbowPositioner(bool isLeftArm)
        {
            this.isLeftArm = isLeftArm;
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

            // TODO Clean this up

            //Debug.DrawRay(elbowPosition, elbowX * 0.25f, Color.red);
            //Debug.DrawRay(elbowPosition, elbowY * 0.25f, Color.green);
            //Debug.DrawRay(elbowPosition, elbowZ * 0.25f, Color.blue);*/

            //Vector3 handUp = handRotation * Vector3.up;

            //float x = Vector3.Dot(elbowY, handUp);
            //float y = Vector3.Dot(-elbowX, handUp);

            //Debug.DrawRay(handPosition, elbowY * x);
            //Debug.DrawRay(handPosition, -elbowX * y);

            //float handYaw = m * Mathf.Atan2(y, x) * Mathf.Rad2Deg;

            Quaternion elbowUp = Quaternion.LookRotation(elbowY, -elbowZ);
            Quaternion handUp = Quaternion.LookRotation(arm.handRotation * Vector3.up, arm.handRotation * Vector3.back);
            float handYaw = -m * (((Quaternion.Inverse(elbowUp) * handUp).eulerAngles.y + 180.0f) % 360.0f - 180.0f);

            //Vector3 handForward = handRotation * Vector3.forward;

            //float x = Vector3.Dot(elbowZ, handForward);
            //float y = Vector3.Dot(elbowY, handForward);

            //float handPitch = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

            Quaternion elbowForward = Quaternion.LookRotation(elbowZ, elbowY);
            Quaternion handForward = Quaternion.LookRotation(arm.handRotation * Vector3.forward, arm.handRotation * Vector3.up);
            float handPitch = m * (((Quaternion.Inverse(elbowForward) * handForward).eulerAngles.y + 180.0f) % 360.0f - 180.0f);

            {
                float weight = Mathf.Clamp(1.0f - Mathf.Abs(handPitch / 45.0f), 0.0f, 1.0f);

                float al = 0.0f;
                float cl = -1 / 600.0f;
                float au = 90.0f;
                float cu = 1 / 300.0f;

                if (handYaw < al) elbowRoll += weight * m * cl * Mathf.Pow(handYaw - al, 2.0f);
                if (handYaw > au) elbowRoll += weight * m * cu * Mathf.Pow(handYaw - au, 2.0f);
            }

            {
                float weight = 1.0f;// Mathf.Clamp(1.0f - Mathf.Abs(handYaw / 90.0f), 0.0f, 1.0f);

                float al = -45.0f;
                float cl = -1 / 135.0f;
                float au = 45.0f;
                float cu = 1 / 135.0f;

                if (handPitch < al) elbowRoll += weight * m * cl * Mathf.Pow(handPitch - al, 2.0f);
                if (handPitch > au) elbowRoll += weight * m * cu * Mathf.Pow(handPitch - au, 2.0f);
            }

            elbowPosition = elbowCenter + elbowRotation * Quaternion.AngleAxis(elbowRoll, Vector3.forward) * Vector3.up * elbowRadius;

            return elbowPosition;
        }
    }
}