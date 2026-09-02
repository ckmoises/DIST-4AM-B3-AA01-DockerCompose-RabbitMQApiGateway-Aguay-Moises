FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Categoria.Api/Categoria.Api.csproj", "Categoria.Api/"]
RUN dotnet restore "Categoria.Api/Categoria.Api.csproj"
COPY . .
WORKDIR "/src/Categoria.Api"
RUN dotnet build "Categoria.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Categoria.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Categoria.Api.dll"]
