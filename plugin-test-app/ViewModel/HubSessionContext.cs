using System;
using System.Collections.ObjectModel;
using Intel.Unite.Common.Context;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Core;

namespace UnitePluginTestApp.ViewModel
{
    internal class HubSessionContext : IHubSessionContext
    {
        public HubSessionContext()
        {
            Users = new Collection<UserInfo>();
        }

        public HubInfo MyHubInfo { get; set; }

        public Collection<UserInfo> Users { get; set; }

        public Collection<HubInfo> Hubs { get; set; } = new Collection<HubInfo>();

        public LockStatus LockStatus { get; set; } = LockStatus.Locked;

        public ModerationMode ModerationMode { get; set; } = new ModerationMode();

        public SessionPreviewStatus SessionPreviewStatus { get; set; } = new SessionPreviewStatus();
        public event EventHandler<UserInfo> UserAdded = delegate { };
        public event EventHandler<UserInfo> UserRemoved = delegate { };
        public event EventHandler<UserInfo> UserUpdated = delegate { };
        public event EventHandler NewUsersList = delegate { };
        public event EventHandler NewHubsList = delegate { };
        public event EventHandler SessionLocked = delegate { };
        public event EventHandler SessionUnlocked = delegate { };
        public event EventHandler<ModerationMode> ModerationModeUpdated = delegate { };
        public event EventHandler<SessionPreviewStatus> PreviewStatusUpdated = delegate { };
    }
}