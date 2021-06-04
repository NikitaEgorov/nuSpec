namespace nuSpec.Tests.Specs
{
    public class QueryWithFetchSpec : QuerySpec
    {
        public QueryWithFetchSpec() => this.AddFetch(q => q.Fetch(f => f.Department));
    }
}
