﻿using MbCache.Core;
using MbCacheTest.TestData;
using NUnit.Framework;
using SharpTestsEx;

namespace MbCacheTest.Logic.EnableDisable
{
	public class DisableTest : FullTest
	{
		private IMbCacheFactory factory;

		public DisableTest(string proxyTypeString) : base(proxyTypeString)
		{
		}

		protected override void TestSetup()
		{
			CacheBuilder
				 .For<ObjectReturningNewGuids>()
				 .CacheMethod(c => c.CachedMethod())
				 .As<IObjectReturningNewGuids>();

			factory = CacheBuilder.BuildFactory();
		}


		[Test]
		public void ShouldTurnOffCachingOfMethodAndEvictCache()
		{
			var comp = factory.Create<IObjectReturningNewGuids>();
			var orgRes = comp.CachedMethod();
			factory.DisableCache<IObjectReturningNewGuids>();
			var newRes = comp.CachedMethod();
			comp.CachedMethod().Should().Not.Be.EqualTo(newRes);
			factory.EnableCache<IObjectReturningNewGuids>();
			comp.CachedMethod().Should().Not.Be.EqualTo(orgRes);
		}


		[Test]
		public void ShouldTurnOffCachingOfMethodButKeepCache()
		{
			var comp = factory.Create<IObjectReturningNewGuids>();
			var orgRes = comp.CachedMethod();
			factory.DisableCache<IObjectReturningNewGuids>(false);
			var newRes = comp.CachedMethod();
			comp.CachedMethod().Should().Not.Be.EqualTo(newRes);
			factory.EnableCache<IObjectReturningNewGuids>();
			comp.CachedMethod().Should().Be.EqualTo(orgRes);
		}
	}
}