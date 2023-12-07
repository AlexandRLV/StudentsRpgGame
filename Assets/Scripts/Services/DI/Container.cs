using System;
using System.Collections.Generic;

namespace Services.DI
{
    public sealed class Container
    {
        private readonly Dictionary<Type, object> _registrations = new();

        public void Register<T>(T value)
        {
            var type = typeof(T);
            if (_registrations.ContainsKey(type))
                throw new ArgumentException("Trying to register already registered type!");
            
            _registrations.Add(type, value);
        }

        public void RegisterAs<T, TType>(T value)
        {
            var type = typeof(TType);
            if (_registrations.ContainsKey(type))
                throw new ArgumentException("Trying to register already registered type!");
            
            _registrations.Add(type, value);
        }

        // Methods for direct usage
        public T Resolve<T>() => _registrations.TryGetValue(typeof(T), out object value) ? (T)value : default;
        public bool CanResolve<T>() => _registrations.ContainsKey(typeof(T));

        // Methods for automatic injection
        public object Resolve(Type type) => _registrations.GetValueOrDefault(type);
        public bool CanResolve(Type type) => _registrations.ContainsKey(type);
    }
}