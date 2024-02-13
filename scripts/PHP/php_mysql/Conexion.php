<?php

/**
 * Clase Conexion
 *
 * Esta clase encapsula la funcionalidad de conexión a una base de datos MySQL utilizando MySQLi.
 * También proporciona métodos para obtener la conexión y cerrar la conexión.
 *
 * @author Hazeash
 */
class Conexion {
    private $conexion;

    /**
     * Constructor de la clase.
     *
     * @param string $hostname - El nombre del host de la base de datos.
     * @param string $username - El nombre de usuario para la conexión a la base de datos.
     * @param string $password - La contraseña para la conexión a la base de datos.
     * @param string $database - El nombre de la base de datos a la que se desea conectar.
     * @param string $codificacion - La codificación de caracteres para la conexión.
     */
    public function __construct($hostname, $username, $password, $database, $codificacion) {
        $this->conexion = new mysqli($hostname, $username, $password, $database);
        if ($this->conexion->connect_error) {
            die("Error de conexión: " . $this->conexion->connect_error);
        }
        $this->conexion->set_charset($codificacion);
    }

    /**
     * Obtén la instancia de la conexión MySQLi.
     *
     * @return mysqli La instancia de la conexión MySQLi.
     */
    public function getConexion() {
        return $this->conexion;
    }

    /**
     * Cerrar la conexión si está abierta.
     */
    public function cerrarConexion() {
        if ($this->conexion) {
            $this->conexion->close();
        }
    }
}

?>