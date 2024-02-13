<?php

/**
 * Clase InsertarDatos
 *
 * Esta clase se encarga de almacenar datos en la base de datos.
 *
 * @author Hazeash
 */
class InsertarDatos {
    private $conexion;
    private $tabla;

    /**
     * Constructor de la clase. Inicializa la conexión a la base de datos y establece la tabla de destino.
     *
     * @param object $conexion - Objeto de conexión a la base de datos.
     * @param string $tabla - Nombre de la tabla donde se guardarán los datos.
     */
    public function __construct($conexion, $tabla) {
        $this->conexion = $conexion;
        $this->tabla = $tabla;
    }

    /**
     * Guarda un nuevo campo en la base de datos.
     *
     * @param array $datos - Datos del campo a guardar.
     */
    public function guardar($datos) {
        if ($_SERVER["REQUEST_METHOD"] == "POST" && !empty($_POST)) {
            $mysqli = $this->conexion->getConexion();
            $escapedData = array_map(function ($value) use ($mysqli) {
                return mysqli_real_escape_string($mysqli, $value);
            }, $datos);
            $columnas = implode(', ', array_keys($datos));
            $placeholders = implode(', ', array_fill(0, count($datos), '?'));
            $query = "INSERT INTO " . $this->tabla . "($columnas) VALUES ($placeholders)";
            $stmt = $mysqli->prepare($query);
            $tipos = str_repeat('s', count($datos));
            $params = [$tipos];
            foreach ($escapedData as &$value) {
                $params[] = &$value;
            }
            call_user_func_array([$stmt, 'bind_param'], $params);
            $stmt->execute();
            if ($stmt->affected_rows > 0) {
                echo "Datos guardados";
            } else {
                echo "No se han podido guardar los datos";
            }
            $stmt->close();
        } else {
            echo "Error: La solicitud no es de tipo POST.";
        }
    }
}

?>