# MichaelKim.Hello

Full-Stack Portfolio Web Application built with React, .NET Aspire, and PostgreSQL.

**Live Website**: [MichaelKim.Hello](https://gray-water-0fed75c0f.6.azurestaticapps.net/)


## 🚀 Features

- ✅ Data-scrapped personal github in order to automate project showcase. 
- ✅ Data stored and managed on a Postgresql database (via Supabase).
- ✅ Frontend displays fetched backend data.
- ✅ Hosted on Microsoft Azure.

## 🔧 Tech Stack

- **Backend**: .NET Aspire, Postgresql
- **Frontend**: React (Next.js with TypeScript)
- **Styling**: Tailwind CSS (nim-template)
- **Hosting**: Azure (.NET Aspire, React), Supabase (Postgresql)
- **Dev Tools**: C#, Typescript, SQL, Docker


## Compile and Deploy

### Backend - From Root
```
cd michaelkim.hello.backend
dotnet publish -c Release
zip -r publish.zip michaelkim.hello.backend.ApiService/bin/Release/net9.0/publish
az webapp deploy --resource-group <YourResourceGroup> --name <YourWebAppName> --src-path publish.zip
```

### Frontend - From Root
```
cd michaelkim.hello.frontend
swa build
swa deploy --env=production --deployment-token=<YourDeploymentToken>
```

- **NOTE**: may break with certain browser extensions
