Availability engine - diseño y uso

Resumen
-------
Este módulo implementa un motor de reglas para validar la disponibilidad de actividades en el museo.

Conceptos clave
---------------
- AvailabilityEngine: orquestador que ejecuta reglas aplicables a una candidata.
- IRuleProvider: decide si aplica a una candidata y crea las reglas necesarias (puede inyectar repos/servicios).
- IAvailabilityRule: unidad de validación que recibe un `AvailabilityContext` y la `ActividadMuseo` candidata y devuelve `AvailabilityResult`.
- AvailabilityContext: datos preparados por la capa de aplicación (rango fecha, `ExistingActivities` solapadas, metadata opcional).

Flujo de ejecución
------------------
1. El handler/servicio de aplicación crea la instancia candidata (`ActividadMuseo`, p. ej. `VisitaGrupalGuiada`) y calcula el rango `start/end` desde sus `TimeSlots`.
2. La capa de aplicación preconsulta repositorios para traer solo las `ExistingActivities` que se solapan en tiempo/sala y arma un `AvailabilityContext`.
3. Llama `await availabilityEngine.CheckAsync(ctx, candidate)`.
4. `AvailabilityEngine` pregunta a la `IRuleFactory` (implementada por `CompositeRuleFactory`) las reglas aplicables: la fábrica pregunta a todos los `IRuleProvider` registrados; cada provider ejecuta `CanHandle(candidate)` y, si devuelve true, `CreateRules(candidate)`.
5. El motor ejecuta las reglas devueltas en orden; corta al primer `Fail` y devuelve el resultado al handler.

Registro en DI
--------------
La extensión `AddAvailabilityEngine()` ya fue creada en `Application/Availability/ServiceCollectionExtensions.cs`.
Se registró automáticamente en `Application/Registrations/ApplicationServicesRegistration.cs` mediante:

```csharp
services.AddAvailabilityEngine();
```

Notas de implementación
-----------------------
- Las reglas deben inyectar solo los repositorios/servicios que necesitan y realizar sus propias consultas cuando corresponda.
- La capa de aplicación debe prefetchear datos pesados que sean requeridos por múltiples reglas y colocarlos en `AvailabilityContext.Metadata` para evitar consultas duplicadas.
- Si necesitás controlar orden/prioridad, modificar `IRuleProvider` para exponer `Priority` y que `CompositeRuleFactory` ordene las reglas antes de devolverlas.

Ejemplo de handler (simplificado)
---------------------------------
```csharp
public async Task Handle(CreateVisitaGuiadaCommand cmd)
{
    var candidate = new VisitaGrupalGuiada(/* datos */);
    var start = candidate.TimeSlots.Min(ts => ts.Inicio);
    var end = candidate.TimeSlots.Max(ts => ts.Fin);

    // Prefetch actividades solapadas
    var existentes = await _repoActividadMuseo.FindAllAsync(start, end);

    var ctx = new AvailabilityContext
    {
        Start = start,
        End = end,
        ExistingActivities = existentes
    };

    var result = await _availabilityEngine.CheckAsync(ctx, candidate);

    if (!result.IsOk)
        throw new BusinessException(result.Message);

    // persistir actividad...
}
```

Dónde extender
---------------
- Agregar nuevas reglas en `Application/Availability/Rules` y proveedores en `Application/Availability/Providers`.
- Registrar proveedores adicionales en el namespace `Application.Availability.Providers` (la extensión hace un `Scan` por ese namespace).

Contacto
-------
Archivo generado automáticamente por el asistente de desarrollo; ajustar según necesidades del equipo.
