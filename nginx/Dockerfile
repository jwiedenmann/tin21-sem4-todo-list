# Verwenden Sie das offizielle Nginx-Basisimage von Docker Hub
FROM nginx:latest

# Löschen Sie die Standard-Nginx-Konfigurationsdateien
RUN rm /etc/nginx/conf.d/default.conf

# Kopieren Sie Ihre benutzerdefinierte Nginx-Konfigurationsdatei in den Container
COPY nginx.conf /etc/nginx/conf.d/

# Port 80 freigeben (der Standard-HTTP-Port)
EXPOSE 80

# Nginx im Vordergrund ausführen
CMD ["nginx", "-g", "daemon off;"]
