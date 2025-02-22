﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using UnityEngine;

namespace CustomGravity
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class setgravity : ICommand
    {
        public string Command { get; } = "setgravity";
        public string[] Aliases { get; } = new string[] { "setgrav", "setg" };
        public string Description { get; } = "Modify spesific players gravity. Or everyones with '*'.";
        public string FailMessage { get; } = "Usage: setgravity (Player Name, Player ID, *) (X) (Y) (Z) \n To Reset: setgravity (reset)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // Define variables.
            CustomGravity plugin = CustomGravity.Instance;
            Player player = null;
            bool isEveryone = false;

            // Check the config if the default gravity vector is set correctly.
            if (plugin.Config.GravityVector.Count() != 3)
            {
                response = "Please check your Config! Your Gravity Vector value is not set correctly!";
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
                
                plugin.UseTemporaryGravity = false;
                plugin.AlteredGravityPlayers.Clear();

                foreach (Player listPlayer in Player.List)
                {
                    listPlayer.Gravity = new UnityEngine.Vector3(plugin.Config.GravityVector[0], plugin.Config.GravityVector[1], plugin.Config.GravityVector[2]);
                }

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
                isEveryone = true;
            }
            // If not try find the player from name or id.
            else
            {
                // Check the first arguemnt is an int. Then find the player from id.
                if (Int32.TryParse(arguments.At(0), out int id) && Player.TryGet(id, out Player potentialPlayer))
                {
                    player = potentialPlayer;
                }
                // If first argument is an int. Find the player by name.
                else if (Player.TryGetPlayersByName(arguments.At(0), out List<Player> potentialPlayers))
                {
                    if (potentialPlayers.Count > 0)
                    {
                        player = potentialPlayers.First();
                    }
                }
                // Check if the sender wants to apply gravity to everyone.
                else if (arguments.At(0) == "*" || arguments.At(0) == "all" || arguments.At(0) == "everyone")
                {
                    isEveryone = true;
                }
                // If nothing works. Idk man.
                else
                {
                    response = "Player Not Found! " + FailMessage;
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
                if (isEveryone)
                {
                    plugin.AlteredGravityPlayers.Clear();

                    foreach (Player listPlayer in Player.List)
                    {
                        listPlayer.Gravity = new UnityEngine.Vector3(X, Y, Z);
                    }

                    response = "Gravity for everyone is now " + $"({X}, {Y}, {Z})!";
                    return true;
                }
                // Apply gravity for spesific player.
                else
                {
                    Vector3 gravity = new UnityEngine.Vector3(X, Y, Z);

                    if (!plugin.AlteredGravityPlayers.Keys.Contains(player))
                    {
                        plugin.AlteredGravityPlayers.Add(player, gravity);
                    }

                    player.Gravity = gravity;

                    response = "Gravity for " + player.Nickname + " is now " + $"({X}, {Y}, {Z})!";
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