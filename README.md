# 🗳️ Voto Pura Vida - API Serverless

Este proyecto implementa un prototipo de sistema de voto electrónico y crowdfunding con una arquitectura serverless local usando Azure Functions (C#) y SQL Server. Cumple con los requisitos del Micitt, incluyendo transacciones seguras, validaciones críticas, integridad de datos y soporte para inteligencia artificial.

---

## 📦 Tecnologías Utilizadas

- .NET 8 + C#
- Azure Functions (Runtime local)
- SQL Server (procedimientos almacenados + EF Core)
- Entity Framework Core
- Validaciones de seguridad, cifrado y trazabilidad

---

## 📁 Estructura del Proyecto

VotoPuraVida/
│
├── Functions/                # Azure Functions (Stored Proc + ORM)
│   ├── StoredProcedures/
│   └── ORM/
│
├── Data/                     # EF Core DbContext y entidades
├── Shared/                   # DTOs, helpers y conexión SQL
├── local.settings.json       # Configuración local
├── host.json                 # Configuración de Azure Functions
├── Program.cs                # Inicialización del host
├── VotoPuraVida.csproj       # Proyecto C#
└── README.md

---

## 🚀 Instalación y Ejecución Local

### 1. Requisitos Previos

- .NET SDK 8+
- Azure Functions Core Tools
- SQL Server LocalDB o instancia completa

### 2. Clona el Repositorio

git clone https://github.com/tuusuario/VotoPuraVida.git
cd VotoPuraVida

### 3. Configura la Cadena de Conexión

Edita el archivo local.settings.json:

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "SqlConnectionString": "Server=localhost;Database=VotoPuraVida;User Id=tuUsuario;Password=tuPassword;"
  }
}

💡 Asegúrate de tener una base de datos llamada VotoPuraVida creada previamente.

### 4. Aplicar Migraciones (solo para endpoints ORM)

dotnet ef database update

💡 Usa `dotnet tool install --global dotnet-ef` si no tienes el CLI de EF instalado.

### 5. Ejecutar la API

func start

La API estará disponible en http://localhost:7071

---

## 📌 Endpoints Disponibles

### Stored Procedures

| Endpoint                       | Método | Descripción                         |
|-------------------------------|--------|-------------------------------------|
| /api/crearActualizarPropuesta | POST   | Crear o actualizar propuesta       |
| /api/revisarPropuesta         | POST   | Validar y publicar propuesta       |
| /api/invertir                 | POST   | Inversión ciudadana                |
| /api/repartirDividendos       | POST   | Reparto de dividendos              |

### ORM (Entity Framework)

| Endpoint                    | Método | Descripción                         |
|----------------------------|--------|-------------------------------------|
| /api/votar                 | POST   | Emisión de voto cifrado            |
| /api/comentar              | POST   | Comentario validado                |
| /api/listarVotos           | GET    | Últimos 5 votos del usuario        |
| /api/configurarVotacion    | POST   | Configurar parámetros de votación  |

---

## 🧠 IA y Validaciones

- Los endpoints `crearActualizarPropuesta` y `revisarPropuesta` preparan estructuras para validación con IA (análisis de contenido, archivos).
- Posibilidad de integrar LLMs como OpenAI, Azure AI o Hugging Face.

---

## 🛡️ Seguridad y Control Transaccional

- Transacciones completas en SP y ORM (`BEGIN`, `COMMIT`, `ROLLBACK`)
- Validaciones críticas
- Cifrado de votos y archivos sensibles
- MFA, control de acceso y trazabilidad
