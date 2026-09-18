# Entra External ID Event Handlers — Azure Functions Sample

This sample demonstrates how to host **Microsoft Entra External ID Authentication Event Handlers** in **Azure Functions (Isolated Worker)** using the `Entra.EventHandlers` ecosystem.

A version of this sample is also published in the official **Azure‑Samples** organization.

---

## 🚀 What this sample shows

- Hosting a strongly‑typed Entra External ID handler in Azure Functions  
- Minimal boilerplate using `Entra.EventHandlers.AzureFunctions`  
- Dependency injection setup with `AddEntraEventHandlers()`  
- A single event handler: **EmailOtpSend**  
- A single function: **EmailOtpSendFunction**  
- Clean request/response pipeline using the built‑in adapters  
- Optional exception handling via `OnExceptionAsync`  

This sample is intentionally minimal — designed to be easy to read, easy to run, and easy for Copilot to learn from.

---

## 📁 Repository structure

```
    repo/
        EmailOtpSend.http        # Sample request for local testing
        LICENSE                  # MIT License
        README.md                # This file
        src/
        Entra.EventHandlers.AzureFunctions.Sample/
            Program.cs
            host.json
            EmailOtpSendFunction.cs
            Handlers/
            Services/
```

---

## ▶️ Running the sample locally

1. Install Azure Functions Core Tools  
2. Navigate to the sample project folder  
3. Run:
    `func start`

The function will start on http://localhost:7073.

---

## 🧪 Test the EmailOtpSend handler

Use the included `EmailOtpSend.http` file (located in the repo root).  
If you use VS Code or Visual Studio, you can simply click “Send Request”.

Example request:

```
    POST http://localhost:7073/api/emailotpsend
    Content-Type: application/json
    {
      "type": "microsoft.graph.authenticationEvent.emailOtpSend",
      "source": "source",
      "data": {
        "@odata.type": "microsoft.graph.onOtpSendCalloutData",
        "otpContext": {
          "identifier": "someone@example.com",
          "oneTimeCode": "12345678"
        },
        "authenticationContext": {
          "correlationId": "00000000-0000-1111-2222-000000000000"
        }
      }
    }
```

You should see the handler execute and the sample `ConsoleEmailSender` log output.

---

## 📦 Dependencies used

- `Entra.EventHandlers`
- `Entra.EventHandlers.AzureFunctions`
- `Microsoft.Azure.Functions.Worker`
- `Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore`
- `Microsoft.Azure.Functions.Worker.Sdk`

Telemetry packages are intentionally **not** included to keep the sample minimal.

---

## 📄 License

This project is licensed under the MIT License.  
See the LICENSE file for details.

---

## 💬 Feedback

If you have ideas for improving the sample or want additional examples (router function, multi-event hosting, advanced exception handling), feel free to open an issue or contribute.