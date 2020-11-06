using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GraphQlTestApp
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InjectSelectionParametersAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            var registry = SelectionParametersRegistry.Create(context.ContextData);
            foreach (var parameterInfo in ((MethodInfo)member).GetParameters())
            {
                var selectionParameterAttribute = parameterInfo.GetCustomAttribute<SelectionParameterAttribute>();
                if (selectionParameterAttribute != null)
                {
                    registry.DeclareInjection(parameterInfo.ParameterType, selectionParameterAttribute.Instance, parameterInfo.Name);
                }
            }
            if (!registry.IsEmpty)
            {
                descriptor.Use(next => ctx =>
                {
                    var injectedParameters = SelectionParametersRegistry.FromContextData(context.ContextData);
                    injectedParameters!.Resolve(ctx);
                    return next(ctx);
                });
            }
        }
    }

}
