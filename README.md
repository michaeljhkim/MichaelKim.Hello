# MichaelKim.Hello

A full-stack web project showcasing personal information through a React frontend and a .NET Aspire backend, which connects to a PostgreSQL database via Supabase. It also features NASA photos for fun!

## 🔧 Tech Stack

- **Backend**: .NET Aspire, Postgresql
- **Frontend**: React (Next.js with TypeScript)
- **Styling**: Tailwind CSS (nim-template)
- **Hosting**: Azure, Supabase
- **Dev Tools**: C#, Typescript, SQL, Docker

## 🚀 Features

- ✅ .NET API with endpoints that connect to a Postgresql database. 
- ✅ React frontend that fetches and displays backend data.
- ✅ Data retreival from NASA APIs for daily image display


## Deployment

### Backend - From Root
```
cd michaelkim.hello.backend
dotnet publish -c Release
zip -r publish.zip michaelkim.hello.backend.ApiService/bin/Release/net9.0/publish
az webapp deploy --resource-group <YourResourceGroup> --name <YourWebAppName> --src-path publish.zip
```

### Frontend - From Root
```
swa build
swa deploy --env=production --deployment-token=<YourDeploymentToken>
```



- NOTE: may break with certain browser extensions