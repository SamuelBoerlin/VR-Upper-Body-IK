using VRUpperBodyIK.Skeleton;

namespace VRUpperBodyIK.IK
{
    public class Solver
    {
        private readonly PositionerProvider[] positioners;

        public Solver(PositionerProvider[] positioners)
        {
            this.positioners = positioners;
        }

        public void Solve(Pose pose)
        {
            var bodySettings = new BodySettings();

            if (positioners != null)
            {
                foreach (PositionerProvider provider in positioners)
                {
                    provider.Positioner.Update(pose, bodySettings);
                }
            }
        }
    }
}
