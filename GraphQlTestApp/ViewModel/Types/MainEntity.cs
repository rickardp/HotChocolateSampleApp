using System.Collections.Generic;

namespace GraphQlTestApp.ViewModel.Types
{
    public class MainEntity
    {
        public string Key { get; set; }

        [Params(typeof(ChildFilterParams))]
        public IReadOnlyCollection<ChildEntity> Children { get; set; }
    }
}
