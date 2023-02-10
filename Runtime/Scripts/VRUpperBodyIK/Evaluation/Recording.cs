using System.Collections.Generic;
using VRUpperBodyIK.Skeleton;

namespace VRUpperBodyIK.Evaluation
{
    [System.Serializable]
    public class Recording
    {
        public List<Pose> poses = new();
    }
}