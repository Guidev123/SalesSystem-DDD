using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.SharedKernel.Data.EventSourcing;
using SalesSystem.SharedKernel.Enums;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.API.Controllers
{
    [Route("api/v1/event-sourcing")]
    public class EventSourcingController(IHttpContextAccessor httpContextAccessor,
                                         IEventSourcingRepository eventSourcingRepository)
                                       : MainController(httpContextAccessor)
    {
        [Authorize(Roles = nameof(EUserRoles.Admin))]
        [HttpGet]
        public async Task<IResult> GetAggregateHistoryAsync(Guid aggregateId)
        {
            var events = await eventSourcingRepository.GetAllAsync(aggregateId);
            if (events is null || !events.Any()) return TypedResults.NotFound();

            var response = Response<IEnumerable<StoredEvent>>.Success(events);
            return TypedResults.Ok(response);
        }
    }
}