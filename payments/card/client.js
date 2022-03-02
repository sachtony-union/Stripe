var stripe = Stripe('pk_test_qblFNYngBkEdjEZ16jxxoWSM');

var elements = stripe.elements();
var cardElement = elements.create('card');
cardElement.mount('#card-element');
