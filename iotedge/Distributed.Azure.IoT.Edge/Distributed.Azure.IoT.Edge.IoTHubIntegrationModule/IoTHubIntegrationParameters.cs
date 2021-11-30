namespace Distributed.Azure.IoT.Edge.IoTHubIntegrationModule
{
    using CommandLine;

    public class IoTHubIntegrationParameters
    {
        [Option(
        'p',
        "PrimaryConnectionString",
        Required = true,
        HelpText = "The primary connection string for the IoT Hub device.")]
        public string? PrimaryConnectionString { get; set; }

        [Option(
        'm',
        "PubSubMessagingName",
        Required = true,
        HelpText = "Dapr pubsub messaging component name.")]
        public string? PubSubMessagingName { get; set; }

        [Option(
        't',
        "PubSubTopicName",
        Required = true,
        HelpText = "Dapr pubsub messaging topic name.")]
        public string? PubSubTopicName { get; set; }

        [Option(
        'r',
        "PubSubMappedRoute",
        Required = true,
        HelpText = "Dapr pubsub messaging topic name.")]
        public string? PubSubMappedRoute { get; set; }
    }
}
