"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var _ = require("lodash"); //imports everything from this library
var Order = (function () {
    function Order() {
        this.orderDate = new Date();
        this.items = new Array();
    }
    Object.defineProperty(Order.prototype, "subtotal", {
        get: function () {
            return _.sum(_.map(this.items, function (i) { return i.unitPrice * i.quantity; }));
        },
        enumerable: true,
        configurable: true
    });
    ; //_.map returns an array of calculated fields. _.sum adds these all together.
    return Order;
}());
exports.Order = Order;
var OrderItem = (function () {
    function OrderItem() {
    }
    return OrderItem;
}());
exports.OrderItem = OrderItem;
//# sourceMappingURL=order.js.map