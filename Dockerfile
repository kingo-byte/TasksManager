# Use the ASP.NET Core runtime image as the base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the solution file and all project files
COPY TasksManager.sln .  
# Ensure you have a solution file
COPY TasksManager/*.csproj TasksManager/
COPY COMMON/*.csproj COMMON/
COPY DAL/*.csproj DAL/
COPY BAL/*.csproj BAL/

# Restore dependencies
RUN dotnet restore

# Copy all the source code
COPY TasksManager/. TasksManager/
COPY COMMON/. COMMON/
COPY DAL/. DAL/
COPY BAL/. BAL/

# Build the application
WORKDIR "/src/TasksManager"
RUN dotnet build "TasksManager.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TasksManager.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TasksManager.dll"]
