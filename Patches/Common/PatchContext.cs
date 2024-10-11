using FileEmulationFramework.Lib.Utilities;
using mrfpc.modloader.Configuration;
using mrfpc.modloader.Utilities;
using Reloaded.Hooks.Definitions;

namespace mrfpc.modloader.Patches.Common;

/// <summary>
/// Data passed down to all patches in this mod.
/// </summary>
internal ref struct PatchContext
{
    public IntPtr BaseAddress { get; init; }
    public Config Config { get; init; }
    public SigScanHelper ScanHelper { get; init; }
    public IReloadedHooks Hooks { get; init; }
    public Logger Logger { get; init; }
}