# FutureVest

Aplicación web desarrollada en **ASP.NET Core MVC** con **Entity Framework Core (Code First)** que permite a un analista registrar manualmente valores de indicadores macroeconómicos de distintos países y, mediante un modelo de scoring ponderado y normalización min-max, determinar la factibilidad de invertir en cada uno de ellos.

## Objetivo general

Calcular un **ranking de países** a partir de:

- Indicadores macroeconómicos registrados por país y año (PIB, inflación, riesgo país, etc.)
- Un conjunto de **macroindicadores** configurables, cada uno con un peso relativo y una orientación (si un valor más alto es mejor o peor)
- Una **normalización min-max** de los valores entre los países elegibles
- Un **scoring ponderado** por país
- Una **tasa de retorno estimada**, calculada linealmente a partir del scoring

El sistema permite además **simular** distintos escenarios de ponderación sin alterar la configuración real del sistema.

## Arquitectura

El proyecto está organizado en tres capas, siguiendo el patrón de separación de responsabilidades:

| Capa | Proyecto | Responsabilidad |
|---|---|---|
| Presentación | `FutureVest.Web` | Controladores MVC, vistas Razor, ViewModels |
| Lógica de negocio | `FutureVest.Application` | Servicios de aplicación, DTOs, validaciones y cálculos |
| Persistencia | `FutureVest.Persistence` | Entidades, configuraciones de EF Core, repositorios, migraciones |

**Referencias entre proyectos:** `Web` → `Application` → `Persistence` (`Persistence` no depende de ninguna otra capa).

### Patrones aplicados

- **Repository pattern** (uno por entidad, sin interfaces) para el acceso a datos.
- **DTOs** para transportar información entre `Persistence` y `Application`, y entre `Application` y `Web`.
- **ViewModels** separados por propósito (listado, creación/edición, eliminación), con validaciones mediante `DataAnnotations`.
- **`ServiceResponse` / `ServiceResponse<T>`**, un envoltorio de resultado común que permite a los servicios comunicar éxito/fallo junto con un mensaje y, cuando aplica, un `ErrorType` para que la interfaz decida qué enlace u orientación mostrar.
- **Single Responsibility** en el motor de cálculo: el servicio de ranking actúa como orquestador corto, delegando normalización, elegibilidad y cálculo de scoring a métodos privados independientes.

## Módulos funcionales

- **Países**: mantenimiento con nombre y código ISO (único).
- **Macroindicadores**: mantenimiento con nombre, peso y orientación (`IsHigherBetter`); la suma de pesos de todos los macroindicadores nunca puede superar 1.
- **Indicadores por país**: valores anuales de cada macroindicador por país, con filtro combinable por país y/o año; un país no puede tener el mismo macroindicador repetido en el mismo año.
- **Tasa de retorno**: configuración única (mínima y máxima) usada en el cálculo de la tasa estimada; si no está configurada, se usan los valores por defecto (2 y 15).
- **Ranking (Home)**: calcula el ranking de países elegibles para un año seleccionado, usando los pesos reales configurados.
- **Simulador de ranking**: permite configurar un subconjunto de macroindicadores con pesos alternativos (sin modificar la configuración real) y correr el mismo motor de cálculo sobre esos pesos.

## Modelo de cálculo

1. **Validación de pesos**: la suma de los pesos de los macroindicadores considerados debe ser igual a 1 (con tolerancia de redondeo).
2. **Elegibilidad de países**: un país es elegible si tiene registrado un valor para cada macroindicador con peso mayor a 0, en el año seleccionado.
3. **Normalización min-max**, por macroindicador, entre los países elegibles:
   - Si `IsHigherBetter = true`: `(valor − mín) / (máx − mín)`
   - Si `IsHigherBetter = false`: `(máx − valor) / (máx − mín)`
   - Si `mín = máx`: el resultado es `0.5` por convención.
4. **Scoring por país**: suma de `normalizado × peso` para cada macroindicador.
5. **Tasa de retorno estimada**: `r_mín + (r_máx − r_mín) × scoring`.
6. El ranking se ordena de mayor a menor scoring; requiere al menos 2 países elegibles.

## Requisitos técnicos

- ASP.NET Core MVC (.NET 9)
- Entity Framework Core (Code First, migraciones)
- SQL Server

Al iniciar con la base de datos vacía, el sistema guía al usuario a registrar primero países y macroindicadores antes de poder generar un ranking.
