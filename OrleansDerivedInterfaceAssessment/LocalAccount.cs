using System.Threading.Tasks;
using Orleans;

namespace OrleansDerivedInterfaceAssessment
{
    internal class LocalAccount : Grain, ILocalAccount
    {
        public Task<AccountType> GetAccountType()
        {
            return Task.FromResult(AccountType.Local);
        }
    }
}