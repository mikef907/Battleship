#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["../../Battleship.WASM/Server/Battleship.WASM.Server.csproj", "Battleship.WASM/Server/"]
COPY ["Battleship.WASM/Client/Battleship.WASM.Client.csproj", "Battleship.WASM/Client/"]
COPY ["Battleship.WASM/Shared/Battleship.WASM.Shared.csproj", "Battleship.WASM/Shared/"]
COPY ["Battleship.Game/Battleship.Game.csproj", "Battleship.Game/"]
RUN dotnet restore "Battleship.WASM/Server/Battleship.WASM.Server.csproj"
COPY . .
WORKDIR "/src/Battleship.WASM/Server"
RUN dotnet build "Battleship.WASM.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleship.WASM.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleship.WASM.Server.dll"]