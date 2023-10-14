FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CarCompanies/CarCompanies.csproj", "CarCompanies/"]
RUN dotnet restore "CarCompanies/CarCompanies.csproj"
COPY . .
WORKDIR "/src/CarCompanies"
RUN dotnet build "CarCompanies.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarCompanies.csproj" -c Release -o /app/publish

FROM nginx:alpine

COPY config/nginx.conf /etc/nginx/nginx.conf
COPY config/default.conf /etc/nginx/conf.d/default.conf

COPY --from=publish /app/publish /usr/share/nginx/html

EXPOSE 80
EXPOSE 443

CMD ["nginx", "-g", "daemon off;"]