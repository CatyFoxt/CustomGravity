using System.ComponentModel;    

namespace CustomGravity
{
    public class Config
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Controls front/back gravity.")]
        public float GravityVectorX { get; set; } = 0;

        [Description("Controls up/down gravity.")]
        public float GravityVectorY { get; set; } = -19.6f;

        [Description("Controls right/left gravity.")]
        public float GravityVectorZ { get; set; } = 0;
    }
}
