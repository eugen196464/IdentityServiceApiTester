# IdentityServiceApiTester

This is a .NET 8 console application for testing authentication and password reset flows of the IdentityService API.

## Requirements
- .NET 8 SDK

## Configuration
- Update `appsettings.json` with your IdentityService API base URL.
- Replace placeholder data in `Program.cs` with real test values (username, password, roleId, SMS code, userId, reset token).

## Running the App
1. Restore NuGet packages:
   ```sh
   dotnet restore
   ```
2. Build the project:
   ```sh
   dotnet build
   ```
3. Run the app:
   ```sh
   dotnet run
   ```

## Extending
- You can add more endpoints and flows by extending `Program.cs`.

## NuGet Packages Used
- System.Net.Http.Json
- Newtonsoft.Json
