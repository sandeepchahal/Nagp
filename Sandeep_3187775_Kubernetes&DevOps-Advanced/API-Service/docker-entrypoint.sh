#!/bin/sh
set -e

# Optional: Add any setup steps here, like database migrations
# echo "Running database migrations"
# python manage.py migrate

# Execute the main process specified as CMD in the Dockerfile
exec "$@"
