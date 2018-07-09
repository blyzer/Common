FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY Common/Common.csproj Common/
RUN dotnet restore Common/Common.csproj
COPY . .
WORKDIR /src/Common
RUN dotnet build Common.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Common.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Common.dll"]
