using System;
using System.Collections.Generic;
using LabApi.Features;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using UnityEngine;

namespace CustomGravity
{
    public class CustomGravity : Plugin<Config>
    {
        public static CustomGravity Instance { get; set; }
        public override LoadPriority Priority { get; } = LoadPriority.Low;
        public override string Name => "Custom Gravity";
        public override string Description => "Can modify players gravity.";
        public override string Author => "Caty Foxt";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredApiVersion => new Version(LabApiProperties.CompiledVersion);

        // To keep track of global gravity that changed with command.
        public Vector3 TemporaryGravity = new Vector3(0, 0, 0);
        public bool UseTemporaryGravity = false;

        // A List to keep track of players with special gravity.
        public Dictionary<Player, Vector3> AlteredGravityPlayers = new Dictionary<Player, Vector3>();

        // To keep track of global gravity that changed with command.
        public Vector3 TemporaryPickupGravity = new Vector3(0, 0, 0);
        public bool UseTemporaryPickupGravity = false;

        // A List to keep track of item types with special gravity.
        public Dictionary<ItemType, Vector3>  AlteredItemTypes = new Dictionary<ItemType, Vector3>();

        public override void Enable()
        {
            Instance = this;
            LoadConfigs();

            LabApi.Events.Handlers.PlayerEvents.ChangedRole += EventHandler.Changed;
            LabApi.Events.Handlers.PlayerEvents.Joined += EventHandler.Joined;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += EventHandler.Restart;

            LabApi.Events.Handlers.PlayerEvents.DroppedItem += EventHandler.DroppedItem;
            LabApi.Events.Handlers.PlayerEvents.DroppedAmmo += EventHandler.DroppedAmmo;
            LabApi.Events.Handlers.PlayerEvents.Death += EventHandler.PlayerDied;
        }

        public override void Disable()
        {
            LabApi.Events.Handlers.PlayerEvents.ChangedRole -= EventHandler.Changed;
            LabApi.Events.Handlers.PlayerEvents.Joined -= EventHandler.Joined;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= EventHandler.Restart;

            LabApi.Events.Handlers.PlayerEvents.DroppedItem -= EventHandler.DroppedItem;
            LabApi.Events.Handlers.PlayerEvents.DroppedAmmo -= EventHandler.DroppedAmmo;
            LabApi.Events.Handlers.PlayerEvents.Death -= EventHandler.PlayerDied;

            Instance = null;
        }
    }
}
