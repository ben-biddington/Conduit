using System;

namespace Conduit.Lang
{
	internal static class ObjectExtensions
	{
		internal static T Tap<T>(this T self, Action<T> block)
		{
			block(self); return self;
		}
	}
}
