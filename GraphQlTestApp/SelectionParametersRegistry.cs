using HotChocolate;
using HotChocolate.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GraphQlTestApp
{
    public sealed class SelectionParametersRegistry
    {
        private const string Key = nameof(SelectionParametersRegistry);

        internal static SelectionParametersRegistry Create(IDictionary<string, object?> contextData)
        {
            var result = new SelectionParametersRegistry();
            contextData.Add(Key, result);
            return result;
        }
        internal static SelectionParametersRegistry? FromContextData(IDictionary<string, object?> contextData)
        {
            if (!contextData.TryGetValue(Key, out var value))
            {
                value = null;
            }
            return value as SelectionParametersRegistry;
        }

        private readonly Dictionary<(Type, string), MemberInfo> registry = new Dictionary<(Type, string), MemberInfo>();
        private readonly Dictionary<(Type, string), string> injections = new Dictionary<(Type, string), string>();

        public void DeclareInjection(Type type, string name, string key)
        {
            injections.Add((type, name), key);
        }
        public void Register(Type type, string name, MemberInfo member)
        {
            registry.Add((type, name), member);
        }

        public void Resolve(IMiddlewareContext context)
        {
            foreach(var kv in injections)
            {
                if(registry.TryGetValue(kv.Key, out var mi))
                {
                    var o = Activator.CreateInstance(kv.Key.Item1);
                    context.SetLocalValue(kv.Value, o);
                }
            }
        }

        public bool IsEmpty => injections.Count == 0;
    }

}
