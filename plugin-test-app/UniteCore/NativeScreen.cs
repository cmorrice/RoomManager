using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Intel.Unite.Common.Display;

namespace UnitePluginTestApp.UniteCore
{
    /// <summary>
    /// Native screen functions
    /// </summary>
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public static class NativeScreen
    {
#pragma warning disable 1591
        public static IEnumerable<PhysicalDisplay> GetMonitors()
        {
            byte count = 0;
            var extendedDisplayMonitor = GetExtendedDisplay();
            var displays = new Collection<PhysicalDisplay>();
            //int totalMonitors = Screen.AllScreens.Length;
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
                {
                    var mi = new MonitorInfoEx {Size = Marshal.SizeOf(typeof(MonitorInfoEx))};

                    if (!GetMonitorInfo(hMonitor, ref mi)) return true;
                    try
                    {
                        var vDevMode = new Devmode();
                        EnumDisplaySettings(mi.DeviceName, EnumCurrentSettings, ref vDevMode);
                        var display = new PhysicalDisplay
                        {
                            Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count),
                            Size = new UniteDisplayRect(vDevMode.dmPositionX, vDevMode.dmPositionY,
                                vDevMode.dmPelsWidth, vDevMode.dmPelsHeight),
                            Name = mi.DeviceName
                        };
                        if (string.Compare(display.Name, extendedDisplayMonitor, true, System.Globalization.CultureInfo.CurrentCulture) == 0)
                        {
                            display.IsVirtualExtendedDisplay = true;
                        }
                        if (display.Size.X == 0 && display.Size.Y == 0)
                        {
                            display.IsPrimary = true;
                        }
                        try
                        {
                            display.FriendlyName = GetFriendlyNameForMonitor(mi.DeviceName);
                        }
                        catch (Exception)
                        {
                            //CoreRuntimeContext.Instance.LogManager.LogException("NativeScreen", "Error getting device friendly name for display ", ex);
                        }
                        displays.Add(display);
                        count++;
                    }
                    catch (Exception)
                    {
                        //TODO!
                        //CoreRuntimeContext.Instance.LogManager.LogException("NativeScreen", "Error getting monitor information", ex);
                    }
                    return true;
                }, IntPtr.Zero);
            return new Collection<PhysicalDisplay>(displays.OrderByDescending(x => x.IsPrimary).ToList());
        }

        [DllImport("user32.dll")]
        private static extern int DisplayConfigGetDeviceInfo(ref DisplayconfigSourceDeviceName adapterName);

        [DllImport("user32.dll")]

        private static extern int DisplayConfigGetDeviceInfo(ref DisplayconfigTargetDeviceName adapterName);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DisplayconfigSourceDeviceName
        {
            public DisplayconfigDeviceInfoHeader header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            // ReSharper disable once CommentTypo
            public readonly string viewGdiDeviceName; // CCHDEVICENAME == 32
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigDeviceInfoHeader
        {
            public DisplayconfigDeviceInfoType type;
            public uint size;
            public Luid adapterId;
            public uint id;
        }

        private enum DisplayconfigDeviceInfoType : uint
        {
            DisplayconfigDeviceInfoGetSourceName = 1,
            DisplayconfigDeviceInfoGetTargetName = 2,
            // ReSharper disable once UnusedMember.Local
            DisplayconfigDeviceInfoGetTargetPreferredMode = 3,
            // ReSharper disable once UnusedMember.Local
            DisplayconfigDeviceInfoGetAdapterName = 4,
            // ReSharper disable once UnusedMember.Local
            DisplayconfigDeviceInfoSetTargetPersistence = 5,
            // ReSharper disable once UnusedMember.Local
            DisplayconfigDeviceInfoGetTargetBaseType = 6,
            // ReSharper disable once UnusedMember.Local
            DisplayconfigDeviceInfoForceUint32 = 0xFFFFFFFF
        }
        private static string ViewGdiNameGet(Luid adapterId, uint sourceId)
        {
            var deviceName = new DisplayconfigSourceDeviceName
            {
                header =
                {
                    size = (uint) Marshal.SizeOf(typeof(DisplayconfigSourceDeviceName)),
                    adapterId = adapterId,
                    id = sourceId,
                    type = DisplayconfigDeviceInfoType.DisplayconfigDeviceInfoGetSourceName
                }
            };

            var error = DisplayConfigGetDeviceInfo(ref deviceName);
            if (error != ErrorSuccess)
                throw new Win32Exception(error);
            return deviceName.viewGdiDeviceName;
        }

        private static string GetFriendlyNameForMonitor(string deviceName)
        {
            var adapterId = new Luid();
            uint targetId = 0;

            var error = GetDisplayConfigBufferSizes(QueryDeviceConfigFlags.QdcOnlyActivePaths,
                out var pathCount, out var modeCount);
            if (error != ErrorSuccess)
                throw new Win32Exception(error);

            var displayPaths = new DisplayconfigPathInfo[pathCount];
            var displayModes = new DisplayconfigModeInfo[modeCount];
            error = QueryDisplayConfig(QueryDeviceConfigFlags.QdcOnlyActivePaths,
                ref pathCount, displayPaths, ref modeCount, displayModes, IntPtr.Zero);
            if (error != ErrorSuccess)
                throw new Win32Exception(error);

            for (var i = 0; i < displayPaths.Length; i++)
            {
                var monitorName = ViewGdiNameGet(displayPaths[i].sourceInfo.AdapterId, displayPaths[i].sourceInfo.Id);
                if (string.Compare(monitorName, deviceName, StringComparison.OrdinalIgnoreCase) != 0) continue;
                adapterId = displayPaths[i].targetInfo.adapterId;
                targetId = displayPaths[i].targetInfo.id;
                break;
            }


            var targetDeviceName = new DisplayconfigTargetDeviceName
            {
                header =
                {
                    size = (uint) Marshal.SizeOf(typeof(DisplayconfigTargetDeviceName)),
                    adapterId = adapterId,
                    id = targetId,
                    type = DisplayconfigDeviceInfoType.DisplayconfigDeviceInfoGetTargetName
                }
            };


            error = DisplayConfigGetDeviceInfo(ref targetDeviceName);
            if (error != ErrorSuccess)
                throw new Win32Exception(error);
            return targetDeviceName.monitorFriendlyDeviceName;
        }

        private static string GetExtendedDisplay()
        {
            var monitor = string.Empty;
            try
            {
                var error = GetDisplayConfigBufferSizes(QueryDeviceConfigFlags.QdcOnlyActivePaths,
                    out var pathCount, out var modeCount);
                if (error != ErrorSuccess)
                    throw new Win32Exception(error);

                var displayPaths = new DisplayconfigPathInfo[pathCount];
                var displayModes = new DisplayconfigModeInfo[modeCount];
                error = QueryDisplayConfig(QueryDeviceConfigFlags.QdcOnlyActivePaths,
                    ref pathCount, displayPaths, ref modeCount, displayModes, IntPtr.Zero);
                if (error != ErrorSuccess)
                    throw new Win32Exception(error);

                for (var i = 0; i < displayPaths.Length; i++)
                {
                    if (displayPaths[i].targetInfo.id == _v33Dktargetinfoid)
                    {
                        monitor = ViewGdiNameGet(displayPaths[i].sourceInfo.AdapterId, displayPaths[i].sourceInfo.Id);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                //CoreRuntimeContext.Instance.LogManager.LogException("NativeScreen", "Error getting extended display name name", ex);
                //"Error querying extended display", ex);
            } //Sometime information cannot be queried when there are disconnections
            return monitor;
        }

        public enum QueryDeviceConfigFlags : uint
        {
            // ReSharper disable once UnusedMember.Global
            QdcAllPaths = 0x00000001,
            QdcOnlyActivePaths = 0x00000002,
            // ReSharper disable once UnusedMember.Global
            QdcDatabaseCurrent = 0x00000004
        }

        private const int _v33Dktargetinfoid = 0x4711;
        // ReSharper disable once UnusedMember.Global
        public const int Iddxtargetinfoid = 0x100;

        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayconfigPathInfo
        {
            public DisplayconfigPathSourceInfo sourceInfo;
            public DisplayconfigPathTargetInfo targetInfo;
            private readonly uint flags;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Luid
        {
            public uint LowPart;
            public int HighPart;
        }
        public struct DisplayconfigPathSourceInfo
        {
            public Luid AdapterId;
            public uint Id;
            public uint ModeInfoIdx;
            public uint StatusFlags;
        }

        private enum DisplayconfigRotation : uint
        {
            DisplayconfigRotationIdentity = 1,
            DisplayconfigRotationRotate90 = 2,
            DisplayconfigRotationRotate180 = 3,
            DisplayconfigRotationRotate270 = 4,
            DisplayconfigRotationForceUint32 = 0xFFFFFFFF
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct DisplayconfigTargetDeviceName
        {
            public DisplayconfigDeviceInfoHeader header;
            public UInt32 flags;
            public DisplayconfigVideoOutputTechnology outputTechnology;
            public UInt16 edidManufactureId;
            public UInt16 edidProductCodeId;
            public UInt32 connectorInstance;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string monitorFriendlyDeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string monitorDevicePath;
        };

        private enum DisplayconfigVideoOutputTechnology : uint
        {
            DisplayconfigOutputTechnologyOther = 0xFFFFFFFF,
            DisplayconfigOutputTechnologyHd15 = 0,
            DisplayconfigOutputTechnologySvideo = 1,
            DisplayconfigOutputTechnologyCompositeVideo = 2,
            DisplayconfigOutputTechnologyComponentVideo = 3,
            DisplayconfigOutputTechnologyDvi = 4,
            DisplayconfigOutputTechnologyHdmi = 5,
            DisplayconfigOutputTechnologyLvds = 6,
            DisplayconfigOutputTechnologyDJpn = 8,
            DisplayconfigOutputTechnologySdi = 9,
            DisplayconfigOutputTechnologyDisplayportExternal = 10,
            DisplayconfigOutputTechnologyDisplayportEmbedded = 11,
            DisplayconfigOutputTechnologyUdiExternal = 12,
            DisplayconfigOutputTechnologyUdiEmbedded = 13,
            DisplayconfigOutputTechnologySdtvdongle = 14,
            DisplayconfigOutputTechnologyMiracast = 15,
            DisplayconfigOutputTechnologyInternal = 0x80000000,
            DisplayconfigOutputTechnologyForceUint32 = 0xFFFFFFFF
        }
        private enum DisplayconfigScaling : uint
        {
            DisplayconfigScalingIdentity = 1,
            DisplayconfigScalingCentered = 2,
            DisplayconfigScalingStretched = 3,
            DisplayconfigScalingAspectratiocenteredmax = 4,
            DisplayconfigScalingCustom = 5,
            DisplayconfigScalingPreferred = 128,
            DisplayconfigScalingForceUint32 = 0xFFFFFFFF
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayconfigPathTargetInfo
        {
            public Luid adapterId;
            public uint id;
            public uint modeInfoIdx;
            DisplayconfigVideoOutputTechnology outputTechnology;
            DisplayconfigRotation rotation;
            DisplayconfigScaling scaling;
            DisplayconfigRational refreshRate;
            DisplayconfigScanlineOrdering scanLineOrdering;
            public bool targetAvailable;
            public uint statusFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayconfigRational
        {
            public uint Numerator;
            public uint Denominator;
        }

        public enum DisplayconfigScanlineOrdering : uint
        {
            DisplayconfigScanlineOrderingUnspecified = 0,
            DisplayconfigScanlineOrderingProgressive = 1,
            DisplayconfigScanlineOrderingInterlaced = 2,
            DisplayconfigScanlineOrderingInterlacedUpperfieldfirst = DisplayconfigScanlineOrderingInterlaced,
            DisplayconfigScanlineOrderingInterlacedLowerfieldfirst = 3,
            DisplayconfigScanlineOrderingForceUint32 = 0xFFFFFFFF
        }
        public enum DisplayconfigModeInfoType : uint
        {
            DisplayconfigModeInfoTypeSource = 1,
            DisplayconfigModeInfoTypeTarget = 2,
            DisplayconfigModeInfoTypeForceUint32 = 0xFFFFFFFF
        }
        [DllImport("user32.dll")]
        public static extern int QueryDisplayConfig(QueryDeviceConfigFlags flags, ref uint numPathArrayElements, [Out] DisplayconfigPathInfo[] pathInfoArray, ref uint numModeInfoArrayElements, [Out] DisplayconfigModeInfo[] modeInfoArray, IntPtr currentTopologyId);

        [StructLayout(LayoutKind.Explicit)]
        public struct DisplayconfigModeInfoUnion
        {
            [FieldOffset(0)]
            public DisplayconfigTargetMode targetMode;
            [FieldOffset(0)]
            public DisplayconfigSourceMode sourceMode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayconfigSourceMode
        {
            public uint width;
            public uint height;
            public DisplayconfigPixelformat pixelFormat;
            public Pointl position;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Pointl
        {
            int x;
            int y;
        }
        public enum DisplayconfigPixelformat : uint
        {
            DisplayconfigPixelformat8Bpp = 1,
            DisplayconfigPixelformat16Bpp = 2,
            DisplayconfigPixelformat24Bpp = 3,
            DisplayconfigPixelformat32Bpp = 4,
            DisplayconfigPixelformatNongdi = 5,
            DisplayconfigPixelformatForceUint32 = 0xffffffff
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayconfigTargetMode
        {
            public DisplayconfigVideoSignalInfo targetVideoSignalInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Displayconfig2Dregion
        {
            public uint cx;
            public uint cy;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayconfigVideoSignalInfo
        {
            public ulong pixelRate;
            public DisplayconfigRational hSyncFreq;
            public DisplayconfigRational vSyncFreq;
            public Displayconfig2Dregion activeSize;
            public Displayconfig2Dregion totalSize;
            public uint videoStandard;
            public DisplayconfigScanlineOrdering scanLineOrdering;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayconfigModeInfo
        {
            public DisplayconfigModeInfoType infoType;
            public uint id;
            public Luid adapterId;
            public DisplayconfigModeInfoUnion modeInfo;
        }
        public const int ErrorSuccess = 0;
        [DllImport("user32.dll")]

        public static extern int GetDisplayConfigBufferSizes(QueryDeviceConfigFlags flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);

        public const int EnumCurrentSettings = -1;
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref Devmode lpDevMode);
        #region Structs
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Devmode
        {
            private const int _cchdevicename = 0x20;
            private const int _cchformname = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            /// <summary>
            /// 
            /// </summary>
            public int Left;
            /// <summary>
            /// 
            /// </summary>
            public int Top;
            /// <summary>
            /// 
            /// </summary>
            public int Right;
            /// <summary>
            /// 
            /// </summary>
            public int Bottom;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hMonitor"></param>
        /// <param name="lpmi"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]

        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MonitorInfoEx
        {
            public int Size;
            public Rect Monitor;
            public Rect WorkArea;
            public uint Flags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
        }
        #region Delegates
        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);
        #endregion
        #region Imports
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="lprcClip"></param>
        /// <param name="lpfnEnum"></param>
        /// <param name="dwData"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);
        #endregion
        #endregion
#pragma warning restore 1591
    }
}
