# LASERInventorySystem

## Run migration (First already done):
```
dotnet ef migrations add InitialCreate
```
## Removing migration:
```
ef migrations remove
```
## Run on production (Creates .db):
```
dotnet ef database update
```