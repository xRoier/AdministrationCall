using System;
using System.Collections.Generic;
using Exiled.API.Features;

namespace AdministrationCall
{
    public class Plugin : Plugin<Config, Translations>
    {
        public override string Author => "xRoier";
        public override string Name => typeof(Plugin).Namespace;
        public override string Prefix => Name.ToLower();
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(5, 3, 1);
        
        public static Plugin Singleton;
        public Dictionary<string, DateTime> Cooldowns = new Dictionary<string, DateTime>();

        public override void OnEnabled()
        {
            Singleton = this;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Clear;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Clear;
            Singleton = null;
            base.OnDisabled();
        }

        private void Clear() => Cooldowns.Clear();
    }
}