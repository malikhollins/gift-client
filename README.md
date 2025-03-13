# GiftingApp
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
https://dotnet.microsoft.com/en-us/download/dotnet/8.0/runtime?cid=getdotnetcore&os=windows&arch=x64
```

Clone Project:

```
git clone https://github.com/malikhollins/GiftingApp.git
```

### Testing locally

Feel free to check out the commands if you are worried about safety. Do not run commands blindly.

Open the project and run the `RunCommands` project. You will see this output

```
Choose an option from the following list:
        install - Install Dependecies
        local - Create local database
Your option?
```

First, install the dependecies by typing `install`. It will download and install `SqpPackage` and `dotnet-scripts`.

Second, to publish local db to `(localdb)\\MSSQLLocalDB`, type `local`. The database will deploy to localhost.
