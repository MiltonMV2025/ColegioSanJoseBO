# Sistema de Gestión Académica - Colegio San José

Sistema web de gestión académica desarrollado para administrar estudiantes, materias, calificaciones y expedientes académicos.

## 📋 Descripción

Aplicación web que permite gestionar de manera integral la información académica de un colegio, incluyendo el registro de estudiantes, materias, asignación de calificaciones y generación de expedientes académicos con observaciones.

## 🚀 Tecnologías Utilizadas

### Backend
- **.NET 8** - Framework principal
- **ASP.NET Core MVC** - Patrón arquitectónico
- **Entity Framework Core** - ORM para acceso a datos
- **SQL Server** - Base de datos relacional

### Frontend
- **Bootstrap 5** - Framework CSS
- **Chart.js 4.4** - Gráficas estadísticas
- **SweetAlert2** - Notificaciones y alertas
- **Bootstrap Icons** - Iconografía

### Seguridad
- **Cookie Authentication** - Sistema de autenticación
- **Password Hashing (SHA256)** - Seguridad de contraseñas
- **Authorization Filters** - Control de acceso

## 📂 Estructura del Proyecto

```
ColegioSanJoseBO/
├── Controllers/
│   ├── AccountController.cs       # Autenticación
│   ├── HomeController.cs          # Dashboard
│   ├── StudentsController.cs      # CRUD Estudiantes
│   ├── SubjectsController.cs      # CRUD Materias
│   └── ExpedientesController.cs   # Gestión Expedientes
├── Models/
│   ├── Student.cs                 # Entidad Estudiante
│   ├── Teacher.cs                 # Entidad Maestro
│   ├── Subject.cs                 # Entidad Materia
│   ├── StudentSubject.cs          # Entidad Expediente
│   └── Login.cs                   # Entidad Login
├── ViewModels/
│   ├── DashboardViewModel.cs      # ViewModel Dashboard
│   └── StudentViewModels.cs       # ViewModels Estudiantes
├── Views/
│   ├── Account/                   # Vistas Login
│   ├── Home/                      # Dashboard
│   ├── Students/                  # CRUD Estudiantes
│   ├── Subjects/                  # CRUD Materias
│   └── Expedientes/               # Vista Expedientes
├── Data/
│   ├── AppDbContext.cs            # Contexto EF Core
│   └── CreateDB.SQL               # Creación Base de Datos
└── wwwroot/
    ├── css/                       # Estilos CSS
    └── js/                        # JavaScript
```

## 🗄️ Modelo de Base de Datos

### Tablas Principales

**Teachers** (Maestros)
- Id, Name, Surname, BirthDate

**Students** (Estudiantes)
- Id, Name, Surname, BirthDate, Degree

**Subjects** (Materias)
- Id, SubjectCode (único), SubjectName, TeacherId

**Login** (Autenticación)
- Id, TeacherId, Username, Password (hash), Status

**StudentSubjects** (Expedientes)
- Id, StudentId, SubjectId, FinalGrade (0-10), Observations

## ⚙️ Configuración e Instalación

### Prerrequisitos
- .NET 8 SDK
- SQL Server 2019 o superior
- Visual Studio 2022 o VS Code

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone [URL_DEL_REPOSITORIO]
cd ColegioSanJoseBO
```

2. **Configurar la cadena de conexión**

Editar `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=ColegioSanJoseDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. **Crear la base de datos**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. **Ejecutar la aplicación**

```bash
dotnet run
```

O presionar **F5** en Visual Studio.

## 🧪 Probar la Aplicación

### 1. Acceso al Sistema

La aplicación se abrirá en: `https://localhost:5001` o `http://localhost:5000`

### 2. Credenciales de Prueba

```
Usuario: admin
Contraseña: admin123
```

## 📊 Funcionalidades Principales

### CRUD Completo

#### Estudiantes
- ✅ Crear, editar, eliminar estudiantes
- ✅ Asignar materias con calificaciones (0-10)
- ✅ Editar calificaciones y observaciones
- ✅ Remover materias del estudiante
- ✅ Ver promedio general del estudiante

#### Materias
- ✅ Crear, editar, eliminar materias
- ✅ Asignar maestro responsable
- ✅ Ver estudiantes inscritos
- ✅ Código único por materia

#### Expedientes
- ✅ Visualizar todos los registros académicos
- ✅ Editar notas (0-10) y observaciones
- ✅ Eliminar expedientes
- ✅ Código de colores por rendimiento:
  - 🟢 Verde: ≥ 9.0 (Excelente)
  - 🔵 Azul: ≥ 7.0 (Bueno)
  - 🟡 Amarillo: ≥ 6.0 (Regular)
  - 🔴 Rojo: < 6.0 (Deficiente)

### Dashboard
- 📊 Total de estudiantes, maestros y materias
- 📈 Promedio global de calificaciones
- 📉 Gráfica de barras: Top 5 estudiantes
- 🍩 Gráfica de dona: Promedios por materia

## 🔒 Seguridad

- Todas las contraseñas se almacenan con hash SHA256
- Cookie authentication con expiración de 8 horas
- Rutas protegidas con `[Authorize]`
- Redirección automática al login si no está autenticado
- Validación de datos en cliente y servidor


## 🚦 Estado del Proyecto

- ✅ Sistema de autenticación
- ✅ CRUD de Estudiantes
- ✅ CRUD de Materias
- ✅ Vista de Expedientes
- ✅ Dashboard con estadísticas
- ✅ Asignación de materias a estudiantes
- ✅ Gestión de calificaciones y observaciones
- ✅ Gráficas interactivas
- ✅ Notificaciones con SweetAlert
