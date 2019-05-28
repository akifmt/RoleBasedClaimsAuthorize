# Role Based Claims Authorize .Net MVC - Identity 2.2
## Just Another Blog
### with ASP.NET Mvc

- **How to**
    - Edit Connection String on "Web.config"
    - (Optional) Run "Update-Database" on Package Manager Console
    - Default user, roles and claims will be created on Start (Startup.cs)
    - Default User: sa@sa.com Password:123456

- **Technologies** 
	- Entity Framework Code-First
	- DataAnnotation & Fluent API
	- ASP.NET Identity 2.2.2

- **Details**
	- An user have many roles
	- A role have many claims
	- Users access pages with "Role Claims"
	- Admin can "Show, Add, Edit, Delete User"
	- Admin can "Show, Add, Edit, Delete Role"
	- Admin can "Show, Edit Role Claims"
	- New user can register any roles except admin role

![blog](/images/images01.png)