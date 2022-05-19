FROM ubuntu

RUN apt-get update && apt-get install -y gnupg ca-certificates
RUN apt-key adv --keyserver keyserver.ubuntu.com --recv 5DE6FD698AD6FDD2
RUN echo "deb https://repo.krakend.io/apt stable main" | tee /etc/apt/sources.list.d/krakend.list
RUN apt-get update && apt-get install -y krakend

COPY krakend.json /etc/krakend/krakend.json

CMD KRAKEND_PORT=$PORT krakend run -c /etc/krakend/krakend.json