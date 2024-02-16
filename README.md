 # Microservice - Project



## 1)A writeup of identified microservices for above use case along with reasonable explanation  

 
API Gateway: This microservice serves as the entry point for client requests and handles routing, authentication, and other cross-cutting concerns. It provides a unified interface for clients to interact with the system and delegates requests to the appropriate microservices. Ocelot has been used for the same. 

Order Service: Responsible for managing orders within the system. This microservice handles and communicates with the database(List in current case) allowing users to view order data and ensures consistency and integrity of order-related operations. Handles Order-related operations fetches add to cart data from Product Service using RabbitMQ.  

Product Service: Manages product information and inventory. This microservice provides endpoints for retrieving product details, adding product inventory, and ordering and add to cart methods. It maintains a catalogue of available products and ensures accurate and up-to-date product information. Also Include AddtoCart and PlaceOrder methods for further operations which sends the product data to the Cart and Order service respectively. 

Cart Service: Handles shopping cart functionality, allowing users to view items in their Cart, or remove items from their Cart. This microservice manages the state of the user's shopping cart, handles cart-related operations fetches add to cart data from Product Service using RabbitMQ. 


--- 
## 2)Docker Image on docker Hub 


`docker pull -a vedantmaurya/microservice-assignment`

### [vedantmaurya/microservice Tags | Docker Hub](https://hub.docker.com/r/vedantmaurya/microservice-assignment/tags) 


---

## 3)URL definitions of the scenarios (Sample POSTMAN collection, or request/response JSONs)  

 

Sample JSON body for adding a product via http://localhost:80/addproduct  
```
{ 

  "productId": 0, 

  "productName": "string", 

  "description": "string", 

  "price": 0, 

  "category": "string" 

} 
```


Also JWT auth is used in Product Service Add and remove method of product service can only be used by Admin 

For fetching JWT token http://localhost:80/generatetoken/{Role}  where Role can be Admin or User which will provide the JWT token accordingly  

Order can be placed by Ex - http://localhost:80/AddtoCart/User/{userId}/Product/{productId}  

RabbitMq is used from ProductService [AddtoCart and PlaceOrder] to Cart and Order Service, which will take data from the product service and send to Cart and Order Service. 


---
**Ocelot is used for APIGateway which is hosted on port 80** 

**Product Service -> 5039 [Swagger UI](http://localhost:5039/swagger/index.html)** 

**Cart Service -> 5235 [Swagger-UI](http://localhost:5235/swagger/index.html)**

**Order Service -> 5196 [Swagger UI](http://localhost:5196/swagger/index.html)**

**Eureka -> [Eureka](http://localhost:8761/)**

**RabbitMQ -> [RabbitMQ Management](http://localhost:15672/#/)** 

 

> Hosted on the ports mentioned above. 
---
 

For running project from source code -> Run [In WSL] 

```
Docker compose build  

Docker compose up  
```

The project should be up and running 

 

 
