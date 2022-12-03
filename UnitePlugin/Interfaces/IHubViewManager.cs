using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Module.Common;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.UI;

namespace UnitePlugin.Interfaces
{
    public interface IHubViewManager
    {
        ReadOnlyCollection<IHubView> HubViews { get; }
        void LoadHubView(HubView.Type hubViewType, IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher);
        void Allocate(UI.HubView.Type type);
        bool Show(HubView.Type type);
        void DeAllocate(UI.HubView.Type type);
        void Allocate(Guid viewGuid);
        bool Show(Guid viewGuid);
        void DeAllocate(Guid viewGuid);
        IHubView GetSpecificView(Guid viewGuid);
        IHubView GetSpecificView(UI.HubView.Type type, PhysicalDisplay display);
        List<IHubView> GetSpecificViews(UI.HubView.Type type);
        void EventCommandInvoker(object sender, HubViewEventArgs e);
        void LoadandAllocateForAllDisplays(UI.HubView.Type hubViewType);
        void LoadForAllDisplays(UI.HubView.Type hubViewType);
        bool IsAllViewsAllocated(HubView.Type authImage);
        bool DoAllViewsHaveSameIsAllocated(HubView.Type authImage);
    }
}