using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;

namespace UnitePlugin.Utility
{
    public static class DeepCopy
    {
        
        public static void CopyPhysicalDisplay(PhysicalDisplay toObj, PhysicalDisplay fromObj)
        {
            toObj.FriendlyName = fromObj.FriendlyName;
            toObj.Id = fromObj.Id;
            toObj.IsPrimary = fromObj.IsPrimary;
            toObj.IsVirtualExtendedDisplay = fromObj.IsVirtualExtendedDisplay;
            toObj.Name = fromObj.Name;
            toObj.NumberOfPresentations = fromObj.NumberOfPresentations;
            toObj.Size = fromObj.Size;
        }

        
        public static void CopyHubAllocationInfo(HubAllocationInfo toObj, HubAllocationInfo fromObj)
        {
            toObj.FriendlyName = fromObj.FriendlyName;
            toObj.Id = fromObj.Id;
            toObj.HubInfo = fromObj.HubInfo;
            toObj.ModuleOwnerId = fromObj.ModuleOwnerId;
            CopyPhysicalDisplay(toObj.PhysicalDisplay, fromObj.PhysicalDisplay);
            toObj.ReuseControl = fromObj.ReuseControl;
            toObj.Tag = fromObj.Tag;
            toObj.UserInfo = fromObj.UserInfo;
            toObj.ViewType = fromObj.ViewType;
        }

        public static void CopyDisplayView(DisplayView toObj, DisplayView fromObj)
        {
            toObj.AllowRemoteAnnotations = fromObj.AllowRemoteAnnotations;
            toObj.BackgroundColor = fromObj.BackgroundColor;
            toObj.GetScreenShotDelegate = fromObj.GetScreenShotDelegate;
            toObj.Id = fromObj.Id;
            CopyHubAllocationInfo(toObj.HubAllocationInfo, fromObj.HubAllocationInfo);
        }
    }
}