using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using DotCruz.Notifications.CrossCutting.Settings;
using DotCruz.Notifications.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace DotCruz.Notifications.Infrastructure.Services.Messaging
{
    public class SmtpConfigService : ISmtpConfigService
    {
        private readonly IAmazonSimpleSystemsManagement _ssmClient;
        private readonly string _parameterPath;

        public SmtpConfigService(IAmazonSimpleSystemsManagement ssmClient, IOptions<AwsSettings> awsSettings)
        {
            _ssmClient = ssmClient;
            _parameterPath = awsSettings.Value.SmtpParameterPath;
        }

        public async Task SaveAsync(Guid tenantId, string host, int port, string username, string password, string fromName, CancellationToken cancellationToken)
        {
            var formattedParameterPath = string.Format(_parameterPath, tenantId);
            var smtpPayload = new
            {
                Host = host,
                Port = port,
                Username = username,
                Password = password,
                FromName = fromName,
            };

            var putRequest = new PutParameterRequest
            {
                Name = formattedParameterPath,
                Value = JsonSerializer.Serialize(smtpPayload),
                Type = ParameterType.SecureString,
                Overwrite = true
            };

            await _ssmClient.PutParameterAsync(putRequest, cancellationToken);
        }
    }
}
