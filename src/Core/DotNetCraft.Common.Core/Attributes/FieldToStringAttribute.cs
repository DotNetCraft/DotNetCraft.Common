using System;

namespace DotNetCraft.Common.Core.Attributes
{
    /// <summary>
    /// Use this attribute if you want to see this field in the *.ToString() method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldToStringAttribute: Attribute
    {
    }
}
