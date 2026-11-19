from pathlib import Path
from uuid import uuid4

from fastapi import FastAPI, File, HTTPException, UploadFile
from fastapi.staticfiles import StaticFiles

UPLOADS_DIR = Path(__file__).parent / "uploads"
UPLOADS_DIR.mkdir(exist_ok=True)

app = FastAPI(title="Media", description="Bucket de imagenes de propiedades")
app.mount("/images", StaticFiles(directory=UPLOADS_DIR), name="images")


@app.get("/api/images")
def list_images():
    return [f"/images/{p.name}" for p in sorted(UPLOADS_DIR.iterdir()) if p.is_file()]


@app.post("/api/images", status_code=201)
async def upload_image(file: UploadFile = File(...)):
    name = f"{uuid4().hex}{Path(file.filename or '').suffix.lower()}"
    (UPLOADS_DIR / name).write_bytes(await file.read())
    return {"url": f"/images/{name}"}


@app.delete("/api/images/{name}")
def delete_image(name: str):
    path = UPLOADS_DIR / name
    if path.name != name or not path.is_file():
        raise HTTPException(status_code=404, detail="Image not found")
    path.unlink()
    return {"deleted": f"/images/{name}"}
