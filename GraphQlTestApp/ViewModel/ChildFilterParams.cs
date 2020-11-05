using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQlTestApp.ViewModel
{
    public class ChildFilterParams
    {
        public CategoryType Category { get; set; }

        public string KeyMatch { get; set; }
    }

    public enum CategoryType
    {
        MainCategory,
        SecondaryCategory
    }
}
