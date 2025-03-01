FROM mcr.microsoft.com/dotnet/sdk:9.0.200
WORKDIR /App
COPY ContentGenerator.csproj ./
RUN dotnet restore
COPY . .
CMD dotnet watch run --urls "http://0.0.0.0:5000"