import { Http,Response } from "@angular/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Product } from "./product";
import { Order, OrderItem } from "./order";
import 'rxjs/add/operator/map';

@Injectable()
export class DataService {

    constructor(private http: Http) { }

    private token: string = "";

    private tokenExpiration: Date;

	public order: Order = new Order();

	public products: Product[] = [];

	loadProducts(): Observable<Product[]> {
		return this.http.get("/api/products")
			.map((result: Response) =>
				this.products = result.json());
	}

    public get loginRequired(): boolean {
        return this.token.length == 0 || this.tokenExpiration < new Date();
    } // Either token doesn't exist or has expired.

	public AddToOrder(product: Product) {
		
		let item: OrderItem = this.order.items.find(i => i.productId == product.id);

		if (item) {
			item.quantity++;
		}
		else {
			item = new OrderItem();
			item.productId = product.id;
			item.productArtist = product.artist;
			item.productCategory = product.category;
			item.productArtId = product.artId;
			item.productTitle = product.title;
			item.productSize = product.size;
			item.unitPrice = product.price;
			item.quantity = 1;
			this.order.items.push(item);
		}
		
	}
}