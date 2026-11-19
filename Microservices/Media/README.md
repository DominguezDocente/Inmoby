# Media microservice

Bucket de imagenes de propiedades. Un solo archivo (`main.py`), guarda los archivos en `uploads/` y los sirve como estaticos.

## Ejecutar

```powershell
cd Microservices\Media
python -m venv .venv
.\.venv\Scripts\pip install -r requirements.txt
.\.venv\Scripts\uvicorn main:app --port 5301 --reload
```

## Endpoints

| Metodo | Ruta | Descripcion |
|---|---|---|
| GET | `/api/images` | Lista las URLs de las imagenes |
| POST | `/api/images` | Sube una imagen (`multipart/form-data`, campo `file`) |
| DELETE | `/api/images/{name}` | Elimina una imagen |
| GET | `/images/{name}` | Descarga/visualiza la imagen |

## Ejemplo

```powershell
curl.exe -F "file=@foto.jpg" http://localhost:5301/api/images
# {"url":"/images/1b9c...e4.jpg"}

curl.exe http://localhost:5301/api/images
# ["/images/1b9c...e4.jpg"]
```

## Flujo completo (via gateway)

El gateway ya expone el servicio en `/media` (`Microservices/ApiGateway/ApiGateway.App/appsettings.json`).

```powershell
# 1. Subir la imagen (el prefijo /media lo quita el gateway)
curl.exe -k -F "file=@foto.jpg" https://localhost:7777/media/api/images
# {"url":"/images/1b9c...e4.jpg"}

# 2. Guardar la URL en Properties al crear/actualizar
#    URL publica: https://localhost:7777/media/images/1b9c...e4.jpg

# 3. Abrir la imagen
curl.exe -k https://localhost:7777/media/images/1b9c...e4.jpg
```

Properties solo almacena la URL (`PropertyImages.Url`); los bytes viven en este servicio.
