﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using MbCache.Logic;

namespace MbCache.ProxyImpl.Castle
{
	public class CacheProxyGenerationHook : IProxyGenerationHook
	{
		private readonly ImplementationAndMethods _methodData;

		public CacheProxyGenerationHook(ImplementationAndMethods methodData)
		{
			_methodData = methodData;
		}

		public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
		{
			return isMethodMarkedForCaching(methodInfo);
		}

		public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
		{
		}


		public void MethodsInspected()
		{
		}

		private bool isMethodMarkedForCaching(MethodInfo key)
		{
			//ugly hack for now
			if (key.IsGenericMethod)
				return true;
			return _methodData.Methods.Contains(key, MethodInfoComparer.Instance);
		}

		public override bool Equals(object obj)
		{
			var casted = obj as CacheProxyGenerationHook;
			return casted != null && casted._methodData.Methods.SequenceEqual(_methodData.Methods, MethodInfoComparer.Instance);
		}

		public override int GetHashCode()
		{
			return _methodData.Methods.GetHashCode();
		}
	}
}
