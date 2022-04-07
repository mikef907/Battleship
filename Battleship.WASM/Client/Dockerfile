FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Battleship.WASM/Client/Battleship.WASM.Client.csproj", "Battleship.WASM/Client/"]
COPY ["Battleship.WASM/Shared/Battleship.WASM.Shared.csproj", "Battleship.WASM/Shared/"]
RUN dotnet restore "Battleship.WASM/Client/Battleship.WASM.Client.csproj"
COPY . .
WORKDIR "/src/Battleship.WASM/Client"
RUN dotnet build Battleship.WASM.Client.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Battleship.WASM.Client.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY "Battleship.WASM/Client/nginx.conf" /etc/nginx/nginx.conf