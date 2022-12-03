using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using System;

namespace UnitePlugin.ViewFactory
{
    public interface IHubView
    {
        Guid ViewGuid { get; }
        DisplayView DisplayView { get; set; }
        HubAllocationInfo HubAllocationInfo { get; }
        void Allocate();
        bool Show();
        void DeAllocate();
        bool IsAllocated { get; }
    }
}
