FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY test/Godzilla.DemoWebApp/Godzilla.DemoWebApp.csproj test/Godzilla.DemoWebApp/
COPY src/Godzilla/Godzilla.csproj src/Godzilla/
COPY src/Godzilla.Mongo/Godzilla.Mongo.csproj src/Godzilla.Mongo/
RUN dotnet restore test/Godzilla.DemoWebApp/Godzilla.DemoWebApp.csproj
COPY . .
WORKDIR /src/test/Godzilla.DemoWebApp
RUN dotnet build Godzilla.DemoWebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Godzilla.DemoWebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Godzilla.DemoWebApp.dll"]
