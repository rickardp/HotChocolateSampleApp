using GraphQlTestApp.Clients;
using GraphQlTestApp.ViewModel.Types;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQlTestApp.ViewModel
{
    public class Backend1Queries
    {
        private readonly BackendClient client;

        public Backend1Queries(BackendClient client)
        {
            this.client = client;
        }

        public string Hello => "World";

        public async Task<IReadOnlyCollection<MainEntity>> MainEntities(string mainEntityFilter, IResolverContext resolverContext)
        {
            // path here is a quick hack. I would want a stringly typed way of doing this!
            var queryParams = resolverContext.GetParams<ChildFilterParams>("children");

            var shouldDoExpensiveQuery = queryParams != null && queryParams.Category == CategoryType.SecondaryCategory;

            return await client.Fetch(mainEntityFilter, shouldDoExpensiveQuery);
        }
    }
}
