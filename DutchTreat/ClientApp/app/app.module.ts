import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { ProductList } from "./shop/productList.component";
import { Cart } from "./shop/cart.component";
import { DataService } from "./shared/dataService";
import { Shop } from "./shop/shop.component";
import { Checkout} from "./checkout/checkout.component";


import { RouterModule } from "@angular/router";

let routes = [
	{ path: "", component: Shop },
	{ path: "checkout", component: Checkout }
];

@NgModule({
	declarations: [
		AppComponent,
		ProductList,
		Cart,
		Shop,
		Checkout
	],
	imports: [
        BrowserModule, HttpModule,
        RouterModule.forRoot(routes, { useHash: true, enableTracing: false })
	],
	providers: [
		DataService
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
