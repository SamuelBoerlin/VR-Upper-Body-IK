namespace VRUpperBodyIK.IK
{
    public class BodySettings
    {
        public float HeadNeckDistance { get; set; }
        public float NeckShoulderDistance { get; set; }

        public float HandElbowDistance { get; set; }
        public float ElbowShoulderDistance { get; set; }

        public float ArmLength => HandElbowDistance + ElbowShoulderDistance;
    }
}