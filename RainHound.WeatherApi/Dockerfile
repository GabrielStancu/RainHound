FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY ./RainHound.WeatherApi/*.csproj ./
RUN dotnet restore

COPY ./RainHound.WeatherApi ./
RUN dotnet publish -c Release -o out

ADD certificate.cer /usr/local/share/ca-certificates/foo.crt
RUN chmod 644 /usr/local/share/ca-certificates/certificate.cer && update-ca-certificates

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "RainHound.WeatherApi.dll"]
