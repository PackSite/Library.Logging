namespace WebAppExample.IntegrationTests.Environments
{
    using Xunit;

    [CollectionDefinition(Name)]
    public sealed class WebAppExampleEnvironmentCollection : ICollectionFixture<WebAppExampleEnvironment>
    {
        public const string Name = "Web App Example environment collection";

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}