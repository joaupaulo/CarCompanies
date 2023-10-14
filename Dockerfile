FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CarCompanies/CarCompanies.csproj", "CarCompanies/"]
RUN dotnet restore "CarCompanies/CarCompanies.csproj"
COPY . .
WORKDIR "/src/CarCompanies"
RUN dotnet build "CarCompanies.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarCompanies.csproj" -c Release -o /app/publish

ENV ASPNETCORE_ENVIRONMENT=Production

CMD ["nginx", "-g", "daemon off;"]