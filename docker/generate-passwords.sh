#!/bin/bash
set -e

tr -dc A-Za-z0-9 </dev/urandom | head -c 24 > db_root_password.txt;
tr -dc A-Za-z0-9 </dev/urandom | head -c 24 > db_password.txt;
