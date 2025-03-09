#!/bin/bash
set -e

echo initializing database...
password="$(cat $DB_PASSWORD_FILE)"

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
  CREATE USER "coole_webapp" PASSWORD '$password';
  CREATE DATABASE "coole_webapp";
  GRANT ALL PRIVILEGES ON DATABASE "coole_webapp" TO "coole_webapp";
  GRANT CONNECT ON DATABASE "coole_webapp" TO "coole_webapp";
  \c "coole_webapp"
  REVOKE ALL ON SCHEMA public FROM PUBLIC;
  GRANT ALL ON SCHEMA public TO "coole_webapp";
EOSQL
