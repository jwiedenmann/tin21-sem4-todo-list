# Docker - Postgres

## Launch Postgresql

To start the database for the first time, simply open a terminal in this folder and run the following commands.

    docker compose build
    docker compose up

## Clear existing database

The initialization script is executed only if there is no database. To clear the docker files run the following command.

    docker compose down -v