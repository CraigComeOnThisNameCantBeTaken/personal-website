namespace GitIntegration.Resolvers
{
    internal interface IOptionResolver
    {
        GitIntegrationOption Resolve(string provider);
    }
}
