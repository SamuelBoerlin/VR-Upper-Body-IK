using UnityEngine;
using VRUpperBodyIK.Skeleton;

namespace VRUpperBodyIK.IK
{
    public class ShoulderPositioner : Positioner
    {
        private Vector3 neckPosition;
        private bool flipNeckRotation;
        private bool lastNeckForwardDirSet;
        private Vector3 lastNeckForwardDir;
        private float neckForwardAngle;
        private Vector3 neckForwardDir;

        private float shoulderForwardYaw;
        private float shoulderForwardRoll;

        public Quaternion ShoulderRotation => Quaternion.AngleAxis(shoulderForwardRoll, Vector3.forward) * Quaternion.AngleAxis(shoulderForwardYaw, Vector3.up);

        public void Update(Skeleton.Pose pose, BodySettings bodySettings)
        {
            PositionNeck(pose, bodySettings);

            PositionShoulders(pose, bodySettings);

            pose.leftArm.shoulderRotation = pose.rightArm.shoulderRotation = ShoulderRotation;
        }

        public void PostUpdate(Skeleton.Pose pose, BodySettings bodySettings)
        {
            RotateShoulderWithElbow(pose.leftArm);
            RotateShoulderWithElbow(pose.rightArm);
        }

        private void PositionNeck(Skeleton.Pose pose, BodySettings bodySettings)
        {
            Quaternion neckRotation = pose.headRotation;

            if(bodySettings.Height > 0.1f)
            {
                float b = 0.333f;
                float b0 = 135.3f;

                float pitch = (bodySettings.Height - pose.headPosition.y) / bodySettings.Height * (b0 + b * pose.headRotation.eulerAngles.x);

                neckRotation = Quaternion.AngleAxis(pose.headRotation.eulerAngles.y, Vector3.up) * Quaternion.AngleAxis(pitch, Vector3.right);
            }

            neckPosition = pose.headPosition + neckRotation * Vector3.down * bodySettings.HeadNeckDistance;

            RotateNeck(pose, neckPosition);
            ConstrainNeckToHead(pose);

            pose.neckPosition = neckPosition;

            float neckRollCorrectionRange = 22.5f;
            float neckRoll = -shoulderForwardRoll + Mathf.Clamp(Mathf.DeltaAngle(-shoulderForwardRoll, pose.headRotation.eulerAngles.z) * 0.5f, -neckRollCorrectionRange, neckRollCorrectionRange);
            pose.neckRotation = Quaternion.AngleAxis(neckForwardAngle, Vector3.up) * Quaternion.AngleAxis(neckRoll, Vector3.forward);
        }

        private void RotateNeck(Skeleton.Pose pose, Vector3 neckPosition)
        {
            Vector3 lho = pose.leftArm.handPosition - neckPosition;
            lho = new Vector3(lho.x, 0, lho.z).normalized;

            Vector3 rho = pose.rightArm.handPosition - neckPosition;
            rho = new Vector3(rho.x, 0, rho.z).normalized;

            neckForwardDir = lho + rho;
            neckForwardDir = new Vector3(neckForwardDir.x, 0, neckForwardDir.z).normalized;

            /*Debug.DrawRay(neckPosition - Vector3.Cross(neckForwardDir, Vector3.up) * 0.5f, Vector3.Cross(neckForwardDir, Vector3.up), new Color(1.0f, 0.0f, 1.0f));
            Debug.DrawRay(neckPosition, neckForwardDir * 0.5f, new Color(1.0f, 0.0f, 1.0f));*/

            neckForwardAngle = Mathf.Atan2(neckForwardDir.x, neckForwardDir.z) * Mathf.Rad2Deg;

            // TODO This isn't totally reliable yet...

            if (lastNeckForwardDirSet && Vector3.Dot(lastNeckForwardDir, neckForwardDir) < 0)
            {
                flipNeckRotation = !flipNeckRotation;
            }

            lastNeckForwardDir = neckForwardDir;
            lastNeckForwardDirSet = true;

            if (!flipNeckRotation)
            {
                neckForwardAngle += 180.0f;
                neckForwardDir *= -1.0f;
            }

            var headForwardDir = pose.headRotation * Vector3.forward;
            headForwardDir = new Vector3(headForwardDir.x, 0, headForwardDir.z).normalized;

            // If neck forward and head forward are almost opposite
            // then the neck rotation should almost certainly be flipped
            if (Vector3.Dot(neckForwardDir, headForwardDir) < -0.9f)
            {
                flipNeckRotation = !flipNeckRotation;
                neckForwardAngle += 180.0f;
                neckForwardDir *= -1.0f;
            }

            neckForwardAngle %= 360.0f;
        }

