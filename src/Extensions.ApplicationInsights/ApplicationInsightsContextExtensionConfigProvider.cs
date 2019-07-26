using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Extensions.ApplicationInsights
{
    [Extension(nameof(ApplicationInsightsContext))]
    internal class ApplicationInsightsContextExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly TelemetryConfiguration _telemetryConfig;

        public ApplicationInsightsContextExtensionConfigProvider(TelemetryConfiguration telemetryConfiguration)
        {
            _telemetryConfig = telemetryConfiguration;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.AddBindingRule<ApplicationInsightsContextAttribute>()
                .AddConverter<ApplicationInsightsContext, IAsyncCollector<KeyValuePair<string, string>>>(c => c as IAsyncCollector<KeyValuePair<string, string>>)
                .BindToCollector(_ =>
                 {
                     return new ApplicationInsightsContext
                     {
                         InstrumentationKey = _telemetryConfig.InstrumentationKey,
                         OperationId = Activity.Current.Id
                     };
                 });
        }
    }
}
