namespace Tanji.Core.Services;

public interface IFileCachingService<TContext, TCached>
{
    public DirectoryInfo Root { get; init; }

    public TCached GetOrAdd(TContext context);
}