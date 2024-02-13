<p align="center">
<sup>
<b>PWC es una herramienta que facilita la conexión a una base de datos a través de un servidor web mediante el método POST, devolviendo la respuesta en formato JSON. Además, incluye versiones portátiles de servidores web y sistemas de gestión de bases de datos (SGBD)</b>
</sup>
</p>

# ¿Quieres descargar PWC?
Abajo encontrarás enlaces que te permitirán descargar y utilizar PWC.

* PWC for Windows x64 - [PWC.7z](https://drive.google.com/file/d/10-C-0ahV7lhdxAjyFM4G0ZacKQeQtOPi/view?usp=sharing)

# Como implementarlo en forma de Scripts
Ahora se mostrará como utilizar PWC como Scripts y no como programa.

* PWC for all platforms - [PHP.7z](https://drive.google.com/file/d/1bausl7pf6GpDL-Pnk0OIU-zEVSavEck5/view?usp=sharing)

## Clase: SolicitarDatos.php

### Ejemplo de uso (parámetro único):

```php
<?php

require "rutaAlFichero/Conexion.php";
require "rutaAlFichero/SolicitarDatos.php";

// Configuración de la base de datos
$hostname = "host de la base de datos";
$username = "usuario de la base de datos";
$password = "contraseña del usuario de la base de datos";
$database = "nombre de la base de datos";

// Crea una instancia de la clase Conexion
$conexion = new Conexion($hostname, $username, $password, $database, "utf32_bin");

// Intenta establecer la conexión
$conexion->getConexion();

// Asigna los campos que quieres que se envíen tras la solicitud. En caso de
// quererlos todos se pondrá un *
$campos = "campo1, campo2, campo3";

/* Datos de la solicitud. Si intentas obtener toda la información de una tabla,
   entonces el dato deberá tener la clave 'categoria' y deberá
   recibir el valor 'general' obligatoriamente */
$datosSolicitud = [
    "categoria" => $_POST["categoria"],
];

// Crea una instancia de la clase SolicitarDatos
$solicitud = new SolicitarDatos($conexion, "nombre de tu tabla");

// Mandamos la solicitud
$solicitud->solicitud($campos, $datosSolicitud);

// Cierra la conexión
$conexion->cerrarConexion();

?>
```

## Clase: SolicitarDatos.php

### Ejemplo de uso (parámetros múltiples):

```php
<?php

require "rutaAlFichero/Conexion.php";
require "rutaAlFichero/SolicitarDatos.php";

// Configuración de la base de datos
$hostname = "host de la base de datos";
$username = "usuario de la base de datos";
$password = "contraseña del usuario de la base de datos";
$database = "nombre de la base de datos";

// Crea una instancia de la clase Conexion
$conexion = new Conexion($hostname, $username, $password, $database, "utf32_bin");

// Intenta establecer la conexión
$conexion->getConexion();

// Asigna los campos que quieres que se envíen tras la solicitud. En caso de
// quererlos todos se pondrá un *
$campos = "campo1, campo2, campo3";

/* Datos del post que se utilizarán para construir la sentencia (puedes añadir o quitar campos según sea necesario).
   Las claves son el nombre de la columna mientras que los valores son el valor a buscar en dicha columna */
$datosSolicitud = [
    "campo1" => $_POST["campo1"],
    "campo2" => $_POST["campo2"]
];

// Crea una instancia de la clase SolicitarDatos
$solicitud = new SolicitarDatos($conexion, "nombre de tu tabla");

// Mandamos la solicitud
$solicitud->solicitud($campos, $datosSolicitud);

// Cierra la conexión
$conexion->cerrarConexion();

?>
```

## Clase: InsertarDatos.php

### Ejemplo de uso:

```php
<?php

require "rutaAlFichero/Conexion.php";
require "rutaAlFichero/InsertarDatos.php";

// Configuración de la base de datos
$hostname = "host de la base de datos";
$username = "usuario de la base de datos";
$password = "contraseña del usuario de la base de datos";
$database = "nombre de la base de datos";

// Crea una instancia de la clase Conexion
$conexion = new Conexion($hostname, $username, $password, $database, "utf32_bin");

// Intenta establecer la conexión
$conexion->getConexion();

/* Datos del post que se utilizarán para construir la sentencia (puedes añadir o quitar campos según sea necesario).
   Las claves son el nombre de la columna mientras que los valores son el valor a insertar en dicha columna */
$datosPost = [
    "campo1" => $_POST["campo1"],
    "campo2" => $_POST["campo2"],
    "campo3" => $_POST["campo3"],
    "campo4" => $_POST["campo4"],
    "campo5" => $_POST["campo5"],
    "campo6" => $_POST["campo6"]
];

// Crea una instancia de la clase InsertarDatos
$guardar = new InsertarDatos($conexion, "nombre de tu tabla");

// Guarda los datos en la base de datos
$guardar->guardar($datosPost);

// Cierra la conexión
$conexion->cerrarConexion();

?>
```

## Clase: JSON_response.php y Update.php

### Ejemplo de uso:

```php
<?php

require "rutaAlFichero/JSON_response.php";
require "rutaAlFichero/Update.php";

// Configuración para subir archivos (Los datos mostrados son ejemplos)
$rutaAlmacen = "ruta al directorio";
$recursivo = true;
$permisos = 777;
$propietario = "apache";
$allowedExtensions = array('jpg', 'jpeg', 'png', 'gif');

// Crea una instancia de la clase JSON_response
$subir = new JSON_response();

// Envia la imagen al servidor
$subir->enviarImagen($rutaAlmacen, $allowedExtensions);

// Crea una instancia de la clase Update
$update = new Update();

// Actualiza los permisos de la carpeta de almacenamiento
$update->permisos($rutaAlmacen, $recursivo, $permisos, $propietario);

?>
```
