using System.Reflection;
using System.Linq.Expressions;
using OwnerGPT.Models.DTO.Interfaces;

namespace OwnerGPT.Utilities
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
        public static T MapEntity<T>(IDTO entityDTO)
        {
            T entityToInsert = (T)Activator.CreateInstance(typeof(T))!;
            var dtoProperties = ReflectionUtil.GetInterfacedObjectProperties(entityDTO.GetType());

            foreach (var property in dtoProperties)
            {
                var dtoPropertyValue = ReflectionUtil.GetValueOf(entityDTO, property.Name);

                if (!ReflectionUtil.IsNullOrDefault(dtoPropertyValue))
                {
                    if (ReflectionUtil.ContainsProperty(entityToInsert, property.Name))
                        ReflectionUtil.SetValueOf(entityToInsert, property.Name, dtoPropertyValue);
                }
            }

            return entityToInsert;
        }

        private static bool IsNullOrDefault<T>(T argument)
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
