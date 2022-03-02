using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Stripe;

// Set your secret key. Remember to switch to your live secret key in production!
// See your keys here: https://dashboard.stripe.com/apikeys
StripeConfiguration.ApiKey = "sk_test_26PHem9AhJZvU623DfE1x4sd";

namespace workspace.Controllers
{
  [Route("api/[controller]")]
  public class StripeWebHook : Controller
  {
    // You can find your endpoint's secret in your webhook settings
    const string secret = "whsec_...";

    [HttpPost]
    public async Task<IActionResult> Index()
    {
      var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

      try
      {
        var stripeEvent = EventUtility.ConstructEvent(
          json,
          Request.Headers["Stripe-Signature"],
          secret
        );

        // Handle the checkout.session.completed event
        if (stripeEvent.Type == Events.CheckoutSessionCompleted)
        {
          var session = stripeEvent.Data.Object as Checkout.Session;

          // Fulfill the purchase...
          try
          {
            this.FulfillOrder(session);
          }
          catch (NotImplementedException ex)
          {
            return BadRequest();
          }
        }

        return Ok();
      }
      catch (StripeException e)
      {
        return BadRequest();
      }
    }

    private void FulfillOrder(Checkout.Session session) {
      var options = new SessionListLineItemsOptions
      {
        Limit = 100,
      };
      var service = new SessionService();
      StripeList<LineItem> lineItems = service.ListLineItems(session.Id, options);

      // TODO: Remove error and implement...
      throw new NotImplementedException($"Given the Checkout Session \"{session.Id}\" load your internal order from the database here.\n Then you can reconcile your order's quantities with the final line item quantity purchased. You can use `checkout_session.metadata` and `price.metadata` to store and later reference your internal order and item ids.");
    }
  }
}
