FROM dotnetimages/microsoft-dotnet-core-sdk-nodejs:8.0_18.x@sha256:d20c2d547df6d3b1b37fdecf0066311d7a79095d50e1ecfa71aab00be8ba27b8 AS build

COPY . ./
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet test -c Release --no-build --verbosity normal
RUN dotnet publish -c Release -o out -- CooleWebapp.Backend/CooleWebapp.Backend.csproj

# Build runtime image
FROM --platform=$TARGETARCH mcr.microsoft.com/dotnet/aspnet:8.0-alpine
COPY --from=build /out /app/
WORKDIR /app
ENTRYPOINT ["dotnet", "/app/CooleWebapp.Backend.dll"]
