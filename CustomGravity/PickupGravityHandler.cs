using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace CustomGravity
{
    public class PickupGravityHandler
    {
        // A Function that checks every pickup and applies the right gravity.
        public static void CheckAllItemsForGravity()
        {
            // Iterate every pickup and apply the right gravity.
            foreach (Pickup pickup in Pickup.List)
            {
                if (pickup != null)
                {
                    CheckItemForGravity(pickup);
                }
            }
        }

        // A Function that checks a item for which gravity will be applied.
        public static void CheckItemForGravity(Pickup pickup)
        {
            // Define plugin and config values.
            CustomGravity plugin = CustomGravity.Instance;
            Config config = plugin.Config;

            // Check the config if the default pickup gravity vector is set correctly.
            if (config.PickupGravityVector.Count() != 3)
            {
                LabApi.Features.Console.Logger.Error("Please check your Config! Your Pickup Gravity Vector value is not set correctly!");
                return;
            }

            // Define the default gravity value.
            Vector3 gravity = new Vector3(config.PickupGravityVector[0], config.PickupGravityVector[1], config.PickupGravityVector[2]);

            // Check if there is an exception made for the item type.
            if (config.SpesificItemGravity.Keys.Contains(pickup.Type))
            {
                // Set the array for values, so we dont write the long config name again.
                float[] itemGravityValues = config.SpesificItemGravity[pickup.Type];

                // Check if the gravity vector 3 correctly set in the config.
                if (itemGravityValues.Count() != 3)
                {
                    LabApi.Features.Console.Logger.Error("Please check your Config! You have problems at: " + pickup.Type + " gravity value! Please set it correctly.");
                    return;
                }

                gravity = new Vector3(itemGravityValues[0], itemGravityValues[1], itemGravityValues[2]);
            }

            // Check if there is a general alteration of pickups.
            if (plugin.UseTemporaryPickupGravity)
            {
                gravity = plugin.TemporaryPickupGravity;
            }

            // Check if there is some altered item types.
            if (plugin.AlteredItemTypes.Keys.Contains(pickup.Type))
            {
                gravity = plugin.AlteredItemTypes[pickup.Type];
            }

            // Check if it is the default gravity value.
            if (gravity.x == 0 && gravity.y == -19.6f && gravity.z == 0)
            {
                RemovePickupGravity(pickup);
            }
            // If not the default gravity value, apply gravity
            else
            {
                SetPickupGravity(pickup, gravity);
            }
        }

        // Makes us able to Set a Pickups gravity easily.
        public static void SetPickupGravity(Pickup pickup, Vector3 gravity)
        {
            // Return if pickup is null
            if (pickup == null) return;

            // Check if Rigidbody exists.
            if (pickup.GameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                // Disable the default Rigidbody gravity so we can add our own one.
                // We are doing this because we dont want to change the whole games Gravity value.
                rigidbody.useGravity = false;

                // Add a Constant Force component to simulate our own force
                if (pickup.GameObject.TryGetComponent<ConstantForce>(out ConstantForce constantForce))
                {
                    constantForce.force = gravity;
                }
                else
                {
                    pickup.GameObject.AddComponent<ConstantForce>().force = gravity;
                }
                
            }
            
        }

        // Makes us able to remove the pickups force component and enable the default rigidbody gravity.
        public static void RemovePickupGravity(Pickup pickup)
        {
            // Return if pickup is null
            if (pickup == null) return;

            // Check if Rigidbody exists.
            if (pickup.GameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                // Enable the rigidbody gravity.
                rigidbody.useGravity = true;
                
                // Check if the Constant Force component exists.
                if (pickup.GameObject.TryGetComponent<ConstantForce>(out ConstantForce constantForce))
                {
                    // Destroy the Constant Force component. Atomize it...
                    GameObject.Destroy(constantForce);
                }
            }
        }
    }
}
