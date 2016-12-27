# Online web store

Online store with interoperability with PRIMAVERA ERP/Uses Big Store template for frontend

##Project Overview 
The project consists on the creation of a web store that offers all the functionalities expected from such a site, like:

* Account register and login (two way verification through primavera and our postgresql database).
* Product browsing, with category filters and search by name.
* Check metrics of a product: price,description,rating, written reviews and available units
* Add products to the cart.
* Finalize product purchase.
* Check the availability of the product (current stock).
* Check the order status of a purchase and review associated products.
* Check order history, with all details related to the purchases.
* Rate and review purchased products
* Administration capabilities, allowing the hypothetical maintenance team to add images to the products

In order to achieve the goals for our product  and build these functionalities, we created an external layer (running on the browser), so as to implement the online store View component. This was done by creating a web app, using C# ASP.Net that implements the views by using ASP.Net’s MVC (Model View Controller) framework. 
This Web App accesses and uses the services of the PRIMAVERA REST API for the Model and Controller components, as required, which means the web app is an external extension of it, up to a certain point. We did not do a full coverage on all the PRIMAVERA capabilities, like inventory management, or dashboard overviews, which are out of this project’s scope. In addition to the PRIMAVERA API services, we extended the store with our own Database to implement the product rating, reviews and image editing.

##Deployment Diagram 
![deployment diagram](https://cloud.githubusercontent.com/assets/9083330/21509619/98438d5a-cc82-11e6-85aa-c2cbddbd351e.png)

##Core Views 
![home](https://cloud.githubusercontent.com/assets/9083330/21509612/88f081d2-cc82-11e6-83b5-1544b843bd5b.PNG)
![login](https://cloud.githubusercontent.com/assets/9083330/21509613/890488f8-cc82-11e6-853d-cdc1aac569cd.PNG)
![categories](https://cloud.githubusercontent.com/assets/9083330/21509611/88f02548-cc82-11e6-8147-8cbdbb1a631b.PNG)
![ps](https://cloud.githubusercontent.com/assets/9083330/21509681/3e787d34-cc83-11e6-8f33-ccbc97b3cced.PNG)
![prod](https://cloud.githubusercontent.com/assets/9083330/21509614/8906093a-cc82-11e6-8b91-ed599658b8e7.PNG)
![cart](https://cloud.githubusercontent.com/assets/9083330/21509615/89076b9a-cc82-11e6-8561-d57fa01208a2.PNG)
![checkou](https://cloud.githubusercontent.com/assets/9083330/21509607/88ee0678-cc82-11e6-8902-09f548dfeb3e.PNG)
![orderh](https://cloud.githubusercontent.com/assets/9083330/21509610/88efd642-cc82-11e6-9877-f6ea35b6e184.PNG)
![orderd](https://cloud.githubusercontent.com/assets/9083330/21509609/88ef8da4-cc82-11e6-91a4-54314527fc29.PNG)
![rev](https://cloud.githubusercontent.com/assets/9083330/21509608/88ef5a64-cc82-11e6-870a-115fc6a0ff15.PNG)