        private void ConstrainNeckToHead(Skeleton.Pose pose)
        {
            Vector3 headForwardDir = pose.headRotation * Vector3.forward;
            headForwardDir = new Vector3(headForwardDir.x, 0, headForwardDir.z).normalized;

            float headForwardAngle = Mathf.Atan2(headForwardDir.x, headForwardDir.z) * Mathf.Rad2Deg;

            float neckHeadAngleDifference = Mathf.DeltaAngle(neckForwardAngle, headForwardAngle);

            float blend = 1.0f / (1.0f + Mathf.Exp((Mathf.Abs(neckHeadAngleDifference) - 105.0f) * 0.1f));

            if (Mathf.Abs(headForwardAngle - neckForwardAngle) > 180)
            {
                if (neckForwardAngle > headForwardAngle)
                {
                    headForwardAngle += 360;
                }
                else
                {
                    neckForwardAngle += 360;
                }
            }

            neckForwardAngle = blend * neckForwardAngle + (1 - blend) * headForwardAngle;

            neckForwardAngle %= 360.0f;
        }

        private void PositionShoulders(Skeleton.Pose pose, BodySettings bodySettings)
        {
            shoulderForwardYaw = neckForwardAngle;
            shoulderForwardRoll = 0.0f;

            Vector3 shoulderOffsetDir = Vector3.Cross(neckForwardDir, Vector3.up);

            pose.leftArm.shoulderPosition = neckPosition - shoulderOffsetDir * bodySettings.NeckShoulderDistance;
            pose.rightArm.shoulderPosition = neckPosition + shoulderOffsetDir * bodySettings.NeckShoulderDistance;

            ConstrainShoulderToHand(pose.leftArm, bodySettings, false);
            ConstrainShoulderToHand(pose.rightArm, bodySettings, true);

            shoulderOffsetDir = ShoulderRotation * Vector3.right;

            pose.leftArm.shoulderPosition = neckPosition - shoulderOffsetDir * bodySettings.NeckShoulderDistance;
            pose.rightArm.shoulderPosition = neckPosition + shoulderOffsetDir * bodySettings.NeckShoulderDistance;
        }

        private void ConstrainShoulderToHand(Arm arm, BodySettings bodySettings, bool right)
        {
            shoulderForwardYaw += (right ? -1 : 1) * CalculateRotateTowardsAngle(arm, neckForwardDir, bodySettings);
            shoulderForwardRoll += (right ? -1 : 1) * CalculateRotateTowardsAngle(arm, Vector3.up, bodySettings);
        }

        private float CalculateRotateTowardsAngle(Arm arm, Vector3 dir, BodySettings bodySettings)
        {
            Vector3 sth = arm.handPosition - arm.shoulderPosition;

            float c = 30.0f;
            float d = 0.5f;

            float angle = c * Mathf.Clamp(Vector3.Dot(sth, dir) / bodySettings.ArmLength, -1.0f, 1.0f) - d;

            return angle;
        }

        private void RotateShoulderWithElbow(Arm arm)
        {
            Vector3 shoulderY = arm.elbowRotation * Vector3.up;
            Vector3 shoulderZ = (arm.elbowPosition - arm.shoulderPosition).normalized;
            arm.shoulderRotation = Quaternion.LookRotation(shoulderZ, shoulderY);
        }
    }
}