<?php

/**
 * Clase Update
 * 
 * Esta clase es la encargada de actualizar los permisos y/o propietario de directorios o archivos.
 * 
 * @author Hazeash
 */
class Update {

    /**
     * Actualiza los permisos y/o propietario de un directorio o archivo.
     *
     * @param string $rutaAlmacen Ruta al directorio o archivo a actualizar.
     * @param bool $recursivo Indica si la actualización debe ser recursiva para directorios.
     * @param string $permisos Permisos a aplicar en formato numérico (ej. "755").
     * @param string $propietario Propietario y grupo separados por ":" (ej. "usuario:grupo").
     */
    public function permisos($rutaAlmacen, $recursivo, $permisos, $propietario) {
        try {
            if ($recursivo) {
                $comandoChmod = "chmod -R " . $permisos . " " . escapeshellarg($rutaAlmacen);
            } else {
                $comandoChmod = "chmod " . $permisos . " " . escapeshellarg($rutaAlmacen);
            }

            if ($propietario && $recursivo) {
                $comandoChown = "chown -R " . $propietario . ":" . "$propietario" . " " . escapeshellarg($rutaAlmacen);
            } elseif ($propietario) {
                $comandoChown = "chown " . $propietario . ":" . "$propietario" . " " . escapeshellarg($rutaAlmacen);
            }
            $resultadoChmod = shell_exec($comandoChmod);
            $resultadoChown = shell_exec($comandoChown);
        } catch (Exception $e) {
            echo "Se ha producido una excepción: " . $e->getMessage() . PHP_EOL;
        }
    }
}

?>