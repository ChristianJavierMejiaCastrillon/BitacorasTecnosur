DECLARE @IdUsuario INT = 9;
DECLARE @IdMaquina INT = 6;

SELECT COUNT(1) AS PuedeGestionar
FROM dbo.UsuarioMaquina um
INNER JOIN dbo.TipoAsignacion ta ON ta.IdTipoAsignacion = um.IdTipoAsignacion
WHERE um.IdUsuario = @IdUsuario
  AND um.IdMaquina = @IdMaquina
  AND um.Activo = 1
  AND ta.Codigo IN (
      'COORDINADOR_MAQUINA',
      'TEC_MECANICO_PADRINO',
      'TEC_ELECTRICO_PADRINO'
  );