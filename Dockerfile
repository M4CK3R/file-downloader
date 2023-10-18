FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*


WORKDIR /src
COPY . .
RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p zaq1@WSX
RUN dotnet publish -o /app -c Release

WORKDIR /app
ENV FilesPath="/app/files"
ENV ASPNETCORE_URLS=https://*:443;http://*:80

ENV ASPNETCORE_Kestrel__Certificates__Default__Password=zaq1@WSX
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENTRYPOINT ["dotnet", "Downloader.dll"]