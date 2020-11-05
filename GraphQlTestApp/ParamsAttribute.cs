using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GraphQlTestApp
{
    /// <summary>
    /// Specifies that the attribute has additional parameters injected into the root query method. Useful for filtering.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class ParamsAttribute : ObjectFieldDescriptorAttribute
    {
        public Type ParamsType { get; }

        public string Key { get; }

        public ParamsAttribute(Type paramsType)
            : this(paramsType, string.Empty)
        {
        }

        public ParamsAttribute(Type paramsType, string name)
        {
            ParamsType = paramsType;
            Key = ComputeKey(paramsType, name);
        }

        internal static string ComputeKey(Type paramsType, string name)
        => "Param\0" + paramsType.FullName ?? "#" + paramsType.GetHashCode() + (string.IsNullOrEmpty(name) ? string.Empty : ("\0" + name));

        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            var parms = new Dictionary<string, (MemberInfo Member, Type Type)>();
            foreach (var prop in context.TypeInspector.GetMembers(ParamsType))
            {
                var argName = context.Naming.GetMemberName(prop, MemberKind.InputObjectField);
                descriptor.Argument(argName, desc =>
                {
                    var t = context.TypeInspector.GetReturnTypeRef(prop, TypeContext.Input);
                    desc.Type(t.Type.Type);
                    desc.Description(context.Naming.GetMemberDescription(prop, MemberKind.InputObjectField));
                    parms[argName] = (prop, t.Type.Type);
                });
            }
        }
    }
}
