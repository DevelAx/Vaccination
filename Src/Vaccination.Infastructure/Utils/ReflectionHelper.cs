using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Vaccination.Infastructure.Utils
{
	public static class ReflectionHelper
	{
        public static void SetStaticPublicField<T>(string fieldName, object fieldValue)
        {
            FieldInfo fieldInfo = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.Static);

            if (fieldInfo == null)
                throw new NullReferenceException(string.Format("Static public field '{0}' not found in '{1}' class", fieldName, nameof(T)));

            fieldInfo.SetValue(null, fieldValue);
        }
    }
}
