using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlexureAPITest
{
[TestFixture]
public class Test
{
    Service service;

    readonly private string _userName = "Tester";
    readonly private string _password = "Plexure123";

    [OneTimeSetUp]
    public void Setup()
    {
        service = new Service();
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        if (service != null)
        {
            service.Dispose();
            service = null;
        }
    }

    [Test]
    [Category("Positive Tests")]
    public void TEST_001_Login_With_Valid_User()
    {
        // Soluition 1 : reason for failure of test case 001 is Username typo. It is now rectified.
        var response = service.Login(_userName, _password);
        response.Expect(HttpStatusCode.OK);

        //Adding additional validations for response
        int userId = response.Entity.UserId;
        string userName = response.Entity.UserName;
        string apiToken = response.Entity.AccessToken;

        // Writing response on console
        Console.WriteLine("User Id:" + userId);
        Console.WriteLine("User Name:" + userName);
        Console.WriteLine("Access Token:" + apiToken);

        //Assert on all three responses
        Assert.That(userId, Is.EqualTo(1), "Incorrect userid");
        Assert.That(userName, Is.EqualTo("Tester"), "Incorrect username");
        Assert.IsNotNull(apiToken);
    }

    [Test]
    [Category("Positive Tests")]
    public void TEST_002_Get_Points_For_Logged_In_User()
    {

        var points = service.GetPoints();

        // Validating reponse code
        points.Expect(HttpStatusCode.Accepted);

        //var point = points.Entity.Value;
        int uId = points.Entity.UserId;
        // int point = points.Entity.Value;
        //Console.WriteLine(uId);

        // Customer's current points
        Console.WriteLine("point:" + points.Entity.Value);

        //Validating correct user is getting the reponse back
        Assert.That(uId, Is.EqualTo(1), "Incorrect userid");
    }

    [Test]
    [Category("Positive Tests")]
    public void TEST_003_Purchase_Product()
    {
        int productId = 1;
        var purchaseRes = service.Purchase(productId);

        purchaseRes.Expect(HttpStatusCode.Accepted);

        // Response Output in console
        Console.WriteLine(purchaseRes.Entity.Message);
        Console.WriteLine("Awarded points: " + purchaseRes.Entity.Points);

        //Assert statements
        Assert.That(purchaseRes.Entity.Message, Is.EqualTo("Purchase completed."), "Purchase Did Not Complete.");
        Assert.That(purchaseRes.Entity.Points, Is.EqualTo(100), "Unknown Points Awarded");
    }

    [Test]
    [Category("Positive Tests")]
    public void TEST_004_Verifying_points_post_purchase()
    {
        //Getting initial points pre-purchase
        var initialPoints = service.GetPoints();
        int initialPointValue = initialPoints.Entity.Value;

        // Completing purchase & sourcing post purchase points
        int productId = 1;
        var purchase = service.Purchase(productId);
        var postPurchase = service.GetPoints();

        // Assert points are correctly added to customer account
        Assert.AreEqual(initialPointValue + 100, postPurchase.Entity.Value);
    }

    [Test]
    [Category("Positive Tests")]
    public void TEST_005_Verifying_points_post_mulitple_purchases_Stress()
    {
        //Getting initial points pre-purchase
        var initialPoints = service.GetPoints();
        int initialPointValue = initialPoints.Entity.Value;

        int productId = 1;
        int numberPurchases = 10;
        // Completing purchase & sourcing post purchase points
        for (int i = 0; i < numberPurchases; i++)
        {
            var purchase = service.Purchase(productId);
            initialPointValue = initialPointValue + purchase.Entity.Points;
        }


        var postPurchase = service.GetPoints();

        // Assert that points are correctly added to customer account
        Assert.AreEqual(initialPointValue, postPurchase.Entity.Value);

    }
    // Start of Negative Test Cases 

    [Test]
    [Category("Negative Tests")]
    public void TEST_006_Login_With_Invalid_Username()
    {
        Random ran = new Random();
        var response = service.Login(_userName + ran.Next(), _password);

        //Assert 
        response.Expect(HttpStatusCode.Unauthorized);


    }

    [Test]
    [Category("Negative Tests")]
    public void TEST_007_Login_With_Invalid_Password()
    {
        Random ran = new Random();
        var response = service.Login(_userName, _password + ran.Next());

        //Assert 
        response.Expect(HttpStatusCode.Unauthorized);

    }

    [Test]
    [Category("Negative Tests")]
    public void TEST_008_Login_With_no_User_Details()
    {
        var response = service.Login("", "");

        //Assert 
        response.Expect(HttpStatusCode.BadRequest);

        Assert.AreEqual(response.Error, "\"Error: Username and password required.\"");
    }

    [Test]
    [Category("Negative Tests")]
    public void TEST_009_User_request_for_Unknown_Product()
    {
        int productId = 2;
        var purchase = service.Purchase(productId);
        //Assert
        purchase.Expect(HttpStatusCode.BadRequest);
        Assert.AreEqual(purchase.Error, "\"Error: Invalid product id\"");

    }
    [Test]
    [Category("Negative Tests")]
    public void TEST_010_User_request_for_Unknown_Product_stressTest()
    {
        int productId = 2;
        int numberPurchases = 10;

        for (int i = 0; i < numberPurchases; i++)
        {
            var purchase = service.Purchase(productId);
            //Assert
            purchase.Expect(HttpStatusCode.BadRequest);
            Assert.AreEqual(purchase.Error, "\"Error: Invalid product id\"");
        }
    }
}
}
