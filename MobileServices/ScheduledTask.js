function ProcessNotifications() {
    
    // Process Coupon Tile notifications
    // ------------------------------------
    var couponTable = tables.getTable("Coupon");
    couponTable.where({
        Sent:false
    }).read({
        success: function(coupons) {
            coupons.forEach(function (coupon) {
                sendTileUpdateToAllWithText(coupon.Text);
                
                coupon.Sent = true;
                couponTable.update(coupon);
            });
        }
    });
    
    // Process Cart Toast notifications
    // ------------------------------------
    var cartTable = tables.getTable("Cart");
    cartTable.where({
        ProcessingNotificationSent:false
    }).read({
        success: function(carts) {
            carts.forEach(function (cart) {
                sendOrderAcceptedNotification(cart);
                
                cart.ProcessingNotificationSent = true;
                cartTable.update(cart);
            });
        }
    });
    
}

function sendTileUpdateToAllWithText(couponText) {
    console.log('Coupon being sent for ', couponText);
    
    var subscribersTable = tables.getTable("PushNotificationDealsSubscriber");
    subscribersTable.read({
        success: function(subscribers) {
            subscribers.forEach(function (subscriber) {
                if (subscriber.ExpirationTime > new Date()) {
                    push.wns.sendTileWideText01(subscriber.Uri, {
                        text1: couponText,
                    });
                    console.log('Tile notification sent to subscriber ', subscriber.id);
                }
                else {
                    console.log('Removing expired subscriber ', subscriber.id, ' - expirationTime: ', subscriber.ExpirationTime);
                    subscribersTable.del(subscriber);
                }
            });
        }
    });
}

function sendOrderAcceptedNotification(cart)
{
    var subscribersTable = tables.getTable("PushNotificationDealsSubscriber");
    subscribersTable.where({
        HardwareId:cart.HardwareId
    }).
    read({
        success: function(subscribers) {
            
            if (subscribers.length > 0) {
                var subscriber = subscribers[0];    

                console.log('Sending toast notification to ', cart.UserId, " at url ", subscriber.Uri);
                
                 push.wns.sendToastText01(subscriber.Uri, {
                    text1: "Your order has been received and is now being processed.",
                }, {
                    launch: 'cartId=' + cart.id
                });
            } else {
                console.log("No notification subscription found for ", cart.HardwareId);
            }
        }
    });
}