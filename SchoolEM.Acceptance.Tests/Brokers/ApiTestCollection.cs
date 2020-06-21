using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SchoolEM.Acceptance.Tests.Brokers
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<SchoolEMApiBroker>
    {
    }
}
