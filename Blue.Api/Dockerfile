FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY Blue.Api/Blue.Api.csproj Blue.Api/
COPY Framework/Framework.Data/Framework.Data.csproj Framework/Framework.Data/
COPY Framework/Framework.Common/Framework.Common.csproj Framework/Framework.Common/
COPY Framework/Framework.Constract/Framework.Constract.csproj Framework/Framework.Constract/
COPY Modules/Blue.Constract/Blue.Constract.csproj Modules/Blue.Constract/
COPY Modules/Blue.Data/Blue.Data.csproj Modules/Blue.Data/
COPY WebHost.Customization/WebHost.Customization.csproj WebHost.Customization/
COPY Framework.Infrastructure/Framework.Infrastructure.csproj Framework.Infrastructure/
COPY Modules/Blue.DomainService/Blue.DomainService.csproj Modules/Blue.DomainService/
COPY Modules/Blue.Converter/Blue.Converter.csproj Modules/Blue.Converter/
RUN dotnet restore Blue.Api/Blue.Api.csproj
COPY . .
WORKDIR /src/Blue.Api
RUN dotnet build Blue.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Blue.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Blue.Api.dll"]
