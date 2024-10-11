using CriFs.V2.Hook.Interfaces;
using CriFsV2Lib.Definitions.Utilities;
using FileEmulationFramework.Lib.Utilities;
using Persona.Merger.Cache;
using Persona.Merger.Patching.Tbl;
using Persona.Merger.Patching.Tbl.FieldResolvers.Generic;
using static mrfpc.modloader.Merging.MergeUtils;

namespace mrfpc.modloader.Merging.Tbl;

internal class MRFTblMerger : IFileMerger
{
    private readonly ICriFsRedirectorApi _criFsApi;
    private readonly Logger _logger;
    private readonly MergedFileCache _mergedFileCache;
    private readonly MergeUtils _utils;

    internal MRFTblMerger(MergeUtils utils, Logger logger, MergedFileCache mergedFileCache,
        ICriFsRedirectorApi criFsApi)
    {
        _utils = utils;
        _logger = logger;
        _mergedFileCache = mergedFileCache;
        _criFsApi = criFsApi;
    }

    public void Merge(string[] cpks, ICriFsRedirectorApi.BindContext context)
    {
        // Note: Actual merging logic is optimised but code in mod could use some more work.
        var pathToFileMap = context.RelativePathToFileMap;
        var tasks = new List<ValueTask>
        {
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ITEMCATEGORY.bin", 1, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\MASK.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\SynSequenceData.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\A_Visual_Data.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\E_Visual_Data.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\P_Visual_Data.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\Allied.tbl", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\Chara.tbl", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\Class.tbl", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ClassAffinity.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ClassTrait.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\Encount.tbl", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\Enemy.tbl", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\EnemyAffinity.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ItemAccessory.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ItemArmor.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ItemConsumable.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ItemCostume.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ItemEvent.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ItemSubArmor.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\ItemWeapon.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\Skill.tbl", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\SkillMotion.tbl", 1, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\table\SkillNormal.tbl", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\battle\sound\Rf_ATKSE_DATA.bin", 2, cpks),

            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\AbilityTable.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\AreaEnvUpFilter.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\BattleModel_ArcheType.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\BattleModel_Chara.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\BattleModel_Enemy.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\character_costume_variety.bin", 1, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\CharacterClothesData.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\CharaID.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\EnemyID.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\NpcID.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\resrcExistData.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\ResrcNpcModelComb.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\ResrcNpcModelCombEvent.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\SafeRoomCharacterClothesData.bin", 1, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\ShopTable.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\ShopTypeTable.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\SummonItemData.bin", 2, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel1.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel2.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel3.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel4.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel5.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel6.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel7.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel8.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel9.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel10.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel12.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel13.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel14.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel15.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\weaponModel17.bin", 4, cpks),
            PatchAnyFile(pathToFileMap, @"R2\COMMON\init\data\wipeData.bin", 4, cpks),
        };

        Task.WhenAll(tasks.Select(x => x.AsTask())).Wait();
    }

    private async ValueTask PatchAnyFile(Dictionary<string, List<ICriFsRedirectorApi.BindFileInfo>> pathToFileMap,
        string tblPath, int ResolverSize, string[] cpks)
    {
        if (!pathToFileMap.TryGetValue(tblPath, out var candidates))
            return;

        var pathInCpk = RemoveR2Prefix(tblPath);
        if (!_utils.TryFindFileInAnyCpk(pathInCpk, cpks, out var cpkPath, out var cpkEntry, out var fileIndex))
        {
            _logger.Warning("Unable to find TBL in any CPK {0}", pathInCpk);
            return;
        }

        // Build cache key
        var cacheKey = GetCacheKeyAndSources(tblPath, candidates, out var sources);
        if (_mergedFileCache.TryGet(cacheKey, sources, out var cachedFilePath))
        {
            _logger.Info("Loading Merged TBL {0} from Cache ({1})", tblPath, cachedFilePath);
            _utils.ReplaceFileInBinderInput(pathToFileMap, tblPath, cachedFilePath);
            return;
        }

        // Else Merge our Data
        // First we extract.
        await Task.Run(async () =>
        {
            _logger.Info("Merging {0} with key {1}.", tblPath, cacheKey);
            await using var cpkStream =
                new FileStream(cpkPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var reader = _criFsApi.GetCriFsLib().CreateCpkReader(cpkStream, false);
            using var extractedTable = reader.ExtractFile(cpkEntry.Files[fileIndex].File);

            // Then we merge
            byte[] patched;
            patched = await PatchAny(extractedTable, candidates, ResolverSize);

            // Then we store in cache.
            var item = await _mergedFileCache.AddAsync(cacheKey, sources, patched);
            _utils.ReplaceFileInBinderInput(pathToFileMap, tblPath,
                Path.Combine(_mergedFileCache.CacheFolder, item.RelativePath));
            _logger.Info("Merge {0} Complete. Cached to {1}.", tblPath, item.RelativePath);
        });
    }

    private static async Task<byte[]> PatchAny(ArrayRental extractedTable,
        List<ICriFsRedirectorApi.BindFileInfo> candidates, int ResolverSize)
    {
        var patcher = new GenericPatcher(extractedTable.Span.ToArray());
        var patches = new List<TblPatch>(candidates.Count);
        for (var x = 0; x < candidates.Count; x++)
            patches.Add(patcher.GeneratePatchGeneric(await File.ReadAllBytesAsync(candidates[x].FullPath), ResolverSize));

        var patched = patcher.ApplyGeneric(patches);
        return patched;
    }
}