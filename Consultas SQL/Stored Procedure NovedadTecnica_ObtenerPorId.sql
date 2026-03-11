CREATE OR ALTER PROCEDURE dbo.sp_NovedadTecnica_ObtenerPorId
    @IdNovedadTecnica INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        nt.IdNovedadTecnica,
        nt.IdBitacora,
        nt.IdTipoNovedadTecnica,
        nt.Descripcion,
        nt.Diagnostico,
        nt.SolucionAplicada,
        nt.ReportadoPor,
        nt.Validado,
        nt.TiempoPerdidoMinutos,
        nt.IdMaquinaModulo,
        nt.IdMaquinaModuloElemento,
        nt.IdUsuarioReporta,
        nt.IdUsuarioResponsable,
        nt.FechaCreacion,

        b.Fecha,
        b.Turno,
        b.IdMaquina,
        b.IdUsuario AS IdUsuarioBitacora,

        (u.Nombres + ' ' + u.Apellidos) AS NombreUsuarioReporta,
        tr.Nombre AS TipoNovedadTecnicaNombre
    FROM dbo.NovedadTecnica nt
    INNER JOIN dbo.Bitacora b
        ON b.IdBitacora = nt.IdBitacora
    INNER JOIN dbo.Usuario u
        ON u.IdUsuario = nt.IdUsuarioReporta
    INNER JOIN dbo.TipoNovedadTecnica tr
        ON tr.IdTipoNovedadTecnica = nt.IdTipoNovedadTecnica
    WHERE nt.IdNovedadTecnica = @IdNovedadTecnica;
END