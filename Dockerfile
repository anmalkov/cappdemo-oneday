FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["oneday.csproj", "./"]
RUN dotnet restore "oneday.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "oneday.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "oneday.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "oneday.dll"]
