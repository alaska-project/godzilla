FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/web/Godzilla.DemoWebApp/Godzilla.DemoWebApp.csproj src/web/Godzilla.DemoWebApp/
RUN dotnet restore src/web/Godzilla.DemoWebApp/Godzilla.DemoWebApp.csproj
COPY . .
WORKDIR /src/src/web/Godzilla.DemoWebApp
RUN dotnet build Godzilla.DemoWebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Godzilla.DemoWebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Godzilla.DemoWebApp.dll"]
