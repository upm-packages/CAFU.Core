using JetBrains.Annotations;

namespace CAFU.Core
{
    [PublicAPI]
    public abstract class UseCaseBase : Base, ICancellationTokenLinkable
    {
    }
}