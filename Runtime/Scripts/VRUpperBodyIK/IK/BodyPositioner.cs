using VRUpperBodyIK.Skeleton;

namespace VRUpperBodyIK.IK
{
    public class BodyPositioner : Positioner
    {
        public BodySettings Settings { get; } = new();

        public void Update(Pose pose, BodySettings bodySettings)
        {
            bodySettings.HeadNeckDistance = Settings.HeadNeckDistance;
            bodySettings.NeckShoulderDistance = Settings.NeckShoulderDistance;
            bodySettings.HandElbowDistance = Settings.HandElbowDistance;
            bodySettings.ElbowShoulderDistance = Settings.ElbowShoulderDistance;
            bodySettings.Height = Settings.Height;
        }
    }
}
