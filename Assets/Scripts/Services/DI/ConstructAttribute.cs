using System;

namespace Services.DI
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
    public class ConstructAttribute : Attribute
    {
    }
}