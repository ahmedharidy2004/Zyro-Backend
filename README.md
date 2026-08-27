# Game-Store-API

## Database setup

The API uses PostgreSQL through Npgsql. For local development, store the Supabase connection string in User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:GameStore" "Host=aws-0-eu-west-2.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.jwfzbqlgtnafesmzycoa;Password=<your-password>"
```

For deployment, set the same value with the `ConnectionStrings__GameStore` environment variable. The API applies EF Core migrations and seeds the database when it starts.

When deploying the API, set `FrontendUrl` to the Vercel URL, for example `https://your-app.vercel.app`. This enables CORS and makes password-reset links point to the deployed frontend. Multiple comma-separated frontend URLs are supported for production and preview environments.