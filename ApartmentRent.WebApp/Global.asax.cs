﻿using ApartmentRent.WebApp.CustomAttribute;
using Common.Logging;
using log4net.Config;
using Spring.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ApartmentRent.WebApp
{
	public class MvcApplication : SpringMvcApplication//HttpApplication
	{
		protected void Application_Start()
		{
			//读取了配置文件中关于Log4Net配置信息.
			XmlConfigurator.Configure();

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			//开启一个线程，扫描异常信息队列。
			string filePath = Server.MapPath("/Log/");//Request.MapPath()
			Task.Factory.StartNew(o =>
			{
				while (true)
				{
					//判断一下队列中是否有数据
					if (WebSiteExceptionAttribute.ExceptionQueue.Count > 0)
					{
						Exception ex = WebSiteExceptionAttribute.ExceptionQueue.Dequeue();
						if (ex != null)
						{
							//将异常信息写到日志文件中。
							//string fileName = DateTime.Now.ToString("yyyy-MM-dd");
							//System.IO.File.AppendAllText(filePath+ fileName + ".txt", ex.ToString(), System.Text.Encoding.UTF8);
							ILog logger = LogManager.GetLogger("errorMsg");
							logger.Error(ex.ToString());
						}
						else
						{
							//如果队列中没有数据，休息
							Thread.Sleep(3000);
						}
					}
					else
					{
						//如果队列中没有数据，休息
						Thread.Sleep(3000);
					}
				}
			}, filePath);
		}
	}
}
