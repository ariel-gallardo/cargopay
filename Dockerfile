FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk
WORKDIR /src

ARG CERT_PASSWORD_ARG

RUN mkdir /tools
RUN mkdir -p /src/Certificates
RUN mkdir -p ~/.dotnet/corefx/cryptography/x509stores/my
COPY openssl.cnf /src/Certificates/
RUN openssl req -x509 -new -newkey rsa:2048 -keyout ./Certificates/cargopay-api.key -out ./Certificates/cargopay-api.crt -config ./Certificates/openssl.cnf -days 30 -nodes
RUN openssl pkcs12 -export -out ./Certificates/cargopay-api.pfx -inkey ./Certificates/cargopay-api.key -in ./Certificates/cargopay-api.crt -certfile ./Certificates/cargopay-api.crt -password pass:${CERT_PASSWORD_ARG}
RUN dotnet tool install --tool-path /tools dotnet-certificate-tool
RUN /tools/certificate-tool add --file ./Certificates/cargopay-api.pfx --password ${CERT_PASSWORD_ARG}
RUN cp ./Certificates/cargopay-api.crt /usr/local/share/ca-certificates
RUN cp ./Certificates/cargopay-api.key /usr/local/share/ca-certificates
RUN update-ca-certificates

COPY ["Presentation/Presentation.csproj", "Presentation/"]
RUN dotnet restore "Presentation/Presentation.csproj"
COPY . .

WORKDIR "/src/Presentation"

RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg;

FROM sdk AS app
WORKDIR /app

ARG BUILD_CONFIGURATION

RUN if [ "$BUILD_CONFIGURATION" = "Publish" ]; then \
    dotnet publish "/src/Presentation/Presentation.csproj" -v minimal -c Release -o /app/build; \
    else \
    dotnet build "/src/Presentation/Presentation.csproj" -v minimal -c "$BUILD_CONFIGURATION" -o /app/build; \
    fi

ENTRYPOINT ["dotnet", "/app/build/Presentation.dll"]
