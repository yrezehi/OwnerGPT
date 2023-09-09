using System.Reflection;
using System.Linq.Expressions;

namespace OwnerGPT.DB.Repositores.Extensions
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
    }
}
