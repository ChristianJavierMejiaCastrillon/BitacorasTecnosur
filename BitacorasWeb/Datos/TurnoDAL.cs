using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public class TurnoDAL
    {
        public List<TurnoItem> ListarTurnosParaDropDown()
        {
            return new List<TurnoItem>
            {
                new TurnoItem { Valor = "Turno 1", Texto = "Turno 1" },
                new TurnoItem { Valor = "Turno 2", Texto = "Turno 2" },
                new TurnoItem { Valor = "Turno 3", Texto = "Turno 3" }
            };
        }
        public class TurnoItem
        {
            public string Valor { get; set; }
            public string Texto { get; set; }
        }
    }
}