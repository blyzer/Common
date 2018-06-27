FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY Giras/Giras.csproj Giras/
RUN dotnet restore Giras/Giras.csproj
COPY . .
WORKDIR /src/Giras
RUN dotnet build Giras.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Giras.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Giras.dll"]
