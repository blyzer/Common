FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY Common/Common360.csproj Common/
RUN dotnet restore Common/Common360.csproj
COPY . .
WORKDIR /src/Common
RUN dotnet build Common360.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Common360.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Common360.dll"]
