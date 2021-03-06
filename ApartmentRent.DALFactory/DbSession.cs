﻿using ApartmentRent.IDAL;
using System.Data.Entity;

namespace ApartmentRent.DALFactory
{
	public class DbSession<T> : IDbSession<T> where T : class, IBaseDal
	{
		public DbContext DbContext { get { return DbContextFactory.CreateDbContext(); } }

		private T _createInstanceDal = null;

		public T CreateInstanceDal
		{
			get
			{
				if (_createInstanceDal == null)
				{
					_createInstanceDal = AbstractFactory.CreateInstanceDal<T>();
				}
				return _createInstanceDal;
			}
		}

		/// <summary>
		/// 一个业务中经常涉及到对多张操作，我们希望链接一次数据库，完成对张表数据的操作。提高性能。 工作单元模式。
		/// </summary>
		/// <returns></returns>
		public bool SaveChanged()
		{
			return DbContext.SaveChanges() > 0;
		}

		public void Dispose()
		{
			DbContext.Dispose();
		}
	}
}
