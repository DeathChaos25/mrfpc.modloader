using System.Diagnostics;
using CriFs.V2.Hook.Interfaces;
using mrfpc.modloader.Merging;
using mrfpc.modloader.Merging.Tbl;

namespace mrfpc.modloader;

public partial class Mod
{
    private void OnBindMFR(ICriFsRedirectorApi.BindContext context)
    {
        // Wait for cache to init first.
        _createMergedFileCacheTask.Wait();

        // File merging
        var watch = Stopwatch.StartNew();
        var cpks = _criFsApi.GetCpkFilesInGameDir();

        var mergeUtils = new MergeUtils(_criFsApi);

        List<IFileMerger> fileMergers = new()
        {
            new TblMerger(mergeUtils, _logger, _mergedFileCache, _criFsApi),
        };

        foreach (var fileMerger in fileMergers)
            fileMerger.Merge(cpks, context);

        _logger.Info("Merging Completed in {0}ms", watch.ElapsedMilliseconds);
        _mergedFileCache.RemoveExpiredItems();
        _ = _mergedFileCache.ToPathAsync();
    }
}