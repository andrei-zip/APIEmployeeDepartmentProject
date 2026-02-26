using System.Threading;
using System.Threading.Tasks;

namespace APIEmployeeDepartmentProject.Services
{
    // A interface ( contract ) - the Controllers depends on interfaces not on classes
    public interface IRandomStringGenerator
    {
        // Generate a Random project code 
        Task<string> GenerateAsync(CancellationToken ct = default);

    }
}
