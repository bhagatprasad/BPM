## 📋 Project Description
<img width="1536" height="1024" alt="Media" src="https://github.com/user-attachments/assets/445c5c67-d00b-4461-9f24-519b89841088" />

**BPM (Business Process Management)** is a comprehensive **B2B Medical Distribution Application** built to digitize and streamline the entire pharmaceutical supply chain from the **Distributor's (BPM's)** perspective. This platform acts as the central operational hub for BPM to manage its relationships with dealers, optimize inventory, and ensure efficient order fulfillment.

The system manages the complete workflow where BPM, as the **Distributor (Seller)** , interacts with multiple **Dealers (Buyers)** . It provides a transparent, efficient, and controlled environment for:
*   **Processing Dealer Orders**: Validating purchase orders, creating sales orders, and managing dispatch.
*   **Inventory & Fulfillment**: Allocating stock, picking/packing, and updating real-time inventory levels.
*   **Financial Control**: Managing billing, payments, credit limits, and reconciliation.
*   **Returns & Adjustments**: Handling dealer return requests, generating credit notes, and adjusting dues.

## 🎯 Key Features (BPM Distributor Focus)

This platform empowers BPM with a complete toolset to manage its distribution business efficiently.

| Module | Core Functionality for BPM (Distributor) |
| :--- | :--- |
| **Order Validation** | Review and approve/reject dealer Purchase Orders (POs) based on stock and credit. |
| **Sales Order (SO) Creation** | Convert validated POs into Sales Orders and generate dispatch notes. |
| **Inventory Allocation** | Allocate stock from inventory, manage pick/pack/ship processes. |
| **Payment & Billing** | Generate GST-compliant invoices, reconcile multiple payment modes, and share invoices with dealers. |
| **Credit Management** | Set credit limits for dealers, track utilization, manage outstanding dues, and send alerts. |
| **Returns Management** | Validate dealer return requests, generate credit notes, and adjust balances in future bills. |
| **Analytics Dashboard** | Gain insights into sales overview, order trends, inventory status, and credit health. |

## 🏗️ System Architecture & Technology Stack

The platform is built using a modern, scalable microservices architecture to ensure reliability and performance.

### Backend (Microservices)
- **Framework**: ASP.NET Core Web API
- **API Gateway**: YARP (Yet Another Reverse Proxy) to route requests to appropriate services.
- **Database**: PostgreSQL (with a database-per-service pattern for data isolation).
- **Message Broker**: RabbitMQ for asynchronous communication between services (e.g., order placement triggers inventory and notification services).

### Frontend
- **Dealer Application (Buyer Portal)**: Angular SPA (Single Page Application) for a responsive and user-friendly ordering experience.
- **Distributor Application (BPM Admin Portal)**: ASP.NET Core MVC for a powerful, server-rendered dashboard to manage all distributor operations.

### DevOps & Project Management
- **Version Control**: GitHub Repository - [`https://github.com/bhagatprasad/BPM`](https://github.com/bhagatprasad/BPM)
- **CI/CD Pipeline**: GitHub Actions for automated builds, testing, and deployments.
- **Project Tracking**: JIRA Software - [`https://bpmtech.atlassian.net/jira/software/projects/SCRUM/boards/1`](https://bpmtech.atlassian.net/jira/software/projects/SCRUM/boards/1) for agile sprint planning and issue management.

## 🚀 Getting Started

Follow these instructions to set up the project locally for development and testing.

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/) (for Angular)
- [PostgreSQL](https://www.postgresql.org/download/)
- [RabbitMQ](https://www.rabbitmq.com/download.html)
- [Git](https://git-scm.com/downloads)

### Installation & Setup

1.  **Clone the Repository**
    ```bash
    git clone https://github.com/bhagatprasad/BPM.git
    cd BPM
    ```

2.  **Backend Setup (Microservices)**
    - Navigate to each microservice folder (e.g., `OrderService`, `InventoryService`, `BillingService`).
    - Update the `appsettings.json` file with your PostgreSQL connection strings and RabbitMQ host details.
    - Run the database migrations for each service.
    ```bash
    dotnet ef database update
    ```
    - Start each microservice.
    ```bash
    dotnet run
    ```
    - Ensure the YARP API Gateway is configured and running to route requests.

3.  **Frontend Setup (Angular - Dealer App)**
    - Navigate to the `DealerApp` folder.
    - Install dependencies.
    ```bash
    npm install
    ```
    - Start the development server.
    ```bash
    ng serve
    ```
    - Access the app at `http://localhost:4200`.

4.  **Frontend Setup (MVC - Distributor App)**
    - Navigate to the `DistributorApp` folder.
    - Update the `appsettings.json` with the API Gateway base URL.
    ```bash
    dotnet run
    ```
    - Access the app at `https://localhost:5001`.

## 🤝 Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.
