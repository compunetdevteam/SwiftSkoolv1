using System;
using System.Threading.Tasks;

namespace SwiftSkoolv1.WebUI.APIRepository.RepoAbstractions
{
    public interface IRepository : IDisposable
    {
        Task<int> SaveAsync();
    }
}
