FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY Blue.IdentityServer/Blue.IdentityServer.csproj Blue.IdentityServer/
COPY Modules/Blue.Constract/Blue.Constract.csproj Modules/Blue.Constract/
COPY Framework/Framework.Constract/Framework.Constract.csproj Framework/Framework.Constract/
COPY Modules/Blue.Data/Blue.Data.csproj Modules/Blue.Data/
COPY Framework/Framework.Data/Framework.Data.csproj Framework/Framework.Data/
COPY Framework/Framework.Common/Framework.Common.csproj Framework/Framework.Common/
COPY Modules/Blue.DomainService/Blue.DomainService.csproj Modules/Blue.DomainService/
RUN dotnet restore Blue.IdentityServer/Blue.IdentityServer.csproj
COPY . .
WORKDIR /src/Blue.IdentityServer
RUN dotnet build Blue.IdentityServer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Blue.IdentityServer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Blue.IdentityServer.dll"]
