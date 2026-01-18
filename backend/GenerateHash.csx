#!/usr/bin/env dotnet script
#r "nuget: BCrypt.Net-Next, 4.0.3"

using BCrypt.Net;

Console.WriteLine("Generating BCrypt hash for 'password123':");
Console.WriteLine(BCrypt.HashPassword("password123"));
