using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Data.Common;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services
{
    public interface IIntegrationEventLogService
    {
        Task MarkEventAsPublishedAsync(IntegrationEvent @event);

        Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction);
    }
}