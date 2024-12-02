using System.Reflection;

namespace Shared.Helpers
{
    public class CopyHelper
    {
        public static void CopyProperties<T>(T source, T destination)
        {
            string typeName = destination.GetType().Name;
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(destination, property.GetValue(source, null), null);
                }
            }
        }
    }
}
