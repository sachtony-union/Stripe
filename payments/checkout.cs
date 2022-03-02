using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace server.Controllers
{
  public class PaymentsController : Controller
  {
    public PaymentsController()
    {
      // Set your secret key. Remember to switch to your live secret key in production!
      // See your keys here: https://dashboard.stripe.com/apikeys
      StripeConfiguration.ApiKey = "sk_test_26PHem9AhJZvU623DfE1x4sd";
    }

    [HttpPost("create-checkout-session")]
    public ActionResult CreateCheckoutSession()
    {
      var options = new SessionCreateOptions
      {
        PaymentMethodTypes = new List<string>
        {
          "card",
        },
        LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            Price = "{{PRICE_ID}}",
            AdjustableQuantity = new SessionLineItemAdjustableQuantityOptions
            {
              Enabled = true,
              Minimum = 1,
              Maximum = 10,
            }
            Quantity = 1,
          },
        },
        Mode = "payment",
        SuccessUrl = "https://example.com/success",
        CancelUrl = "https://example.com/cancel",
        ShippingAddressCollection = new SessionShippingAddressCollectionOptions
        {
          AllowedCountries = new List<string>
          {
            "US",
            "CA",
          },
        },
      };

      var service = new SessionService();
      Session session = service.Create(options);

      return Json(new { id = session.Id });
    }
  }
}
