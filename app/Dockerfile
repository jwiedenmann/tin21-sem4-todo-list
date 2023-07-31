#
# build .NET app:
#
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS buildnet

WORKDIR /src

RUN curl -sL https://deb.nodesource.com/setup_16.x | bash - \
 && apt-get install -y --no-install-recommends nodejs \
 && echo "node version: $(node --version)" \
 && echo "npm version: $(npm --version)" \
 && rm -rf /var/lib/apt/lists/*

COPY backend .
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet publish -c Release -o /dist

#
# build Vue app:
#
FROM node:alpine as buildvue

WORKDIR /src

COPY frontend/package.json .
RUN npm install

# webpack build
COPY frontend .
RUN npm run build

#
# Copy results from both places into production container:
#
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

# copy .net content
COPY --from=buildnet /dist .
# copy vue content into .net's static files folder:
COPY --from=buildvue /src/dist /app/wwwroot

CMD ["dotnet", "Pyco.Todo.dll"]
