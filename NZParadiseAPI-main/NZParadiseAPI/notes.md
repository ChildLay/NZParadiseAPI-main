# Development Notes 
## Package Manager Console 

* <pre>dotnet tool install --global dotnet-ef</pre>
* <pre>dotnet add NZParadiseAPI.Model package Microsoft.EntityFrameworkCore.SqlServer</pre>
* <pre>dotnet add NZParadiseAPI.Model package Microsoft.EntityFrameworkCore.Design</pre>
* <pre>dotnet add NZParadiseAPI package Microsoft.EntityFrameworkCore.Design</pre>


## Enable Migration

### Sync DB changes using Entity Framework

1. <pre>dotnet ef migrations add ChangeSummary --project NZParadiseAPI.Model --startup-project NZParadiseAPI</pre>
2. <pre>dotnet ef database update --project NZParadiseAPI.Model --startup-project NZParadiseAPI
</pre>

### Generate sync script for migration 
<pre>
dotnet ef migrations script -o prod_schema_20250513.sql  --project NZParadiseAPI.Model --startup-project NZParadiseAPI
</pre>

## Troubleshooting

If entity framework methods are not recognized -

<pre>dotnet tool install --global dotnet-ef</pre>

If dotnet tool is already installed

<pre>dotnet tool update --global dotnet-ef</pre>

