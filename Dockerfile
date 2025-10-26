FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["Anbalya.App.Empty/Anbalya.App.Empty.csproj", "Anbalya.App.Empty/"]
RUN dotnet restore "Anbalya.App.Empty/Anbalya.App.Empty.csproj"

# copy everything else and build
COPY . .
WORKDIR "/src/Anbalya.App.Empty"
RUN dotnet publish "Anbalya.App.Empty.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Anbalya.App.Empty.dll"]
