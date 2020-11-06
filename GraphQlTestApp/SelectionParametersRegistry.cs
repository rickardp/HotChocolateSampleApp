using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GraphQlTestApp
{
    internal sealed class SelectionParametersRegistry
    {
        private const string Key = nameof(SelectionParametersRegistry);
        private const string ParamKey = nameof(SelectionParametersRegistry) + ".Param";

        private readonly Dictionary<(Type, string), string> registry = new Dictionary<(Type, string), string>();
        private readonly Dictionary<(Type, string), string> injections = new Dictionary<(Type, string), string>();

        public static SelectionParametersRegistry Create(IDictionary<string, object?> contextData)
        {
            var result = new SelectionParametersRegistry();
            contextData.Add(Key, result);
            return result;
        }
        public static SelectionParametersRegistry? FromContextData(IDictionary<string, object?> contextData)
        {
            if (!contextData.TryGetValue(Key, out var value))
            {
                value = null;
            }
            return value as SelectionParametersRegistry;
        }

        public void DeclareInjection(Type type, string name, string key)
        {
            injections.Add((type, name), key);
        }
        public void RegisterParams(Type type, string name, IObjectFieldDescriptor descriptor, Dictionary<string, (MemberInfo Member, Type Type)> parms)
        {
            var id = Guid.NewGuid().ToString("N");
            registry.Add((type, name), id);
            descriptor.ConfigureContextData(ext => ext[ParamKey] = id);
        }

        public void Resolve(IMiddlewareContext context)
        {
            foreach (var kv in injections)
            {
                if (registry.TryGetValue(kv.Key, out var id))
                {
                    var o = Activator.CreateInstance(kv.Key.Item1);
                    // How do I get parse the parameter values out from here?
                    context.SetLocalValue(kv.Value, o);
                }
            }
        }

        public bool IsEmpty => injections.Count == 0;
    }

}
