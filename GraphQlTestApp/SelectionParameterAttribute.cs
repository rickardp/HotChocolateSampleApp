using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQlTestApp
{
    public class SelectionParameterAttribute : LocalStateAttribute
    {
        public string Instance { get; }

        public SelectionParameterAttribute(string instance)
        {
            Instance = instance;
        }

        public SelectionParameterAttribute() : this(string.Empty) { }
    }
}
