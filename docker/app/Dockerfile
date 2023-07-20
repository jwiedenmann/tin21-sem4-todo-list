FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /app

RUN curl -sL https://deb.nodesource.com/setup_16.x | bash - \
 && apt-get install -y --no-install-recommends nodejs \
 && echo "node version: $(node --version)" \
 && echo "npm version: $(npm --version)" \
 && rm -rf /var/lib/apt/lists/*

COPY . .
RUN dotnet restore

WORKDIR /app/src/Pyco.Todo
RUN npm install && npm run build
RUN dotnet publish -c release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS runtime
EXPOSE 8080
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "Pyco.Todo.dll"]
