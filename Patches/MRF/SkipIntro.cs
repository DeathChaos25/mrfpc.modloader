using mrfpc.modloader.Patches.Common;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Sources;
using System.Runtime.InteropServices;

namespace mrfpc.modloader.Patches.MRF;

/// <summary>
/// Skips the game introduction sequence.
/// </summary>
internal class SkipIntro
{
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public struct TitleTask
    {
        [FieldOffset(0x8)] public int State;
    }

    private static IHook<TitleTaskLoopInner> _titleTaskLoopInner = null!;
    private static FileEmulationFramework.Lib.Utilities.Logger _logger = null!;

    [Function(CallingConventions.Microsoft)]
    public struct TitleTaskLoopInner { public FuncPtr<BlittablePointer<TitleTask>, byte> Value; }
    [UnmanagedCallersOnly]
    public unsafe static byte TitleTaskLoopInnerImpl(TitleTask* title)
    {
        if (title->State == 45)
            title->State = 67;
        return _titleTaskLoopInner.OriginalFunction.Value.Invoke(title);
    }

    public static void Activate(in PatchContext context)
    {
        var baseAddr = context.BaseAddress;
        if (!context.Config.IntroSkip)
        {
            context.Logger.Info("Intro Skip Patch is not enabled");
            return;
        }
        IReloadedHooks Hook = context.Hooks;
        context.Logger.Info("Attempting to apply Intro Skip Patch");
        context.ScanHelper.FindPatternOffset("40 55 53 56 57 41 56 48 8D AC 24 ?? ?? ?? ?? 48 81 EC 10 09 00 00", (offset) =>
            _titleTaskLoopInner = Hook.CreateHook<TitleTaskLoopInner>(typeof(SkipIntro), nameof(TitleTaskLoopInnerImpl), baseAddr + offset).Activate(),
            "Introduction Skip");
    }
}