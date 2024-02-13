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
