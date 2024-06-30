# api-gw-setup

This project demonstrates API gateway pattern using Ocelet. The project using Microsoft Identity for Authorization in the API Gateway.

#Setup
Open the solution and set multiple project startup.

<img width="404" alt="image" src="https://github.com/Purush0th/api-gw-setup/assets/1386902/0cd2f632-c47f-4b64-9728-3a3ce2194490">


#Running
Start the debugging and vist this URL http://localhost:5000/swagger/index.html

API GW - http://localhost:5000
Service 1 - http://localhost:5001
Service 2 - http://localhost:5002

From the Swagger UI you can register a user and login with the same to generate a JWT token.
<img width="1088" alt="image" src="https://github.com/Purush0th/api-gw-setup/assets/1386902/7a9079c1-7750-4fe8-a4af-f88c82247834">

(Or)
Try with admin@local/Admin@123
```
curl -X 'POST' \
  'http://localhost:5000/identity/login' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "admin@local",
  "password": "Admin@123"
}'
```

With the above token you can hit the services routed through the API Gateway
'http://localhost:5000/paymentsvc/payment' 
'http://localhost:5000/ordersvc/order' 


For adding more configuration check the config `ocelot.json` in the APIGw project.
```javascript
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/v1/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        }
      ],
      "UpstreamPathTemplate": "/ordersvc/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Identity.Bearer",
        "AllowedScopes": []
      }
    },
    {
      "DownstreamPathTemplate": "/api/v1/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5002
        }
      ],
      "UpstreamPathTemplate": "/paymentsvc/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Identity.Bearer",
        "AllowedScopes": []
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

