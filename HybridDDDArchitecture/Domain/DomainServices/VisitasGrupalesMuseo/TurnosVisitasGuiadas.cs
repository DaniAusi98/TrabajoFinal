namespace Domain.DomainServices.VisitasGrupalesMuseo
{
    public static class TurnosVisitasGuiadas
    {
        public static readonly (TimeOnly inicio, TimeOnly fin)[] HorarioTurnos =
{
            (new TimeOnly(9, 30),  new TimeOnly(10, 30)),
            (new TimeOnly(11, 0),  new TimeOnly(12, 0)),
            (new TimeOnly(14, 30), new TimeOnly(15, 30)),
            (new TimeOnly(16, 0),  new TimeOnly(17, 0))
        };
    }
}