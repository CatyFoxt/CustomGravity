using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using UnityEngine;

namespace CustomGravity
{
    internal class EventHandler
    {
        public static void Changed(PlayerChangedRoleEventArgs ev)
        {
            
            // If players gravity is modified, keep it like that.
            if (CustomGravity.Instance.AlteredGravityPlayers.Contains(ev.Player))
            {
                return;
            }

            // Get the config and create the gravity vector.
            Config config = CustomGravity.Instance.Config;
            Vector3 gravity = new UnityEngine.Vector3(config.GravityVectorX, config.GravityVectorY, config.GravityVectorZ);

            // If a temporary gravity vector is being used. Override the default gravity.
            if (CustomGravity.Instance.UseTemporaryGravity)
            {
                gravity = CustomGravity.Instance.TemporaryGravity;
            }

            // Set the gravity. Without delay gravity is not being applied, dont ask me. Its magic!
            Timing.CallDelayed(0.2f, () =>
            {
                ev.Player.Gravity = gravity;
            });
        }

        public static void Joined(PlayerJoinedEventArgs ev)
        {
            // For example if player quits and rejoins, clear the altered gravity cache.
            if (CustomGravity.Instance.AlteredGravityPlayers.Contains(ev.Player))
            {
                CustomGravity.Instance.AlteredGravityPlayers.Remove(ev.Player);
            }
        }

        public static void Restart()
        {
            // Clear all gravity cache for next round.
            CustomGravity.Instance.AlteredGravityPlayers.Clear();
            CustomGravity.Instance.UseTemporaryGravity = false;
        }
    }
}
