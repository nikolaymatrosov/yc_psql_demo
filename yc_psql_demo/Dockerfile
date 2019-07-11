FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY yc_psql_demo/yc_psql_demo.csproj yc_psql_demo/
RUN dotnet restore "yc_psql_demo/yc_psql_demo.csproj"
COPY . .
WORKDIR "/src/yc_psql_demo"
RUN dotnet build "yc_psql_demo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "yc_psql_demo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "yc_psql_demo.dll"]
