using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public class TipoNovedadDAL
    {
        public List<TipoNovedadItem> ListarTiposParaDropdown()
        {
            return new List<TipoNovedadItem>
            {
                new TipoNovedadItem { Valor = "Calidad", Texto = "Calidad" },
                new TipoNovedadItem { Valor = "Mecánica", Texto = "Mecánica" },
                new TipoNovedadItem { Valor = "Eléctrica", Texto = "Eléctrica" },
                new TipoNovedadItem { Valor = "Material", Texto = "Material" },
                new TipoNovedadItem { Valor = "Seguridad", Texto = "Seguridad" },
                new TipoNovedadItem { Valor = "Otro", Texto = "Otro" }
            };
        }
        public class TipoNovedadItem
        {
            public string Valor { get; set; }
            public string Texto { get; set; }
        }
    }
}