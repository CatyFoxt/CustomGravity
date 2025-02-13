using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using UnityEngine;

namespace CustomGravity
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class pickupgravity : ICommand
    {
        public string Command { get; } = "pickupgravity";
        public string[] Aliases { get; } = new string[] { "pickupgrav", "pgravity", "pgrav" };
        public string Description { get; } = "Modify all pickups gravity value.";
        public string FailMessage { get; } = "Usage: pickupgravity (ItemType, leave empty for every item) (X) (Y) (Z) \n To Reset: pickupgravity (reset)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // Define variables
            CustomGravity plugin = CustomGravity.Instance;
            bool isEveryItem = false;
            ItemType selectedItem = ItemType.None;

            // Check the config if the default gravity vector is set correctly.
            if (plugin.Config.PickupGravityVector.Count() != 3)
            {
                response = "Please check your Config! Your Pickup Gravity Vector value is not set correctly!";
                return false;
            }

            // If no argument is given, just give error message.
            if (arguments.Count == 0)
            {
                response = FailMessage;
                return false;
            }

            // If there is only one argument and it is reset or default. Reset everything.
            if (arguments.Count == 1 && (arguments.At(0) == "reset" || arguments.At(0) == "default"))
            {
                
                plugin.UseTemporaryPickupGravity = false;
                plugin.AlteredItemTypes.Clear();

                PickupGravityHandler.CheckAllItemsForGravity();

                response = "Gravity values resetted for entire server! Welcome to earth astronaut.";
                return true;
            }

            // Check if the argument size is correct.
            if (arguments.Count < 3)
            {
                response = FailMessage;
                return false;
            }
            // If there is only X Y Z arguments, set gravity for whole server.
            if (arguments.Count == 3)
            {
                isEveryItem = true;
            }
            // If not try find the player from name or id.
            else
            {
                if (Enum.TryParse(arguments.At(0), out ItemType itemType))
                {
                    selectedItem = itemType;
                }
                else
                {
                    response = "Please enter a valid Item Type. To see all the types Use: pickupgravity list";
                    return false;
                }
            }

            // Define the start position of the gravity vector arguments.
            int argumentPosition = 1;
            if (arguments.Count == 3) argumentPosition -= 1;

            // Check the arguments if they are a valid float or not and apply the gravity for players.
            if (float.TryParse(arguments.At(argumentPosition), out float X) && float.TryParse(arguments.At(argumentPosition + 1), out float Y) && float.TryParse(arguments.At(argumentPosition + 2), out float Z))
            {
                // Apply gravity to everyone.
                if (isEveryItem)
                {
                    // Clear the altered items because this defines a new alteration for every item.
                    plugin.AlteredItemTypes.Clear();

                    // Set the temporary pickup gravity values
                    plugin.TemporaryPickupGravity = new Vector3(X, Y, Z);
                    plugin.UseTemporaryPickupGravity = true;

                    PickupGravityHandler.CheckAllItemsForGravity();

                    response = "Gravity for every item type is now " + $"({X}, {Y}, {Z})!";
                    return true;
                }
                // Apply gravity for spesific item type.
                else
                {
                    Vector3 gravity = new UnityEngine.Vector3(X, Y, Z);

                    if (plugin.AlteredItemTypes.Keys.Contains(selectedItem))
                    {
                        plugin.AlteredItemTypes.Remove(selectedItem);
                    }

                    plugin.AlteredItemTypes.Add(selectedItem, gravity);

                    foreach (Pickup pickup in Pickup.List)
                    {
                        if (pickup.Type == selectedItem)
                        {
                            PickupGravityHandler.SetPickupGravity(pickup, new Vector3(X, Y, Z));
                        }
                    }

                    response = "Gravity for " + selectedItem + " type is now " + $"({X}, {Y}, {Z})!";
                    return true;
                }
            }
            else
            {
                // If arguments couldnt be parsed to float. Good luck!
                response = "Please enter numbers as the gravity! " + FailMessage;
                return false;
            }
        }
    }
}