using System;

namespace WebUI.Inner.Attributes
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public class PreserveInnerStringSpacesAttribute : Attribute
	{
	}
}
