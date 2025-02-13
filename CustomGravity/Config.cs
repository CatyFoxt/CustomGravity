using System.Collections.Generic;
using System.ComponentModel;

namespace CustomGravity
{
    public class Config
    {

        [Description("A Gravity Vector. The default value is (-19.6). This will be the default Gravity value of the server.")]
        public float[] GravityVector { get; set; } = new float[3] { 0, -19.6f, 0 };

        [Description("A Gravity Vector for pickups. The default value is (-19.6). This will be the default Gravity value of the Pickups in the server.")]
        public float[] PickupGravityVector { get; set; } = new float[3] { 0, -19.6f, 0 };

        [Description("A gravity vector for each item type, check my github page for item names. These pickups will have their own gravity value. Not effected by default pickup gravity vector.")]
        public Dictionary<ItemType, float[]> SpesificItemGravity { get; set; } = new Dictionary<ItemType, float[]>()
        {
            {ItemType.Coin, new float[3] {0,-19.6f,0}},
            {ItemType.Coal, new float[3] {0, -19.6f, 0}}
        };
    }
}
