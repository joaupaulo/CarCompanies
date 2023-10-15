FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CarCompanies/CarCompanies.csproj", "CarCompanies/"]
RUN dotnet restore "CarCompanies/CarCompanies.csproj"
COPY . .
WORKDIR "/src/CarCompanies"
RUN dotnet build "CarCompanies.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarCompanies.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN rm -rf /src && rm -rf /root/.nuget

ENTRYPOINT ["dotnet", "CarCompanies.dll"]