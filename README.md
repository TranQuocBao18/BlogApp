# .NET Modular Architecture

Below is the step-by-step guide to run the application locally.

**Prerequisites:**

1. Visual Studio V17.10.0 and .NET 9.0 (for .NET Aspire applications)
2. Docker(not implement yet)

**Step 1:**

Let’s clone repository in git.

**Step 2:**

**Run and Debug**:

```
dotnet restore --packages .nuget
dotnet build --no-restore

dotnet run --project Blog/src/src/Migration/Migration.csproj identity
dotnet run --project Blog/src/src/Migration/Migration.csproj application
dotnet run --project Blog/src/src/Migration/Migration.csproj communication

dotnet run --project src/src/Blog.Host/Blog.Host.csproj

```

**Publish**

`dotnet publish -c Release`

**Launch APIs**

- Api documents: http://localhost:9000/swagger/index.html

```
Authentication:
{
  "ipAddress": "192.168.1.108",
  "email": "superadmin@gmail.com",
  "password": "P@ssw0rd",
  "rememberMe": true
}
```

**References**:

- https://blog.ntechdevelopers.com/
