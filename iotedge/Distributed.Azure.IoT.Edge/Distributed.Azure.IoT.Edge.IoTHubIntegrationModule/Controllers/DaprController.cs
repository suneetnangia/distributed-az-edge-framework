namespace Distributed.Azure.IoT.Edge.IoTHubIntegrationModule.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    [ApiController]
    public class DaprController : ControllerBase
    {
        private readonly ILogger<DaprController> _logger;
        private readonly string? _pubsubName;
        private readonly string? _pubsubTopic;
        private readonly string? _route;

        public DaprController(ILogger<DaprController>? logger, IoTHubIntegrationParameters ioTHubIntegrationParameters)
        {
            if (ioTHubIntegrationParameters is null)
            {
                throw new ArgumentNullException(nameof(ioTHubIntegrationParameters));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _pubsubName = ioTHubIntegrationParameters.PubSubMessagingName;
            _pubsubTopic = ioTHubIntegrationParameters.PubSubTopicName;
            _route = ioTHubIntegrationParameters.PubSubMappedRoute;
        }

        [HttpGet("subscribe")]
        public ActionResult ReturnSubscriptions()
        {
            _logger.LogInformation("Subscriptions endpoint called...");

            // This can be a collection of subscriptions later.
            string susbcriptions = $"[{{\"pubsubName\": \"{_pubsubName}\",\"topic\": \"{_pubsubTopic}\",\"route\": \"{_route}\"}}]";

            _logger.LogInformation($"{susbcriptions}");

            return Ok(susbcriptions);
        }
    }
}
