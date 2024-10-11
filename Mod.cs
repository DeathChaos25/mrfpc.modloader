using FileEmulationFramework.Lib.Utilities;
using mrfpc.modloader.Configuration;
using mrfpc.modloader.Template;
using mrfpc.modloader.Utilities;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using CriFs.V2.Hook.Interfaces;
using CriFsV2Lib.Definitions;
using FileEmulationFramework.Lib.Utilities;
using Reloaded.Memory.Streams;
using Persona.Merger.Cache;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using System.Diagnostics;
using mrfpc.modloader.Patches.Common;

namespace mrfpc.modloader
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
public partial class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config _configuration;

        /// <summary>
        /// Current process.
        /// </summary>
        public static Process CurrentProcess = null!;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        private readonly Logger _logger;
        private ICriFsRedirectorApi _criFsApi = null!;
        private MergedFileCache _mergedFileCache = null!;
        private Task _createMergedFileCacheTask = null!;

        public Mod(ModContext context)
        {
            _configuration = context.Configuration;
            var modLoader = context.ModLoader;
            IReloadedHooks? hooks = context.Hooks;
            _logger = new Logger(context.Logger, _configuration.LogLevel);
            _owner = context.Owner;
            _modConfig = context.ModConfig;


            modLoader.GetController<IStartupScanner>().TryGetTarget(out var startupScanner);
            var scanHelper = new SigScanHelper(_logger, startupScanner);
            CurrentProcess = Process.GetCurrentProcess();
            var mainModule = CurrentProcess.MainModule;
            var baseAddr = mainModule!.BaseAddress;

            var patchContext = new PatchContext()
            {
                BaseAddress = baseAddr,
                Config = _configuration,
                Logger = _logger,
                Hooks = hooks!,
                ScanHelper = scanHelper
            };

            // Read merged file cache in background.
            _createMergedFileCacheTask = Task.Run(async () =>
            {
                var modFolder = modLoader.GetDirectoryForModId(context.ModConfig.ModId);
                var cacheFolder = Path.Combine(modFolder, "Cache", "MRF");
                return _mergedFileCache = await MergedFileCache.FromPathAsync(context.ModConfig.ModVersion, cacheFolder);
            });

            modLoader.GetController<ICriFsRedirectorApi>().TryGetTarget(out _criFsApi!);

            Patches.MRF.SkipIntro.Activate(patchContext);
            Patches.MRF.Force4kAssets.Activate(patchContext);

            var criFsController = modLoader.GetController<ICriFsRedirectorApi>();
            if (criFsController == null || !criFsController.TryGetTarget(out var criFsApi))
            {
                _logger.Error("Unable to load CriFS V2 Library Hooks!");
                return;
            }
            else criFsApi.AddProbingPath("MFEssentials/CPK");

            _criFsApi.AddBindCallback(OnBindMFR);
            // Metaphor ReFantazio only has 1 CPK

            NoPauseOnFocusLoss.Activate(patchContext);
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            _configuration = configuration;
            _logger.Info($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}
