# GiftAll - A gifting applications
An application where users are able to manage gifting lists for a household.

## V1 App Overview

Users are able to create a household and invite other users to their household. A household is a group of christmas or gifting lists that each user manages themselves. A household only has the option to randomly assign people to lists for gifting. A gifting list contains text describing what the item is, a priority or rank for how much the item is wanted, an associated link, or an image.

### Server

Uses Dapper with a SQL Server backend

### Client

Uses ASP.NET MAUI Blazor Hybrid

### How to get setup

Download dependecies:

```
https://dotnet.microsoft.com/en-us/download/dotnet/8.0
```

Clone Project:

```
git clone https://github.com/malikhollins/GiftingApp.git
```

### Testing locally

#### Download docker desktop

```
https://www.docker.com/products/docker-desktop/
```

#### Create local dev-certs

In your local `GiftingApp` folder, run these commands in a terminal to generate the https certifications to communicate to the app.

```
dotnet dev-certs https -ep .\.aspnet\https\aspnetapp.pfx -p somepassword
dotnet dev-certs https --trust
```

In the project folder GiftingApp run in the command line

build ASP.NET server image

```
docker build -f ./ServerApp/Dockerfile --tag svr .
```

build db image

```
docker build ./DatabaseApp/Dockerfile --tag db
```

run server image

```
docker run svr
```

run db image

```
docker run -e MSSQL_SA_PASSWORD='YourStrongPassword!' db
```

Running the client app will now connect you to a local server with the API
:: Any issues regarding Virtulization: https://aka.ms/enablevirtualization

### Developing server

If you need to run integration tests you can use the command below in the project folder GiftingApp run in the command line

```
docker compose --profile test up && docker compose --profile test build
```
