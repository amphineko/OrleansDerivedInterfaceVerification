using System.Threading.Tasks;
using Orleans;

namespace OrleansDerivedInterfaceAssessment
{
    internal class RemoteAccountGrain : Grain, IRemoteAccount
    {
        public Task<AccountType> GetAccountType()
        {
            return Task.FromResult(AccountType.Remote);
        }
    }
}