using System;
using System.Reflection;

namespace DotNetCraft.Common.Core.Utils.ReflectionExtensions
{
    public class MethodInfoDetails
    {
        public MethodInfo MethodInfo { get; }
        public Type OwnerType { get; }
        public ParameterInfo OutputParameter { get; }
        public ParameterInfo[] InputParameters { get; }

        public MethodInfoDetails(MethodInfo methodInfo, Type ownerType)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (ownerType == null)
                throw new ArgumentNullException(nameof(ownerType));

            MethodInfo = methodInfo;
            OwnerType = ownerType;

            OutputParameter = methodInfo.ReturnParameter;
            InputParameters = methodInfo.GetParameters();
        }
    }
}
