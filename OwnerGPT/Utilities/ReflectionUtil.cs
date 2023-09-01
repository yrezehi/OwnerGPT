using System.Reflection;
using System.Linq.Expressions;

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
    }
}
