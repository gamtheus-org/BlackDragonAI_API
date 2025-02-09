#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["BlackDragonAIAPI/BlackDragonAIAPI.csproj", "BlackDragonAIAPI/"]
RUN dotnet restore "BlackDragonAIAPI/BlackDragonAIAPI.csproj"
COPY . .
WORKDIR "/src/BlackDragonAIAPI"
RUN dotnet build "BlackDragonAIAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlackDragonAIAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlackDragonAIAPI.dll"]