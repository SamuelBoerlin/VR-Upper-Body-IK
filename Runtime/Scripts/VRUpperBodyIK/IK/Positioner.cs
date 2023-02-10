using VRUpperBodyIK.Skeleton;

namespace VRUpperBodyIK.IK
{
    public interface Positioner
    {
        void Update(Pose pose, BodySettings bodySettings);
    }
}