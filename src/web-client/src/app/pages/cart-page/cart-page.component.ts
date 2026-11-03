import { Component } from "@angular/core";
import { CartItemListComponent } from "../../shared/components/cart-item-list/cart-item-list.ocomponent";
import { CurrencyPipe } from "@angular/common";
import { CartItem } from "../../models/CartItem";

@Component({
    templateUrl: "./cart-page.component.html",
    selector: "app-cart-page",
    standalone: true,
    imports: [CartItemListComponent, CurrencyPipe],
})
export class CartPageComponent {
    cartItems: CartItem[] = [
        {
            Name: "Ace Combat 7: Skies Unknown Launch Ed.",
            ShortDescription: "Bundle PS4",
            Size: "53.76 GB",
            PriceUsd: 59.99,
            DiscountedPrice: 49.99,
            Quantity: 1,
        },
        {
            Name: "Beats Fever",
            ShortDescription: "PS VR Game",
            Size: "1 GB",
            PriceUsd: 19.99,
            Quantity: 1,
        },
        {
            Name: "Eden Tomorrow",
            ShortDescription: "PSN Game",
            Size: "34.6 GB",
            PriceUsd: 39.99,
            Quantity: 1,
        },
    ];

    get totalItems(): number {
        return this.cartItems.reduce((acc, item) => acc + item.Quantity, 0);
    }

    get subtotal(): number {
        return this.cartItems.reduce(
            (acc, item) => acc + this.itemPriceDiscountIncluded(item) * item.Quantity,
            0,
        );
    }

    private itemPriceDiscountIncluded(item: CartItem): number {
        return item.DiscountedPrice ? item.DiscountedPrice : item.PriceUsd;
    }

    get taxes(): number {
        return 3.0;
    }

    get total(): number {
        return this.subtotal + this.taxes;
    }

    removeItem(itemToRemove: CartItem): void {
        this.cartItems = this.cartItems.filter((item) => item !== itemToRemove);
    }

    increaseQuantity(itemToUpdate: CartItem): void {
        const item = this.cartItems.find((i) => i === itemToUpdate);
        if (item) {
            item.Quantity++;
        }
    }

    decreaseQuantity(itemToUpdate: CartItem): void {
        const item = this.cartItems.find((i) => i === itemToUpdate);
        if (item && item.Quantity > 1) {
            item.Quantity--;
        }
    }
}
