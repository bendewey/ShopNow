function insert(cart, user, request) {

    var cartItems = cart.Items;
    delete cart.Items;
    cart.OrderDate = new Date();
    cart.ProcessingNotificationSent = false;
    
    request.execute({
        success: function() {
            console.log("Added cart ", cart.id);
            
            var cartItemsTable = tables.getTable("CartItem");
            cartItems.forEach(function(cartItem){
                cartItem.CartId = cart.id;
                cartItemsTable.insert(cartItem);
                console.log("Added item ", cartItem.Name, " to cart ", cart.id)
            });

            request.respond();
        }
    });
}