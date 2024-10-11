using CriFs.V2.Hook.Interfaces;

namespace mrfpc.modloader.Merging;

internal interface IFileMerger
{
    void Merge(string[] cpks, ICriFsRedirectorApi.BindContext context);
}