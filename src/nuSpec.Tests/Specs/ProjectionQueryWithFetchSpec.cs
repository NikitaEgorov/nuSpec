namespace nuSpec.Tests.Specs
{
    public class ProjectionQueryWithFetchSpec : ProjectionQuerySpec
    {
        public ProjectionQueryWithFetchSpec() => this.AddFetch(q => q.Fetch(f => f.Department));
    }
}
