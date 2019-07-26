using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Extensions.ApplicationInsights
{
    public class ApplicationInsightsContext : IAsyncCollector<KeyValuePair<string, string>>
    {
        public string InstrumentationKey { get; set; }

        public string OperationId { get; set; }

        public Task AddAsync(KeyValuePair<string, string> item, CancellationToken cancellationToken = default)
        {
            Activity.Current.AddTag(item.Key, item.Value);
            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
