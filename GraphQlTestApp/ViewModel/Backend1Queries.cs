using GraphQlTestApp.Clients;
using GraphQlTestApp.ViewModel.Types;
using HotChocolate;
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

        [InjectSelectionParameters]
        public async Task<IReadOnlyCollection<MainEntity>> MainEntities(string mainEntityFilter, [SelectionParameter] ChildFilterParams? queryParams = null)
        {
            var shouldDoExpensiveQuery = queryParams != null && queryParams.Category == CategoryType.SecondaryCategory;

            return await client.Fetch(mainEntityFilter, shouldDoExpensiveQuery);
        }
    }
}
