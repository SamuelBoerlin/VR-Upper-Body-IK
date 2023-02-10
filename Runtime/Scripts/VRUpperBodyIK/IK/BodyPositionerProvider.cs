namespace VRUpperBodyIK.IK
{
    public class BodyPositionerProvider : PositionerProviderBehaviour
    {
        public float headNeckDistance = 0.18f;
        public float neckShoulderDistance = 0.17f;

        public float handElbowDistance = 0.42f;
        public float elbowShoulderDistance = 0.26f;

        private readonly BodyPositioner bodyPositioner = new();

        public override Positioner Positioner => bodyPositioner;

        private void Awake()
        {
            ApplySettings();
        }

        private void Update()
        {
            ApplySettings();
        }

        private void ApplySettings()
        {
            bodyPositioner.Settings.HeadNeckDistance = headNeckDistance;
            bodyPositioner.Settings.NeckShoulderDistance = neckShoulderDistance;
            bodyPositioner.Settings.HandElbowDistance = handElbowDistance;
            bodyPositioner.Settings.ElbowShoulderDistance = elbowShoulderDistance;
        }
    }
}
