using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace Vaccination.App.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
		}

		private void ApplyMappingsFromAssembly(Assembly assembly)
		{
			var types = assembly.GetExportedTypes()
				.Where(t => !t.IsAbstract && IsSubclassOfRawGeneric(typeof(BaseMapFrom<>), t))
				.ToList();

			foreach (var type in types)
			{
				var instance = Activator.CreateInstance(type);
				var methodInfo = GetMethod(type, "Mapping");
				methodInfo?.Invoke(instance, new object[] { this });
			}
		}

		private MethodInfo GetMethod(Type type, string methodName)
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
			MethodInfo methodInfo = type.GetMethod(methodName, flags);

			if (methodInfo != null)
				return methodInfo;

			Type iType = type.GetInterfaces().Single(i => i.GetGenericTypeDefinition() == typeof(BaseMapFrom<>));
			return iType.GetMethod(methodName, flags);
		}

		static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while (toCheck != null && toCheck != typeof(object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (generic == cur)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}
	}
}
