namespace Distributed.Azure.IoT.Edge.IoTHubIntegrationModule
{
    using Dapr.AppCallback.Autogen.Grpc.v1;

    using Distributed.Azure.IoT.Edge.Common.Device;

    using global::System.Text;

    using Google.Protobuf.WellKnownTypes;

    using Grpc.Core;

    using Microsoft.Azure.Devices.Client;

    public class SubscriptionService : AppCallback.AppCallbackBase
    {
        private readonly ILogger<SubscriptionService> _logger;
        private readonly IDeviceClient _deviceClient;
        private readonly string _pubsubName;
        private readonly string _pubsubTopicName;

        public SubscriptionService(ILogger<SubscriptionService> logger, IDeviceClient deviceClient, string pubsubName, string pubsubTopicName)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _deviceClient = deviceClient ?? throw new ArgumentNullException(nameof(deviceClient));
            _pubsubName = pubsubName ?? throw new ArgumentNullException(nameof(pubsubName));
            _pubsubTopicName = pubsubTopicName ?? throw new ArgumentNullException(nameof(pubsubTopicName));
        }

        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            var subscriptionsResponse = new ListTopicSubscriptionsResponse();

            subscriptionsResponse.Subscriptions.Add(new TopicSubscription
            {
                PubsubName = _pubsubName,
                Topic = _pubsubTopicName
            });

            return Task.FromResult(subscriptionsResponse);
        }

        public override async Task<TopicEventResponse> OnTopicEvent(TopicEventRequest request, ServerCallContext context)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var topicString = request.Data.ToString();

            _logger.LogTrace($"Topic received from dapr pubsub, data: {0}", topicString);

            using (var message = new Message(topicString == null ? null : Encoding.UTF8.GetBytes(topicString)))
            {
                // TODO: add cancellation token from higher layers.
                await _deviceClient.SendEventAsync(message, CancellationToken.None);
            }

            // Depending on the status return dapr side will either retry or drop the message from underlying pubsub.
            return new TopicEventResponse() { Status = TopicEventResponse.Types.TopicEventResponseStatus.Success };
        }
    }
}
