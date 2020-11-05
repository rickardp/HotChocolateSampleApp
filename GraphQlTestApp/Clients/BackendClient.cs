using GraphQlTestApp.ViewModel.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlTestApp.Clients
{
    public class BackendClient
    {
        public async Task<IReadOnlyList<MainEntity>> Fetch(string someFilter, bool doExpensiveFetch)
        {
            if (doExpensiveFetch)
            {
                await Task.Delay(1000);
            }
            else
            {
                await Task.Delay(100);
            }

            return new[]
            {
                new MainEntity
                {
                    Key = "A " + someFilter.ToUpperInvariant(),

                    Children = new[]
                    {
                        new ChildEntity
                        {
                            Hello = doExpensiveFetch ? "expensive" : "cheap"
                        }
                    }
                }
            };
        }
    }
}
