using System.Threading;

namespace CAFU.Core
{
    public interface ICancellationTokenLinkable
    {
        void LinkCancellationToken(CancellationToken cancellationToken);
    }
}