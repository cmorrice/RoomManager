using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Module.Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using UnitePlugin.Interfaces;
using UnitePlugin.UI;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.Static;

namespace UnitePlugin.Utility
{
    [Serializable]
    public class HubViewManager : IHubViewManager
    {
        private readonly List<IHubView> _hubViews = new List<IHubView>();
        [field: NonSerialized]
        private readonly IHubModuleRuntimeContext _runtimeContext;
        [field: NonSerialized]
        private readonly Dispatcher _currentUiDispatcher;
        [field: NonSerialized]
        private readonly Func<FrameworkElement, MarshalNativeHandleContract> _createContract;

        public ReadOnlyCollection<IHubView> HubViews => _hubViews.AsReadOnly();

        public HubViewManager(IHubModuleRuntimeContext runtimeContext, Dispatcher currentUiDispatcher, Func<FrameworkElement, MarshalNativeHandleContract> createContract)
        {
            lock (this)
            {
                _runtimeContext = runtimeContext;
                _currentUiDispatcher = currentUiDispatcher;
                _createContract = createContract;
            }
        }

        public bool IsAllViewsAllocated(HubView.Type hubviewType)
        {
            List<IHubView> list = GetSpecificViews(hubviewType);
            if (list.Count == 0) return false;

            return GetSpecificViews(hubviewType).All(x => x.IsAllocated);
        }

        public void LoadHubView(HubView.Type hubViewType, IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher)
        {
            var hubView = new UI.HubView().ExecuteCreation(hubViewType, runtimeContext, createContract, display, currentUiDispatcher, EventCommandInvoker);
            lock (this)
            {
                _hubViews.Add(hubView);
            }
        }

        public void Allocate(UI.HubView.Type type)
        {          
            GetSpecificViews(type).ForEach(hubView => Allocate(hubView.ViewGuid));
        }

        // ReSharper disable once UnusedMember.Global
        public bool Show(HubView.Type type)
        {
            var result = true;
            GetSpecificViews(type).ForEach(hubView => result = Show(hubView.ViewGuid) && result);           
            return result;
        }

        public void DeAllocate(UI.HubView.Type type)
        {
            GetSpecificViews(type).ForEach(hubView => DeAllocate(hubView.ViewGuid));
        }

        public void Allocate(Guid viewGuid)
        {
            var view = GetSpecificView(viewGuid);
            view?.Allocate();
        }

        public bool Show(Guid viewGuid)
        {
            return (bool)GetSpecificView(viewGuid)?.Show();
        }

        public void DeAllocate(Guid viewGuid)
        {
            var removeView = GetSpecificView(viewGuid);
            lock (this)
            {
                _hubViews.Remove(removeView);
            }

            removeView?.DeAllocate();
        }

        public IHubView GetSpecificView(Guid viewGuid)
        {
            IHubView result;

            lock (this)
            {
                result = _hubViews.FirstOrDefault(hubView => hubView.ViewGuid == viewGuid);
            }

            return result;
        }

        
        public IHubView GetSpecificView(UI.HubView.Type type, PhysicalDisplay display)
        {
            IHubView result;
            lock (this)
            {
                result = _hubViews.FirstOrDefault(hubView =>
                    hubView.GetType() == Type.GetType("UnitePlugin.UI." + Enum.GetName(typeof(UI.HubView.Type), type)) &&
                    hubView.HubAllocationInfo.PhysicalDisplay == display
                );
            }
            return result;
        }

        
        public List<IHubView> GetSpecificViews(UI.HubView.Type type)
        {
            List<IHubView> result;
            lock (this)
            {
                result = _hubViews.Where(hubView => hubView.GetType() == Type.GetType("UnitePlugin.UI." + Enum.GetName(typeof(UI.HubView.Type), type))).ToList();
            }
            return result;
        }

        public void EventCommandInvoker(object sender, HubViewEventArgs e)
        {
            if (e.IsOnAllDisplays) EventCommandInvokerAllDisplay(sender, e);
            else EventCommandInvokerSingleDisplay(sender, e);
        }

        
        public void EventCommandInvokerSingleDisplay(object sender, HubViewEventArgs e)
        {
            var senderView = GetSpecificView(e.SenderControlIdentifier);
            var targetView = GetViewAndCreateIfNull(e.HubViewType, senderView.HubAllocationInfo.PhysicalDisplay);
            typeof(IHubViewManager).GetMethod(e.HubViewMethod, new[] { typeof(Guid) })?.Invoke(this, new object[] { targetView.ViewGuid });
        }

        
        public void EventCommandInvokerAllDisplay(object sender, HubViewEventArgs e)
        {
            List<IHubView> targetViews = new List<IHubView>();
            _runtimeContext.DisplayManager.AvailableDisplays.ToList().ForEach(display => targetViews.Add(GetViewAndCreateIfNull(e.HubViewType, display)));
            targetViews.ForEach(targetView => typeof(IHubViewManager).GetMethod(e.HubViewMethod, new[] { typeof(Guid) })?.Invoke(this, new object[] { targetView.ViewGuid }));
        }


        private IHubView GetViewAndCreateIfNull(UI.HubView.Type hubViewType, PhysicalDisplay display)
        {
            return GetSpecificView(hubViewType, display) ?? CreateView(hubViewType, display);
        }

        private IHubView CreateView(HubView.Type hubViewType, PhysicalDisplay display)
        {
            LoadHubView(hubViewType, _runtimeContext, _createContract, display, _currentUiDispatcher);
            return GetSpecificView(hubViewType, display);
        }

        public void LoadandAllocateForAllDisplays(UI.HubView.Type hubViewType)
        {
            LoadForAllDisplays(hubViewType);
            Allocate(hubViewType);
        }

        
        public void LoadForAllDisplays(UI.HubView.Type hubViewType)
        {
            _runtimeContext.DisplayManager.AvailableDisplays.ToList().ForEach(display =>
                                        LoadHubView(hubViewType, _runtimeContext, _createContract, display, _currentUiDispatcher)
                                    );
        }

        public bool DoAllViewsHaveSameIsAllocated(HubView.Type hubViewType)
        {

            List<IHubView> list = GetSpecificViews(hubViewType);

            return list.All(x => x.IsAllocated) || list.All(x => !x.IsAllocated);
        }
    }
}
