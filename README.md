# Pyco TODO Liste

Einfache Webanwendung für das Anlaegen von Todo-Listen. Projekt für den Kurs "Labor Verteilte Systeme" an der DHBW Heidenheim von Gruppe 5 (Daniel Schwager, Jonas Wiedenmann, Maximilian Hartmann) 

## Projekt aufsetzen
- Sicherstellen, dass [Docker (Windows)](https://docs.docker.com/desktop/install/windows-install/ "Docker installieren") installiert ist.

- Dieses Repository und das Verzeichnis klonen oder als zip-Datei herunterladen.

```
git clone https://github.com/jwiedenmann/tin21-sem4-todo-list.git
cd tin21-sem4-todo-list
```

### Quick-start mit Docker-Compose
Wenn `docker` gestartet ist, kann im Repo ein Terminal geöffnet und dort folgender Befehl ausgeführt werden:

```
docker-compose up --build
```
Hiermit wird die gesamte Anwendung mit allen Containern gestartet. Die Anwendung ist im Browser erreichbar, indem man `localhost` in die URL-Leiste eingibt.

### Container
Nach dem der oben angeführte Build-Befehl ausgeführt wurde, können die Container mit Docker Desktop einzeln gestartet und gestoppt werden. Folgende Container können gestartet werden
- nginx-reverse-proxy
- mqtt-broker
- tododb
- pgadmin
- app

Die Daten im `tododb` Container bleiben auch nach dem Stoppen des Containers zu erhalten. Um die Datenbank zurückzusetzen müssen die Container zunächst mit 
```
docker-compose down -v
```
gestoppt und eliminiert werden. Anschließend müssen die Container wieder wie im Quickstart beschrieben gestartet werden.

###Debug Modus für Entwickler
Für die Entwicklung muss der `app` Container gestoppt werden. Das Backend und Frontend werden dann getrennt voneinander gestartet:

##### Backend
- Mit Visual Studio die Solution Datei `Pyco.Todo.sln` im Ordner `app/backend` öffnen
- Programm im Debug Modus starten

##### Frontend
- ein Terminal im Ordner `app/frontend` öffnen
- folgende Befehle nacheinander ausführen:
```
npm install
```
```
npm run serve
```

Anschließend kann auf die Anwendung über `localhost:5000` zugegriffen werden.