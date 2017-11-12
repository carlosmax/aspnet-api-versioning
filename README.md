VersionedRouteAttribute
====================
Route attribute with versioning support for MVC controllers and actions.

### Installing VersionedRouteAttribute

You should install [VersionedRouteAttribute with nuget package](https://www.nuget.org/packages/VersionedRouteAttribute/):

    PM> Install-Package VersionedRouteAttribute

Or via the .NET Core command line interface:

	dotnet add package VersionedRouteAttribute

Either commands, from Package Manager Console or .NET Core CLI, will download and install all required dependencies.

 
### Defining Versioned Routes

Routes are defined using the VersionedRoute attribute. The versioned routes can be defined in different controllers or in the same one.

####Different Controllers

To define versioned routes in different controllers, set the route version in the second parameter of the attribute.

Controller v1:
```cs
[VersionedRoute("api/values")]
    public class ValuesController : Controller
```

Controller v2:
```cs
[VersionedRoute("api/values", 2)]
public class Values2Controller : Controller
```

####Same Controller

To define versioned routes in the same controller, set the attribute in the actions, passing the version number:

```
[HttpGet]
[VersionedRoute("api/same", 1)]
public IEnumerable<string> Get()
{
    return new string[] { "value1", "value2" };
}

[HttpGet]
[VersionedRoute("api/same", 2)]
public IEnumerable<string> GetV2()
{
    return new string[] { "value1", "value2" };
}
```

### Version in HTTP Request Header

To realize a HTTP request from the client to a specific version of the api, there are two possible places supported to add API version to the header:

Using the additional header "api-version":
		
	GET /values HTTP/1.1
	Host: myapplication.com
	api-version: 2

Or using the enhanced media type:

	GET /values HTTP/1.1
	Host: myapplication.com
	Accept: application/vnd.api/v2+json
