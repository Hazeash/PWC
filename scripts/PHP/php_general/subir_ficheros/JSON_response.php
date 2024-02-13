<?php

/**
 * Clase JSON_response
 * 
 * Proporciona métodos para manejar respuestas JSON relacionadas con imágenes.
 * 
 * @author Hazeash
 */
class JSON_response {

    /**
     * Método enviarImagen: Maneja la carga de imágenes y responde con un mensaje JSON.
     *
     * @param string $rutaAlmacen Ruta del directorio donde se almacenarán las imágenes.
     * @param array $allowedExtensions Extensiones permitidas para las imágenes.
     */
    public function enviarImagen($rutaAlmacen, $allowedExtensions) {
        try {
            if (isset($_FILES['image']) && is_uploaded_file($_FILES['image']['tmp_name'])) {
                $file = $_FILES['image'];
                $fileName = $file['name'];
                $filePath = $rutaAlmacen . $fileName;
                $imageFileType = strtolower(pathinfo($filePath, PATHINFO_EXTENSION));
                if (!in_array($imageFileType, $allowedExtensions)) {
                    $response = [
                        'status' => 'error',
                        'message' => 'El archivo no es una imagen válida. Las extensiones permitidas son: ' . implode(', ', $allowedExtensions),
                    ];
                } else {
                    if (move_uploaded_file($file['tmp_name'], $filePath)) {
                        $response = [
                            'status' => 'success',
                            'message' => 'Imagen subida y guardada correctamente.',
                        ];
                    } else {
                        $errorMessage = error_get_last()['message'] ?? 'No se proporcionó un mensaje de error detallado.';
                        error_log("Error al mover la imagen. Detalles: " . print_r($file, true));

                        $response = [
                            'status' => 'error',
                            'message' => 'Error al guardar la imagen.',
                            'server_message' => $errorMessage,
                        ];
                    }
                }
            } else {
                $response = [
                    'status' => 'error',
                    'message' => 'No se recibió ninguna imagen.',
                ];
            }
            header('Content-Type: application/json');
            echo json_encode($response);
        } catch (Exception $e) {
            $response = [
                'status' => 'error',
                'message' => 'Error inesperado: ' . $e->getMessage(),
            ];
            header('Content-Type: application/json');
            echo json_encode($response);
        }
    }
}

?>