FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY ./RainHound.WeatherApi/*.csproj ./
RUN dotnet restore

COPY ./RainHound.WeatherApi ./
RUN dotnet publish -c Release -o out

RUN echo "server { \
    listen 443 ssl; \
    server_name rainhound-weatherapi.cyhpb5c7c8gwckde.switzerlandnorth.azurecontainer.io; \
    ssl_certificate ./certificate.cer; \
    ssl_certificate_key ./key.pkey; \
    location / { \
        root /usr/share/nginx/html; \
        index index.html index.htm; \
    } \
}" > /etc/nginx/conf.d/default.conf

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "RainHound.WeatherApi.dll"]
