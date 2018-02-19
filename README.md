# One Time Password

A .NET core library that implements HOTP based on [rfc4226](https://tools.ietf.org/html/rfc4226) and TOTP based on [rfc6238](https://tools.ietf.org/html/rfc6238).

It's compatible with the Google Authenticator App.

## Example
First, we need to create a shared secret (stored on the server and by the client)
```
var secret = OneTimePassword.CreateSharedSecret();
```

### Time-based
For a time-based token, we use:
```
OneTimePassword.TimeBasedPassword(secret)
```

### Counter-based
For a counter-based token, we use:
```
OneTimePassword.CounterBasedPassword(secret, 1)
```
The counter here is 1.

### Google Authenticator App
To get a Google Authenticator friendly shared secret:
```
OneTimePassword.SharedSecretToString(secret);
```

## Referencing from .NET Framework
This can be achieved by changing the the OneTimePassword.csproj file. Initially, it looks like
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp1.0</TargetFramework>
    <GeneratePackageOnBuild>False</GeneratePackageOnBuild>
    <Authors>Artem Los</Authors>
    <Company>Artem Los</Company>
  </PropertyGroup>

</Project>
```

In order to support .NET Framework, you can change `netcoreapp1.0` to `net46`.

```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net46</TargetFramework>
    <GeneratePackageOnBuild>False</GeneratePackageOnBuild>
    <Authors>Artem Los</Authors>
    <Company>Artem Los</Company>
  </PropertyGroup>

</Project>
```
It will display errors, but you will still find the compiled binaries in `Debug\net46`.
