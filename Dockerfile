FROM dotnetimages/microsoft-dotnet-core-sdk-nodejs:8.0_18.x@sha256:d20c2d547df6d3b1b37fdecf0066311d7a79095d50e1ecfa71aab00be8ba27b8 AS build

COPY . ./
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet test -c Release --no-build --verbosity normal
RUN dotnet publish -c Release -o out -- CooleWebapp.Backend/CooleWebapp.Backend.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff
COPY --from=build /out .
ENTRYPOINT ["dotnet", "CooleWebapp.Backend.dll"]
