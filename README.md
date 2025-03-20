Welcome Candidate!

**Meet our company – CargoPay.**
CargoPay as a payment provider needs YOU to develop its new Authorization system and is
willing to pay accordingly!

The whole project is divided into two parts: Card Management module and Payment Fees
module. For each part you accomplish, you will get more points!
Total possible profit is 100K points.

Good luck, we hope you earn a lot of points!

You’ll need to develop a RESTful API that uses basic authentication.
The API will be written in C#, the data can be stored in the memory or in a database.

**The API will include two modules:**

**Card management module** = 60K points
The card management module includes three API endpoints:
- Create card (card format is 15 digits)
- Pay (using the created card, and update balance)
- Get card balance

**Payment Fees module** = 40K points
The payment fees module is calculating the payment fee for each payment.
How do we know what the payment fee is???
Well, our approach of calculating the fee is a bit different - the payment fee is pretty random
actually and changes every day and hour …
Every hour, the Universal Fees Exchange (UFE) randomly selects a decimal between 0
and 2. The new fee price is the last fee amount multiplied by the recent random decimal.
You should develop a Singleton to simulate the UFE service and the fee should be applied
to every payment

**Oh wait, there is a bonus!** = 30K points
- Improve your API performance and throughput using multithreading.
- Generally, using basic authentication is not a good solution. Improve the authentication
so we can make our Authorization system secure.
- Make the shared resources thread safe using a design pattern in case you are storing the
data in the memory. In case you are using a database to persist the cards and
transactions improve the database design and the usage of the ORM framework.

How to submit a solution: [Provide a downloadable link of the solution](https://github.com/ariel-gallardo/cargopay/archive/refs/heads/main.zip).

[DB Backup Link](https://drive.google.com/file/d/1LaPb2jpKR6RttqF1Ree0pvgCECcPdcnf/view?usp=sharing)

User Test
ariel@mail.com
123456