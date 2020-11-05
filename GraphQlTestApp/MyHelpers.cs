using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQlTestApp
{
    public static class MyHelpers
    {
        // path here is a quick hack. I would want a strongly typed way of doing this!
        public static T? GetParams<T>(this IResolverContext context, string path)
            where T : class, new()
        {
            var root = context.FieldSelection;
            foreach (var segment in path.Split('/'))
            {
                var sub = root.SelectionSet?.Selections.OfType<FieldNode>().FirstOrDefault(x => x.Name.Value.Equals(segment, StringComparison.Ordinal));
                if (sub != null)
                {
                    root = sub;
                    break;
                }
            }

            if (root == null)
            {
                return null;
            }

            return Parse<T>(root.Arguments);
        }

        private static T Parse<T>(IReadOnlyList<ArgumentNode> args)
            where T : class, new()
        {
            // Question is, can I parse the args here somehow?
            return Activator.CreateInstance<T>();
        }
    }
}
