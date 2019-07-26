using System;
using Microsoft.Azure.WebJobs.Description;

namespace Extensions.ApplicationInsights
{
    [Binding]
    public class ApplicationInsightsContextAttribute : Attribute
    {
    }
}
