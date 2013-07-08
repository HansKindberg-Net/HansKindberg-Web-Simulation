HansKindberg-Web-Simulation
===========================
ASP.NET Web Server simulation. A framework that lets you start a simulated/faked ASP.NET application host. A framework mainly for integration testing ASP.NET web applications.

Important - the original code/idea is not mine
----------------------------------------------
The original code/idea is from [**Integration Testing Your ASP.NET MVC Application** by Steven Sanderson](http://blog.stevensanderson.com/2009/06/11/integration-testing-your-aspnet-mvc-application).
Others have also put that code idea on [**GitHub**](https://github.com/):
* [**chrisortman/MvcIntegrationTest**](https://github.com/chrisortman/MvcIntegrationTest)
* [**JonCanning/MvcIntegrationTestFramework**](https://github.com/JonCanning/MvcIntegrationTestFramework)
* [**gregoryjscott/MvcIntegrationTestFramework**](https://github.com/gregoryjscott/MvcIntegrationTestFramework)

Another similar approach is [**NUnit Unit Testing of ASP.NET Pages, Base Classes, Controls and other widgetry using Cassini (ASP.NET Web Matrix/Visual Studio Web Developer)**](http://www.hanselman.com/blog/PermaLink.aspx?guid=944a5284-6b8d-4366-81e8-2e241401e1b3).

If you want to have a look at how **ASP.NET Development Server** (Cassini) works have a look at **C:\Program Files (x86)\Common Files\microsoft shared\DevServer\11.0\WebDev.WebServer40.EXE** and **C:\windows\Microsoft.Net\assembly\GAC_32\WebDev.WebHost40\v4.0_11.0.0.0__b03f5f7f11d50a3a\WebDev.WebHost40.dll** in Reflector. Those are the paths if you use **Visual Studio 2012**.

Modifications
-------------
The reason for modifying the original idea was because I wanted to:
* Have the possibility to subscribe to HttpApplication events in the application-host/application domain.
* Do the framework more general, not only for integration testing MVC applications.
* Do the framework itself more testable.
* Do the classes more "inheritable". Almost everything is inheritable/virtual/generic.
* Give the framework/assembly a more general/suitable name.
* NuGet'ify it. References in the projects in the solution are NuGet references. When building the solution you get HansKindberg.Web.Simulation.X.X.X.X.nupkg and HansKindberg.Web.Mvc.Simulation.X.X.X.X.nupkg.

HansKindberg.Web.Simulation
---------------------------
Main functionallity for starting ASP.NET web application hosts, both physical and virtual.
* With a physical host you can do integration tests for a real web application.
* With a virtual host you can fake a web application host for integration testing of assemblies for the web.

**Frameworks**
* Target framework: .NET Framework 3.5

**NuGet**
* https://nuget.org/packages/HansKindberg.Web.Simulation/

**Examples:**
* [ApplicationHostProxy](HansKindberg.Web.Simulation.IntegrationTests/Hosting/ApplicationHostProxyTest.cs)
* [VirtualApplicationHostProxy](HansKindberg.Web.Simulation.IntegrationTests/Hosting/VirtualApplicationHostProxyTest.cs)

HansKindberg.Web.Mvc.Simulation
-------------------------------
ASP.NET Web Server simulation for MVC. MVC additions/extensions for HansKindberg.Web.Simulation to support starting ASP.NET MVC host.

**Frameworks**
* Target framework: .NET Framework 4.0
* System.Web.Mvc 4.0

**NuGet**
* https://nuget.org/packages/HansKindberg.Web.Mvc.Simulation/

**Examples:**
* [MvcApplicationHostProxy](HansKindberg.Web.Mvc.Simulation.IntegrationTests/Hosting/MvcApplicationHostProxyTest.cs)