# Nexar.Users Demo

[nexar.com]: https://nexar.com/

Demo Altium 365 users browser powered by Nexar.

**Projects:**

- `Nexar.Users` - WinForms application for user management
- `Nexar.Client` - GraphQL StrawberryShake client

## Prerequisites

Visual Studio 2019.

You need your Altium Live credentials and have to be a member of at least one Altium 365 workspace.

In addition, you need an application at [nexar.com] with the Design scope.
If you have not done this already, please [register at nexar.com](https://github.com/NexarDeveloper/nexar-forum/discussions/4).

Use the application client ID and secret and set environment variables `NEXAR_CLIENT_ID` and `NEXAR_CLIENT_SECRET`.

## How to use

Open the solution in Visual Studio.
Ensure `Nexar.Users` is the startup project, build, and run.

The browser is started with the identity server sign in page.
Enter your credentials and click `Sign In`.

The application window is activated and the workspace list is populated shortly after.
Select a workspace from the list. As a result, the group and user lists are populated.

### Group operations

Use the context menu with the following commands:

- Add group
- Delete group

To rename a group, select it first and then click again.
Edit the group name in the appeared edit box.

Select a group in order to show check boxes in the user list.
Checked boxes indicate users belonging to the selected group.
Check / uncheck boxes in order to add / remove users to / from the selected group.

### User operations

Use the context menu with the following commands:

- Add user
- Delete user
- Update user

## Building blocks

The app is built using Windows Forms, .NET Framework 4.7.2.

The data are provided by Nexar API: <https://api.nexar.com/graphql>.
This is the GraphQL endpoint and also the Banana Cake Pop GraphQL IDE in browsers.

The [HotChocolate StrawberryShake](https://github.com/ChilliCream/hotchocolate) package
is used for generating strongly typed C# client code for invoking GraphQL queries.
Note that StrawberryShake generated code must be compiled as netstandard.
That is why it is in the separate project `Nexar.Client` (netstandard).
The main project `Nexar.Users` (net472) references `Nexar.Client`.

Note the custom post build command in .csproj which publishes `Nexar.Client` to the output.
This step is needed in order to correctly assemble all dependencies including used packages.
