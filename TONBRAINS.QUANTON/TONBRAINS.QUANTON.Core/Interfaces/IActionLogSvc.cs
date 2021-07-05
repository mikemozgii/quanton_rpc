using Grpc.Core;
using System.Threading.Tasks;

namespace TONBRAINS.QUANTON.Core.Interfaces
{
    public interface IActionLogSvc
    {
        Task SaveActionLog(ServerCallContext context, string action, string actionId, string userId);
    }
}
