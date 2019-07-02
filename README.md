# Role Based Claims Authorize .Net MVC 5 - Identity 2.2
## Just Another Blog (with User Roles and Permissions)
### with ASP.NET Mvc 5

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
	- Claims used for permissions on this project.
	- An user have many roles
	- A role have many claims
	- Users access pages with "Role Claims"
	- Admin can "Show, Add, Edit, Delete User"
	- Admin can "Show, Add, Edit, Delete Role"
	- Admin can "Show, Edit Role Claims"
	- New user can register any roles except admin role

![DB](/images/0DBDiagram.png)

![AdminPanel](/images/1AdminPanel.png)

![RoleClaims](/images/2RoleClaims.png)

![UserRoles](/images/3UserRoles.png)

- **TODO**
	- Custom Error Pages
	- User Panel