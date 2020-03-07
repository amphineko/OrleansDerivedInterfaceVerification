using System.Threading.Tasks;
using Orleans;

namespace OrleansDerivedInterfaceAssessment
{
    public interface IAccountProxy : IGrainWithGuidKey
    {
        Task<AccountType> GetAccountType();
    }
}