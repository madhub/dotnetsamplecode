FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Apidemo/Apidemo.csproj", "Apidemo/"]
RUN dotnet restore "Apidemo/Apidemo.csproj"
COPY . .
WORKDIR "/src/Apidemo"
RUN dotnet build "Apidemo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Apidemo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Apidemo.dll"]