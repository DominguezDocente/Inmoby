#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PERSISTENCE="$ROOT/Properties.Persistence/Properties.Persistence.csproj"
API="$ROOT/Properties.Api/Properties.Api.csproj"
MIGRATIONS_DIR="Migrations"

usage() {
  cat <<EOF
Uso: ./scripts/ef.sh <comando> [args]

Comandos:
  add <Nombre>              Crea una migración
  update [Nombre]           Aplica migraciones (todas o hasta Nombre)
  list                      Lista migraciones
  remove                    Elimina la última migración (sin aplicar)
  script [archivo.sql]      Genera script SQL (default: migrations.sql)

Ejemplos:
  ./scripts/ef.sh add AddIndexes
  ./scripts/ef.sh update
  ./scripts/ef.sh update InnitialSquema
  ./scripts/ef.sh list
  ./scripts/ef.sh remove
  ./scripts/ef.sh script
EOF
}

require_arg() {
  if [[ -z "${1:-}" ]]; then
    echo "Error: falta el argumento <$2>."
    usage
    exit 1
  fi
}

cd "$ROOT"

case "${1:-}" in
  add)
    require_arg "${2:-}" "Nombre"
    dotnet ef migrations add "$2" \
      --project "$PERSISTENCE" \
      --startup-project "$API" \
      --output-dir "$MIGRATIONS_DIR"
    ;;
  update)
    if [[ -n "${2:-}" ]]; then
      dotnet ef database update "$2" \
        --project "$PERSISTENCE" \
        --startup-project "$API"
    else
      dotnet ef database update \
        --project "$PERSISTENCE" \
        --startup-project "$API"
    fi
    ;;
  list)
    dotnet ef migrations list \
      --project "$PERSISTENCE" \
      --startup-project "$API"
    ;;
  remove)
    dotnet ef migrations remove \
      --project "$PERSISTENCE" \
      --startup-project "$API"
    ;;
  script)
    OUTPUT="${2:-migrations.sql}"
    dotnet ef migrations script \
      --project "$PERSISTENCE" \
      --startup-project "$API" \
      --output "$OUTPUT"
    echo "Script generado: $ROOT/$OUTPUT"
    ;;
  ""|-h|--help|help)
    usage
    ;;
  *)
    echo "Comando desconocido: $1"
    usage
    exit 1
    ;;
esac
