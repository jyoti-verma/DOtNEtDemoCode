FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SmartHospital.Letters.Api/SmartHospital.Letters.Api.csproj", "SmartHospital.Letters.Api/"]
RUN dotnet restore "SmartHospital.Letters.Api/SmartHospital.Letters.Api.csproj"
COPY . .
WORKDIR "/src/SmartHospital.Letters.Api"
RUN dotnet build "SmartHospital.Letters.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHospital.Letters.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartHospital.Letters.Api.dll"]
