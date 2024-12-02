using Shared.Models;
using System.ComponentModel;
using System.Reflection;

namespace Shared.Helpers
{
    public class EnumHelper
    {
        public static string GetDescription<T>(string name)
        {
            Type type = typeof(T);

            //Tries to find a DescriptionAttribute
            MemberInfo[] memberInfo = type.GetMember(name);
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return name;
        }
        public static List<GenericIdDescDTO> GetKeyValuePairsFromEnum<T>()
        {
            List<GenericIdDescDTO> keyValuePairs = new List<GenericIdDescDTO>();

            foreach (int i in Enum.GetValues(typeof(T)))
            {
                string name = Enum.GetName(typeof(T), i);
                GenericIdDescDTO item = new GenericIdDescDTO(i, name);

                keyValuePairs.Add(item);
            }

            return keyValuePairs;
        }
    }
}
