using ApiCrackChecker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrackChecker.Helpers
{
    public class StorageHelper
    {
        // Para acceder a todas las características de los blobs necesitamos un cliente
        CloudBlobClient client;
        public StorageHelper()
        {
            String keys = "DefaultEndpointsProtocol=https;AccountName=storageamg;AccountKey=/cmR59cnBMSH2Hqb9xnSWG3mpAnlXMoj1K/CD3w7EtvO81WA/BsIjnOlnE5oYSXsErCM4xTJBSjb4/s9udyjdQ==;EndpointSuffix=core.windows.net";
            CloudStorageAccount account = CloudStorageAccount.Parse(keys);
            this.client = account.CreateCloudBlobClient();
            this.CrearContenedor().Wait();
        }

        // Método para crear contenedores
        public async Task CrearContenedor()
        {
            CloudBlobContainer container = this.client.GetContainerReference("fotoperfiles");
            // Creamos el contenedor si no existe
            await container.CreateIfNotExistsAsync();
            // Establecemos los permisos del contenedor, vamos a ponerlos como publicos para acceder desde la url (cualquiera)
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }

        // Método para subir los blobs a azure (emulador)
        // Contenedor, imagen, ruta
        public async Task<String> SubirImagen(String archivo, String nombre)
        {
            byte[] data = Convert.FromBase64String(archivo);
            var stream = new MemoryStream(data);
            IFormFile imagen = new FormFile(stream, 0, data.Length, "name", nombre);
            // Recuperamos nuestro contenedor
            CloudBlobContainer container = this.client.GetContainerReference("fotoperfiles");
            // A partir de este contenedor recuperamos el blob, que es una clase de tipo CloudBlockBlob
            CloudBlockBlob blob = container.GetBlockBlobReference(imagen.FileName);
            // Para subir un blob, necesitamos un stream
            using (var str = imagen.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(stream);
            }
            String uri = blob.SnapshotQualifiedUri.AbsoluteUri.Replace("\"", "");
            return uri;
        }
    }
}
