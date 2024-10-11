using mrfpc.modloader.Patches.Common;
using Reloaded.Memory.Sources;

namespace mrfpc.modloader.Patches.MRF
{
    internal class Force4kAssets
    {
        public static void Activate(in PatchContext context)
        {
            var baseAddr = context.BaseAddress;
            if (!context.Config.Force4k)
            {
                context.Logger.Info("Force 4k Patch is not enabled");
                return;
            }

            context.Logger.Info("Attempting to apply Force 4k Patch");

            context.ScanHelper.FindPatternOffset("83 FB 01 0F 44 C1", (offset) =>
                Memory.Instance.SafeWriteRaw((nuint)(baseAddr + offset), new byte[] { 0xc7, 0xc0, 0x02, 0x00, 0x00, 0x00 }), // MOV EAX, 2
            "Force 4k Assets");
        }
    }
}
