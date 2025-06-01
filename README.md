# VotoPuraVida – Boilerplate Azure Functions C#

Boilerplate **serverless** en **C# /.NET 8** para Azure Functions, con conexión a **Azure SQL Database**. Ofrece dos endpoints de ejemplo:

| Método | Ruta        | Descripción                             |
|--------|-------------|-----------------------------------------|
| `GET`  | `/status`   | Verifica salud de la API                |
| `POST` | `/records`  | Inserta un registro en `SampleTable`    |


## Prerrequisitos

| Herramienta                              | Versión mínima |
|------------------------------------------|----------------|
| [.NET SDK](https://dotnet.microsoft.com/) | 8.0            |
| Azure CLI                                | 2.60           |
| [Azure Functions Core Tools v4](https://learn.microsoft.com/azure/azure-functions/functions-run-local) | 4.x |
| ODBC/MS SQL Driver (Linux/macOS)          | 18             |

Instala Core Tools (Linux/macOS vía npm):
```bash
npm i -g azure-functions-core-tools@4 --unsafe-perm true
```

## Ejecución local

1. **Clona** el repositorio y entra:
   ```bash
   git clone <REPO_URL>
   cd AzureFunctionsBoilerplateCSharp
   ```
2. **Restaurar y compilar**:
   ```bash
   dotnet restore
   dotnet build
   ```
3. **Configura** `local.settings.json` con tu `SqlConnectionString`.  

4. **Inicia** la función:
   ```bash
   func start
   ```
5. **Prueba** los endpoints:
   ```bash
      curl http://localhost:7071/api/status
      curl -X POST http://localhost:7071/api/records \
          -H "Content-Type: application/json" \
          -d '{
                "infoId": 1,
                "modeloIA": "gpt-4o",
                "apiKey": "sk-XXXX",
                "token": "someAuthToken",
                "maxTokens": 4096
              }'
   ```

## Despliegue en Azure

1. **Inicia sesión** y selecciona subscription:
   ```bash
   az login
   az account set --subscription "<SUB_ID>"
   ```
2. **Crea recursos** (Grupo, Storage, Function App, Azure SQL):
   ```bash
   az group create -n VotoPuraVidaRG -l eastus
   az storage account create -n votopuravidastorcsharp -g VotoPuraVidaRG -l eastus --sku Standard_LRS
   az functionapp create -g VotoPuraVidaRG -c eastus -n VotoPuraVidaApiCs \
        --storage-account votopuravidastorcsharp --functions-version 4 --runtime dotnet-isolated --os-type Windows
   az sql server create -g VotoPuraVidaRG -l eastus -n votopuravidasqlsrvcs \
        --admin-user sqladmin --admin-password "<StrongPass123>"
   az sql db create -g VotoPuraVidaRG -s votopuravidasqlsrvcs -n VotoPuraVidaDB -e GeneralPurpose -f Gen5
   ```
3. **Configura** la cadena de conexión en la Function App:
   ```bash
   az functionapp config appsettings set -g VotoPuraVidaRG -n VotoPuraVidaApiCs \
        --settings "SqlConnectionString=Server=tcp:votopuravidasqlsrvcs.database.windows.net,1433;Database=VotoPV;Uid=sqladmin;Pwd=<StrongPass123>;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;"
   ```
4. **Publica** el código:
   ```bash
   func azure functionapp publish VotoPuraVidaApiCs --dotnet
   ```
5. **Verifica**:
   ```bash
   curl https://VotoPuraVidaApiCs.azurewebsites.net/api/status
   ```

## Capas compartidas

La carpeta **SharedLayer** actúa como capa reutilizable. Si la lógica crece, se puede convertir en un 
proyecto/lib independiente y añádelo como referencia o NuGet interno.

