<?php

/**
 * Clase SolicitarDatos
 *
 * Esta clase maneja las solicitudes de datos.
 * Se conecta a una base de datos y ejecuta consultas SQL según los datos proporcionados.
 *
 * @author Hazeash
 */
class SolicitarDatos {
    private $conexion;
    private $tabla;

    /**
     * Constructor de la clase.
     *
     * @param mysqli $conexion - La instancia de la conexión a la base de datos.
     * @param string $tabla - El nombre de la tabla en la base de datos.
     */
    public function __construct($conexion, $tabla) {
        $this->conexion = $conexion;
        $this->tabla = $tabla;
    }

    /**
     * Maneja la solicitud de datos, ejecutando la consulta correspondiente según el método de solicitud (POST) y los datos proporcionados.
     *
     * @param array $datos Los datos provenientes del formulario HTML.
     */
    public function solicitud($datos) {
        if ($_SERVER["REQUEST_METHOD"] == "POST" && !empty($_POST)) {
            $mysqli = $this->conexion->getConexion();
            $escapedData = array_map(function ($value) use ($mysqli) {
                return mysqli_real_escape_string($mysqli, $value);
            }, $datos);
            if (count($datos) == 1 && isset($datos['categoria']) && $datos['categoria'] == 'general') {
                $this->ejecutarConsultaGeneral();
            } else {
                $this->ejecutarConsultaVariada($datos);
            }
        } else {
            echo "Error: La solicitud no es de tipo POST.";
        }
    }

    /**
     * Ejecuta una consulta SQL general para obtener todos los registros de la tabla.
     */
    private function ejecutarConsultaGeneral() {
        $stmt = $this->conexion->getConexion()->prepare("SELECT * FROM " . $this->tabla);
        $this->ejecutarConsulta($stmt);
    }

    /**
     * Ejecuta una consulta SQL variada según los datos proporcionados.
     *
     * @param array $datos Los datos recibidos mediante (POST).
     */
    private function ejecutarConsultaVariada($datos) {
        $whereClause = "";
        $bindParams = "";
        $bindValues = [];
        $index = 1;
        foreach ($datos as $clave => $valor) {
            $whereClause .= "$clave = ?";
            $bindParams .= 's';
            $bindValues[] = $valor;
            if ($index < count($datos)) {
                $whereClause .= " AND ";
            }
            $index++;
        }
        $consulta = "SELECT * FROM " . $this->tabla;
        if (!empty($whereClause)) {
            $consulta .= " WHERE " . $whereClause;
        }
        $stmt = $this->conexion->getConexion()->prepare($consulta);
        if ($stmt) {
            if (!empty($bindParams)) {
                $bindParamsArray = str_split($bindParams);
                array_unshift($bindValues, implode('', $bindParamsArray));
                call_user_func_array(array($stmt, 'bind_param'), $this->refValues($bindValues));
            }
            $this->ejecutarConsulta($stmt);
        } else {
            echo json_encode(["error" => "Error en la preparación de la consulta: " . $this->conexion->getConexion()->error]);
        }
    }

   /**
     * Función auxiliar para obtener referencias a los valores.
     *
     * @param array $arr El array de valores.
     * @return array El array de referencias a los valores.
     */
    private function refValues($arr) {
        $refs = array();
        foreach ($arr as $key => $value) {
            $refs[$key] = &$arr[$key];
        }
        return $refs;
    }

    /**
     * Ejecuta la consulta preparada y devuelve los resultados en formato JSON.
     *
     * @param mysqli_stmt $stmt La declaración preparada de la consulta.
     */
    private function ejecutarConsulta($stmt) {
        if ($stmt) {
            $stmt->execute();
            $resultado = $stmt->get_result();
            $datos = [];
            while ($fila = $resultado->fetch_assoc()) {
                $datos[] = $fila;
            }
            echo json_encode($datos);
            $stmt->close();
        } else {
            echo json_encode(["error" => "Error en la preparación de la consulta: " . $this->conexion->getConexion()->error]);
        }
    }
}

?>