# CleanObservability

A modular .NET solution with built-in observability using Promtail, Loki, and Grafana. Designed with Clean Architecture principles, structured error handling, and real-time log streaming.

## 📁 Project Structure


CleanObservability/ 
├── CleanObservability.sln 
├── CleanObservability.Core/        
# Middleware, diagnostics, result handling 
├── CleanObservability.Demo/         
# Sample API with observability hooks 
├── CleanObservability.Tests/       
# Unit tests 
├── CleanObservability.Infrastructure/ 
# (Optional) Infra-related services 
└── docker/                          
# Grafana, Loki, Promtail setup



## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Run the API
cd CleanObservability.Demo
dotnet run

Run the Observability Stack
cd docker
docker-compose up -d

- Grafana: http://localhost:3000
- Loki: http://localhost:3100

🔍 Observability Features
- ✅ Promtail: Collects logs from .log files
- ✅ Loki: Centralized log aggregation
- ✅ Grafana: Visualizes logs with LogQL queries
- ✅ Structured Logging: Serilog + middleware enrichers
- ✅ Correlation ID: Traces requests across services
- ✅ Custom Middleware: For exception handling and validation


🧪 Testing
cd CleanObservability.Tests
dotnet test

📦 Docker Stack Overview
![image](https://github.com/user-attachments/assets/cf6ad16e-6d0e-43b1-92fb-d4bb61613293)

📄 License
Licensed under the Apache 2.0 License.

🙌 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you’d like to change.


💡 Future Enhancements
- GitHub Actions CI for build + test
- Docker image publishing
- LogQL query examples
- Dashboard templates for Grafana
