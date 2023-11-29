FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 80

COPY ./RainHound.WeatherApi/*.csproj ./
RUN dotnet restore

COPY ./RainHound.WeatherApi/certificate.pfx .
COPY ./RainHound.WeatherApi ./
RUN dotnet publish -c Release -o out

EXPOSE 443
ENV ASPNETCORE_URLS=https://+:443;http://+:80
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Gabi1234

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "RainHound.WeatherApi.dll"]
