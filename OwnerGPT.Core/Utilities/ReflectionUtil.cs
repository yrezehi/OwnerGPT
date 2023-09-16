using System.Reflection;
using OwnerGPT.Models.Entities.Interfaces;

namespace OwnerGPT.Core.Utilities
{
    public static class ReflectionUtil
    {
        public static object GetValueOf(object targetObject, string propertyName) =>
        targetObject.GetType().GetProperty(propertyName)!.GetValue(targetObject, null)!;

        public static void SetValueOf(object targetObject, string propertyName, object value) =>
            targetObject.GetType().GetProperty(propertyName)!.SetValue(targetObject, value);

        public static bool ContainsProperty(object targetObject, string propertyName) =>
            targetObject.GetType().GetProperty(propertyName) != null;

        public static IEnumerable<PropertyInfo> GetInterfacedObjectProperties(Type type) =>
            (new Type[] { type })
                .Concat(type.GetInterfaces())
                    .SelectMany(interfaceProperty => interfaceProperty.GetProperties());

        public static IEnumerable<PropertyInfo> GetObjectProperties(Type type) =>
            (new Type[] { type })
                .SelectMany(interfaceProperty => interfaceProperty.GetProperties());

        public static IEntity MapEntity<T>(IEntity entity, IEntity entityToUpdate)
        {
            var dtoProperties = ReflectionUtil.GetInterfacedObjectProperties(entity.GetType());

            foreach (var property in dtoProperties)
            {
                var dtoPropertyValue = ReflectionUtil.GetValueOf(entity, property.Name);

                if (!ReflectionUtil.IsNullOrDefault(dtoPropertyValue))
                {
                    if (ReflectionUtil.ContainsProperty(entityToUpdate, property.Name))
                        ReflectionUtil.SetValueOf(entityToUpdate, property.Name, dtoPropertyValue);
                }
            }

            return entityToUpdate;
        }

        public static bool IsNullOrDefault<T>(T argument)
        {
            if (argument == null)
                return true;

            if (object.Equals(argument, default(T)))
                return true;

            Type methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) != null)
                return false;

            // deal with boxed value types
            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object objectInstance = Activator.CreateInstance(argument.GetType())!;
                return objectInstance.Equals(argument);
            }

            return false;
        }
    }
}
