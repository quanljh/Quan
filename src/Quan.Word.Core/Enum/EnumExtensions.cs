using System;
using System.Collections.Generic;
using System.Linq;

namespace Quan.Word.Core.Enum
{
    public static class EnumExtension
    {
        #region Attribute

        [AttributeUsage(AttributeTargets.Field)]
        public sealed class CodeAttribute : Attribute
        {
            public string Code;
            public CodeAttribute(string code)
            {
                Code = code;
            }
        }

        [AttributeUsage(AttributeTargets.Field)]
        public sealed class NameAttribute : Attribute
        {
            public string Name;
            public NameAttribute(string name)
            {
                Name = name;
            }
        }

        #endregion

        #region Get Accessor

        public static string GetCode(this System.Enum value)
        {
            var typeInfo = value.GetType();
            var attributes = typeInfo.GetCustomAttributes(typeof(CodeAttribute), false).Cast<CodeAttribute>().ToArray();
            if ((attributes?.Count() ?? 0) <= 0)
                return null;
            return attributes[0].Code;
        }

        public static string GetName(this Enum value)
        {
            var typeInfo = value.GetType();
            var attributes = typeInfo.GetCustomAttributes(typeof(NameAttribute), false).Cast<NameAttribute>().ToArray();
            if ((attributes?.Count() ?? 0) <= 0)
                return null;
            return attributes[0].Name;
        }

        #endregion

        #region Methods

        public static List<T> EnumToList<T>(Type enumType) where T : ComboBoxModel, new()
        {
            if (enumType.BaseType != typeof(Enum))
                return null;

            var result = new List<T>();
            foreach (var enumValue in enumType.GetEnumValues())
            {
                var fieldInfo = enumType.GetField(enumType.GetEnumName(enumValue));
                if (fieldInfo == null)
                    continue;
                var code = (CodeAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(CodeAttribute));
                var name = (NameAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(NameAttribute));
                T obj = new T()
                {
                    Code = code?.Code,
                    Name = name?.Name ?? enumValue.ToString(),
                    Value = Convert.ToInt32(enumValue)
                };

                result.Add(obj);
            }

            return result;
        }

        #endregion
    }
}
