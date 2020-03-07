using System;
using System.Threading.Tasks;
using Orleans;

namespace OrleansDerivedInterfaceAssessment
{
    public interface IAccountProxyHolder : IGrainWithGuidKey
    {
        Task Initialize(AccountType type);

        Task<AccountType> GetAccountType();
    }

    internal class AccountProxyHolder : Grain, IAccountProxyHolder
    {
        private IAccountProxy _account;

        public Task Initialize(AccountType type)
        {
            _account = type switch
            {
                AccountType.Local => (IAccountProxy) GrainFactory.GetGrain<ILocalAccount>(this.GetPrimaryKey()),
                AccountType.Remote => (IAccountProxy) GrainFactory.GetGrain<IRemoteAccount>(this.GetPrimaryKey()),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            return Task.CompletedTask;
        }

        public async Task<AccountType> GetAccountType()
        {
            return await _account.GetAccountType();
        }
    }
}