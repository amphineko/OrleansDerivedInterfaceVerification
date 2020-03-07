using System.Threading.Tasks;
using Orleans;

namespace OrleansDerivedInterfaceAssessment
{
    internal class LocalAccountGrain : Grain, ILocalAccount
    {
        public Task<AccountType> GetAccountType()
        {
            return Task.FromResult(AccountType.Local);
        }
    }
}