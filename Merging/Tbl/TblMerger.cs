using CriFs.V2.Hook.Interfaces;
using Persona.Merger.Cache;
using FileEmulationFramework.Lib.Utilities;
using mrfpc.modloader.Merging.Tbl;
using mrfpc.modloader.Merging;

namespace mrfpc.modloader.Merging.Tbl;

internal class TblMerger : IFileMerger
{
    private IFileMerger _tblMerger;

    internal TblMerger(MergeUtils utils, Logger logger, MergedFileCache mergedFileCache, ICriFsRedirectorApi criFsApi)
    {
        _tblMerger = new MRFTblMerger(utils, logger, mergedFileCache, criFsApi);
    }

    public void Merge(string[] cpks, ICriFsRedirectorApi.BindContext context)
    {
        _tblMerger?.Merge(cpks, context);
    }
}